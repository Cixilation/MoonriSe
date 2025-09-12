using System;
using System.Collections;
using UnityEngine;

public class PlantController : MonoBehaviour
{
    [SerializeField] private Plants plantData;

    public event Action OnPhaseChange; 

    public Plants PlantData
    {
        get { return plantData; }
    }

    private bool isAvailable = true;

    public bool IsAvailable
    {
        get { return isAvailable; }
        set { isAvailable = value; }
    }

    private int currentPhase = 0;
    private float phaseDuration;
    private GameObject _currentPlantInstance;

    public GameObject CurrentPlantInstance
    {
        get { return _currentPlantInstance; }
        set { _currentPlantInstance = value; }
    }

    public bool IsReadyToCollect => currentPhase == plantData.phases;
    public void SetPlantData(Plants newPlantData)
    {
        IsAvailable = false;
        if (newPlantData == null)
        {
            Debug.LogError("Plant data cannot be null!");
            return;
        }
        plantData = newPlantData;
        phaseDuration = (float)plantData.timeToLive / plantData.phases;
        currentPhase = 0;
        if (_currentPlantInstance != null)
        {
            Destroy(_currentPlantInstance);
        }
        SpawnPhase(currentPhase);
        StartCoroutine(GrowPlant());
    }

    private IEnumerator GrowPlant()
    {
        Debug.Log("Plant has started to grow ");
        for (currentPhase = 0; currentPhase < plantData.phases; currentPhase++)
        {
            yield return new WaitForSeconds(phaseDuration);
            SpawnPhase(currentPhase + 1);
            Debug.Log("Current Phase " + currentPhase + "/" +  plantData.phases);
        }
        Debug.Log("Plant is fully grown.");
        OnPhaseChange?.Invoke(); 
    }

    public void RemovePlant()
    {
        StopCoroutine(nameof(GrowPlant));
        if (_currentPlantInstance != null)
        {
            Destroy(_currentPlantInstance);
        }
        plantData = null;
        currentPhase = 0;
        phaseDuration = 0;
        IsAvailable = true;
        OnPhaseChange?.Invoke();
    }

    private void SpawnPhase(int phaseIndex)
    {
        if (_currentPlantInstance != null)
        {
            Destroy(_currentPlantInstance);
        }
        if (phaseIndex >= 0 && phaseIndex < plantData.plantsPhase.Count)
        {
            _currentPlantInstance = Instantiate(plantData.plantsPhase[phaseIndex], transform.position, Quaternion.identity, transform);
        }
    }

    public void CollectPlant()
    {
        InstantiateDrops.instance.InstantiateDrop(this.transform.position, plantData.plantName, 0);
    }
}
