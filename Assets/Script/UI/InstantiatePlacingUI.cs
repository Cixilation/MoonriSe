using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InstantiatePlacingUI : MonoBehaviour
{
    private List<GameObject> instantiatedUIElements = new List<GameObject>();
    [SerializeField] private GameObject ObjectUIPrefab;
    [SerializeField] private Transform ObjectUIParent;
    [SerializeField] private Plants wood;

    public static InstantiatePlacingUI instance;

    private void Awake()
    {
        instance = this;
    }

    private void OnDisable()
    {
        DestroyPlacementUI();
    }

    private void OnEnable()
    {
        
        CreatePlacementUI();
    }
    
    public void RefreshInventoryUI()
    {
        DestroyPlacementUI();
        CreatePlacementUI();
    }

    private void CreatePlacementUI()
    {
        foreach (FarmArea farm in GameData.Instance.GameFarmAreas)
        {
            if (farm.woodCost <= wood.quantity)
            {
                GameObject obj = Instantiate(ObjectUIPrefab, ObjectUIParent);
                Image plantImg = obj.transform.Find("ItemImage").GetComponent<Image>();
                plantImg.sprite = farm.farmSprite;
                TextMeshProUGUI total = obj.transform.Find("Total").GetComponent<TextMeshProUGUI>();
                GameObject background = obj.transform.Find("Unequip").gameObject;
                total.text = "";
                background.gameObject.SetActive(false);   
                instantiatedUIElements.Add(obj);
                SetupButton(obj, farm.farmName, "FarmArea");
            }
        }
        foreach (Plants plant in GameData.Instance.GameResources)
        {
            if (plant.plantName == "Wood") continue;
            if (plant.plantName != "Dirt" && plant.seedQuantity <= 0) continue;
            GameObject obj = Instantiate(ObjectUIPrefab, ObjectUIParent);
            Image plantImg = obj.transform.Find("ItemImage").GetComponent<Image>();
            plantImg.sprite = plant.seedSprite;
            TextMeshProUGUI totalSeedInTotal = obj.transform.Find("Total").GetComponent<TextMeshProUGUI>();
            totalSeedInTotal.text = plant.plantName != "Dirt" ? plant.seedQuantity.ToString() : "";
            GameObject background = obj.transform.Find("Unequip")?.gameObject;
            if (plant.plantName == "Dirt" && background != null)
            {
                background.SetActive(false);
            }
            SetupButton(obj, plant.plantName, "Plant");
            instantiatedUIElements.Add(obj);
        }

    }

    private void SetupButton(GameObject obj, string objectName, string type)
    {
        Button plantBtn = obj.transform.GetComponent<Button>();
        plantBtn.onClick.RemoveAllListeners();
        plantBtn.onClick.AddListener(() => OnPlantButtonClick(objectName, type));
    }

    private void OnPlantButtonClick(string name, string type)
    {
        BuildingManager.instance.SelectObject(name, type);

    }

    
    private void DestroyPlacementUI()
    {
        foreach (GameObject uiElement in instantiatedUIElements)
        {
            Destroy(uiElement);
        }
        instantiatedUIElements.Clear();
    }

}
