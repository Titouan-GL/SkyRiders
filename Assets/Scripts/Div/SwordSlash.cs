using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordSlash : MonoBehaviour
{
    [SerializeField]private float damage = 40f;
    [SerializeField]private GameObject instanciateOnHit;
    public List<GameObject> cannotDamageMore = new List<GameObject>();
    public Vector3 initialPosition;
    public Quaternion initialRotation;
    private CameraScript cameraScript;
    
    void Awake(){
        initialPosition = transform.localPosition;
        initialRotation = transform.localRotation;
        cameraScript = Camera.main.gameObject.GetComponent<CameraScript>();
    }
    void OnEnable()
    {
        cannotDamageMore.Clear();
    }

    void OnDisable()
    {
        cannotDamageMore.Clear();
        transform.localPosition = initialPosition;
        transform.localRotation = initialRotation;
    }

    private void OnCollisionEnter(Collision collision)
    {
        foreach (ContactPoint contact in collision.contacts)
        {
            GameObject otherGameObject = contact.otherCollider.gameObject;
            if(!cannotDamageMore.Contains(otherGameObject)){
                cannotDamageMore.Add(otherGameObject);

                if(otherGameObject.GetComponentInChildren<NPC>()){
                    otherGameObject.GetComponentInChildren<NPC>().takeDamage(damage);
                    if(instanciateOnHit != null){
                        Instantiate(instanciateOnHit, contact.point, Quaternion.LookRotation(contact.point - transform.position));
                        cameraScript.Shake();
                    }
                }
            }
        }
        transform.localPosition = initialPosition;
        transform.localRotation = initialRotation;
    }


}
