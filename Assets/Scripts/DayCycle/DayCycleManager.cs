using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DayCycleManager : MonoBehaviour
{

    private string _nextQuestId;

    private void OnEnable()
    {
        GameEventsManager.dayEvents.OnDayEnd += HandleEndDay;
    }

    private void OnDisable()
    {
        GameEventsManager.dayEvents.OnDayEnd -= HandleEndDay;
    }

    private void HandleEndDay(string nextQuestId)
    {
        Debug.Log("HandleEndDay() запущено");
        _nextQuestId = nextQuestId;
        PlayerPrefs.SetString("NextQuestId", nextQuestId);
        PlayerPrefs.Save();
        StartCoroutine(EndDayRoutine());
    }
    private void Start()
    {
        string questId = PlayerPrefs.GetString("NextQuestId", "");
        if (!string.IsNullOrEmpty(questId))
        {
            ActivateQuestPoint(questId);
        }
    }


    private IEnumerator EndDayRoutine()
    {
        if (SceneTransition.Instance != null)
        {
            SceneTransition.SwitchToScene("House"); //  Спочатку ініціалізуємо сцену

            yield return SceneTransition.Instance.FadeOut(); //  Потім анімація
        }
        Debug.Log("Новий день!! Тут буде новий квест " + _nextQuestId);
        ActivateQuestPoint(_nextQuestId);
    }

    private void ActivateQuestPoint(string questId)
    {
        QuestPoint[] allPoints = FindObjectsOfType<QuestPoint>(true); // true = include inactive
        foreach (QuestPoint point in allPoints)
        {
            if (point.GetQuestId() == questId)
            {
                point.gameObject.SetActive(true);
                //Debug.Log("Активовано квест поінт: " + questId);
                return;
            }
        }

    }

}
