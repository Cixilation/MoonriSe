using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Teleporter : MonoBehaviour
{
    [SerializeField] private string targetSceneName; 
    [SerializeField]  private GameObject background;
    private GameObject canvasObject;
    private TextMeshProUGUI difficultyText;

    private void Start()
    {
        canvasObject = GameObject.Find("Confirmation");
        canvasObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (canvasObject == null)
            {
                canvasObject = GameObject.Find("Confirmation");
            }
            canvasObject.gameObject.SetActive(true);
            background.gameObject.SetActive(true);
            PauseGame.instance.TogglePause();
        }
    }

    public void CancelTeleport()
    {
        canvasObject.gameObject.SetActive(false);
        background.gameObject.SetActive(false);
        PauseGame.instance.TogglePause();
    }

    public void ChangeTargetScene()
    {
        SceneManager.LoadScene(targetSceneName);

    }
}