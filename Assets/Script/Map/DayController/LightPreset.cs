using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "lighting Preset", menuName = "Game Data/Lighting Preset")]
public class LightPreset : ScriptableObject
{
   public Gradient AmbientColor;
   public Gradient DirectionalColor;
   public Gradient FogColor;
}
