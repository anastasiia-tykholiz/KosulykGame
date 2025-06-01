using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Granny : MonoBehaviour
{
    [SerializeField] private GameObject _wallJumpAbility;
    [SerializeField] private GameObject _doubleJumpAbility;

    [SerializeField] private GameObject _potion1;
    [SerializeField] private GameObject _potion2;
    [SerializeField] private GameObject _potion3;

    private void Start()
    {
        if (PlayerPrefs.HasKey("DJSpawned"))
            Instantiate(_doubleJumpAbility, transform.position, Quaternion.identity);

        if (PlayerPrefs.HasKey("WJSpawned"))
            Instantiate(_wallJumpAbility, transform.position, Quaternion.identity);

        if (PlayerPrefs.HasKey("Potion1"))
            _potion1.SetActive(true);
        if (PlayerPrefs.HasKey("Potion2"))
            _potion2.SetActive(true);
        if (PlayerPrefs.HasKey("Potion3"))
            _potion3.SetActive(true);
    }

    public void TaskComplete(string locationThatComplete)
    {
        if (locationThatComplete == "forest" && !PlayerPrefs.HasKey("Potion1"))
        {
            Instantiate(_doubleJumpAbility, transform.position, Quaternion.identity);
            PlayerPrefs.SetString("Potion1", "true");
            _potion1.SetActive(true);
            PlayerPrefs.SetString("DJSpawned", "true");
        }
        else if (locationThatComplete == "pineForest" && !PlayerPrefs.HasKey("Potion2"))
        {
            Instantiate(_wallJumpAbility, transform.position, Quaternion.identity);
            PlayerPrefs.SetString("Potion2", "true");
            _potion2.SetActive(true);
            PlayerPrefs.SetString("WJSpawned", "true");
        }
        else if (locationThatComplete == "swamp" && !PlayerPrefs.HasKey("Potion3"))
        {
            PlayerPrefs.SetString("Potion3", "true");
            _potion3.SetActive(true);
        }
    }

}

