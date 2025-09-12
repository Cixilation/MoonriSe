using UnityEngine;

public class SetAllUIOff : MonoBehaviour
{
    [SerializeField] private GameObject interactableUI;
    [SerializeField] private GameObject storeUI;
    [SerializeField] private GameObject uiBackground;   
    [SerializeField] private GameObject inventoryUI;
    [SerializeField] private GameObject farmUI;
    [SerializeField] private GameObject fishingPanel;

    void Start()
    {
        if (interactableUI != null) interactableUI.SetActive(false);
        if (storeUI != null) storeUI.SetActive(false);
        if (uiBackground != null) uiBackground.SetActive(false);
        if (inventoryUI != null) inventoryUI.SetActive(false);
        if (farmUI != null) farmUI.SetActive(false);
        if (fishingPanel != null) fishingPanel.SetActive(false);
    }
}