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
        Debug.Log("input action detected");

        // Add actions with multiple bindings: one for space bar, one for the VR controller X button
        fireAction = actionMap.AddAction("Fire");
        fireAction.AddBinding("<Keyboard>/space");
        fireAction.AddBinding("<OculusTouchController>{LeftHand}/buttonWest"); // X button on Oculus controllers

        // Subscribe to the action
        fireAction.performed += ctx =>
        {
            Debug.Log("Fire action performed!");
            TriggerParticleEffect();
        };

        // Enable the action map
        actionMap.Enable();
        Debug.Log("Input action map enabled.");
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
        Debug.Log("TriggerParticleEffect called.");

        if (!superPowerUnlocked)
        {
            Debug.LogWarning("Attempted to fire particles, but super power is not unlocked.");
            return; // Ensure power is only activated if unlocked
        }

        if (killParticleSystem != null)
        {
            killParticleSystem.Clear();
            killParticleSystem.transform.position = transform.position;
            killParticleSystem.transform.rotation = transform.rotation;
            killParticleSystem.Play();
            Debug.Log("Playing particle system at position: " + transform.position);

            if (!killParticleSystem.isPlaying)
            {
                Debug.LogError("Particle System failed to play.");
            }
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