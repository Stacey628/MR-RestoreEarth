using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class EnlargeAbility2 : MonoBehaviour
{
    private InputActionMap actionMap;
    private InputAction bButtonOrKeyAction;

    [Header("Particle Effect")]
    public GameObject superPowerParticle; // 预制件：SuperPower_2 粒子效果

    private void Awake()
    {
        // Create an action map
        actionMap = new InputActionMap("InputActions");

        // Add an action with binding for the B button on the right-hand VR controller
        bButtonOrKeyAction = actionMap.AddAction("ActivatePower", binding: "<XRController>{RightHand}/buttonSouth");

        // Subscribe to the action
        bButtonOrKeyAction.performed += ctx => ActivatePowerUp();

        // Enable the action map
        actionMap.Enable();
    }

    private void OnDestroy()
    {
        // Unsubscribe to prevent memory leaks
        bButtonOrKeyAction.performed -= ctx => ActivatePowerUp();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ActivatePowerUp();
        }
    }

    private void ActivatePowerUp()
    {
        // Find the object clicked on by the player
        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            // Get the clicked object
            GameObject clickedObject = hit.collider.gameObject;

            // Spawn the particle effect at the hand's position
            Vector3 handPosition = OVRInput.GetLocalControllerPosition(OVRInput.Controller.RTouch);
            Quaternion handRotation = OVRInput.GetLocalControllerRotation(OVRInput.Controller.RTouch);
            if (superPowerParticle != null)
            {
                Instantiate(superPowerParticle, handPosition, handRotation);
            }

            // Scale up the clicked object
            clickedObject.transform.localScale *= 2f;
        }
    }
}

    