using System;
using UnityEngine;

public class Plant : MonoBehaviour, IInteractable
{
    public event Action PlantsValueChanged;

    public PlantType plantType;

    public enum PlantType
    {
        hub,
        forest,
        pineForest,
        swamp
    }
    private void OnEnable()
    {
        if (PlayerPrefs.HasKey(plantType.ToString()))
        {
            PlayerPrefs.SetString("LocationComplete", plantType.ToString());
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        
    }
    public void Interact()
    {
        PlantsValueChanged?.Invoke();
        Destroy(gameObject);
    }
}
