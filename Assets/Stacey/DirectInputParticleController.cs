using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DirectInputParticleController : MonoBehaviour
{
    public ParticleSystem particle1; // Particle for right-hand A button
    public ParticleSystem particle2; // Particle for right-hand B button
    public ParticleSystem particle3; // Particle for left-hand X button

    // Define InputActions for both XR controller and keyboard testing
    private InputAction fireParticle1Action;
    private InputAction fireParticle2Action;
    private InputAction fireParticle3Action;

    void OnEnable()
    {
        // Initialize input actions for both the headset and editor testing
        InitializeInputActions();

        // Enable input actions
        fireParticle1Action.Enable();
        fireParticle2Action.Enable();
        fireParticle3Action.Enable();
    }

    void OnDisable()
    {
        // Disable input actions
        fireParticle1Action.Disable();
        fireParticle2Action.Disable();
        fireParticle3Action.Disable();
    }

    private void InitializeInputActions()
    {
        if (Application.isEditor)
        {
            // Keyboard bindings for testing in the Unity Editor
            fireParticle1Action = new InputAction("FireParticle1", binding: "<Keyboard>/j"); // J simulates RightHand A
            fireParticle2Action = new InputAction("FireParticle2", binding: "<Keyboard>/k"); // K simulates RightHand B
            fireParticle3Action = new InputAction("FireParticle3", binding: "<Keyboard>/l"); // L simulates LeftHand X
        }
        else
        {
            // XR controller bindings
            fireParticle1Action = new InputAction("FireParticle1", binding: "<XRController>{RightHand}/buttonSouth"); // A button
            fireParticle2Action = new InputAction("FireParticle2", binding: "<XRController>{RightHand}/buttonEast"); // B button
            fireParticle3Action = new InputAction("FireParticle3", binding: "<XRController>{LeftHand}/buttonSouth"); // X button
        }

        // Subscribe to input actions performing events
        fireParticle1Action.performed += ctx => PlayParticle(particle1);
        fireParticle2Action.performed += ctx => PlayParticle(particle2);
        fireParticle3Action.performed += ctx => PlayParticle(particle3);
    }

    private void PlayParticle(ParticleSystem particle)
    {
        if (particle != null)
        {
            particle.Play();
        }
        else
        {
            Debug.LogWarning("Particle system not assigned!");
        }
    }
}