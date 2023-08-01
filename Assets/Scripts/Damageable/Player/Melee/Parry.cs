using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parry : MonoBehaviour
{
    [SerializeField] private float parryTime = 0.3f;
    [SerializeField]private GameObject instanciateOnParry;
    [SerializeField]private GameObject instanciateOnGuard;
    [SerializeField]private Transform parryPoint;
    [SerializeField]private Vector3 parryPointDefaultLocation;
    private float avoidParrySpam = 0.5f;
    private float avoidSpamTime = 0f;
    private float parryCurrentTime = 0.3f;

    private bool willDeactivate = false;

    void OnEnable(){
        willDeactivate = false;
        if(avoidSpamTime <= 0){
            parryCurrentTime = parryTime;
            avoidSpamTime = avoidParrySpam;
        }
        parryPointDefaultLocation = parryPoint.localPosition;
    }

    void OnDisable(){
        parryCurrentTime = 0;
    }

    public bool Parried(){
        avoidSpamTime = 0f;
        return (parryCurrentTime > 0);
    }
    
    public bool CanDeactivate(){
        return parryCurrentTime <= 0;
    }

    public void WillDeactivate(){
        willDeactivate = true;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(willDeactivate && parryCurrentTime <= 0){
            gameObject.SetActive(false);
        }
        parryCurrentTime -= Time.fixedDeltaTime;
        avoidSpamTime -= Time.deltaTime;
    }

    public void InstanciateOnParry(Vector3 position){
        if(instanciateOnParry != null){
            Instantiate(instanciateOnParry, parryPoint.position, Quaternion.LookRotation(parryPoint.position - position));
        }
    }

    public void InstanciateOnGuard(Vector3 position){
        if(instanciateOnGuard != null){
            Instantiate(instanciateOnGuard, parryPoint.position, Quaternion.LookRotation(parryPoint.position - position));
        }
    }
}
