using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatureFlyOut : MonoBehaviour
{
    public AnimationCurve flightCurve;
    private Vector3 targetPosition;
    private float animationDuration = 3.0f;
    private float elapsedTime = 0.0f;
    private Vector3 startPosition;
    private bool initialized = false;

    public void Initialize(Vector3 portalCenter, float portalSize)
    {
        startPosition = portalCenter + Random.insideUnitSphere * (portalSize / 2);
        transform.position = startPosition;

        //calculate target position
        targetPosition = startPosition + new Vector3(Random.Range(-5.0f, 5.0f), Mathf.Max(Random.Range(1.0f, 3.0f),(startPosition.y-10)), Random.Range(-5.0f, 5.0f));

        elapsedTime = 0.0f;
        initialized = true;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        elapsedTime += Time.deltaTime;
        float t = elapsedTime / animationDuration;

        if (t < 1.0f)
        {
            float curveValue = flightCurve.Evaluate(t);
            transform.position = Vector3.Lerp(startPosition, targetPosition, t);
        }
        else
        {
            enabled = false;
        }
    }
}
