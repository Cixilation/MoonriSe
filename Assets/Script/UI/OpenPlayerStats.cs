using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class OpenPlayerStats : MonoBehaviour
{
    [SerializeField] private GameObject UserPlayerStats;
    [SerializeField] private GameObject UserLevel;
    [SerializeField] private GameObject UserMoney;
    [SerializeField] private GameObject uiElement;
    [SerializeField] private GameObject uibackground;
    [SerializeField] private Slider HealthSlider;
    [SerializeField] private Slider ExpSlider;
    [SerializeField] private TextMeshProUGUI Money;
    [SerializeField] private TextMeshProUGUI Level;
    [SerializeField] private TextMeshProUGUI Experience;
    [SerializeField] private TextMeshProUGUI Health;
    [SerializeField] private Animator PlayerAnimator;
    
    public static OpenPlayerStats Instance;

    private void Awake()
    {
        Instance = this;
    }

    private bool isStatsVisible = false;

    private void Start()
    {
        UserPlayerStats.SetActive(false);
        updateMoney();
        UpdateMaximumHealth();
        UpdateMaximumExperience();
        GameObject.Find("DiedUserInterface").SetActive(false);
    }
    void Update()
    {
        // Debug.Log(UserPlayerStats.activeSelf);
        if (Input.GetKeyDown(KeyCode.C)) 
        {
            isStatsVisible = !isStatsVisible;
            PauseGame.instance.TogglePause();
            UserPlayerStats.SetActive(isStatsVisible);
        }
        
        Experience.text = FormatMoney(GameData.Instance.GamePlayerStats.experience);
        HealthSlider.value = Mathf.Lerp(HealthSlider.value, GameData.Instance.GamePlayerStats.health, Time.deltaTime * 5);
        ExpSlider.value = Mathf.Lerp(ExpSlider.value, GameData.Instance.GamePlayerStats.experience, Time.deltaTime * 5);
        CheckForLevelUp();

        if (HealthSlider.value <= 0)
        {    
            if (PlayerAnimator == null)
            {
                PlayerAnimator = GameObject.Find("F_AA_002").GetComponent<Animator>();
            }
            die();
            PlayerData.Instance.currentEquipmentName = "";
            PlayerData.Instance.equipSword = false;
        }
    }
    
    private void die()
    {
        if (GameData.Instance.GamePlayerStats.hasDied)
        {
            return;
        }
        GameData.Instance.GameLevel.worldLevel = 1;
        GameData.Instance.GamePlayerStats.hasDied = true;
        PlayerAnimator.SetTrigger("die");
        StartCoroutine(FadeExitOutUI());
    }
    IEnumerator FadeExitOutUI()
    {
        yield return new WaitForSeconds(2f);
        uiElement.SetActive(true);
        uibackground.SetActive(true);
        CanvasGroup canvasGroup = uiElement.GetComponent<CanvasGroup>();
        if (!canvasGroup)
        {
            canvasGroup = uiElement.AddComponent<CanvasGroup>();
        }
        canvasGroup.alpha = 1;

        float fadeDuration = 1.0f;
        float fadeDelay = 2.0f; 

        yield return new WaitForSeconds(fadeDelay); 

        // Fade out effect
        float elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(1, 0, elapsedTime / fadeDuration);
            yield return null; 
        }
        canvasGroup.alpha = 0;
        uiElement.SetActive(false); 
        uibackground.SetActive(false);
        if (GameData.Instance != null)
        {
            Destroy(GameData.Instance.gameObject);
            GameData.Instance = null;
        }

        SceneManager.LoadScene("Opening");
    }

    public void updateMoney()
    {
        Money.text = FormatMoney(GameData.Instance.GamePlayerStats.money);
        UserMoney.GetComponent<TextMeshProUGUI>().text = FormatMoney(GameData.Instance.GamePlayerStats.money);
    }
    private void UpdateMaximumHealth()
    {
        // Player Health
        GameData.Instance.GamePlayerStats.maximumHealth = 1000 + 
                                                          (GameData.Instance.GamePlayerStats.level * 200);
        HealthSlider.maxValue = GameData.Instance.GamePlayerStats.maximumHealth;
        GameData.Instance.GamePlayerStats.health = GameData.Instance.GamePlayerStats.maximumHealth;
        HealthSlider.value = HealthSlider.maxValue;
        Health.text =  FormatMoney(GameData.Instance.GamePlayerStats.maximumHealth);
    }
    private void UpdateMaximumExperience()
    {
        // Player Experience
        GameData.Instance.GamePlayerStats.level += 1;
        Level.text = "lv." + GameData.Instance.GamePlayerStats.level;
        UserLevel.GetComponent<TextMeshProUGUI>().text = GameData.Instance.GamePlayerStats.level.ToString();
        
        GameData.Instance.GamePlayerStats.maximumExperienceToLevelUp = Mathf.FloorToInt(150 * 
            Mathf.Pow(GameData.Instance.GamePlayerStats.level, 1.5f)) * GameData.Instance.GameLevel.worldLevel;
        
        ExpSlider.value = Mathf.Lerp(ExpSlider.value, GameData.Instance.GamePlayerStats.experience, Time.deltaTime * 5);
        ExpSlider.maxValue = GameData.Instance.GamePlayerStats.maximumExperienceToLevelUp;

    }
    private void CheckForLevelUp()
    {
        if (GameData.Instance.GamePlayerStats.experience >= GameData.Instance.GamePlayerStats.maximumExperienceToLevelUp)
        {
            GameData.Instance.GamePlayerStats.experience -= GameData.Instance.GamePlayerStats.maximumExperienceToLevelUp;
            UpdateMaximumHealth();
            UpdateMaximumExperience();
        }
    }

    private string FormatMoney(float amount)
    {
        if (amount >= 1_000_000_000)
            return $"241B+";
        else if (amount >= 1_000_000)
            return $"{Math.Round(amount / 1_000_000, 2)}M";
        else if (amount >= 1_000)
            return $"{Math.Round(amount / 1_000, 2)}K";
        else
            return amount.ToString();
    }
    
}
