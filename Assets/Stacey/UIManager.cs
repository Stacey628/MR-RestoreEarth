using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UIManager : MonoBehaviour
{

    public GameObject startMenu;         // Start Menu panel
    public GameObject tutorialMenu;      // Tutorial panel
    public GameObject gameplayElements;  // Main 3D gameplay parent object

    public Button startButton;           // Start button reference
    public Button tutorialButton;        // Tutorial button reference
    public Button backButton;            // Back button reference
    public AudioSource audioSource;      // Audio source for button clicks

    public GameObject powerUpMessage;    // UI element for power-up message

    private void Start()
    {
        ShowStartMenu();  // Initialize with the start menu visible
        InitializeUIElements();
    }

    private void InitializeUIElements()
    {
        if (powerUpMessage != null)
        {
            powerUpMessage.SetActive(false);  // Ensure the power-up message is inactive initially
        }

        if (gameplayElements != null)
        {
            gameplayElements.SetActive(false);  // Ensure all 3D elements are hidden initially
        }
    }

    private void PlayButtonClickSound()
    {
        if (audioSource != null)
        {
            audioSource.Play();
        }
    }

    public void OnStartButtonClicked()
    {
        PlayButtonClickSound();
        SetMenuVisibility(false, false, true);  // Hide all UI, show gameplay elements

        SetButtonVisibility(startButton, false);
        SetButtonVisibility(tutorialButton, false);
    }

    public void OnTutorialButtonClicked()
    {
        PlayButtonClickSound();
        SetMenuVisibility(false, true, false);  // Show tutorial panel

        SetButtonVisibility(startButton, false);
        SetButtonVisibility(tutorialButton, false);
        SetButtonVisibility(backButton, true);  // Enable back button for navigation
    }

    public void OnBackButtonClicked()
    {
        PlayButtonClickSound();
        ShowStartMenu();  // Return to start and reset UI
    }

    public void ShowStartMenu()
    {
        SetMenuVisibility(true, false, false);  // Show start menu

        SetButtonVisibility(startButton, true);
        SetButtonVisibility(tutorialButton, true);
        SetButtonVisibility(backButton, false);

        if (gameplayElements != null)
        {
            gameplayElements.SetActive(false);  // Ensure 3D elements remain hidden until start is clicked
        }
    }

    private void SetMenuVisibility(bool showStart, bool showTutorial, bool showGameplay)
    {
        startMenu.SetActive(showStart);
        tutorialMenu.SetActive(showTutorial);
        gameplayElements.SetActive(showGameplay);
    }

    private void SetButtonVisibility(Button button, bool isVisible)
    {
        if (button != null)
        {
            button.gameObject.SetActive(isVisible);
        }
    }

    public void ShowPowerUpMessage()
    {
        if (powerUpMessage != null)
        {
            StartCoroutine(ShowMessageCoroutine());
        }
    }

    private IEnumerator ShowMessageCoroutine()
    {
        powerUpMessage.SetActive(true);
        yield return new WaitForSeconds(2.5f);  // Display the message for 2.5 seconds
        powerUpMessage.SetActive(false);
    }
}










