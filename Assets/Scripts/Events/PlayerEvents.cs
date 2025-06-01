using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEvents : MonoBehaviour
{
    public event Action<int /*newLevel*/> onPlayerLevelChange;

    public void PlayerLevelChange(int newLevel)
    {
        onPlayerLevelChange?.Invoke(newLevel);
    }

}
