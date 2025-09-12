using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;
using UnityEngine.UI;

public class SpawnOwnedResources : MonoBehaviour
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
        foreach (Plants plant in GameData.Instance.GameResources)
        {
            if (plant.plantName == "Dirt")
            {
                continue;
            }
            if (plant.plantName == "Wood"  && plant.quantity > 0)
            {
                GameObject uiElement = Instantiate(toolUIPrefab, toolParentUI);
                TextMeshProUGUI seedNameText = uiElement.transform.Find("ItemName").GetComponentInChildren<TextMeshProUGUI>();
                Transform itemImageTransform = uiElement.transform.Find("ItemImage");
                seedNameText.text = plant.plantName ;
                Image itemImage = itemImageTransform.GetComponent<Image>();
                if (itemImage != null)
                    itemImage.sprite = plant.seedSprite;
                TextMeshProUGUI seedQuantity = uiElement.transform.Find("Total").GetComponentInChildren<TextMeshProUGUI>();
                seedQuantity.text = plant.quantity.ToString();
                instantiatedUIElements.Add(uiElement);
                continue;
            }
            
            if (plant.seedQuantity > 0 && plant.plantName != "Dirt")
            {
                GameObject uiElement = Instantiate(toolUIPrefab, toolParentUI);
                TextMeshProUGUI seedNameText = uiElement.transform.Find("ItemName").GetComponentInChildren<TextMeshProUGUI>();
                Transform itemImageTransform = uiElement.transform.Find("ItemImage");
                seedNameText.text = plant.plantName + " Sapling";
                Image itemImage = itemImageTransform.GetComponent<Image>();
                if (itemImage != null)
                    itemImage.sprite = plant.seedSprite;
                TextMeshProUGUI seedQuantity = uiElement.transform.Find("Total").GetComponentInChildren<TextMeshProUGUI>();
                seedQuantity.text = plant.seedQuantity.ToString();
                instantiatedUIElements.Add(uiElement);
            }


            if (plant.quantity > 0)
            {
                GameObject plantElement = Instantiate(toolUIPrefab, toolParentUI);
                TextMeshProUGUI plantNameText = plantElement.transform.Find("ItemName").GetComponentInChildren<TextMeshProUGUI>();
                Transform plantImageTransform = plantElement.transform.Find("ItemImage");
                plantNameText.text = plant.plantName;
                Image plantImage = plantImageTransform.GetComponent<Image>();
                if (plantImage != null)
                    plantImage.sprite = plant.plantSprite;
                TextMeshProUGUI plantQuantity = plantElement.transform.Find("Total").GetComponentInChildren<TextMeshProUGUI>();
                plantQuantity.text = plant.quantity.ToString();
                instantiatedUIElements.Add(plantElement);
            }
        }
        
        FishScriptableObject fish = GameData.Instance.GameFish;
        if (fish.quantity <= 0) return;
        GameObject fishElement = Instantiate(toolUIPrefab, toolParentUI);
        TextMeshProUGUI fishNameText = fishElement.transform.Find("ItemName").GetComponentInChildren<TextMeshProUGUI>();
        Transform fishImageTransform = fishElement.transform.Find("ItemImage");
        fishNameText.text = fish.fishName;
        Image fishImage = fishImageTransform.GetComponent<Image>();
        if (fishImage != null)
            fishImage.sprite = fish.fishSprite;
        TextMeshProUGUI fishQuantity = fishElement.transform.Find("Total").GetComponentInChildren<TextMeshProUGUI>();
        fishQuantity.text = fish.quantity.ToString();
        instantiatedUIElements.Add(fishElement);
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