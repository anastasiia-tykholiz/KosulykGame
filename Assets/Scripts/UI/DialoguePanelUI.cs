using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Ink.Runtime;
using UnityEngine.EventSystems;

public class DialoguePanelUI : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private GameObject contentParent;
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private DialogueChoiceButton[] choiceButtons;

    private void Awake()
    {
        contentParent.SetActive(false);
        ResetPanel();
    }

    private void OnEnable()
    {
        GameEventsManager.dialogueEvents.onDialogueStarted += DialogueStarted;
        GameEventsManager.dialogueEvents.onDialogueFinished += DialogueFinished;
        GameEventsManager.dialogueEvents.onDisplayDialogue += DisplayDialogue;
    }

    private void OnDisable()
    {
        GameEventsManager.dialogueEvents.onDialogueStarted -= DialogueStarted;
        GameEventsManager.dialogueEvents.onDialogueFinished -= DialogueFinished;
        GameEventsManager.dialogueEvents.onDisplayDialogue -= DisplayDialogue;
    }

    private void DialogueStarted()
    {
        contentParent.SetActive(true);
    }

    private void DialogueFinished()
    {
        contentParent.SetActive(false);

        // reset anything for next time
        ResetPanel();
    }

    private void DisplayDialogue(string dialogueLine, List<Choice> dialogueChoices)
    {
        dialogueText.text = dialogueLine;

        // defensive check - if there are more choices coming in than we can support, log an error
        if (dialogueChoices.Count > choiceButtons.Length)
        {
            Debug.LogError("More dialogue choices ("
                + dialogueChoices.Count + ") came through than are supported ("
                + choiceButtons.Length + ").");
            Debug.LogError(dialogueChoices);
        }

        // start with all of the choice buttons hidden
        foreach (DialogueChoiceButton choiceButton in choiceButtons)
        {
            choiceButton.gameObject.SetActive(false);
        }

        if (dialogueChoices.Count == 0)
            EventSystem.current.SetSelectedGameObject(null);

        // enable and set info for buttons depending on ink choice information
        int choiceButtonIndex = dialogueChoices.Count - 1;
        for (int inkChoiceIndex = 0; inkChoiceIndex < dialogueChoices.Count; inkChoiceIndex++)
        {
            Choice dialogueChoice = dialogueChoices[inkChoiceIndex];
            DialogueChoiceButton choiceButton = choiceButtons[choiceButtonIndex];

            choiceButton.gameObject.SetActive(true);
            choiceButton.SetChoiceText(dialogueChoice.text);
            choiceButton.SetChoiceIndex(inkChoiceIndex);

            if (inkChoiceIndex == 0)
            {
                choiceButton.SelectButton();
                GameEventsManager.dialogueEvents.UpdateChoiceIndex(inkChoiceIndex);
            }

            choiceButtonIndex--;
        }
    }

    public DialogueChoiceButton GetButtonByInkIndex(int inkIndex)
    {
        // ������� ����������: �������� ���-������ -> ������ � �����
        int btnIdx = (choiceButtons.Length - 1) - inkIndex;
        if (btnIdx >= 0 && btnIdx < choiceButtons.Length)
            return choiceButtons[btnIdx];
        return null;
    }


    private void ResetPanel()
    {
        dialogueText.text = "";
    }
}
