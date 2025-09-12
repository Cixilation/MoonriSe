using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class TheFish : MonoBehaviour
{
    [SerializeField] private List<GameObject> FishPrefabs;
    [SerializeField] private GameObject FishingArea;
    private int FishCount = 100;

    private List<GameObject> spawnedFishes = new List<GameObject>();
    private Dictionary<GameObject, Vector3> fishDirections = new Dictionary<GameObject, Vector3>();
    private Bounds fishingAreaBounds;

    private void Start()
    {
        Collider fishingAreaCollider = FishingArea.GetComponent<Collider>();
        if (fishingAreaCollider != null)
        {
            fishingAreaBounds = fishingAreaCollider.bounds;
            SpawnFish();
        }
        else
        {
            Debug.LogError("Fishing Area must have a Collider!");
        }
    }

    private void Update()
    {
        MoveFish();
    }

    private void SpawnFish()
    {
        for (int i = 0; i < FishCount; i++)
        {
            GameObject fishPrefab = FishPrefabs[Random.Range(0, FishPrefabs.Count)];
            Vector3 spawnPosition = GetRandomPositionInFishingArea();
            GameObject spawnedFish = Instantiate(fishPrefab, spawnPosition, Quaternion.identity, FishingArea.transform);

            spawnedFishes.Add(spawnedFish);
            Vector3 randomDirection = GetRandomDirection();
            fishDirections[spawnedFish] = randomDirection;
            
        }
    }  
    private void DeSpawnFish()
    {
        foreach (GameObject fish in spawnedFishes)
        {
            Destroy(fish);
        }  
    }

    private Vector3 GetRandomPositionInFishingArea()
    {
        float randomX = Random.Range(fishingAreaBounds.min.x, fishingAreaBounds.max.x);
        float randomZ = Random.Range(fishingAreaBounds.min.z, fishingAreaBounds.max.z);
        float lockedY = fishingAreaBounds.center.y;
        return new Vector3(randomX, lockedY, randomZ);
    }

    private Vector3 GetRandomDirection()
    {
        float randomX = Random.Range(-1f, 1f);
        float randomZ = Random.Range(-1f, 1f);
        return new Vector3(randomX, 0, randomZ).normalized;
    }

    private void MoveFish()
    {
        foreach (GameObject fish in spawnedFishes)
        {
            if (fish == null) continue;

            Vector3 currentDirection = fishDirections[fish];
            float speed = Random.Range(2f, 5f);
            Vector3 newPosition = fish.transform.position + currentDirection * (speed * Time.deltaTime);

            newPosition.x = Mathf.Clamp(newPosition.x, fishingAreaBounds.min.x, fishingAreaBounds.max.x);
            newPosition.z = Mathf.Clamp(newPosition.z, fishingAreaBounds.min.z, fishingAreaBounds.max.z);
            newPosition.y = fishingAreaBounds.center.y;

            fish.transform.position = newPosition;

            if (newPosition.x <= fishingAreaBounds.min.x || newPosition.x >= fishingAreaBounds.max.x)
            {
                currentDirection.x = -currentDirection.x;
            }
            if (newPosition.z <= fishingAreaBounds.min.z || newPosition.z >= fishingAreaBounds.max.z)
            {
                currentDirection.z = -currentDirection.z;
            }

            fishDirections[fish] = currentDirection;
        }
    }
}
