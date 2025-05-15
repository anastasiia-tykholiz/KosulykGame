using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Image _healthBarFilling;
    [SerializeField] private Stats _stats;

    private void OnEnable()
    {
        _stats.HealthChanged += OnHealthChanged;
    }
    private void OnDisable()
    {
        _stats.HealthChanged -= OnHealthChanged;
    }
    private void OnHealthChanged(float valueAsPercentage)
    {
        _healthBarFilling.fillAmount = valueAsPercentage;
    }
}
