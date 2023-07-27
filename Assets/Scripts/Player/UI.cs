using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    private InputAndAnimations inputs;
    [SerializeField] private List<RectTransform> target;
    public List<Vector2> originalPositions;
    // Start is called before the first frame update
    void Start()
    {
        inputs = GetComponentInChildren<InputAndAnimations>();

        foreach(RectTransform rt in target){
            originalPositions.Add(rt.anchoredPosition);
            rt.anchoredPosition *= 30;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(inputs.aiming || inputs.flying){
            for(int i = 0; i < target.Count; i ++){
                target[i].anchoredPosition = Vector2.Lerp(target[i].anchoredPosition, originalPositions[i], 0.3f);
            }
        }
        else{
            for(int i = 0; i < target.Count; i ++){
                target[i].anchoredPosition = Vector2.Lerp(target[i].anchoredPosition, originalPositions[i]*30, 0.3f);
            }
        }
    }
}
