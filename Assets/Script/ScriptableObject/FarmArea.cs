using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FarmArea", menuName = "ScriptableObjects/FarmArea", order = 1)]
public class FarmArea : ScriptableObject
{
    public string farmName;
    public Sprite farmSprite;
    public int woodCost;    
    public GameObject farmPrefab;
}
