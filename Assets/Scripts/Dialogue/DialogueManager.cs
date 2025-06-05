using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ink.Runtime;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;
using UnityEngine.EventSystems;

public class DialogueManager : MonoBehaviour
{
    [Header("Ink Story")]
    [SerializeField] private TextAsset inkJson;

    private Story story;
    private int currentChoiceIndex = -1;

    private bool dialoguePlaying = false;
    private bool justOpened = false;
    private bool _submitLocked = false;

     private InkExternalFunctions inkExternalFunctions;
     private InkDialogueVariables inkDialogueVariables;

    private void Awake()
    {
       story = new Story(inkJson.text);
       inkExternalFunctions = new InkExternalFunctions();
       inkExternalFunctions.Bind(story);
       inkDialogueVariables = new InkDialogueVariables(story);
    }

    private void OnDestroy()
    {
       inkExternalFunctions.Unbind(story);
    }

    private void OnEnable()
    {
        GameEventsManager.dialogueEvents.onEnterDialogue += EnterDialogue;
        GameEventsManager.inputEvents.onSubmitPressed += SubmitPressed;
        GameEventsManager.dialogueEvents.onUpdateChoiceIndex += UpdateChoiceIndex;
        GameEventsManager.dialogueEvents.onUpdateInkDialogueVariable += UpdateInkDialogueVariable;
        GameEventsManager.questEvents.onQuestStateChange += QuestStateChange;

        GameEventsManager.inputEvents.onMovePressed += MovePressed;
    }

    private void OnDisable()
    {
        GameEventsManager.dialogueEvents.onEnterDialogue -= EnterDialogue;
        GameEventsManager.inputEvents.onSubmitPressed -= SubmitPressed;
        GameEventsManager.dialogueEvents.onUpdateChoiceIndex -= UpdateChoiceIndex;
        GameEventsManager.dialogueEvents.onUpdateInkDialogueVariable -= UpdateInkDialogueVariable;
        GameEventsManager.questEvents.onQuestStateChange -= QuestStateChange;

        GameEventsManager.inputEvents.onMovePressed -= MovePressed;
    }

    private void QuestStateChange(Quest quest)
    {
        GameEventsManager.dialogueEvents.UpdateInkDialogueVariable(
            quest.info.id + "State",
            new StringValue(quest.state.ToString())
        );
    }

    private void UpdateInkDialogueVariable(string name, Ink.Runtime.Object value)
    {
        inkDialogueVariables.UpdateVariableState(name, value);
    }

    private void UpdateChoiceIndex(int choiceIndex)
    {
        this.currentChoiceIndex = choiceIndex;
    }

    private void EnterDialogue(string knotName)
    {
        // don't enter dialogue if we've already entered
        if (dialoguePlaying)
        {
            return;
        }

        dialoguePlaying = true;
        justOpened = true;
        Debug.Log("Enter the dialogue with " + knotName);
        

        // inform other parts of our system that we've started dialogue
        GameEventsManager.dialogueEvents.DialogueStarted();

        // freeze player movement
        GameEventsManager.playerEvents.DisablePlayerMovement();

        // input event context
        GameEventsManager.inputEvents.ChangeInputEventContext(InputEventContext.DIALOGUE);

        // jump to the knot
        if (!knotName.Equals(""))
        {
            story.ChoosePathString(knotName);
        }
        else
        {
            Debug.LogWarning("Knot name was the empty string when entering dialogue.");
        }

        // start listening for variables
        inkDialogueVariables.SyncVariablesAndStartListening(story);

        // kick off the story
        ContinueOrExitStory();
    }

    private void ContinueOrExitStory()
    {
        // make a choice, if applicable
        if (story.currentChoices.Count > 0 && currentChoiceIndex != -1)
        {
            story.ChooseChoiceIndex(currentChoiceIndex);
            // reset choice index for next time
            currentChoiceIndex = -1;
        }

        if (story.canContinue)
        {
            string dialogueLine = story.Continue();
            Debug.Log(dialogueLine);

            // handle the case where there's an empty line of dialogue
            // by continuing until we get a line with content
            while (IsLineBlank(dialogueLine) && story.canContinue)
            {
                dialogueLine = story.Continue();
            }
            // handle the case where the last line of dialogue is blank
            // (empty choice, external function, etc...)
            if (IsLineBlank(dialogueLine) && !story.canContinue)
            {
                Debug.Log($"1canContinue={story.canContinue}  choices={story.currentChoices.Count}");

                ExitDialogue();
            }
            else
            {
                GameEventsManager.dialogueEvents.DisplayDialogue(dialogueLine, story.currentChoices);
            }
        }
        else if (story.currentChoices.Count == 0)
        {
            Debug.Log($"2canContinue={story.canContinue}  choices={story.currentChoices.Count}");

            ExitDialogue();
        }
    }

    private void ExitDialogue()
    {
        Debug.Log("Exit the dialogue");
        dialoguePlaying = false;

        // inform other parts of our system that we've finished dialogue
        GameEventsManager.dialogueEvents.DialogueFinished();

        // let player move again
        GameEventsManager.playerEvents.EnablePlayerMovement();

        // input event context
        GameEventsManager.inputEvents.ChangeInputEventContext(InputEventContext.DEFAULT);

        // stop listening for dialogue variables
        inkDialogueVariables.StopListening(story);

        // reset story state
        story.ResetState();
    }

    private bool IsLineBlank(string dialogueLine)
    {
        return dialogueLine.Trim().Equals("") || dialogueLine.Trim().Equals("\n");
    }



    private void SubmitPressed(InputEventContext inputEventContext)
    {
        // if the context isn't dialogue, we never want to register input here
        if (!inputEventContext.Equals(InputEventContext.DIALOGUE) || _submitLocked)
        {
            return;
        }
        // діалоги без вибору (звичайні репліки): блокуємо лиш на 1 кадр
        bool hasChoices = story.currentChoices.Count > 0;
        _submitLocked = true;

        if (justOpened && currentChoiceIndex == -1) { justOpened = false; _submitLocked = false; return; }
        justOpened = false;


        StartCoroutine(UnlockNextFrame());  


        ContinueOrExitStory();
    }

    private void MovePressed(Vector2 dir)
    {
        if (!dialoguePlaying || story.currentChoices.Count == 0) return;

        int delta = 0;
        if (dir.y > 0.1f) delta = -1;   // Up
        if (dir.y < -0.1f) delta = 1;   // Down
        if (delta == 0) return;

        currentChoiceIndex =
            (currentChoiceIndex + delta + story.currentChoices.Count) % story.currentChoices.Count;

        GameEventsManager.dialogueEvents.UpdateChoiceIndex(currentChoiceIndex);

        var ui = GameObject.FindObjectOfType<DialoguePanelUI>();
        if (ui != null)
        {
            var btn = ui.GetButtonByInkIndex(currentChoiceIndex);
            if (btn != null)
            {
                EventSystem.current.SetSelectedGameObject(btn.gameObject);
            }
        }

    }
    private IEnumerator UnlockNextFrame()
    {
        yield return null;          // рівно 1 кадр
        _submitLocked = false;
    }

}
