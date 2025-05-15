using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealingPlant : MonoBehaviour, IInteractable
{
    [SerializeField] private float _amount = 10;
    [SerializeField] private Stats _stats;

    public void Interact()
    {
        IncreaseHealth();
        Destroy(gameObject);
    }

    private void IncreaseHealth()
    {
        _stats.IncreaseHealth(_amount);
    }
}
