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

    [Header("Sound Settings")]
    public AudioClip soundA;
    public AudioClip soundB;
    public AudioClip soundX;
    public AudioClip soundY;
    private AudioSource audioSourceA;
    private AudioSource audioSourceB;
    private AudioSource audioSourceX;
    private AudioSource audioSourceY;

    [Header("Prefab Settings")]
    public List<GameObject> affectedPrefabsA; // For RightHand primaryButton (A)
    public List<GameObject> affectedPrefabsB; // For RightHand secondaryButton (B)
    public List<GameObject> affectedPrefabsX; // For LeftHand primaryButton (X)
    public List<GameObject> affectedPrefabsY; // For LeftHand secondaryButton (Y)

    private bool leftPrimaryButtonLastState = false;
    private bool leftSecondaryButtonLastState = false;
    private bool rightPrimaryButtonLastState = false;
    private bool rightSecondaryButtonLastState = false;

    void Start()
    {
        // Add separate AudioSources
        audioSourceA = gameObject.AddComponent<AudioSource>();
        audioSourceB = gameObject.AddComponent<AudioSource>();
        audioSourceX = gameObject.AddComponent<AudioSource>();
        audioSourceY = gameObject.AddComponent<AudioSource>();
    }

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
                    OnButtonPressed(particle3, audioSourceX, soundX);
                    ApplyRandomEffectToPrefabs(affectedPrefabsX); // X button
                }
                leftPrimaryButtonLastState = primaryButtonValue;
            }

            if (leftHandDevice.TryGetFeatureValue(UnityEngine.XR.CommonUsages.secondaryButton, out bool secondaryButtonValue))
            {
                if (secondaryButtonValue && !leftSecondaryButtonLastState)
                {
                    OnButtonPressed(particle4, audioSourceY, soundY);
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
                    OnButtonPressed(particle1, audioSourceA, soundA);
                    ApplyRandomEffectToPrefabs(affectedPrefabsA); // A button
                }
                rightPrimaryButtonLastState = primaryButtonValue;
            }

            if (rightHandDevice.TryGetFeatureValue(UnityEngine.XR.CommonUsages.secondaryButton, out bool secondaryButtonValue))
            {
                if (secondaryButtonValue && !rightSecondaryButtonLastState)
                {
                    OnButtonPressed(particle2, audioSourceB, soundB);
                    ApplyRandomEffectToPrefabs(affectedPrefabsB); // B button
                }
                rightSecondaryButtonLastState = secondaryButtonValue;
            }
        }
    }

    private void OnButtonPressed(ParticleSystem particle, AudioSource audioSource, AudioClip sound)
    {
        if (particle != null)
        {
            particle.Play();
        }
        else
        {
            Debug.LogWarning("Particle system not assigned!");
        }

        if (audioSource != null && sound != null)
        {
            audioSource.PlayOneShot(sound);
        }
        else
        {
            Debug.LogWarning("AudioSource or Sound clip not assigned!");
        }

        ScalePortal(scaleFactor);
    }

    private void ApplyRandomEffectToPrefabs(List<GameObject> prefabs)
    {
        foreach (var prefab in prefabs)
        {
            if (prefab != null)
            {
                int outcome = Random.Range(0, 3);

                switch (outcome)
                {
                    case 0:
                        Destroy(prefab);
                        Debug.Log("Destroyed: " + prefab.name);
                        break;
                    case 1:
                        Instantiate(prefab, prefab.transform.position + Vector3.right * 2f, Quaternion.identity);
                        Debug.Log("Duplicated: " + prefab.name);
                        break;
                    case 2:
                        prefab.transform.localScale *= 2f;
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