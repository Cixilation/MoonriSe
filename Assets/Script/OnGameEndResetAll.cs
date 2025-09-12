using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnGameEndResetAll : MonoBehaviour
{
    [SerializeField] private List<Tools> tools = new List<Tools>();
    [SerializeField] private List<Plants> resources = new List<Plants>();
    [SerializeField] private PlayerStats playerStats;
    [SerializeField] private Plants wood;
    [SerializeField] private Plants berries;
    [SerializeField] private Plants bamboo;
    [SerializeField] private Plants tomato;
    [SerializeField] private FishScriptableObject fish;

    private void Awake()
    {
        ResetGame();
    }

    private void Start()
    {
        ResetGame();
    }

    public void ResetGame()
    {
        foreach (Tools tool in tools)
        {
            if (tool.toolName != "Axe")
            {
                tool.isToolOwned = false;
            }
        }
        foreach (Plants resource in resources)
        {
            resource.quantity = 0;
            resource.seedQuantity = 0;
        }
        playerStats.money = 100;
        playerStats.experience = 0;
        playerStats.level = 0;
        playerStats.experience = 0;
        playerStats.hasDied = false;
        tomato.timeToLive = 180;
        berries.timeToLive = 300;
        bamboo.timeToLive = 600;
        fish.quantity = 0;
        
        // MaximumHealth(level)=BaseHealth+(Level×HealthIncrement)
        playerStats.maximumHealth = 1000 + (playerStats.level * 200);
        // ExperienceRequired(level)=BaseExperience×(Level ^GrowthFactor)
        playerStats.maximumExperienceToLevelUp = Mathf.FloorToInt(150 * Mathf.Pow(playerStats.level , 1.5f)) * GameData.Instance.GameLevel.worldLevel;
        
        playerStats.health = playerStats.maximumHealth;
    }
}

