using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Combat : MonoBehaviour
{
    private InputAndAnimations inputs;
    [SerializeField] private Transform firePoint;
    [SerializeField] private List<Transform> firePointsPlane;
    [SerializeField] private Transform fireDirection;
    [SerializeField] private GameObject bullet;
    [SerializeField] private List<GameObject> slashes;
    [SerializeField] private GameObject slashCollider;
    
    private float reloadTime = 0.1f;
    private float reloadTimeCurrent = 0f;

    // Start is called before the first frame update
    void Start()
    {
        inputs = GetComponentInChildren<InputAndAnimations>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(reloadTimeCurrent > 0){
            reloadTimeCurrent -= Time.deltaTime;
        }
        else if(inputs.firing && !inputs.flying){
            reloadTimeCurrent = reloadTime;
            Instantiate(bullet, firePoint.position, fireDirection.rotation * Quaternion.Euler(270, 0, 0));
        }
        else if(inputs.firing && inputs.flying){
            reloadTimeCurrent = reloadTime;
            foreach(Transform point in firePointsPlane){
                Instantiate(bullet, point.position, point.rotation);
            }
        }
    }

    public void InstantiateSlash(int index){
        slashes[index].SetActive(false);
        slashes[index].SetActive(true);
        slashCollider.SetActive(false);
        slashCollider.SetActive(true);

    }

}
