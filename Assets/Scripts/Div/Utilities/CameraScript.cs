using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class CameraScript : MonoBehaviour
{
    [SerializeField]private Transform playerTransform;
    [SerializeField]private Transform parentTransform;
    [SerializeField]private Transform fireTargetTransfom;
    [SerializeField]private Transform parentPositionWhenAiming;
    [SerializeField]private VisualEffect ve;

    private float sensitivity = 4f;
    private Vector3 velocity = Vector3.zero;
    private Vector3 velocitybis = Vector3.zero;
    private UtilitiesNonStatic UNS;

    private Vector3 positionWhenAiming = new Vector3(0f, 7f, -21f);

    float rotationX = 0f;
    float rotationY = 0f;
    float rotationZ = 0f;

    private Quaternion initialRotation;
    private Vector3 initialPosition;

    private float minAngle = -30f;
    private float maxAngle = 20f;

    private float minAngleAim = -45f;
    private float maxAngleAim = 40f;

    private Camera m_camera;

    private float fieldOfViewDefault;

    [SerializeField]private AnimationCurve curve;
    [SerializeField]private float shakeDuration = 0.2f;
    [SerializeField]private float shakeStrengthMultiplier = 1.4f;
    // Start is called before the first frame update
    void Awake(){
        initialRotation = transform.localRotation;
        initialPosition = transform.localPosition;
        m_camera = GetComponentInChildren<Camera>();
        fieldOfViewDefault = m_camera.fieldOfView;
        UNS = UtilitiesStatic.GetUNS();
    }

    void Update(){
        RaycastHit hit;

        if(Physics.Raycast(transform.position, transform.forward, out hit, 1000f)){
            fireTargetTransfom.position = hit.point;
        }
        else{
            fireTargetTransfom.position = transform.position + transform.forward * 1000f;
        }
    }

    public void FixedUpdateNormal(float speed)
    {
        RegularFollow();
        if((rotationX < minAngle || rotationX > maxAngle)){
            rotationX = Mathf.Lerp(rotationX, 0f, 0.05f);
        }

        float objectiveZ = rotationZ > 180 ? 360 : 0f;
        rotationZ = Mathf.Lerp(rotationZ, objectiveZ, 0.1f);
        parentTransform.localEulerAngles = new Vector3(rotationX, rotationY, rotationZ);


        transform.localRotation = Quaternion.Slerp(transform.localRotation, initialRotation, 0.1f);
        transform.localPosition = Vector3.Lerp(transform.localPosition, initialPosition, 0.1f);
        
        parentTransform.position = Vector3.SmoothDamp(parentTransform.position, playerTransform.position, ref velocity, 0.3f);
        m_camera.fieldOfView = Mathf.Lerp(m_camera.fieldOfView, fieldOfViewDefault + speed/3, 0.03f);
        PreventPlayerOutOfScreen();  
    }

    public void FixedUpdateAiming()
    {
        RegularFollow();
        if((rotationX < minAngleAim || rotationX > maxAngleAim)){
            rotationX = Mathf.Lerp(rotationX, 0f, 0.05f);
        }

        float objectiveZ = rotationZ > 180 ? 360 : 0f;
        rotationZ = Mathf.Lerp(rotationZ, objectiveZ, 0.1f);
        parentTransform.localEulerAngles = new Vector3(rotationX, rotationY, rotationZ);

        parentTransform.position = Vector3.SmoothDamp(parentTransform.position, parentPositionWhenAiming.position, ref velocitybis, 0.1f);
        transform.localPosition = Vector3.SmoothDamp(transform.localPosition, positionWhenAiming, ref velocity, 0.05f);
    }

    public void FixedUpdatePlane(float speed){
        rotationY = parentTransform.localEulerAngles.y;
        rotationX = parentTransform.localEulerAngles.x;
        rotationZ = parentTransform.localEulerAngles.z;
        parentTransform.Rotate(Input.GetAxis("Mouse Y") * -sensitivity, Input.GetAxis("Mouse X") * sensitivity, 0f);
        
        //setting back parent if just transformed
        transform.localRotation = Quaternion.Slerp(transform.localRotation, Quaternion.identity, 0.1f);
        transform.localPosition = Vector3.Lerp(transform.localPosition, new Vector3(0, initialPosition.y/4, initialPosition.z/1.5f), 0.1f);

        ve.SetFloat("SpawnRate", speed/2);
        
        parentTransform.position = Vector3.SmoothDamp(parentTransform.position, playerTransform.position, ref velocity, 0.3f);
        m_camera.fieldOfView = Mathf.Lerp(m_camera.fieldOfView, fieldOfViewDefault + speed/3, 0.03f);
    }

    private void RegularFollow(){
        ve.SetFloat("SpawnRate", 0);
        //cramping and setting rotations
        rotationY += Input.GetAxis("Mouse X") * sensitivity;

        rotationX += Input.GetAxis("Mouse Y") * sensitivity*-1;

        while(rotationX > 180){
            rotationX -= 360;
        }
        while(rotationX < -180){
            rotationX += 360;
        }
    }

    private void PreventClipping(){
        RaycastHit hit;


        Vector3 raycastDir = transform.position - parentTransform.position;
        Debug.DrawRay(parentTransform.position, raycastDir.normalized * raycastDir.magnitude, Color.magenta);
        if(Physics.Raycast(parentTransform.position, raycastDir.normalized, out hit, raycastDir.magnitude, UNS.layerMaskLevel)){
            transform.position = Vector3.Lerp(transform.position, hit.point, 0.6f);
        }
    }

    private void PreventPlayerOutOfScreen(){
        // Convert the world position to a viewport point
        Vector3 viewportPoint = m_camera.WorldToViewportPoint(playerTransform.position);

        int savemepls = 100;//to prevent infinite loops
        while((viewportPoint.x < 0.1 || viewportPoint.x > 0.9 || viewportPoint.y < 0.1 || viewportPoint.y > 0.9) && savemepls > 0){
            viewportPoint = m_camera.WorldToViewportPoint(playerTransform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(playerTransform.position - transform.position, Vector3.up), 0.005f);
            savemepls -= 1;
        }
    }

    public void Shake(){
        StartCoroutine(Shaking());
    }

    IEnumerator Shaking(){
        Vector3 startPosition = transform.localPosition;
        float elapsedTime = 0f;

        while(elapsedTime < shakeDuration){
            elapsedTime += Time.deltaTime;
            float strength = curve.Evaluate(elapsedTime/shakeDuration)*shakeStrengthMultiplier;
            transform.localPosition = startPosition + Random.insideUnitSphere * strength;
            yield return null; 
        }
        
        transform.localPosition = startPosition;
    }
}
