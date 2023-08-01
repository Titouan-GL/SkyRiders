using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UtilitiesStatic : MonoBehaviour
{
    static public void Shake(){
        Camera.main.gameObject.GetComponent<CameraScript>().Shake();
    }

    static public LayerMask GetLevelLayerMask(){
        int layerMask = 1 << 12 | 1 << 8;
        return layerMask;
    }

    static public UtilitiesNonStatic GetUNS(){
        return GameObject.FindGameObjectWithTag("UNS").GetComponent<UtilitiesNonStatic>();
    }
}
