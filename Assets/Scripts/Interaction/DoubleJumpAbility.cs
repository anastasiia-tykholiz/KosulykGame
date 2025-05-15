using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleJumpAbility : MonoBehaviour, IInteractable
{
    [SerializeField] private PlayerData _playerData;

    private void Start()
    {
        PlayerPrefs.SetString("DJSpawned", "true");
    }
    public void Interact()
    {
        PlayerPrefs.SetInt("amountOfJumps", 2);
        PlayerPrefs.DeleteKey("DJSpawned");
        Destroy(gameObject);
    }
}
