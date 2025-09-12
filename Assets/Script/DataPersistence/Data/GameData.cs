using System;
using System.Collections.Generic;
using UnityEngine;

public class GameData : MonoBehaviour
{
    private static GameData instance;

    public static GameData Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<GameData>();

                if (instance == null)
                {
                    GameObject singletonObject = new GameObject("GameData");
                    instance = singletonObject.AddComponent<GameData>();
                }

                DontDestroyOnLoad(instance.gameObject);
            }

            return instance;
        }
        set
        {
            instance = value;
        }
    }

    [SerializeField] private List<Tools> tools = new List<Tools>();
    [SerializeField] private List<Plants> resources = new List<Plants>();
    [SerializeField] private List<FarmArea> farmAreas = new List<FarmArea>();
    [SerializeField] private PlayerStats playerStats;

    [SerializeField] private Plants wood;
    [SerializeField] private Plants berries;
    [SerializeField] private Plants bamboo;
    [SerializeField] private Plants tomato;
    [SerializeField] private FishScriptableObject fish;
    [SerializeField] private WorldScriptableObject world;
    public List<Tools> GameTools => tools;
    public List<Plants> GameResources => resources;
    public List<FarmArea> GameFarmAreas => farmAreas;
    public PlayerStats GamePlayerStats => playerStats;
    public Plants GameWood => wood;
    public Plants GameBerry => berries;
    public Plants GameBamboo => bamboo;
    public Plants GameTomato => tomato;

    public WorldScriptableObject GameLevel => world;

    public FishScriptableObject GameFish => fish;
    
    private void Awake()
    {
        GameTomato.timeToLive = 180;
        GameBerry.timeToLive = 300;
        GameBamboo.timeToLive = 600;
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }
}