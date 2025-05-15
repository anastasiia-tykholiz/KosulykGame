using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Door : MonoBehaviour, IInteractable
{
    [SerializeField] private Scene HubScene;
    [SerializeField] private Scene HouseScene;
    public void Interact()
    {

        if (SceneManager.GetActiveScene() == HubScene)
        {
            SceneTransition.SwitchToScene(HouseScene.ToString());
        }
        else if (SceneManager.GetActiveScene() == HouseScene)
        {
            SceneTransition.SwitchToScene(HubScene.ToString());
        }
    }
}
