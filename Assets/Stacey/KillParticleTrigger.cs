using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class KillParticleTrigger : MonoBehaviour
{
    public ParticleSystem killparticleSystem;
    private InputDevice leftController;
    private bool isPrimaryButtonPressed = false;

    void Start()
    {
        // Initialize the controller (left hand in this setup)
        var leftHandDevices = new List<InputDevice>();
        InputDevices.GetDevicesAtXRNode(XRNode.LeftHand, leftHandDevices);

        if (leftHandDevices.Count > 0)
        {
            leftController = leftHandDevices[0];
        }
    }

    void Update()
    {
        // Check if the device is valid and check button press state
        if (leftController.isValid)
        {
            bool primaryButtonState = false;

            // This checks if the primary button (X button for left hand) is pressed
            if (leftController.TryGetFeatureValue(CommonUsages.primaryButton, out primaryButtonState))
            {
                // Trigger the particle system when the X button is pressed
                if (primaryButtonState && !isPrimaryButtonPressed)
                {
                    // Update the particle system's position to match the controller and play it
                    killparticleSystem.transform.position = transform.position;
                    killparticleSystem.transform.rotation = transform.rotation;
                    killparticleSystem.Play();
                }

                // Update the button press state
                isPrimaryButtonPressed = primaryButtonState;
            }
        }
    }
}



 
