using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDeactivate : MonoBehaviour
{
    [SerializeField]private float lifeTime = 0.3f;
    public float lifeTimeMax;
    [SerializeField]private List<GameObject> instantiateOnDeactivate;

    void Awake(){
        lifeTimeMax = lifeTime;
    }

    void OnEnable(){
        lifeTime = lifeTimeMax;
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        lifeTime -= Time.fixedDeltaTime;
        if(lifeTime < 0){
            foreach(GameObject go in instantiateOnDeactivate){
                Instantiate(go, transform.position, transform.rotation);
            }
            lifeTime = lifeTimeMax;
            gameObject.SetActive(false);
        }
    }
}
