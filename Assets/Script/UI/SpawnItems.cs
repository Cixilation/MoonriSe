using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SpawnItems : MonoBehaviour
{
    [SerializeField] private Transform parentUI;
    [SerializeField] private GameObject toolUIPrefab;
    [SerializeField] private GameObject rightSideUI;
    private Dictionary<Tools, GameObject> toolUIMap = new Dictionary<Tools, GameObject>();

    private void OnDisable()
    {
        PauseGame.instance.Unpause();
    }

    private void Start()
    {
        foreach (Tools tool in GameData.Instance.GameTools)
        {
            if (tool.toolName != "Axe")
            {
                if (toolUIPrefab != null && parentUI != null)
                {
                    GameObject toolUI = Instantiate(toolUIPrefab, parentUI);
                    toolUI.GetComponent<PassingTools>().tool = tool;
                    TextMeshProUGUI itemNameText = toolUI.GetComponentInChildren<TextMeshProUGUI>();
                    Transform itemImageTransform = toolUI.transform.Find("ItemImage");

                    if (itemImageTransform != null)
                    {
                        Image itemImage = itemImageTransform.GetComponent<Image>();
                        if (itemImage != null)
                            itemImage.sprite = tool.toolIcon;
                    }

                    if (itemNameText != null)
                        itemNameText.text = tool.toolName;

                    Transform ownedTransform = toolUI.transform.Find("Owned");
                    if (ownedTransform != null)
                    {
                        UpdateOwnedOpacity(ownedTransform.gameObject, tool.isToolOwned);
                    }

                    toolUIMap[tool] = toolUI;
                }
                else
                {
                    Debug.LogError("Tool prefab or parent UI is not assigned!");
                }
            }
        }
    }

    private void UpdateOwnedOpacity(GameObject owned, bool isOwned)
    {
        Image ownedImage = owned.GetComponent<Image>();
        if (ownedImage != null)
        {
            Color color = ownedImage.color;
            color.a = isOwned ? 0.25f : 0f;
            ownedImage.color = color;
        }
    }

    public void BuyTool()
    {
        PassingTools passingTools = rightSideUI?.GetComponent<PassingTools>();
        Tools tool = null;
        if (passingTools != null)
        {
            tool = passingTools.tool;
            if (GameData.Instance.GamePlayerStats.money < tool.toolPrice)
            {
                ErrorShowing.ShowError("You don't have the Money!", Input.mousePosition, 3f);
                return;
            }

            if (tool.isToolOwned == true)
            {
                return;
            }
            GameData.Instance.GamePlayerStats.money -= tool.toolPrice;
        }
        if (GameData.Instance.GameTools.Contains(tool))
        {
            tool.isToolOwned = true;
            if (toolUIMap.TryGetValue(tool, out GameObject toolUI))
            {
                Transform ownedTransform = toolUI.transform.Find("Owned");
                if (ownedTransform != null)
                {
                    UpdateOwnedOpacity(ownedTransform.gameObject, true);
                }
            }
        }
        OpenPlayerStats.Instance.updateMoney();
    }
}
