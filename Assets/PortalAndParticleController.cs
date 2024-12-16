using System.Collections;
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

    void Start()
    {
        SetupParticleCollisions(particle1);
        SetupParticleCollisions(particle2);
        SetupParticleCollisions(particle3);
        SetupParticleCollisions(particle4);
    }

    void Update()
    {
        DetectButtonPresses();
        CheckPortalProximity();
    }

    private void SetupParticleCollisions(ParticleSystem ps)
    {
        if (ps == null) return;

        var collision = ps.collision;
        collision.enabled = true;
        collision.sendCollisionMessages = true;
    }

    private void DetectButtonPresses()
    {
        var devices = new List<UnityEngine.XR.InputDevice>();
        UnityEngine.XR.InputDevice leftHandDevice, rightHandDevice;

        // Retrieve Left Hand Controller
        InputDevices.GetDevicesAtXRNode(XRNode.LeftHand, devices);
        leftHandDevice = devices.FirstOrDefault();

        // Check buttons for the Left Hand Controller
        if (leftHandDevice != null)
        {
            if (leftHandDevice.TryGetFeatureValue(UnityEngine.XR.CommonUsages.primaryButton, out bool leftPrimaryButtonValue) && leftPrimaryButtonValue)
                OnButtonPressed(particle3); // X button assumed

            if (leftHandDevice.TryGetFeatureValue(UnityEngine.XR.CommonUsages.secondaryButton, out bool leftSecondaryButtonValue) && leftSecondaryButtonValue)
                OnButtonPressed(particle4); // Y button assumed
        }

        // Retrieve Right Hand Controller
        InputDevices.GetDevicesAtXRNode(XRNode.RightHand, devices);
        rightHandDevice = devices.FirstOrDefault();

        // Check buttons for the Right Hand Controller
        if (rightHandDevice != null)
        {
            if (rightHandDevice.TryGetFeatureValue(UnityEngine.XR.CommonUsages.primaryButton, out bool rightPrimaryButtonValue) && rightPrimaryButtonValue)
                OnButtonPressed(particle1); // A button assumed

            if (rightHandDevice.TryGetFeatureValue(UnityEngine.XR.CommonUsages.secondaryButton, out bool rightSecondaryButtonValue) && rightSecondaryButtonValue)
                OnButtonPressed(particle2); // B button assumed
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

    private void ScalePortal(float amount)
    {
        Vector3 currentScale = transform.localScale;
        Vector3 newScale = currentScale + new Vector3(amount, 0, amount);
        newScale = Vector3.Max(minScale, Vector3.Min(newScale, maxScale));
        transform.localScale = newScale;

        if (playerTransform != null)
        {
            Vector3 directionToPlayer = (playerTransform.position - transform.position).normalized;
            transform.position += directionToPlayer * amount * 0.1f;
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

    private void OnParticleCollision(GameObject other)
    {
        // Perform effect on collision
        int outcome = Random.Range(0, 3); // 0 = disappear, 1 = duplicate, 2 = scale and rotate

        switch (outcome)
        {
            case 0:
                // Disappear
                Destroy(other);
                break;
            case 1:
                // Duplicate
                Instantiate(other, other.transform.position + Vector3.right * 1.5f, Quaternion.identity);
                break;
            case 2:
                // Scale up and rotate
                other.transform.localScale *= 1.5f;
                other.transform.Rotate(Vector3.up, 180);
                break;
        }
    }
}