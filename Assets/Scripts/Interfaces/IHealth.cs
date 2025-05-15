using System;
using UnityEngine;

public interface IHealth
{
    public event Action<float> HealthChanged;
    public void DecreaseHealth(float value);
    public void IncreaseHealth(float value);
    public void Death();
}
