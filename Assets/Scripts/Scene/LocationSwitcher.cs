using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LocationSwitcher : MonoBehaviour, IInteractable
{
    private bool _canInteract = true;
    private enum LocationToSwitch
    {
        hub,
        forest,
        pineForest,
        swamp,
        house
    }
    [SerializeField] private LocationToSwitch _locationToSwitch;

    public void Interact()
    {
        if (_canInteract == true)
        {
            _canInteract = false;
            SceneTransition.SwitchToScene(_locationToSwitch.ToString());
        }
        
    }
    
}
