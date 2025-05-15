using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskController : MonoBehaviour, IInteractable
{
    private Animator _animator;

    [SerializeField] private GameObject _plantsCounter;
    [SerializeField] private Granny _granny;
    private void Start()
    {
        _animator = GetComponent<Animator>();
    }

    public void Interact()
    {
        if (PlayerPrefs.HasKey("TaskComplete") == true)
        {
            _animator.SetTrigger("Burn");
            PlayerPrefs.SetString("NoTask", "true");
            PlayerPrefs.SetString("GameStage", PlayerPrefs.GetString("TaskComplete"));
            _granny.TaskComplete(PlayerPrefs.GetString("TaskComplete"));
            PlayerPrefs.DeleteKey("TaskComplete");
            PlayerPrefs.DeleteKey("TotalPlants");
            _plantsCounter.SetActive(false);
        }
    }
}
