using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UIManager : MonoBehaviour
{
 
    public GameObject startMenu;         // Start menu UI panel
    public GameObject tutorialMenu;      // Tutorial UI panel
    public GameObject gameplayElements;  // Main 3D gameplay elements

    public Button nextButton;            // "Next" button
    public AudioSource audioSource;      // Reference to the AudioSource
    public AudioClip buttonClickSound;   // The sound clip played for button clicks

    void Start()
    {
        ShowStartMenu();
    }

    private void PlayButtonClickSound()
    {
        if (audioSource != null && buttonClickSound != null)
        {
            audioSource.PlayOneShot(buttonClickSound);
        }
    }

    public void OnStartButtonClicked()
    {
        PlayButtonClickSound();
        startMenu.SetActive(false);
        tutorialMenu.SetActive(false);
        gameplayElements.SetActive(true);
    }

    public void OnTutorialButtonClicked()
    {
        PlayButtonClickSound();
        startMenu.SetActive(false);
        tutorialMenu.SetActive(true);
    }

    public void OnBackButtonClicked()
    {
        PlayButtonClickSound();
        tutorialMenu.SetActive(false);
        startMenu.SetActive(true);
    }

    public void ShowStartMenu()
    {
        startMenu.SetActive(true);
        tutorialMenu.SetActive(false);
        gameplayElements.SetActive(false);

        if (nextButton != null)
        {
            nextButton.gameObject.SetActive(false);
        }
    }
}





