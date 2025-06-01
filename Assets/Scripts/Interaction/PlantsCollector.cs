using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlantsCollector : MonoBehaviour
{
    private List<Plant> _plantsList = new List<Plant>();

    private int _totalPlants;
    private int _foundPlants;

    private string _plantType;

    private void Awake()
    {
        FindPlants();
    }

    private void OnEnable()
    {
        foreach (Plant plant in _plantsList)
        {
            plant.PlantsValueChanged += OnPlantPicked;
        }
    }
    private void OnDisable()
    {
        foreach (Plant plant in _plantsList)
        {
            plant.PlantsValueChanged -= OnPlantPicked;
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

    private void OnPlantPicked()
    {
        _foundPlants++;
        GameEventsManager.questEvents.PlantCollected(_plantType.ToString(), _foundPlants);
    }

}
