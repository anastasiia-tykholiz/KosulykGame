using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class DialogueChoiceButton : MonoBehaviour, ISelectHandler, IPointerClickHandler, ISubmitHandler
{
    [Header("Components")]
    [SerializeField] private Button button;
    [SerializeField] private TextMeshProUGUI choiceText;

    private int choiceIndex = -1;

    public void SetChoiceText(string choiceTextString)
    {
        choiceText.text = choiceTextString;
    }

    public void SetChoiceIndex(int choiceIndex)
    {
        this.choiceIndex = choiceIndex;
    }

    public void SelectButton()
    {
        button.Select();
    }

    public void OnSelect(BaseEventData eventData)
    {
        GameEventsManager.dialogueEvents.UpdateChoiceIndex(choiceIndex);
    }

    private void Choose()
    {
        GameEventsManager.dialogueEvents.UpdateChoiceIndex(choiceIndex);
        //GameEventsManager.inputEvents.SubmitPressed();   // має бути

       
    }

    public void OnPointerClick(PointerEventData e) => Choose(); // клік мишею
    public void OnSubmit(BaseEventData e) => Choose();

}
