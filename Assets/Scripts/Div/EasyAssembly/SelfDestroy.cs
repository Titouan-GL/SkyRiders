using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDestroy : MonoBehaviour
{
    [SerializeField]private float lifeTime = 3f;
    [SerializeField]private List<GameObject> instantiateOnDeath;

    // Update is called once per frame
    void FixedUpdate()
    {
        lifeTime -= Time.fixedDeltaTime;
        if(lifeTime < 0){
            foreach(GameObject go in instantiateOnDeath){
                Instantiate(go, transform.position, transform.rotation);
            }
            Destroy(gameObject);
        }
    }
}
