using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DuplicateParameciumSingleScene : MonoBehaviour
{
    public GameObject parameciumPrefab; // Assign the Paramecium prefab in the Inspector
    public Transform portalTransform;  // Assign the portal Transform in the Inspector
    public float spawnRange = 5f;      // Range around the portal to spawn objects

    private float lastPortalSize;                      // Store the last known portal size
    private List<GameObject> parameciumObjects = new List<GameObject>(); // Track all duplicated objects
    private bool portalEnlarged = false;              // Flag to check if the portal size has increased
    private Coroutine decreaseCoroutine;              // Store the ongoing decrease coroutine

    void Start()
    {
        Debug.Log("DuplicateParameciumSingleScene started.");

        // Ensure references are assigned in the scene
        if (parameciumPrefab == null || portalTransform == null)
        {
            Debug.LogError("Missing references! Please assign the Paramecium prefab and Portal Transform in the Inspector.");
            return;
        }

        // Perform the initial duplication
        lastPortalSize = portalTransform.localScale.x; // Initialize the portal size
        DuplicateParameciumObjects(5); // Start with 5 duplicates
    }

    void Update()
    {
        // Get the current portal size
        float currentPortalSize = portalTransform.localScale.x;

        if (currentPortalSize > lastPortalSize)
        {
            // Portal size has increased: Gradually reduce objects based on speed
            float portalSpeed = currentPortalSize - lastPortalSize; // Calculate the portal size change rate
            Debug.Log($"Portal size has increased. Speed: {portalSpeed}");

            if (!portalEnlarged)
            {
                portalEnlarged = true; // Set the flag to prevent further duplication
                if (decreaseCoroutine != null)
                {
                    StopCoroutine(decreaseCoroutine); // Stop any existing coroutine
                }
                decreaseCoroutine = StartCoroutine(GraduallyReduceParameciumObjects(portalSpeed));
            }
        }

        // Update the last known portal size
        lastPortalSize = currentPortalSize;
    }

    private void DuplicateParameciumObjects(int count)
    {
        Debug.Log($"Duplicating {count} objects...");
        for (int i = 0; i < count; i++)
        {
            // Generate a random position around the portal
            Vector3 randomPosition = portalTransform.position + new Vector3(
                Random.Range(-spawnRange, spawnRange), // X-axis offset
                Random.Range(-spawnRange, spawnRange), // Y-axis offset
                Random.Range(-spawnRange, spawnRange)  // Z-axis offset
            );

            // Instantiate a duplicate of the Paramecium prefab
            GameObject newParamecium = Instantiate(parameciumPrefab, randomPosition, Quaternion.identity);
            parameciumObjects.Add(newParamecium); // Add to the list of duplicates

            newParamecium.name = $"Paramecium_{parameciumObjects.Count}"; // Assign a unique name
            Debug.Log($"Spawned {newParamecium.name} at {randomPosition}");
        }
    }

    private IEnumerator GraduallyReduceParameciumObjects(float portalSpeed)
    {
        // Map portalSpeed to a delay time (e.g., faster portal = faster removal)
        float delayTime = Mathf.Clamp(1f / portalSpeed, 0.1f, 2f); // Inverse relation between speed and delay
        Debug.Log($"Gradual reduction started with delay time: {delayTime}");

        while (parameciumObjects.Count > 1)
        {
            // Remove the last object in the list
            GameObject parameciumToRemove = parameciumObjects[parameciumObjects.Count - 1];
            parameciumObjects.Remove(parameciumToRemove);
            Destroy(parameciumToRemove);

            Debug.Log($"Removed {parameciumToRemove.name}. Remaining objects: {parameciumObjects.Count}");

            yield return new WaitForSeconds(delayTime); // Wait based on the portal speed
        }

        Debug.Log("Reduction complete. Only one object remains.");
    }
}
