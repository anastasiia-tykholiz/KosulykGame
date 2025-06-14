using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public class QuestPoint : MonoBehaviour
{
    [Header("Dialogue (optional)")]
    [SerializeField] private string dialogueKnotName; 
    
    [Header("Quest")]
    [SerializeField] private QuestInfoSO questInfoForPoint;

    [Header("Config")]
    [SerializeField] private bool startPoint = true;
    [SerializeField] private bool finishPoint = true;

    private bool playerIsNear = false;
    private string questId;
    private QuestState currentQuestState;

    private QuestIcon questIcon;

    private void Awake()
    {
        questId = questInfoForPoint.id;
        questIcon = GetComponentInChildren<QuestIcon>();
    }

    private void OnEnable()
    {
        GameEventsManager.questEvents.onQuestStateChange += QuestStateChange;
        GameEventsManager.inputEvents.onInteractPressed += InteractPressed;
        Quest quest = QuestManager.Instance.GetQuestById(questId);

        if (QuestManager.Instance != null)
        {
            Quest currentQuest = QuestManager.Instance.GetQuestById(questId);
            if (quest != null)
            {
                currentQuestState = currentQuest.state;
                questIcon.SetState(currentQuestState, startPoint, finishPoint);
            }
            else
            {
                Debug.LogWarning($"QuestPoint: Quest з id {questId} не знайдено у QuestManager.");
            }
        }
    }

    private void OnDisable()
    {
        GameEventsManager.questEvents.onQuestStateChange -= QuestStateChange;
        GameEventsManager.inputEvents.onInteractPressed -= InteractPressed;
    }

    private void InteractPressed(InputEventContext inputEventContext)
    {
       
        if (inputEventContext != InputEventContext.DEFAULT) return;

        if (!playerIsNear)
        {
            return;
        }

        // if we have a knot name defined, try to start dialogue with it
        if (!dialogueKnotName.Equals(""))
        {
            
            GameEventsManager.dialogueEvents.EnterDialogue(dialogueKnotName);
        }
        // otherwise, start or finish the quest immediately without dialogue
        else
        {
            // start or finish a quest
            if (currentQuestState.Equals(QuestState.CAN_START) && startPoint)
            {
                GameEventsManager.questEvents.StartQuest(questId);
            }
            else if (currentQuestState.Equals(QuestState.CAN_FINISH) && finishPoint)
            {
                GameEventsManager.questEvents.FinishQuest(questId);
            }
        }
    }

    private void QuestStateChange(Quest quest)
    {
        // only update the quest state if this point has the corresponding quest
        if (quest.info.id.Equals(questId))
        {
            currentQuestState = quest.state;
            //Debug.Log("QUEST with id: " + questId + " updated to state: " + currentQuestState);
            questIcon.SetState(currentQuestState, startPoint, finishPoint);
        }
    }

    private void OnTriggerEnter2D(Collider2D otherCollider)
    {
        if (otherCollider.CompareTag("Player"))
        {
            playerIsNear = true;
        }
    }
    private void OnTriggerExit2D(Collider2D otherCollider)
    {
        if (otherCollider.CompareTag("Player"))
        {
            playerIsNear = false;
        }
    }
    public string GetQuestId()
    {
        return questInfoForPoint.id;
    }

}
