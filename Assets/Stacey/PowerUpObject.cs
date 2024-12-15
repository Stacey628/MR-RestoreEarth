using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PowerUpObject : MonoBehaviour
{
    private InputActionMap actionMap;
    private InputAction xButtonOrKeyAction;

    private void Awake()
    {
        // Create an action map
        actionMap = new InputActionMap("InputActions");

        // Add an action with multiple bindings: one for keyboard and one for the VR controller
        xButtonOrKeyAction = actionMap.AddAction("ActivatePower", binding: "<Keyboard>/x");

        // Additional binding for the X button on the left-hand VR controller
        xButtonOrKeyAction.AddBinding("<XRController>{LeftHand}/buttonNorth");

        // Subscribe to the action
        xButtonOrKeyAction.performed += ctx => ActivatePowerUp();

        // Enable the action map
        actionMap.Enable();
    }

    private void OnDestroy()
    {
        // Unsubscribe to prevent memory leaks
        xButtonOrKeyAction.performed -= ctx => ActivatePowerUp();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ActivatePowerUp();
            Destroy(gameObject);
        }
    }

    private void ActivatePowerUp()
    {
        // Enable superpower feature
        var killParticleTrigger = FindObjectOfType<KillParticleTrigger>();
        if (killParticleTrigger != null)
        {
            killParticleTrigger.EnableSuperPower();
        }

        // Show the power-up UI message
        var uiManager = FindObjectOfType<UIManager>();
        if (uiManager != null)
        {
            uiManager.ShowPowerUpMessage();
        }
    }
}