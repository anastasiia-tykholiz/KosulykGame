using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static Plant;

public class PlantsCounter : MonoBehaviour
{
    public event Action<Plant.PlantType> TaskCompleted;

    private TextMeshProUGUI _textMeshPro;
    [SerializeField] private GameObject _taskCounter;

    private List<Plant> _plantsList = new List<Plant>();

    private int _totalPlants;
    private int _foundPlants;

    private string _plantType;

    private void Awake()
    {
        _textMeshPro = GetComponent<TextMeshProUGUI>();
        FindPlants();
        CheckIfTaskComplete();
        UpdateCounterText();
        CheckIfHaveTask();
    }
    private void Start()
    {
        if (_totalPlants > 0 && PlayerPrefs.GetString("GameStage") != _plantType && PlayerPrefs.GetString("LocationComplete") != _plantType)
        {
            PlayerPrefs.SetInt("TaskValue", _totalPlants);

            PlayerPrefs.DeleteKey("NoTask");
        }
        if (PlayerPrefs.HasKey("NoTask"))
        {
            _taskCounter.SetActive(false);
        }
        CheckIfHaveTask();
    }
    private void OnEnable()
    {
        foreach (Plant plant in _plantsList)
        {
            plant.PlantsValueChanged += OnPlantsValueChanged;          
        }
    }
    private void OnDisable()
    {
        foreach (Plant plant in _plantsList)
        {
            plant.PlantsValueChanged -= OnPlantsValueChanged;
        }
    }

    private void FindPlants()
    {
        Plant[] plantsArray = FindObjectsOfType<Plant>();
        
        _totalPlants = plantsArray.Length;
        _plantsList.AddRange(plantsArray);
        if (_totalPlants > 0)
        {
            _plantType = _plantsList[0].plantType.ToString();
        }
        
    }

    private void OnPlantsValueChanged()
    {
        _foundPlants += 1;
        UpdateCounterText();
    }
    private void UpdateCounterText()
    {
        CheckIfTaskComplete();
        if (PlayerPrefs.HasKey("TaskComplete") == true)
        {
            _foundPlants = PlayerPrefs.GetInt("TotalPlants");
            _totalPlants = PlayerPrefs.GetInt("TotalPlants");
        }

        _textMeshPro.text = $"{_foundPlants}/{_totalPlants}";
    }
    private void CheckIfTaskComplete()
    {
        if (_foundPlants == _totalPlants && PlayerPrefs.HasKey("TaskComplete") == false)
        {
            PlayerPrefs.SetInt("TotalPlants", _totalPlants);       
            TaskCompleted?.Invoke(_plantsList[0].plantType);
        }
    }
    private void CheckIfHaveTask()
    {
        if (PlayerPrefs.HasKey("TaskValue"))
        {
            _textMeshPro.text = $"{_foundPlants}/{PlayerPrefs.GetInt("TaskValue")}";
        }
    }
}
