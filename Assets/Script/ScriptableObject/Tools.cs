using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Tools", menuName = "ScriptableObjects/Tools")]
public class Tools : ScriptableObject
{
    public string toolName;
    public Sprite toolIcon;
    public string toolDescription;
    public int toolPrice;
    public bool isToolOwned;
}
