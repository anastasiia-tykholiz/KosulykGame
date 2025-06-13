using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MainMenuButtons : MonoBehaviour
{
    private bool _isSceneLoading;

    public void NewGameButton()
    {
        if (_isSceneLoading == false)
        {
            PlayerPrefs.DeleteAll();
            PlayerPrefs.Save();
            QuestManager.SkipSaveOnDisable = true;
            var oldManager = FindObjectOfType<QuestManager>();
            if (oldManager) Destroy(oldManager.gameObject);

            SceneTransition.SwitchToScene("Hub");
            _isSceneLoading = true;
        }
    }

    public void PlayButton()
    {
        if (_isSceneLoading == false)
        {
            SceneTransition.SwitchToScene("Hub");
            _isSceneLoading = true;
        }     
    }

    public void ExitButton()
    {
        Application.Quit();
    }


}
