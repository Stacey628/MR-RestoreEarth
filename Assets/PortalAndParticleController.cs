using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR;
using System.Linq;

public class PortalAndParticleController : MonoBehaviour
{
    [Header("Portal Settings")]
    public float scaleFactor = 0.1f;
    public Vector3 maxScale = new Vector3(5f, 0.1f, 5f);
    public Vector3 minScale = new Vector3(0.5f, 0.1f, 0.5f);
    public Transform playerTransform;
    public float triggerDistance = 1.5f;
    public string sceneName = "universe";

    [Header("Particle Settings")]
    public ParticleSystem particle1; // For RightHand primaryButton (A)
    public ParticleSystem particle2; // For RightHand secondaryButton (B)
    public ParticleSystem particle3; // For LeftHand primaryButton (X)
    public ParticleSystem particle4; // For LeftHand secondaryButton (Y)

    [Header("Prefab Settings")]
    public List<GameObject> affectedPrefabsA; // For RightHand primaryButton (A)
    public List<GameObject> affectedPrefabsB; // For RightHand secondaryButton (B)
    public List<GameObject> affectedPrefabsX; // For LeftHand primaryButton (X)
    public List<GameObject> affectedPrefabsY; // For LeftHand secondaryButton (Y)

    // Button state cache
    private bool leftPrimaryButtonLastState = false;
    private bool leftSecondaryButtonLastState = false;
    private bool rightPrimaryButtonLastState = false;
    private bool rightSecondaryButtonLastState = false;

    void Update()
    {
        DetectButtonPresses();
        CheckPortalProximity();
    }

    private void DetectButtonPresses()
    {
        UnityEngine.XR.InputDevice leftHandDevice, rightHandDevice;
        var devices = new List<UnityEngine.XR.InputDevice>();
        InputDevices.GetDevicesAtXRNode(XRNode.LeftHand, devices);
        leftHandDevice = devices.FirstOrDefault();
        InputDevices.GetDevicesAtXRNode(XRNode.RightHand, devices);
        rightHandDevice = devices.FirstOrDefault();

        // Left Hand Button Detection
        if (leftHandDevice != null)
        {
            if (leftHandDevice.TryGetFeatureValue(UnityEngine.XR.CommonUsages.primaryButton, out bool primaryButtonValue))
            {
                if (primaryButtonValue && !leftPrimaryButtonLastState)
                {
                    OnButtonPressed(particle3);
                    ApplyRandomEffectToPrefabs(affectedPrefabsX); // X button
                }
                leftPrimaryButtonLastState = primaryButtonValue;
            }

            if (leftHandDevice.TryGetFeatureValue(UnityEngine.XR.CommonUsages.secondaryButton, out bool secondaryButtonValue))
            {
                if (secondaryButtonValue && !leftSecondaryButtonLastState)
                {
                    OnButtonPressed(particle4);
                    ApplyRandomEffectToPrefabs(affectedPrefabsY); // Y button
                }
                leftSecondaryButtonLastState = secondaryButtonValue;
            }
        }

        // Right Hand Button Detection
        if (rightHandDevice != null)
        {
            if (rightHandDevice.TryGetFeatureValue(UnityEngine.XR.CommonUsages.primaryButton, out bool primaryButtonValue))
            {
                if (primaryButtonValue && !rightPrimaryButtonLastState)
                {
                    OnButtonPressed(particle1);
                    ApplyRandomEffectToPrefabs(affectedPrefabsA); // A button
                }
                rightPrimaryButtonLastState = primaryButtonValue;
            }

            if (rightHandDevice.TryGetFeatureValue(UnityEngine.XR.CommonUsages.secondaryButton, out bool secondaryButtonValue))
            {
                if (secondaryButtonValue && !rightSecondaryButtonLastState)
                {
                    OnButtonPressed(particle2);
                    ApplyRandomEffectToPrefabs(affectedPrefabsB); // B button
                }
                rightSecondaryButtonLastState = secondaryButtonValue;
            }
        }
    }

    private void OnButtonPressed(ParticleSystem particle)
    {
        if (particle != null)
        {
            particle.Play();
        }
        else
        {
            Debug.LogWarning("Particle system not assigned!");
        }

        ScalePortal(scaleFactor);
    }

    private void ApplyRandomEffectToPrefabs(List<GameObject> prefabs)
    {
        // Apply random effect to 3D prefabs
        foreach (var prefab in prefabs)
        {
            if (prefab != null)
            {
                int outcome = Random.Range(0, 3); // 0 = disappear, 1 = duplicate, 2 = scale and rotate

                switch (outcome)
                {
                    case 0:
                        // Disappear
                        Destroy(prefab);
                        Debug.Log("Destroyed: " + prefab.name);
                        break;
                    case 1:
                        // Duplicate
                        Instantiate(prefab, prefab.transform.position + Vector3.right * 1.5f, Quaternion.identity);
                        Debug.Log("Duplicated: " + prefab.name);
                        break;
                    case 2:
                        // Scale up and rotate
                        prefab.transform.localScale *= 1.5f;
                        prefab.transform.Rotate(Vector3.up, 180);
                        Debug.Log("Scaled and Rotated: " + prefab.name);
                        break;
                }
            }
        }
    }

    private void ScalePortal(float amount)
    {
        Vector3 currentScale = transform.localScale;
        Vector3 newScale = currentScale + new Vector3(amount, amount, amount);
        newScale = Vector3.Max(minScale, Vector3.Min(newScale, maxScale));
        transform.localScale = newScale;

        if (playerTransform != null)
        {
            Vector3 directionToPlayer = (playerTransform.position - transform.position).normalized;
            transform.position += directionToPlayer * amount * 0.01f;
        }
    }

    private void CheckPortalProximity()
    {
        if (playerTransform == null)
        {
            Debug.LogWarning("Player Transform is not assigned.");
            return;
        }

        float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);

        if (distanceToPlayer <= triggerDistance)
        {
            Debug.Log("Portal is close to the player. Loading scene: " + sceneName);
            SceneManager.LoadScene(sceneName);
        }
    }
}