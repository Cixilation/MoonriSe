    using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitingDungeon : MonoBehaviour
{  
    private GameObject canvasObject;
    [SerializeField] private GameObject canvasObject2;
    private TextMeshProUGUI difficultyText; 

    private void Start()
    {
        canvasObject = GameObject.Find("ExitConfirmation"); 
        canvasObject.SetActive(false);
        canvasObject2.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            canvasObject.SetActive(true);
            canvasObject2.SetActive(true);
            PauseGame.instance.TogglePause();
        }
    }

    public void CancelTeleport()
    {
        canvasObject.SetActive(false);
        canvasObject2.SetActive(false);
        PauseGame.instance.TogglePause();
    }

    public void ChangeTargetScene()
    {
        GameData.Instance.GameLevel.worldLevel += 1;  
        if (GameData.Instance != null)
        {
            Destroy(GameData.Instance.gameObject);
            GameData.Instance = null;
        }
        
        SceneManager.LoadScene("Opening");
    }
}