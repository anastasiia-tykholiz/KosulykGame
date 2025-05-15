using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStage : MonoBehaviour
{
    [SerializeField] private PlantsCounter _plantsCounter;

    [SerializeField] private bool _restart;
    private void OnEnable()
    {
        _plantsCounter.TaskCompleted += OnTaskCompleted;
    }
    private void OnDisable()
    {
        _plantsCounter.TaskCompleted -= OnTaskCompleted;
    }

    private void Update()
    {
        if (_restart == true)
        {
            PlayerPrefs.DeleteAll();
            _restart = false;
            SceneTransition.SwitchToScene("Hub");
        }
    }

    private void OnTaskCompleted(Plant.PlantType plantType)
    {
        PlayerPrefs.SetString("TaskComplete", plantType.ToString());
        PlayerPrefs.SetString(plantType.ToString(), plantType.ToString());
    }
}
