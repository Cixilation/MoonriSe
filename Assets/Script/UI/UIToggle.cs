using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UIToggle
{
    public void ToggleUIVisibility(GameObject uiElement)
    {
        uiElement.SetActive(!uiElement.activeSelf); 
    }
}
