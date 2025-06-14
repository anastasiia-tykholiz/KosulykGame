using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DayCycleManager : MonoBehaviour
{
    private void OnEnable()
    {
        GameEventsManager.dayEvents.OnDayEnd += HandleEndDay;
    }

    private void OnDisable()
    {
        GameEventsManager.dayEvents.OnDayEnd -= HandleEndDay;
    }

    private void HandleEndDay()
    {
        Debug.Log("HandleEndDay() запущено");
        StartCoroutine(EndDayRoutine());
    }

    private IEnumerator EndDayRoutine()
    {
        Debug.Log("Початок рутини нового дня");
        //if (SceneTransition.Instance != null)
        //{
        //    Debug.Log("FadeOut...");
        //    yield return SceneTransition.Instance.FadeOut();
        //}

        //yield return new WaitForSeconds(0.5f);
        //Debug.Log("SwitchToScene tooooo House");
        //SceneTransition.SwitchToScene("House");  // Замість SceneManager.LoadScene
        //yield return new WaitForSeconds(0.5f);

        //if (SceneTransition.Instance != null)
        //{
        //    Debug.Log("FadeIn...");
        //    yield return SceneTransition.Instance.FadeIn();
        //}

        if (SceneTransition.Instance != null)
        {
            Debug.Log("SwitchToScene → House");
            SceneTransition.SwitchToScene("House"); //  Спочатку ініціалізуємо сцену

            Debug.Log("FadeOut...");
            yield return SceneTransition.Instance.FadeOut(); //  Потім анімація
        }

        Debug.Log("Новий день!! Тут буде новий квест");
    }
}
