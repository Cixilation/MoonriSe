using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;
using UnityEngine.UI;

public class SpawnOwnedTool : MonoBehaviour
{
    [SerializeField] private GameObject toolUIPrefab;
    [SerializeField] private Transform toolParentUI;
    
    private List<GameObject> instantiatedUIElements = new List<GameObject>();

    private void OnEnable()
    {
        CreateInventoryUI();
    }

    private void OnDisable()
    {
        DestroyInventoryUI();
    }


    public void RefreshInventoryUI()
    {
        DestroyInventoryUI();
        CreateInventoryUI();
    }

    private void CreateInventoryUI()
    {
        foreach (Tools tool in GameData.Instance.GameTools)
        {
            if (!tool.isToolOwned) continue;
            GameObject uiElement = Instantiate(toolUIPrefab, toolParentUI);
            TextMeshProUGUI toolNameText = uiElement.transform.Find("ItemName").GetComponentInChildren<TextMeshProUGUI>();
            Transform itemImageTransform = uiElement.transform.Find("ItemImage");
            toolNameText.text = tool.name;
            
            Image itemImage = itemImageTransform.GetComponent<Image>();
            if (itemImage != null)
                itemImage.sprite = tool.toolIcon;
            GameObject equipTag = uiElement.transform.Find("Equip").gameObject;
            if (PlayerData.Instance.currentEquipmentName == tool.toolName)
            {
               equipTag.SetActive(true);
            }
            else
            {
                equipTag.SetActive(false);
            }

            EquipToolScript toolScript = uiElement.GetComponent<EquipToolScript>();
            toolScript.tool = tool;
            instantiatedUIElements.Add(uiElement);
        }
    }

    private void DestroyInventoryUI()
    {
        foreach (GameObject uiElement in instantiatedUIElements)
        {
            Destroy(uiElement);
        }
        instantiatedUIElements.Clear();
    }
}