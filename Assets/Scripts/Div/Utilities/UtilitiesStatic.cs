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

    static public Vector3 LerpRotationVector3(Vector3 current, Vector3 target, float lerpValue){
        return current + (target - current) * lerpValue ;
    }

    static public float RotationClamped(float rotation){
        return rotation > 180 ? ((rotation%180)-180) * -1 : rotation;
    }

    static public Vector3 Vector3RotationClamped(Vector3 rotation){
        return new Vector3(RotationClamped(rotation.x), RotationClamped(rotation.y), RotationClamped(rotation.z));
    }
}
