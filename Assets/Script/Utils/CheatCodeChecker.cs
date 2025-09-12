using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CheatCodeChecker : MonoBehaviour
{
    [SerializeField] public string buffer;
    [SerializeField] private float maxTimerDif = 1;
    [SerializeField] private GameObject cheatCodeCheckerUI;
    [SerializeField] private GameObject UIBackground;
    
    private List<string> validPatterns = new List<string>() {"duaempatsatuu", "poksupremacy", "makemixyuey", "subcoandsbadut", "casemakernangis", "secreout", "secrein", "fishingmania" };
    private float timeDif;
    
    private void Start()
    {
        timeDif = maxTimerDif;
        cheatCodeCheckerUI.SetActive(false);
        UIBackground.SetActive(false);
    }

    private void Update()
    {
        timeDif -= Time.deltaTime;
        if (timeDif <= 0)
        {
            buffer = "";
        }

        if (Input.anyKeyDown)
        {
            string key = Input.inputString; 
            if (!string.IsNullOrEmpty(key))
            {
                addToBuffer(key.ToLower()); 
            }
        }
    }

    void addToBuffer(string c)
    {
        timeDif = maxTimerDif;
        buffer += c;
        checkPatterns();
    }

    void checkPatterns()
    {

        if (buffer.EndsWith(validPatterns[0]))
        {
            showCheatCodeCheckerUI(validPatterns[0]);
            foreach (Tools tool in GameData.Instance.GameTools)
            {
                tool.isToolOwned = true;
            }
        } else if (buffer.EndsWith(validPatterns[1]))
        {
            showCheatCodeCheckerUI(validPatterns[1]);
            foreach (Plants plants in GameData.Instance.GameResources)
            {
                if (plants.plantName == "Wood")
                {
                    plants.quantity += 24100;
                    continue;
                }
                plants.quantity += 241;
                plants.seedQuantity += 241;
            }
            GameData.Instance.GameFish.quantity += 241;
        } else if (buffer.EndsWith(validPatterns[2]))
        {
            showCheatCodeCheckerUI(validPatterns[2]);
            GameData.Instance.GamePlayerStats.money = 241241241;
            OpenPlayerStats.Instance.updateMoney();
        } else if (buffer.EndsWith(validPatterns[3]))
        {
            showCheatCodeCheckerUI(validPatterns[3]);
            GameData.Instance.GamePlayerStats.experience += 241241241;
        } else if (buffer.EndsWith(validPatterns[4]))
        {
            showCheatCodeCheckerUI(validPatterns[4]);
            GameData.Instance.GameTomato.timeToLive = 10;
            GameData.Instance.GameBerry.timeToLive = 10;
            GameData.Instance.GameBamboo.timeToLive = 10;
        } else if (buffer.EndsWith(validPatterns[5]))
        {
            showCheatCodeCheckerUI(validPatterns[5]);

            GameObject exitPoint = GameObject.Find("ExitPoint");
            if (exitPoint != null)
            {
                GameObject player = GameObject.FindWithTag("Player");
                if (player != null)
                {
                    player.transform.position = exitPoint.transform.position + new Vector3(0f, 0.5f, 0);
                }
            }
        } else if (buffer.EndsWith(validPatterns[6]))
        {
            showCheatCodeCheckerUI(validPatterns[6]);

            GameObject exitPoint = GameObject.Find("SpawnToTeleporter");
            if (exitPoint != null)
            {
                GameObject player = GameObject.FindWithTag("Player");
                if (player != null)
                {
                    player.transform.position = exitPoint.transform.position + new Vector3(0f, 0.5f, 0);
                }
            }
        } else if (buffer.EndsWith(validPatterns[7]))
        {
            showCheatCodeCheckerUI(validPatterns[7]);

            GameObject exitPoint = GameObject.Find("FishSpawnPoint");
            if (exitPoint != null)
            {
                GameObject player = GameObject.FindWithTag("Player");
                if (player != null)
                {
                    player.transform.position = exitPoint.transform.position + new Vector3(0f, 0.5f, 0);
                }
            }
        }

    }


    void showCheatCodeCheckerUI(string cheatCode)
    {
        UIBackground.SetActive(true);
        cheatCodeCheckerUI.SetActive(true);
        Transform cheatDescription = cheatCodeCheckerUI.transform.Find("Cheat");
        cheatDescription.GetComponentInChildren<TextMeshProUGUI>().text = cheatCode;
        StartCoroutine(FadeOutUI(cheatCodeCheckerUI));
    }
    IEnumerator FadeOutUI(GameObject uiElement)
    {
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
        UIBackground.SetActive(false);
    }
}
