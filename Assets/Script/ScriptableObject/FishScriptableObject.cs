using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewFish", menuName = "ScriptableObjects/Fish")]
public class FishScriptableObject : ScriptableObject
{
    public string fishName;
    public string fishDescription;
    public Sprite fishSprite;
    public int quantity;
    public int price;

}