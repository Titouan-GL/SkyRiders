using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlastWave : MonoBehaviour
{
    private LineRenderer lineRenderer;
    public float maxRadius;
    public float speed;
    public int pointsCount;
    public float startWidth;

    // Start is called before the first frame update
    void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = pointsCount + 1;
    }

    private IEnumerator Blast(){
        float currentRadius = 0f;

        while(currentRadius < maxRadius){
            currentRadius += Time.deltaTime * speed;
            Draw(currentRadius);
            yield return null;
        }
    }

    private void Draw(float currentRadius){
        float angleBetweenPoints = 360f/pointsCount;

        for(int i = 0; i <= pointsCount; i++){
            float angle = i * angleBetweenPoints * Mathf.Deg2Rad;
            Vector3 direction = new Vector3(0f, Mathf.Cos(angle), Mathf.Sin(angle));
            Vector3 position = direction * currentRadius;

            lineRenderer.SetPosition(i, position);
        }

        lineRenderer.widthMultiplier = Mathf.Lerp(0f, startWidth, 1f-currentRadius/maxRadius);
    }

    void OnEnable(){
        StartCoroutine(Blast());
    }
}
