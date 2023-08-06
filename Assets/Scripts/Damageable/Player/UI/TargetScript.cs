using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetScript : MonoBehaviour
{
    private List<RectTransform> target = new List<RectTransform>();
    [HideInInspector] public List<Vector2> originalPositions;

    void Start()
    {
        TransformIntoList(GetComponentsInChildren<RectTransform>());
        foreach(RectTransform rt in target){
            originalPositions.Add(rt.anchoredPosition);
            rt.anchoredPosition *= 30;
        }
    }

    void TransformIntoList(RectTransform[] array){
        foreach(RectTransform transform in array){
            target.Add(transform);
        }
    }

    public void TargetFocus(){
        for(int i = 1; i < target.Count; i ++){
            target[i].anchoredPosition = Vector2.Lerp(target[i].anchoredPosition, originalPositions[i], 0.3f);
        }
    }

    public void TargetUnFocus(){
        for(int i = 1; i < target.Count; i ++){
            target[i].anchoredPosition = Vector2.Lerp(target[i].anchoredPosition, originalPositions[i]*30, 0.2f);
        }
        target[0].anchoredPosition = Vector2.Lerp(target[0].anchoredPosition, originalPositions[0], 0.3f);
    }
}
