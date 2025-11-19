// Imports
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class NPCInteract : MonoBehaviour
{
    public GameObject pressEPanel;  
    public GameObject dialoguePanel;  
    public TMPro.TMP_Text dialogueText;   

    bool playerInRange = false;

    // Dialogue management
    int currentLineIndex = 0;
    bool dialogueOpen = false;
    // Dialogue lines
    public string[] dialogueLines = new string[]
    {
        "Bonjour, Luminaris!",
        "Vous vous trouvez dans le golem.",
        "Ce dernier est pollué par la mode Fortnite xD"
    };

    // Vendeur
    public GameObject vendeurScript;

    void Start()
    {
        // Initialement, cacher les panneaux
        if (pressEPanel) pressEPanel.SetActive(false);
        if (dialoguePanel) dialoguePanel.SetActive(false);
    }

    void Update()
    {
        // Vérifier l'entrée du joueur pour ouvrir
        if (playerInRange && Input.GetKeyDown(KeyCode.E) && dialogueOpen == false)
        {
            dialogueOpen = true;
            currentLineIndex = 0;
            OpenDialogue();

            // Afficher les lignes de dialogue
        } else if (dialogueOpen && Input.GetKeyDown(KeyCode.E))
            {
                currentLineIndex++;
            // Eviter débordement
            if (currentLineIndex >= dialogueLines.Length)
            {
                // On appelle le vendeur
                vendeurScript.GetComponent<vendeur>().OpenShop();

                dialoguePanel.SetActive(false);
                dialogueOpen = false;
                currentLineIndex = 0;
                return;
            }
                // Mettre à jour le texte
                OpenDialogue();
            }
    }

    // Ouvrir le dialogue
    void OpenDialogue()
    {
        if (dialoguePanel == null || dialogueText == null) return;

        dialogueText.text = dialogueLines[currentLineIndex];
        dialoguePanel.SetActive(true);
    }

    // Trigger detection
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            if (pressEPanel) pressEPanel.SetActive(true);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            if (pressEPanel) pressEPanel.SetActive(false);
            if (dialoguePanel) dialoguePanel.SetActive(false);
        }
    }
}
