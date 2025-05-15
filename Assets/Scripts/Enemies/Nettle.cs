using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nettle : MonoBehaviour
{
    [SerializeField] private float _damage = 10;
    private Stats _stats;
    private bool _canDecreaseHealth;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Stats stats))
        {
            _stats = stats;
            _canDecreaseHealth = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Stats stats))
        {
            _canDecreaseHealth = false;
        }
    }
    private void Update()
    {
        TryDecreaseHealth();
    }
    private void TryDecreaseHealth()
    {
        if (_canDecreaseHealth == true)
        {
            _stats.DecreaseHealth(_damage);
        }
    }
}
