/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Combat : MonoBehaviour
{
    private InputAndAnimations inputs;
    [SerializeField] private Transform firePoint;
    [SerializeField] private List<Transform> firePointsPlane;
    [SerializeField] private List<GameObject> slashes;
    [SerializeField] private GameObject slashCollider;
    [SerializeField] private GameObject guardCollider;
    [SerializeField] private Weapon weaponMech1;
    [SerializeField] private Weapon weaponMech2;
    [SerializeField] private Weapon weaponPlane1;

    [SerializeField]private float life = 200;

    [SerializeField]public float knockbackTimeMax = 0.3f;
    public float knockbackTime = 0f;

    // Start is called before the first frame update
    void Start()
    {
        inputs = GetComponentInChildren<InputAndAnimations>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(guardCollider.GetComponent<Parry>().CanDeactivate() && !inputs.guard){
            guardCollider.SetActive(false);
        }
        else{
            guardCollider.SetActive(true);
        }
        if(inputs.firing && !inputs.flying){
            weaponMech1.OnHold();
        }
        else if(inputs.firing && inputs.flying){
            weaponPlane1.OnHold();
        }
        if(!inputs.aiming){
            weaponMech1.Reload();
        }
    }

    public void InstantiateSlash(int index){
        slashes[index].SetActive(false);
        slashes[index].SetActive(true);
        slashCollider.SetActive(false);
        slashCollider.SetActive(true);

    }

    public void takeDamage(float damagesDealt, bool knockback = false){
        life -= damagesDealt;
        if(knockback){
            inputs.TakeDamage();
            knockbackTime = knockbackTimeMax;
        }
    }

}
*/