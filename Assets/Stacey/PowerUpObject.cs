using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpObject : MonoBehaviour
{
   
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Ensure the collided object is the player
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

            Destroy(gameObject); // Optionally destroy the power-up object after collision
        }
    }
}


