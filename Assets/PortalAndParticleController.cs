using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
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

    // 按键状态缓存
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

        // 左手按钮检测
        if (leftHandDevice != null)
        {
            bool primaryButtonValue;
            if (leftHandDevice.TryGetFeatureValue(UnityEngine.XR.CommonUsages.primaryButton, out primaryButtonValue))
            {
                if (primaryButtonValue && !leftPrimaryButtonLastState)
                {
                    OnButtonPressed(particle3); // X button
                }
                leftPrimaryButtonLastState = primaryButtonValue;
            }

            bool secondaryButtonValue;
            if (leftHandDevice.TryGetFeatureValue(UnityEngine.XR.CommonUsages.secondaryButton, out secondaryButtonValue))
            {
                if (secondaryButtonValue && !leftSecondaryButtonLastState)
                {
                    OnButtonPressed(particle4); // Y button
                }
                leftSecondaryButtonLastState = secondaryButtonValue;
            }
        }

        // 右手按钮检测
        if (rightHandDevice != null)
        {
            bool primaryButtonValue;
            if (rightHandDevice.TryGetFeatureValue(UnityEngine.XR.CommonUsages.primaryButton, out primaryButtonValue))
            {
                if (primaryButtonValue && !rightPrimaryButtonLastState)
                {
                    OnButtonPressed(particle1); // A button
                }
                rightPrimaryButtonLastState = primaryButtonValue;
            }

            bool secondaryButtonValue;
            if (rightHandDevice.TryGetFeatureValue(UnityEngine.XR.CommonUsages.secondaryButton, out secondaryButtonValue))
            {
                if (secondaryButtonValue && !rightSecondaryButtonLastState)
                {
                    OnButtonPressed(particle2); // B button
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

    private void ScalePortal(float amount)
    {
        Vector3 currentScale = transform.localScale;
        Vector3 newScale = currentScale + new Vector3(amount, amount, amount);
        newScale = Vector3.Max(minScale, Vector3.Min(newScale, maxScale));
        transform.localScale = newScale;
        Debug.Log($"Portal Scale: {newScale}");

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
