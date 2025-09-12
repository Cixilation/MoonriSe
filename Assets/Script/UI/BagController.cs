using System;
using Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BagController : MonoBehaviour
{
    [SerializeField] private GameObject bagUI;
    [SerializeField] private GameObject EquipmentBagUI;
    [SerializeField] private GameObject ResourceBagUI;
    [SerializeField] private GameObject EquipmentButtonUI;
    [SerializeField] private GameObject ResourceButtonUI;
    [SerializeField] private CinemachineVirtualCamera bagCamera;
    private bool isBagOpen = false;

    private void Start()
    {
        ToggleBag();
        ToggleBag();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            if (bagCamera == null)
            {
                bagCamera = GameObject.Find("OpenBagCamera").GetComponent<CinemachineVirtualCamera>();
            }
            ToggleBag();
        }
    }

    private void ToggleBag()
    {
        isBagOpen = !isBagOpen;
        PlayerData.Instance.canWalk = !isBagOpen;
        if (SceneManager.GetActiveScene().name != "Dungeon")
        {
            if (isBagOpen)
            {
                bagCamera.Priority = 50;
            }
            else
            {
                bagCamera.Priority = 0;
            }
        }
        
        bagUI.SetActive(isBagOpen);
        
        Image buttonImage = EquipmentButtonUI.GetComponent<Button>().image;
        if (buttonImage != null)
        {
            Color currentColor = buttonImage.color;
            float h, s, v;
            Color.RGBToHSV(currentColor, out h, out s, out v);
            v = 1f;
            buttonImage.color = Color.HSVToRGB(h, s, v);
        }
        
        
        Image buttonImage2 = ResourceButtonUI.GetComponent<Button>().image;
        if (buttonImage2 != null)
        {
            Color currentColor = buttonImage2.color;
            float h, s, v;
            Color.RGBToHSV(currentColor, out h, out s, out v);
            v = 0.65f;
            buttonImage2.color = Color.HSVToRGB(h, s, v);
        }
       
        EquipmentBagUI.SetActive(true);
        ResourceBagUI.SetActive(false);
    }
}