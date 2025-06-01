using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskController : MonoBehaviour, IInteractable
{
    private Animator _animator;

    [Header("Посилання")]
    [SerializeField] private GameObject _plantsCounter;
    [SerializeField] private Granny _granny;
    [SerializeField] private GameObject _craftingPanel;
    [SerializeField] private CraftingUIManager _craftingUIManager;

    private void Start()
    {
        _animator = GetComponent<Animator>();
    }

    public void Interact()
    {
        if (PlayerPrefs.HasKey("TaskComplete"))
        {
            string location = PlayerPrefs.GetString("TaskComplete");
            if (IsPotionAlreadyCrafted(location))
            {
                CompleteTask(location);
            }
            else
            {
                StartCrafting(location);
            }
        }
    }

    private void StartCrafting(string location)
    {
        _craftingPanel.SetActive(true);
        _craftingUIManager.InitForLevel(location, this, _granny);
        _plantsCounter.SetActive(false);
    }

    public void CompleteTask(string location)
    {
        _animator.SetTrigger("Burn");
        PlayerPrefs.SetString("NoTask", "true");
        PlayerPrefs.SetString("GameStage", location);
        _granny.TaskComplete(location);
        PlayerPrefs.DeleteKey("TaskComplete");
        PlayerPrefs.DeleteKey("TotalPlants");
        _plantsCounter.SetActive(false);
    }

    private bool IsPotionAlreadyCrafted(string location)
    {
        return location switch
        {
            "forest" => PlayerPrefs.HasKey("Potion1"),
            "pineForest" => PlayerPrefs.HasKey("Potion2"),
            "swamp" => PlayerPrefs.HasKey("Potion3"),
            _ => false
        };
    }
}
