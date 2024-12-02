using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LatchMonster : MonoBehaviour
{
    public GameObject hiddenAsset;
    public GameObject particleEffectPrefab;
    public Transform metaXRCameraTransform;
    public GameObject mainAsset;

    private GameObject duplicatedModel;
    private bool isAssetVisible = false;
    private bool particleEffectTriggered = false;

    private void Start()
    {
        if (hiddenAsset != null)
        {
            hiddenAsset.SetActive(false);
            Debug.Log("Hidden asset set to inactive at Start.");
        }
        else
        {
            Debug.LogError("Hidden asset not assigned in Inspector!");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Trigger entered by: " + other.gameObject.name);

        if (metaXRCameraTransform != null && other.transform == metaXRCameraTransform && !isAssetVisible)
        {
            Debug.Log("Camera proxy collided, processing logic...");
            ShowHiddenAsset();
            TriggerParticleEffect(); // Call to trigger the particle effect
            DuplicateAndAttachToCamera(mainAsset);
            Invoke("HideHiddenAsset", 5f);
        }
    }

    private void ShowHiddenAsset()
    {
        if (hiddenAsset != null)
        {
            hiddenAsset.SetActive(true);
            isAssetVisible = true;
            Debug.Log("Hidden asset is now visible.");
        }
        else
        {
            Debug.LogError("Hidden asset reference is null.");
        }
    }

    private void HideHiddenAsset()
    {
        if (hiddenAsset != null)
        {
            hiddenAsset.SetActive(false);
            isAssetVisible = false;
            Debug.Log("Hidden asset is now hidden.");
        }

        if (duplicatedModel != null)
        {
            Destroy(duplicatedModel);
            Debug.Log("Duplicated model destroyed.");
        }

        // Reset flag if we need further testing after hide
        particleEffectTriggered = false;
    }

    private void TriggerParticleEffect()
    {
        if (!particleEffectTriggered)
        {
            if (particleEffectPrefab != null)
            {
                // Instantiate particle effect with no parent to avoid unintended movement.
                var particleInstance = Instantiate(particleEffectPrefab, transform.position, Quaternion.identity);
                particleEffectTriggered = true;
                Debug.Log("Particle effect triggered.");

                // Optionally, clear the particle effect after some time to ensure it's not hanging in the scene forever.
                Destroy(particleInstance, 3f); // Assumes a lifetime where the particles finish their effect
            }
            else
            {
                Debug.LogError("Particle effect prefab is null.");
            }
        }
    }

    private void DuplicateAndAttachToCamera(GameObject originalObject)
    {
        if (metaXRCameraTransform != null && originalObject != null)
        {
            duplicatedModel = Instantiate(originalObject, metaXRCameraTransform);
            duplicatedModel.transform.localPosition = Vector3.zero;
            duplicatedModel.transform.localRotation = Quaternion.identity;

            Debug.Log("Duplicated model attached to camera.");
        }
        else
        {
            Debug.LogError("Invalid camera transform or original object.");
        }
    }
}