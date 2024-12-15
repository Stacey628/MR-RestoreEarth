using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatureFlyOut : MonoBehaviour
{
    public AnimationCurve flightCurve;
    private Vector3 startPosition;
    private Vector3 targetPosition;
    private float animationDuration = 3.0f;
    private float elapsedTime = 0.0f;
    private float maxHeight = 5.0f;
    private bool initialized = false;
    private Rigidbody rb;

    public void Initialize(Vector3 portalCenter, float portalSize)
    {
        startPosition = portalCenter + Random.insideUnitSphere * (portalSize / 2);
        startPosition.y = Mathf.Max(startPosition.y, portalCenter.y);

        transform.position = startPosition;

        //calculate target position
        Vector3 randomDirection = Random.onUnitSphere.normalized;
        targetPosition = portalCenter + Random.onUnitSphere * (portalSize *5f);
        targetPosition.y = Mathf.Max(targetPosition.y, portalCenter.y);

        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            rb.useGravity = false;
        }

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
        if (!initialized) return;
        elapsedTime += Time.deltaTime;
        float t = Mathf.Clamp01(elapsedTime / animationDuration);

        Vector3 horizontalPosition = Vector3.Lerp(startPosition, targetPosition, t);

        float heightFactor = 1 - Mathf.Pow((t-0.5f)/0.5f, 2);
        float verticalPositionY = Mathf.Lerp(startPosition.y, targetPosition.y, t) + maxHeight * heightFactor;

        transform.position = new Vector3(horizontalPosition.x, verticalPositionY, horizontalPosition.z);

        if (t < 1.0f)
        {
            transform.position = Vector3.Lerp(startPosition, targetPosition, t);
        }
        else
        {
            if (rb != null)
            {
                rb.useGravity = true;
            }
            enabled = false;
        }
    }
}
