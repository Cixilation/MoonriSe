using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FoodHealthRegen : MonoBehaviour
{
   public void RestoreHealth(TextMeshProUGUI FoodNameText)
   {
      string FoodName = FoodNameText.text;
      if (FoodName == "Tomato")
      {
         GameData.Instance.GameTomato.quantity -= 1;
         GameData.Instance.GamePlayerStats.health += GameData.Instance.GamePlayerStats.maximumHealth * 0.05f;
      } else if (FoodName == "Berries")
      {
         GameData.Instance.GameBerry.quantity -= 1;
         GameData.Instance.GamePlayerStats.health += GameData.Instance.GamePlayerStats.maximumHealth * 0.1f;
      } else if (FoodName == "Bamboo")
      {
         GameData.Instance.GameBamboo.quantity -= 1;
         GameData.Instance.GamePlayerStats.health += GameData.Instance.GamePlayerStats.maximumHealth * 0.15f;  
      }  else if (FoodName == "Fish")
      {
         GameData.Instance.GameFish.quantity -= 1;
         GameData.Instance.GamePlayerStats.health += GameData.Instance.GamePlayerStats.maximumHealth * 0.07f;
      }
      GameObject.Find("ResourceGridLayout").GetComponent<SpawnOwnedResources>().RefreshInventoryUI();
   }
}
