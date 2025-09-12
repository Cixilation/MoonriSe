using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipToolScript : MonoBehaviour
{
    public Tools tool;
    public void ChangeEquipment()
    {
        PlayerData.Instance.currentEquipmentName = tool.toolName;  
        GameObject player = GameObject.Find("Player");
        Player playerScript = player.GetComponent<Player>();
        playerScript.ChangeTool(tool);
        
        GameObject inventory = GameObject.Find("EquipmentGridLayout");
        inventory.GetComponent<SpawnOwnedTool>().RefreshInventoryUI();
    }
    
}
