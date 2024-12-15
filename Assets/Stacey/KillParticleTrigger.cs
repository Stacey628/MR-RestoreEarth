using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class KillParticleTrigger : MonoBehaviour
{
    public ParticleSystem killParticleSystem;
    private InputActionMap actionMap;
    private InputAction fireAction;
    private bool superPowerUnlocked = false; // Track whether the power is available

    private void Awake()
    {
        // Create an action map
        actionMap = new InputActionMap("InputActions");

        // Add actions with multiple bindings: one for space bar, one for the VR controller X button
        fireAction = actionMap.AddAction("Fire");
        fireAction.AddBinding("<Keyboard>/space");
        fireAction.AddBinding("<XRController>{LeftHand}/buttonNorth"); // X button on Oculus controllers

        // Subscribe to the action
        fireAction.performed += ctx => TriggerParticleEffect();

        // Enable the action map
        actionMap.Enable();
    }

    private void OnDestroy()
    {
        // Unsubscribe to prevent memory leaks
        fireAction.performed -= ctx => TriggerParticleEffect();
    }

    public void EnableSuperPower()
    {
        superPowerUnlocked = true;
        Debug.Log("Super power enabled.");
        DisplayUIMessage();
    }

    private void TriggerParticleEffect()
    {
        if (!superPowerUnlocked) return; // Ensure power is only activated if unlocked

        if (killParticleSystem != null)
        {
            killParticleSystem.transform.position = transform.position;
            killParticleSystem.transform.rotation = transform.rotation;
            Debug.Log("Playing particle system at position: " + transform.position);
            killParticleSystem.Play();
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