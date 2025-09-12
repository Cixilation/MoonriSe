using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class InstantiateDrops : MonoBehaviour
{ 
    [SerializeField] private Sprite berry;
    [SerializeField] private Sprite exp;
    [SerializeField] private Sprite tomato;
    [SerializeField] private Sprite bamboo;
    [SerializeField] private Sprite wood;

    [SerializeField] private GameObject dropAbleObject;
    
    public static InstantiateDrops instance;

    private void Awake()
    {
        instance = this;
    }
    
    public void InstantiateDrop(Vector3 position, string dropObjectName, int expMultiplier)
    {
        FollowPlayer followPlayer;
        float randomX;
        float randomZ;
        Vector3 randomizedPosition;
        int i = Random.Range(1, 4);
        for (int j = 0; j < i; j++)
        {
            randomX = Random.Range(0f, 1.5f) * (Random.value > 0.5f ? 1 : -1); 
            randomZ = Random.Range(0f, 1.5f) * (Random.value > 0.5f ? 1 : -1);
            randomizedPosition = position + new Vector3(randomX, 0f, randomZ);
            followPlayer = dropAbleObject.GetComponent<FollowPlayer>();
            followPlayer.dropObjectName = dropObjectName;
            Debug.Log($"Instantiated drop at: {randomizedPosition}");
            if (dropObjectName == "wood")
            {
                dropAbleObject.GetComponent<SpriteRenderer>().sprite = wood;
            } else if (dropObjectName == "Tomato")
            {
                dropAbleObject.GetComponent<SpriteRenderer>().sprite = tomato;
            } else if (dropObjectName == "Bamboo")
            {
                dropAbleObject.GetComponent<SpriteRenderer>().sprite = bamboo; 
            } else if (dropObjectName == "exp")
            {
                followPlayer.expMultiplier = (int) 150 * (expMultiplier / 2);
                dropAbleObject.GetComponent<SpriteRenderer>().sprite = exp; 
            } else if (dropObjectName == "Berries")
            {
                dropAbleObject.GetComponent<SpriteRenderer>().sprite = berry;
            }
            else
            {
                Debug.Log("What kind of object from the drop object?");
            }
            Instantiate(dropAbleObject, randomizedPosition, Quaternion.identity);
        }
    }
}
