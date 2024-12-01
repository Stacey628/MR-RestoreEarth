using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueBoxSelection : MonoBehaviour
{
    public TMP_Text dialogText;
    public GameObject original360Galacy;
    public Button dialogueButton;

    private string[] dialogLines = {
        "In the creatures universe, their daily existence revolves around creating new planets, with each creature holding a distinct role in this process",
        "The creatures discover that humans are destroying Earth, threatening its ecosystems and resources. Simultaneously, they realize that Earth's inhabitants—humans and non-humans alike—are perfect materials for creating new planets. With this dual purpose, the creatures journey to Earth: to save it while gathering resources for their cosmic creations.",
        "To survive on Earth, a creature must possess a human host. In return, if a human desires the creature’s superpowers, they must exchange their memories."
    };

    private int currentLineIndex = 0;

    // Start is called before the first frame update
    void Start()
    {
        dialogText.text = dialogLines[currentLineIndex];
        dialogueButton.onClick.AddListener(ShowNextline);
    }

    // Update is called once per frame
    void ShowNextline()
    {
        currentLineIndex++;
        if (currentLineIndex < dialogLines.Length)
        {
            dialogText.text = dialogLines[currentLineIndex];
        }
        else
        {
            original360Galacy.SetActive(false);
            dialogueButton.interactable = false;
        }
    }
}
