using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneTransition : MonoBehaviour
{
    public static SceneTransition Instance { get; private set; }
    
    private static bool shouldPlayOpeningAnimation = false;

    private Animator _animator;
    private AsyncOperation loadingSceneOperation;

    public static void SwitchToScene(string sceneName)
    {
        Instance._animator.SetTrigger("sceneClosing");

        Instance.loadingSceneOperation = SceneManager.LoadSceneAsync(sceneName);

        Instance.loadingSceneOperation.allowSceneActivation = false;
    }

    private void Start()
    {
        Instance = this;

        _animator = GetComponent<Animator>();

        if (shouldPlayOpeningAnimation)
        {
            _animator.SetTrigger("sceneOpening");

            shouldPlayOpeningAnimation = false;
        }
    }

    public void OnAnimationOver()
    {
        Debug.Log(" Анімація завершена, активуємо сцену");
        shouldPlayOpeningAnimation = true;
        loadingSceneOperation.allowSceneActivation = true;
    }


    public IEnumerator FadeOut()
    {
        _animator.SetTrigger("sceneClosing");
        yield return new WaitForSeconds(1f); // або анімаційний час
    }

    public IEnumerator FadeIn()
    {
        _animator.SetTrigger("sceneOpening");
        yield return new WaitForSeconds(1f);
    }
}