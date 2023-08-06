using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIScript : MonoBehaviour
{
    [SerializeField] private TargetScript target;
    [SerializeField] private List<TargetScript> rocketTargets;
    [HideInInspector] public List<Vector2> originalPositions;

    [SerializeField] private RectTransform rechargeWeapon1;
    private List<GameObject> ammoWeapon1 = new List<GameObject>();
    [SerializeField] private RectTransform rechargeWeapon2;
    private List<GameObject> ammoWeapon2 = new List<GameObject>();
    [SerializeField] private Transform parentOfAmmo;
    [SerializeField] private float rechargeWidth;

    [SerializeField] private RectTransform fuelBar;
    [SerializeField] private float fuelWidth;

    [SerializeField] private RectTransform lifeBar;
    [SerializeField] private float lifeBarWidth;

    bool rocketFocused = false;

    private Canvas canvas;

    public void Awake(){
        canvas = GetComponent<Canvas>();
    }


    public void UpdateRecharges(float weapon1, float weapon2){
        rechargeWeapon1.sizeDelta = new Vector2(rechargeWidth * weapon1, rechargeWeapon1.sizeDelta.y);
        rechargeWeapon2.sizeDelta = new Vector2(rechargeWidth * weapon2, rechargeWeapon2.sizeDelta.y);
    }

    public void UpdateFuel(float fuel){
        fuelBar.sizeDelta = new Vector2(fuelWidth * fuel, fuelBar.sizeDelta.y);
    }

    public void UpdateLifeBar(float life){
        lifeBar.sizeDelta = new Vector2(lifeBarWidth * life, lifeBar.sizeDelta.y);
    }

    public void UpdateAmmo(int weapon1, int weapon2){
        for(int i = 0; i < weapon1; i ++){
            ammoWeapon1[i].SetActive(true);
        }
        for(int i = weapon1; i < ammoWeapon1.Count; i ++){
            ammoWeapon1[i].SetActive(false);
        }
        for(int i = 0; i < weapon2; i ++){
            ammoWeapon2[i].SetActive(true);
        }
        for(int i = weapon2; i < ammoWeapon2.Count; i ++){
            ammoWeapon2[i].SetActive(false);
        }
    }


    public void InitializeAmmo(float number, int weapon, GameObject instantiation){
        for(int i = 0; i < number; i ++){
            GameObject go = Instantiate(instantiation, Vector3.zero, Quaternion.identity, parentOfAmmo);
            RectTransform rt = go.GetComponent<RectTransform>();
            rt.anchoredPosition = new Vector2((rt.sizeDelta.x/2) + i * (rt.sizeDelta.x + 10), -10 -(60 * weapon));
            if(weapon == 0){
                ammoWeapon1.Add(go);
            }
            else{
                ammoWeapon2.Add(go);
            }
        }
    }

    public void UpdateRocketTargetPosition(int index, Vector3 viewportPosition){       
        RectTransform rectTransform = canvas.GetComponent<RectTransform>(); 
        float screenWidth = rectTransform.rect.width;
        float screenHeight = rectTransform.rect.height;

        // Convert viewport position to anchored position relative to the center of the screen
        Vector2 uiPosition = new Vector2(viewportPosition.x * screenWidth - screenWidth * 0.5f, viewportPosition.y * screenHeight - screenHeight * 0.5f);
        rocketTargets[index].GetComponent<RectTransform>().anchoredPosition = uiPosition;
    }

    public void RocketAim(){
        foreach(TargetScript ts in rocketTargets){
            ts.TargetFocus();
        }
        rocketFocused = true;
    }

    public void RocketUnAim(){
        foreach(TargetScript ts in rocketTargets){
            ts.TargetUnFocus();
        }
    }


    public void UIAim(){
        target.TargetFocus();
    }

    public void UINotAim(){
        target.TargetUnFocus();
        RocketUnAim();
    }
}
