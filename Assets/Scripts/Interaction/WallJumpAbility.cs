using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallJumpAbility : MonoBehaviour, IInteractable
{
    [SerializeField] private PlayerData _playerData;

    private void Start()
    {
        PlayerPrefs.SetString("WJSpawned", "true");
    }
    public void Interact()
    {
        PlayerPrefs.SetString("canWallJump", "true");
        PlayerPrefs.DeleteKey("WJSpawned");
        Destroy(gameObject);
    }
}
