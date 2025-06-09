using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CollectPlantQuestStep : QuestStep
{
    [SerializeField] private Plant.PlantType plantType;
    [SerializeField] private int amountToCollect;
    private int current;
    public Plant.PlantType PlantType => plantType;
    public int AmountToCollect => amountToCollect;

    private void OnEnable()
    {
        GameEventsManager.questEvents.onPlantCollected += OnPlantCollected;
    }
    private void OnDisable()
    {
        GameEventsManager.questEvents.onPlantCollected -= OnPlantCollected;
    }

    private void OnPlantCollected(string id, int total)
    {
        if (id != plantType.ToString()) return;
        current += 1;
        UpdateStep();
        if (current >= amountToCollect){ FinishQuestStep(); }
    }

    private void UpdateStep()
    {
        string state = current.ToString();
        ChangeState(state);
    }

    protected override void SetQuestStepState(string state)
    {
        this.current = System.Int32.Parse(state);
        UpdateStep();
    }
}
