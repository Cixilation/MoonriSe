using Cinemachine;
using Unity.VisualScripting;
using UnityEngine;

public class SwitchCamera : MonoBehaviour
{
    private static readonly int Change = Animator.StringToHash("Change");
    [SerializeField] private GameObject cameraOne;
    [SerializeField] private GameObject cameraTwo;

    private bool isCameraOneActive = true;

    public void ToggleCamera()
    {
        GetComponent<Animator>().SetTrigger(Change);
        if(cameraOne == null)
        {
            cameraOne = GameObject.Find("Player").transform.Find("Main Camera").gameObject;
        }

        if (cameraTwo == null)
        {
            cameraTwo = GameObject.Find("BuilderCam");
        }
        if (isCameraOneActive)
        {
            // cameraOne.GetComponent<CinemachineFreeLook>().Priority = 0;
            // cameraTwo.GetComponent<CinemachineFreeLook>().Priority = 200;
            cameraOne.SetActive(false);
            cameraTwo.SetActive(true);
            Cursor.visible = true;  
            Cursor.lockState = CursorLockMode.None; 
        }
        else
        {
            // cameraOne.GetComponent<CinemachineFreeLook>().Priority = 50;
            // cameraTwo.GetComponent<CinemachineFreeLook>().Priority = 0;
            cameraOne.SetActive(true);
            cameraTwo.SetActive(false);
            // Cursor.visible = false;  
            // Cursor.lockState = CursorLockMode.Locked; 
        }

        isCameraOneActive = !isCameraOneActive;
    }
}