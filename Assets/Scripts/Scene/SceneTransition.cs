using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneTransition : MonoBehaviour
{
    private static SceneTransition instance;
    private static bool shouldPlayOpeningAnimation = false;

    private Animator _animator;
    private AsyncOperation loadingSceneOperation;

    public static void SwitchToScene(string sceneName)
    {
        instance._animator.SetTrigger("sceneClosing");

        instance.loadingSceneOperation = SceneManager.LoadSceneAsync(sceneName);

        instance.loadingSceneOperation.allowSceneActivation = false;
    }

    private void Start()
    {
        instance = this;

        _animator = GetComponent<Animator>();

        if (shouldPlayOpeningAnimation)
        {
            _animator.SetTrigger("sceneOpening");

            shouldPlayOpeningAnimation = false;
        }
    }

    public void OnAnimationOver()
    {
        shouldPlayOpeningAnimation = true;

        loadingSceneOperation.allowSceneActivation = true;
    }
}