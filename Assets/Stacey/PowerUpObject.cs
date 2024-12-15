using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpObject : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Ensure the collided object is the player
        {
            ActivatePowerUp();
            Destroy(gameObject); // Optionally destroy the power-up object after collision
        }
    }

#if UNITY_EDITOR
    private void Update()
    {
        // Simulate pressing the X button in the Editor (can trigger the power-up for testing)
        if (Input.GetKeyDown(KeyCode.X))
        {
            Debug.Log("Simulating X button press in Editor for power-up activation.");
            ActivatePowerUp();
        }
    }
#endif

    private void ActivatePowerUp()
    {
        // Enable superpower feature
        var killParticleTrigger = FindObjectOfType<KillParticleTrigger>();
        if (killParticleTrigger != null)
        {
            killParticleTrigger.EnableSuperPower();
        }

        // Show the power-up UI message
        var UIManager = FindObjectOfType<UIManager>();
        if (UIManager != null)
        {
            UIManager.ShowPowerUpMessage();
        }
    }
}
