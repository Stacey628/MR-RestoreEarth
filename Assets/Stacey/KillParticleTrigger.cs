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
        var leftHandDevices = new List<InputDevice>();
        InputDevices.GetDevicesAtXRNode(XRNode.LeftHand, leftHandDevices);

        if (leftHandDevices.Count > 0)
        {
            leftController = leftHandDevices[0];
        }
    }

    public void EnableSuperPower()
    {
        superPowerUnlocked = true;
        DisplayUIMessage(); // Show the UI message
    }

    void Update()
    {
        if (!superPowerUnlocked) return; // Exit if power not unlocked

        if (leftController.isValid)
        {
            bool primaryButtonState = false;

            if (leftController.TryGetFeatureValue(CommonUsages.primaryButton, out primaryButtonState))
            {
                if (primaryButtonState && !isPrimaryButtonPressed)
                {
                    killparticleSystem.transform.position = transform.position;
                    killparticleSystem.transform.rotation = transform.rotation;
                    killparticleSystem.Play();
                }

                isPrimaryButtonPressed = primaryButtonState;
            }
        }
    }

    private void DisplayUIMessage()
    {
        UIManager uiController = FindObjectOfType<UIManager>();
        if (uiController != null)
        {
            uiController.ShowPowerUpMessage();
        }
    }

}




 
