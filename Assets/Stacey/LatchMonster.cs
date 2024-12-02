using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LatchMonster : MonoBehaviour
{
    public GameObject hiddenAsset;
    public GameObject particleEffectPrefab;
    public Transform metaXRCameraTransform; // Reference to the camera's transform
    public GameObject mainAsset; // Asset to be cloned to the camera

    public Vector3 offset = new Vector3(0, -1.5f, 0); // Adjust this to control how 'low' the asset appears

    private GameObject duplicatedModel;

    private void Start()
    {
        if (hiddenAsset != null)
        {
            hiddenAsset.SetActive(false); // Ensure the hidden asset is inactive at the start
            Debug.Log("Hidden asset set to inactive at Start.");
        }
        else
        {
            Debug.LogError("Hidden asset not assigned in Inspector!");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if the interacting object is the passthrough camera
        if (metaXRCameraTransform != null && other.transform == metaXRCameraTransform)
        {
            // Ensure the logic is executed only once
            if (hiddenAsset && !hiddenAsset.activeInHierarchy)
            {
                Debug.Log("Camera proxy collided, processing logic...");
                hiddenAsset.SetActive(true);
                TriggerParticleEffect();
                CloneMainAssetToCamera(mainAsset);

                // Allow hidden asset to disappear after 5 seconds
                Invoke("HideHiddenAsset", 5f);

                // Disable the script after execution
                Debug.Log("Disabling LatchMonster script after execution.");
                this.enabled = false;
            }
        }
    }

    private void HideHiddenAsset()
    {
        if (hiddenAsset != null)
        {
            hiddenAsset.SetActive(false); // Hide the hidden asset
            Debug.Log("Hidden asset is now hidden.");
        }

        // Do not destroy duplicatedModel here
    }

    private void TriggerParticleEffect()
    {
        if (particleEffectPrefab != null)
        {
            // Instantiate particles effect at the current position
            Instantiate(particleEffectPrefab, transform.position, Quaternion.identity);
            Debug.Log("Particle effect triggered.");
        }
        else
        {
            Debug.LogError("Particle effect prefab is null.");
        }
    }

    private void CloneMainAssetToCamera(GameObject originalObject)
    {
        if (metaXRCameraTransform != null && originalObject != null && duplicatedModel == null)
        {
            // Clone and attach mainAsset to the camera
            duplicatedModel = Instantiate(originalObject, metaXRCameraTransform);
            duplicatedModel.transform.localPosition = offset; // Apply offset to position the asset lower
            duplicatedModel.transform.localRotation = Quaternion.identity;

            Debug.Log("Main asset cloned and attached to camera with offset.");
        }
        else
        {
            Debug.LogError("Invalid setup or main asset already cloned.");
        }
    }
}
