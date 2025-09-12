using System.Collections.Generic; // Use List<T>
using UnityEngine;

[CreateAssetMenu(fileName = "NewPlant", menuName = "ScriptableObjects/Plant")]
public class Plants : ScriptableObject
{
    public string plantName;
    public string plantDescription;
    public Sprite plantSprite;
    public Sprite seedSprite;
    public int quantity;
    public int seedQuantity;
    public int phases;
    public int seedPrice;
    public int plantPrice;
    public int timeToLive;
    public List<GameObject> plantsPhase; 
}