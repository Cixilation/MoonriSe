using System;
using TMPro;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;
using UnityEngine.UI;

public class PassingTools : MonoBehaviour
{
    public Tools tool;
    

    public void setTool()
    {
        GameObject  rightSideStore = GameObject.Find("RightSideStore");
        rightSideStore.GetComponent<PassingTools>().tool = tool;

        Transform itemImageBackgroundTransform = rightSideStore.transform.Find("ItemSpriteBackground");
        Transform ownedDecoration = rightSideStore.transform.Find("OwnedDecoration");
        Transform itemImageTransform = itemImageBackgroundTransform.transform.Find("ItemSprite");
        Transform itemName = rightSideStore.transform.Find("ItemName");
        Transform itemDescription = rightSideStore.transform.Find("ItemDescription");
        Transform itemMoney = rightSideStore.transform.Find("ItemPrice");
        Transform buyItemTransform = rightSideStore.transform.Find("Buy");
        if (buyItemTransform != null)
        {
            TMPro.TextMeshProUGUI buyText = buyItemTransform.GetComponentInChildren<TMPro.TextMeshProUGUI>();
            if (buyText != null)
            {
                buyText.text = tool.isToolOwned ? "Owned" : "Buy";
            }
        }
        if (tool.isToolOwned)
        {
            ownedDecoration.gameObject.SetActive(true);
        }
        else
        {
            ownedDecoration.gameObject.SetActive(false);
        }
        itemMoney.GetComponent<TextMeshProUGUI>().text = tool.toolPrice.ToString();
        if (GameData.Instance.GamePlayerStats.money < tool.toolPrice)
        {
            itemMoney.GetComponent<TextMeshProUGUI>().color = new Color32(255, 20, 23, 255); 
        }
        else
        {
            itemMoney.GetComponent<TextMeshProUGUI>().color = new Color32(254, 234, 89, 255); 
        }
        itemDescription.GetComponent<TextMeshProUGUI>().text = tool.toolDescription;
        itemName.GetComponent<TextMeshProUGUI>().text = tool.toolName;
        itemImageTransform.GetComponent<Image>().sprite = tool.toolIcon;
    }
}