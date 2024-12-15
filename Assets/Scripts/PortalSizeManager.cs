using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class PortalSizeManager : MonoBehaviour
{
    public float minPortalSize = 0.0f;      // Minimum size of the portal
    public float maxPortalSize = 5.0f;      // Maximum size of the portal
    public float waitDuration = 15.0f;     // Time to wait before expansion starts
    public float expansionDuration = 20.0f; // Time taken to expand to max size

    private float elapsedTime = 0.0f;      // Tracks elapsed time

    public GameObject portalObject;        // Reference to the portal GameObject
    public Transform portalCenter;         // Center point of the portal

    // Public property to expose the current portal size
    public float portalSize
    {
        get
        {
            if (portalObject != null)
            {
                return portalObject.transform.localScale.x;
            }
            return 0f;
        }
    }

    void Start()
    {
        UpdatePortalSize(minPortalSize); // Set initial portal size
    }

    void Update()
    {
        elapsedTime += Time.deltaTime;

        if (elapsedTime > waitDuration && elapsedTime <= waitDuration + expansionDuration)
        {
            // Normalize time for expansion
            float t = (elapsedTime - waitDuration) / expansionDuration;
            float currentSize = Mathf.Lerp(minPortalSize, maxPortalSize, t);

            // Ensure portal size does not exceed maxPortalSize
            if (currentSize > maxPortalSize)
            {
                currentSize = maxPortalSize;
            }

            // Update portal size during expansion
            UpdatePortalSize(currentSize);
        }
        else if (elapsedTime > waitDuration + expansionDuration)
        {
            // After expansion, keep the portal at its maximum size
            UpdatePortalSize(maxPortalSize);
        }
    }

    private void UpdatePortalSize(float size)
    {
        if (portalObject != null)
        {
            Vector3 newScale = new Vector3(size, size, portalObject.transform.localScale.z);
            portalObject.transform.localScale = newScale;
        }
    }

    public Vector3 CalculateCreatureStartPosition()
    {
        float currentSize = portalSize;
        // Generate a random position within the portal's bounds
        Vector3 randomPosition = Random.insideUnitSphere * (currentSize / 2);
        randomPosition.z = 0; // Flatten on the Z-axis if the portal is a 2D plane
        return portalCenter.position + randomPosition;
    }

    // Debug visualization in the editor
    private void OnDrawGizmos()
    {
        if (portalCenter != null)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(portalCenter.position, portalSize / 2);
        }
    }
}

#if UNITY_EDITOR
// Custom Editor to display runtime portal size in the inspector
[CustomEditor(typeof(PortalSizeManager))]
public class PortalSizeManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        PortalSizeManager manager = (PortalSizeManager)target;
        EditorGUILayout.LabelField("Current Portal Size", manager.portalSize.ToString("F2"));
    }
}
#endif
