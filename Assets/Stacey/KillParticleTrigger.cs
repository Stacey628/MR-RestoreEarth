using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class KillParticleTrigger : MonoBehaviour
{
    public ParticleSystem killparticleSystem;
    private InputDevice leftController;
    private bool isPrimaryButtonPressed = false;
    private bool superPowerUnlocked = false; // Track whether the power is available

    void Start()
    {
        // Improved approach to finding and validating the left controller
        List<InputDevice> devices = new List<InputDevice>();
        InputDevices.GetDevicesWithCharacteristics(InputDeviceCharacteristics.Left | InputDeviceCharacteristics.Controller, devices);

        if (devices.Count > 0)
        {
            leftController = devices[0];
            Debug.Log("Left controller found: " + leftController.name);
        }
        else
        {
            Debug.LogWarning("Left controller not found.");
        }

        // Optionally log all available devices for further debugging
        foreach (var device in devices)
        {
            Debug.Log("Found device: " + device.name + " with characteristics: " + device.characteristics);
        }
    }

    public void EnableSuperPower()
    {
        superPowerUnlocked = true;
        Debug.Log("Super power enabled.");
        DisplayUIMessage(); // Show the UI message
    }

    void Update()
    {
#if UNITY_EDITOR
        // Simulate X button press in Unity Editor
        if (superPowerUnlocked && Input.GetKeyDown(KeyCode.X))
        {
            Debug.Log("Simulating X button press to shoot particles");
            TriggerParticleEffect();
        }
#endif

        // Ensure superpower is unlocked before performing actions
        if (!superPowerUnlocked) return;

        // Attempt to recover controller if it's not valid
        if (!leftController.isValid)
        {
            Debug.LogWarning("Left controller is not valid. Retrying device detection.");
            Start(); // Re-attempt to find left controller
            return; // Exit Update loop early if controller not found
        }

        if (leftController.isValid)
        {
            bool primaryButtonState = false;

            if (leftController.TryGetFeatureValue(CommonUsages.primaryButton, out primaryButtonState))
            {
                if (primaryButtonState && !isPrimaryButtonPressed)
                {
                    Debug.Log("Primary button pressed on left controller.");
                    TriggerParticleEffect();
                }

                isPrimaryButtonPressed = primaryButtonState;
            }
        }
    }

    private void TriggerParticleEffect()
    {
        if (killparticleSystem != null)
        {
            killparticleSystem.transform.position = transform.position;
            killparticleSystem.transform.rotation = transform.rotation;
            Debug.Log("Playing particle system at position: " + transform.position);
            killparticleSystem.Play();
        }
        else
        {
            Debug.LogError("Particle system not assigned to KillParticleTrigger.");
        }
    }

    private void DisplayUIMessage()
    {
        UIManager uiController = FindObjectOfType<UIManager>();
        if (uiController != null)
        {
            Debug.Log("Displaying UI message.");
            uiController.ShowPowerUpMessage();
        }
        else
        {
            Debug.LogWarning("UIManager not found.");
        }
    }
}


