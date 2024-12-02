using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LatchMonster : MonoBehaviour
{
   
    public GameObject hiddenAsset; // Assign your UI text object in the editor
    public GameObject particleEffectPrefab; // Assign your particle effect prefab in the editor
    public Transform metaXRCameraTransform; // Assign the XR camera transform in the editor
    public GameObject mainAsset; // This script should be attached to this asset

    private GameObject duplicatedModel;
    private bool isAssetVisible = false;

    private void Start()
    {
        if (hiddenAsset != null)
        {
            hiddenAsset.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Output name to see what triggers the interaction
        Debug.Log("Trigger entered by: " + other.gameObject.name);

        // Verify collision with camera Transform
        if (metaXRCameraTransform != null && other.transform == metaXRCameraTransform)
        {
            Debug.Log("Camera proxy collided, processing logic...");
            ShowHiddenAsset();
            TriggerParticleEffect();
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
    }

    private void TriggerParticleEffect()
    {
        if (particleEffectPrefab != null)
        {
            Instantiate(particleEffectPrefab, transform.position, Quaternion.identity);
            Debug.Log("Particle effect triggered.");
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
    }
}