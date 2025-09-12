using System;
using UnityEngine;

public class FarmingArea : MonoBehaviour
{
    [SerializeField] private SwitchCamera switchCameraScript;
    [SerializeField] private CameraController cameraControllerScript;
    [SerializeField] private GameObject PlaceUI;
    [SerializeField] private GameObject Player;

    private void Start()
    {
        switchCameraScript.ToggleCamera();
        switchCameraScript.ToggleCamera();
        cameraControllerScript.isActive = !cameraControllerScript.isActive;
        cameraControllerScript.isActive = !cameraControllerScript.isActive;
    }

    void Update()
    {
        if (cameraControllerScript == null)
        {
            cameraControllerScript = GameObject.Find("BuilderCam").GetComponent<CameraController>();
        }

        if (Player == null)
        {
            Player = GameObject.FindWithTag("Player");
        }
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            cameraControllerScript.SetCameraPosition(Player.transform.position);
            switchCameraScript.ToggleCamera();
            cameraControllerScript.isActive = !cameraControllerScript.isActive;
            PlaceUI.gameObject.SetActive(cameraControllerScript.isActive);
            PlayerData.Instance.builderMode = cameraControllerScript.isActive;
        }
    }



   
}