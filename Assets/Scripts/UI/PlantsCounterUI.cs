using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlantsCounterUI : MonoBehaviour
{

    private string questId;
    private int stepIdx;

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI counterText;
    [SerializeField] private Image icon;

    [Header("Icons for Plants Location")]
    [SerializeField] private PlantIconSet iconSet;


    private void OnEnable()
    {
        var qe = GameEventsManager.questEvents;
        qe.onQuestStateChange += OnQuestChanged;
        qe.onQuestStepStateChange += OnStepChanged;
    }
    private void OnDisable()
    {
        var qe = GameEventsManager.questEvents;
        qe.onQuestStateChange -= OnQuestChanged;
        qe.onQuestStepStateChange -= OnStepChanged;
    }

    private IEnumerator Start()
    {
        // чекаємо, поки QuestManager ініціалізується
        while (QuestManager.Instance == null) yield return null;
        TryBindToActiveCollectQuest();
        Refresh();
    }

    /* ---------- події ---------- */
    private void OnQuestChanged(Quest q)
    {
        TryBindToActiveCollectQuest();
        Refresh();
    }
    private void OnStepChanged(string id, int idx, QuestStepState st)
    {
        if (id == questId && idx == stepIdx) Refresh();
        else TryBindToActiveCollectQuest();
    }

    /* ---------- головні методи ---------- */
    private void TryBindToActiveCollectQuest()
    {
        foreach (var q in QuestManager.Instance.AllQuests)
        {
            //Debug.Log($"Check quest {q.info.id}  state={q.state}");
            if (q.state != QuestState.IN_PROGRESS) continue;

            int idx = q.GetQuestData().questStepIndex;
            if (idx >= q.info.questStepPrefabs.Length) continue;

            var prefab = q.info.questStepPrefabs[idx];
            if (prefab.GetComponentInChildren<CollectPlantQuestStep>(true) == null) continue;

            questId = q.info.id;
            stepIdx = idx;
            break;
        }
    }

    private void Refresh()
    {
        // якщо наразі немає кроку-збирання сховати лічильник і вийти
        if (string.IsNullOrEmpty(questId)) { counterText.text = ""; icon.enabled = false; return; }

        Quest quest = QuestManager.Instance.GetQuestById(questId);

        var prefab = quest.info.questStepPrefabs[stepIdx];
        var collectStep = prefab.GetComponentInChildren<CollectPlantQuestStep>(true);

        int max = collectStep.AmountToCollect;

        var stepState = quest.GetQuestData().questStepStates[stepIdx];
        int cur = 0;
        if (!string.IsNullOrEmpty(stepState.state)) int.TryParse(stepState.state, out cur);

        counterText.text = $"{cur}/{max}";

        if (icon && iconSet)
        {
            icon.enabled = true;
            icon.sprite = iconSet.GetSprite(collectStep.PlantType);
        }

        gameObject.SetActive(!string.IsNullOrEmpty(questId));
    }
}
