using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIScript : MonoBehaviour
{
    [SerializeField] private List<RectTransform> target;
    [HideInInspector] public List<Vector2> originalPositions;
    // Start is called before the first frame update
    void Start()
    {

        foreach(RectTransform rt in target){
            originalPositions.Add(rt.anchoredPosition);
            rt.anchoredPosition *= 30;
        }
    }


    public void UIAim(){
        for(int i = 0; i < target.Count; i ++){
            target[i].anchoredPosition = Vector2.Lerp(target[i].anchoredPosition, originalPositions[i], 0.3f);
        }
    }

    public void UINotAim(){
        for(int i = 0; i < target.Count; i ++){
            target[i].anchoredPosition = Vector2.Lerp(target[i].anchoredPosition, originalPositions[i]*30, 0.3f);
        }
    }
}
