using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PassMarketItems : MonoBehaviour
{
    public Plants plant;
    public bool isSeed;
    public bool isFish;
    public FishScriptableObject fish;
    public void setResource()
    {
        GameObject  rightSideStore = GameObject.Find("ResourceRightSideStore");
        rightSideStore.GetComponent<PassMarketItems>().plant = plant;
        Transform itemImageBackgroundTransform = rightSideStore.transform.Find("ResourceSpriteBackground");
        Transform itemImageTransform = itemImageBackgroundTransform.transform.Find("ResourceSprite");
        Transform resourceName = rightSideStore.transform.Find("ResourceName");
    
        Transform resourceMoney = rightSideStore.transform.Find("ResourcePrice");
        Transform resourceQuantity = rightSideStore.transform.Find("ResourceQuantity");
        Transform OwnedLabel = rightSideStore.transform.Find("OwnedLabel");
        
        resourceQuantity.GetComponent<TextMeshProUGUI>().text = 1.ToString();
        resourceMoney.GetComponent<TextMeshProUGUI>().color = new Color32(254, 234, 89, 255); 
        if (isSeed)
        {
            rightSideStore.GetComponent<PassMarketItems>().isSeed = true;
            rightSideStore.GetComponent<PassMarketItems>().isFish = false;
            resourceName.GetComponent<TextMeshProUGUI>().text = plant.plantName + " Sapling";
            resourceMoney.GetComponent<TextMeshProUGUI>().text = plant.seedPrice.ToString();
            itemImageTransform.GetComponent<Image>().sprite = plant.seedSprite;
            OwnedLabel.GetComponent<TextMeshProUGUI>().text = plant.seedQuantity.ToString() + " owned";
        }  else if (isFish)
        {
            rightSideStore.GetComponent<PassMarketItems>().isSeed = false;
            rightSideStore.GetComponent<PassMarketItems>().isFish = true;
            itemImageTransform.GetComponent<Image>().sprite = GameData.Instance.GameFish.fishSprite;
            resourceName.GetComponent<TextMeshProUGUI>().text = GameData.Instance.GameFish.fishName;
            resourceMoney.GetComponent<TextMeshProUGUI>().text = GameData.Instance.GameFish.price.ToString();
            OwnedLabel.GetComponent<TextMeshProUGUI>().text = GameData.Instance.GameFish.quantity.ToString()  + " owned";
        }
        else
        {
            rightSideStore.GetComponent<PassMarketItems>().isSeed = false;
            rightSideStore.GetComponent<PassMarketItems>().isFish = false;
            itemImageTransform.GetComponent<Image>().sprite = plant.plantSprite;
            resourceName.GetComponent<TextMeshProUGUI>().text = plant.plantName;
            resourceMoney.GetComponent<TextMeshProUGUI>().text = plant.plantPrice.ToString();
            OwnedLabel.GetComponent<TextMeshProUGUI>().text = plant.quantity.ToString()  + " owned";
        }
    }

    public void AddQuantity()
    {
        GameObject  rightSideStore = GameObject.Find("ResourceRightSideStore");
        TextMeshProUGUI resourceText = rightSideStore.transform.Find("ResourceQuantity").GetComponent<TextMeshProUGUI>();
        int currentQuantity = int.Parse(resourceText.text);
        resourceText.text = (currentQuantity + 1).ToString();
        if (rightSideStore.GetComponent<PassMarketItems>().isSeed)
        {
            rightSideStore.transform.Find("ResourcePrice").GetComponent<TextMeshProUGUI>().text = ((currentQuantity + 1) * rightSideStore.GetComponent<PassMarketItems>().plant.seedPrice).ToString();
        }
        else
        {
            rightSideStore.transform.Find("ResourcePrice").GetComponent<TextMeshProUGUI>().text = ((currentQuantity + 1) * rightSideStore.GetComponent<PassMarketItems>().plant.plantPrice).ToString();
        }
    }
    public void RemoveQuantity()
    {
       
        GameObject  rightSideStore = GameObject.Find("ResourceRightSideStore");
        TextMeshProUGUI resourceText = rightSideStore.transform.Find("ResourceQuantity").GetComponent<TextMeshProUGUI>();
        int currentQuantity = int.Parse(resourceText.text);
        if (currentQuantity > 1)
        {
    
            resourceText.text = (currentQuantity - 1).ToString();
            if (rightSideStore.GetComponent<PassMarketItems>().isSeed)
            {
                rightSideStore.transform.Find("ResourcePrice").GetComponent<TextMeshProUGUI>().text = ((currentQuantity - 1) * rightSideStore.GetComponent<PassMarketItems>().plant.seedPrice).ToString();
            }
            else
            {
                rightSideStore.transform.Find("ResourcePrice").GetComponent<TextMeshProUGUI>().text = ((currentQuantity - 1) * rightSideStore.GetComponent<PassMarketItems>().plant.plantPrice).ToString();
            }
        }
       
    }
}
