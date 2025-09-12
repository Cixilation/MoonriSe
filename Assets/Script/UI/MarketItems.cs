using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MarketItems : MonoBehaviour
{
    [SerializeField] private Transform parentUI;
    [SerializeField] private GameObject resourceUIPrefab;
    [SerializeField] private GameObject rightSideUI;
    private List<GameObject> instantiatedUIElements = new List<GameObject>();
    private void OnDisable()
    {
        PauseGame.instance.Unpause();
    }
    
    void Start()
    {
        setResourceUI();
    }



    public void RefreshUI()
    {
        destoryResourcesUI();
        setResourceUI();
    }

    private void setResourceUI()
    {
        
        foreach (Plants plant in GameData.Instance.GameResources)
        {
            if (plant.plantName == "Dirt")
            {
                continue;
            }
            GameObject resourceUI = Instantiate(resourceUIPrefab, parentUI);
            resourceUI.GetComponent<PassMarketItems>().plant = plant;
            resourceUI.GetComponent<PassMarketItems>().isSeed = false;
            resourceUI.GetComponent<PassMarketItems>().isFish = false;
            TextMeshProUGUI resourceNameText = resourceUI.transform.Find("ItemName").GetComponentInChildren<TextMeshProUGUI>();
            TextMeshProUGUI resourceTotal = resourceUI.transform.Find("Total").GetComponentInChildren<TextMeshProUGUI>();
            
            resourceTotal.text = plant.quantity.ToString();
            resourceNameText.text = plant.plantName;
            Transform resourceImageTransform = resourceUI.transform.Find("ItemImage");
            resourceImageTransform.GetComponent<Image>().sprite = plant.plantSprite;
            instantiatedUIElements.Add(resourceUI);
            
        }
        
        foreach (Plants plant in GameData.Instance.GameResources)
        {
            if (plant.plantName == "Dirt" || plant.plantName == "Wood")
            {
                continue;
            }
            GameObject resourceUI = Instantiate(resourceUIPrefab, parentUI);
            resourceUI.GetComponent<PassMarketItems>().plant = plant;
            resourceUI.GetComponent<PassMarketItems>().isSeed = true;
            resourceUI.GetComponent<PassMarketItems>().isFish = false;
            TextMeshProUGUI resourceNameText = resourceUI.transform.Find("ItemName").GetComponentInChildren<TextMeshProUGUI>();
            TextMeshProUGUI resourceTotal = resourceUI.transform.Find("Total").GetComponentInChildren<TextMeshProUGUI>();
            
            resourceTotal.text = plant.seedQuantity.ToString();
            resourceNameText.text = plant.plantName + " Sapling";
            Transform resourceImageTransform = resourceUI.transform.Find("ItemImage");
            resourceImageTransform.GetComponent<Image>().sprite = plant.seedSprite;
            instantiatedUIElements.Add(resourceUI);
        }
        
        GameObject resourceUIF = Instantiate(resourceUIPrefab, parentUI);
        TextMeshProUGUI resourceNameTextF = resourceUIF.transform.Find("ItemName").GetComponentInChildren<TextMeshProUGUI>();
        TextMeshProUGUI resourceTotalF = resourceUIF.transform.Find("Total").GetComponentInChildren<TextMeshProUGUI>();
        Transform resourceImageTransformF = resourceUIF.transform.Find("ItemImage");
        resourceUIF.GetComponent<PassMarketItems>().isFish = true;
        resourceUIF.GetComponent<PassMarketItems>().isSeed = false;
        resourceTotalF.text = GameData.Instance.GameFish.quantity.ToString();
        resourceNameTextF.text = GameData.Instance.GameFish.fishName;
        resourceImageTransformF.GetComponent<Image>().sprite =  GameData.Instance.GameFish.fishSprite;
        instantiatedUIElements.Add(resourceUIF);
    }

    private void destoryResourcesUI()
    {
        foreach (GameObject UI in instantiatedUIElements)
        {
            Destroy(UI);   
        }
        instantiatedUIElements.Clear();
    }
    
    
    

    public void BuyResource()
    {
        Debug.Log("Buying resource");
        GameObject  rightSideStore = GameObject.Find("ResourceRightSideStore");
        TextMeshProUGUI resourceText = rightSideStore.transform.Find("ResourceQuantity").GetComponent<TextMeshProUGUI>();
        int currentQuantity = int.Parse(resourceText.text);
        
        if (rightSideStore.GetComponent<PassMarketItems>().isSeed)
        {
            if (GameData.Instance.GamePlayerStats.money >=
                currentQuantity * rightSideStore.GetComponent<PassMarketItems>().plant.seedPrice)
            {
                GameData.Instance.GamePlayerStats.money -=
                    currentQuantity * rightSideStore.GetComponent<PassMarketItems>().plant.seedPrice;
                rightSideStore.GetComponent<PassMarketItems>().plant.seedQuantity += currentQuantity;
            }
            else
            {
                ErrorShowing.ShowError("You don't have the Money!", Input.mousePosition, 5f);
            }
        }
        else if (rightSideStore.GetComponent<PassMarketItems>().isFish)
        {
            if (GameData.Instance.GamePlayerStats.money >=
                currentQuantity * GameData.Instance.GameFish.price)
            {
                GameData.Instance.GamePlayerStats.money -= currentQuantity * GameData.Instance.GameFish.price;
                GameData.Instance.GameFish.quantity += currentQuantity;
            }   else
            {
                ErrorShowing.ShowError("You don't have the Money!", Input.mousePosition, 5f);
            }
        } 
        else 
        {
            if (GameData.Instance.GamePlayerStats.money >=
                currentQuantity * rightSideStore.GetComponent<PassMarketItems>().plant.plantPrice)
            {
                GameData.Instance.GamePlayerStats.money -= currentQuantity * rightSideStore.GetComponent<PassMarketItems>().plant.plantPrice;
                rightSideStore.GetComponent<PassMarketItems>().plant.quantity += currentQuantity;
            }   else
            {
                ErrorShowing.ShowError("You don't have the Money!", Input.mousePosition, 5f);
            }
        }
        OpenPlayerStats.Instance.updateMoney();
    }
    
    public void SellResource()
    {
        Debug.Log("Selling resource");
        GameObject  rightSideStore = GameObject.Find("ResourceRightSideStore");
        TextMeshProUGUI resourceText = rightSideStore.transform.Find("ResourceQuantity").GetComponent<TextMeshProUGUI>();
        int currentQuantity = int.Parse(resourceText.text);
        if (rightSideStore.GetComponent<PassMarketItems>().isSeed)
        {
            if (rightSideStore.GetComponent<PassMarketItems>().plant.seedQuantity < currentQuantity) 
            {
                ErrorShowing.ShowError("You don't have the quantity!", Input.mousePosition, 5f);
            }
            else
            {
                rightSideStore.GetComponent<PassMarketItems>().plant.seedQuantity -= currentQuantity;
                GameData.Instance.GamePlayerStats.money += currentQuantity * rightSideStore.GetComponent<PassMarketItems>().plant.seedPrice;
            }
        }   
        else if (rightSideStore.GetComponent<PassMarketItems>().isFish)
        {
            if (GameData.Instance.GameFish.quantity < currentQuantity)
            {
                ErrorShowing.ShowError("You don't have the quantity!", Input.mousePosition, 5f);
            }   else
            {
                GameData.Instance.GameFish.quantity -= currentQuantity;
                GameData.Instance.GamePlayerStats.money += currentQuantity * GameData.Instance.GameFish.price;
            }
        } 
        else
        {
            if (rightSideStore.GetComponent<PassMarketItems>().plant.quantity < currentQuantity) 
            {
                ErrorShowing.ShowError("You don't have the quantity!", Input.mousePosition, 5f);
            }
            else
            {
                rightSideStore.GetComponent<PassMarketItems>().plant.quantity -= currentQuantity;
                GameData.Instance.GamePlayerStats.money += currentQuantity * rightSideStore.GetComponent<PassMarketItems>().plant.plantPrice;
            }
        }
        OpenPlayerStats.Instance.updateMoney();
    }
}
