using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalBehavior : MonoBehaviour
{
    public GameObject portalSphere;
    public Transform player;
    public float detectionRange = 5.0f;
    public float sphereSpeed = 10f;
    public float followDistance = 5.0f;

    private bool isSphereFlying = false;
    private Vector3 targetPosition;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float distanceToPlayer = Vector3.Distance(player.position, transform.position);

        if (distanceToPlayer <= detectionRange && !isSphereFlying)
        {
            isSphereFlying = true;
            targetPosition = player.position - player.forward * followDistance;
        }
        {
            if (isSphereFlying)
            {
                MoveSphere();
            }
        }
    }
    void MoveSphere()
    {
        portalSphere.transform.position = Vector3.MoveTowards(portalSphere.transform.position, targetPosition, sphereSpeed * Time.deltaTime);
    
        if (Vector3.Distance(portalSphere.transform.position, targetPosition) < 0.1f)
        {
            isSphereFlying = false;
        }
    }
}
