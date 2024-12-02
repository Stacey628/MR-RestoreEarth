using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LatchMonster : MonoBehaviour
{
   
    public GameObject hiddenAsset;
    public GameObject particleEffectPrefab; 
    public Transform metaXRCameraTransform; 
    public GameObject mainAsset; 

    public Vector3 offset = new Vector3(0, -1.5f, 0); 

    private GameObject duplicatedModel;
    private ParticleSystem particleSystemInstance;

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

        if (particleEffectPrefab == null)
        {
            Debug.LogError("Particle effect prefab is not assigned!");
        }
        else
        {
           
            GameObject instance = Instantiate(particleEffectPrefab);
            particleSystemInstance = instance.GetComponent<ParticleSystem>();
            if (particleSystemInstance != null)
            {
                particleSystemInstance.Stop();
                instance.SetActive(false); 
            }
            else
            {
                Debug.LogError("No ParticleSystem component found on the particleEffectPrefab!");
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (metaXRCameraTransform != null && other.transform == metaXRCameraTransform)
        {
            if (hiddenAsset && !hiddenAsset.activeInHierarchy)
            {
                Debug.Log("Camera proxy collided, processing logic...");
                hiddenAsset.SetActive(true);
                PlayParticleEffect(); 
                CloneMainAssetToCamera(mainAsset);

                Invoke("HideHiddenAsset", 5f);
                Debug.Log("Disabling LatchMonster script after execution.");
                this.enabled = false; 
            }
        }
    }

    private void HideHiddenAsset()
    {
        if (hiddenAsset != null)
        {
            hiddenAsset.SetActive(false); 
            Debug.Log("Hidden asset is now hidden.");
        }
    }

    private void PlayParticleEffect()
    {
        if (particleSystemInstance != null)
        {
            particleSystemInstance.gameObject.SetActive(true); 
            particleSystemInstance.Play();                     
            Debug.Log("Particle effect activated.");
        }
    }

    private void CloneMainAssetToCamera(GameObject originalObject)
    {
        if (metaXRCameraTransform != null && originalObject != null && duplicatedModel == null)
        {
            duplicatedModel = Instantiate(originalObject, metaXRCameraTransform);
            duplicatedModel.transform.localPosition = offset;
            duplicatedModel.transform.localRotation = Quaternion.identity;

            Debug.Log("Main asset cloned and attached to camera with offset.");
        }
        else
        {
            Debug.LogError("Invalid setup or main asset already cloned.");
        }
    }
}


