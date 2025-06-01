using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStage : MonoBehaviour
{
    [SerializeField] private bool _restart;


    private void Update()
    {
        if (_restart == true)
        {
            PlayerPrefs.DeleteAll();
            _restart = false;
            SceneTransition.SwitchToScene("Hub");
        }
    }
}
