using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class Stats : CoreComponent, IHealth
{
    public event Action<float> HealthChanged;
    public bool IsTakeDamage { get => _isTakeDamage; private set => _isTakeDamage = value; }
    public bool IsDead { get => _isDead; private set => _isDead = value; }

    [SerializeField] private float _maxHealth;
    [SerializeField] private float _decreaseHealthCoolDown = 1;
    [SerializeField] private float _startCoolDownTime;

    [SerializeField] private int level = 0;
    [SerializeField] private int exp;
    [SerializeField] private int expPerLevel = 100;

    public int Level => level;
    public int EXP => exp;

    [SerializeField] private bool _canDecreaseHealth = true;

    private bool _isTakeDamage;
    private bool _isDead;

    private float _currentHealth;

    protected override void Awake()
    {
        base.Awake();

        _currentHealth = _maxHealth;
        _startCoolDownTime = _decreaseHealthCoolDown;
        level = PlayerPrefs.GetInt("level", level);
        exp = PlayerPrefs.GetInt("xp", exp);
        GameEventsManager.playerEvents.PlayerLevelChange(level);
    }
    private void Update()
    {
        DecreaseHealthCoolDown();
    }

    public void DecreaseHealth(float amount)
    {
        if (_canDecreaseHealth == true)
        {
            _isTakeDamage = true;
            _canDecreaseHealth = false;
            _currentHealth -= amount;

            if (_currentHealth <= 0)
            {
                _currentHealth = 0;
                Death();
            }
            else
            {
                float currentHealthAsPercantage = (float)_currentHealth / _maxHealth;
                HealthChanged?.Invoke(currentHealthAsPercantage);
            }
        }
        
    }
    public void CantTakeDamage()
    {
        _isTakeDamage = false;
    }
    private void DecreaseHealthCoolDown()
    {
        if (_canDecreaseHealth == false)
        {
            if (_startCoolDownTime >= 0)
            {
                _startCoolDownTime -= Time.deltaTime;
            }
            else
            {
                _canDecreaseHealth = true;
                _startCoolDownTime = _decreaseHealthCoolDown;
            }
        }
    }
    public void IncreaseHealth(float amount) 
    {
        _currentHealth = Mathf.Clamp(_currentHealth + amount, 0, _maxHealth);
        float currentHealthAsPercantage = (float)_currentHealth / _maxHealth;
        HealthChanged?.Invoke(currentHealthAsPercantage);
    }

    public void Death()
    {
        HealthChanged?.Invoke(0);
        _isDead = true;
    }

    public void RestartLvl()
    {
        SceneTransition.SwitchToScene("Hub");
    }

    public void AddEXP(int amount)
    {
        exp += amount;
        while (exp >= expPerLevel)
        {
            exp -= expPerLevel;
            level++;
            GameEventsManager.playerEvents.PlayerLevelChange(level);
        }
        PlayerPrefs.SetInt("level", level);
        PlayerPrefs.SetInt("exp", exp);
        PlayerPrefs.Save();
    }
}
