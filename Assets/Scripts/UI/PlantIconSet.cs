using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Plant Location Icon Set")]
public class PlantIconSet : ScriptableObject
{
    [Serializable] struct Pair { public Plant.PlantType type; public Sprite sprite; }
    [SerializeField] private List<Pair> items;

    private readonly Dictionary<Plant.PlantType, Sprite> _dict = new();
    void OnEnable() { foreach (var p in items) _dict[p.type] = p.sprite; }

    public Sprite GetSprite(Plant.PlantType t) => _dict[t];
}
