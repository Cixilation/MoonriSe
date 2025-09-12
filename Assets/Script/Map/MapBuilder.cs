using System;
using UnityEngine;
using System.Collections.Generic;
using Random = UnityEngine.Random;

[System.Serializable]
public class PlacedObjectData
{
    public string objectName;
    public Vector3 position;

    public PlacedObjectData(string name, Vector3 pos)
    {
        objectName = name;
        position = pos;
    }
}

public class MapBuilder : MonoBehaviour
{
    [SerializeField] private Color gridColor = Color.green;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private GameObject normalTreePrefab;
    [SerializeField] private GameObject palmTreePrefab;
    public static MapBuilder instance;

    private void Awake()
    {
        instance = this;
    }

    private List<PlacedObjectData> placedObjectsData = new List<PlacedObjectData>();

    // private void OnDrawGizmos()
    // {
    //     Gizmos.color = gridColor;
    //     for (int x = 40; x < GridData.Instance.GridWidth - 40; x++)
    //     {
    //         for (int z = 40; z < GridData.Instance.GridHeight - 40; z++)
    //         {
    //             Vector3 gridPosition = new Vector3(x * GridData.Instance.GridSize - 40, 0f, z * GridData.Instance.GridSize - 40);
    //             gridPosition += new Vector3((x - 1) *  GridData.Instance.GridSize / 2, 0, (z - 1) *  GridData.Instance.GridSize / 2);
    //
    //             Gizmos.DrawCube(gridPosition, Vector3.one * GridData.Instance.GridSize);
    //         }
    //     }
    // }

    public bool PlaceObjectOnGrid(GameObject objectPrefab, int gridX, int gridZ, int objectSizeX = 1, int objectSizeZ = 1)
    {

        if (gridX <  GridData.Instance.BorderOffset || gridX >  (GridData.Instance.GridWidth + GridData.Instance.BorderOffset)  || gridZ < GridData.Instance.BorderOffset || gridZ >  (GridData.Instance.GridHeight + GridData.Instance.BorderOffset))
        {
            // Debug.Log("Cannot place object outside the valid grid area." + gridX + ", " + gridZ);
            return false;
        }

        Vector3 gridPosition = new Vector3(gridX *  GridData.Instance.GridSize, 50f, gridZ *  GridData.Instance.GridSize); 
        gridPosition += new Vector3((objectSizeX - 1) *  GridData.Instance.GridSize / 2, 0, (objectSizeZ - 1) *  GridData.Instance.GridSize / 2);

        Ray ray = new Ray(gridPosition, Vector3.down);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, groundLayer))
        {
            Vector3 finalPosition = new Vector3(gridPosition.x, hit.point.y +  GridData.Instance.ObjectHeightOffset, gridPosition.z);
            Vector3 halfExtents = new Vector3(objectSizeX *  GridData.Instance.GridSize / 2, 1f, objectSizeZ *  GridData.Instance.GridSize / 2);

            Collider[] colliders = Physics.OverlapBox(finalPosition, halfExtents);
            bool isOverlapWithNonTerrain = false;
            foreach (Collider collider in colliders)
            {
                if (collider.gameObject.name != "Terrain") 
                {
                    isOverlapWithNonTerrain = true;
                    // Debug.Log("Overlapping with: " + collider.gameObject.name);
                }
            }

            if (!isOverlapWithNonTerrain)
            {
                Quaternion randomRotation = Quaternion.Euler(0, Random.Range(0, 360), 0);
                Instantiate(objectPrefab, finalPosition, randomRotation);
                placedObjectsData.Add(new PlacedObjectData(objectPrefab.name, finalPosition));
                return true;
            }
            else
            {
                // Debug.Log("Cannot place " + objectPrefab.name + " at " + finalPosition + ": Area is occupied.");
                return false;
            }
        }
        else
        {
            Debug.Log("No ground detected at grid position.");
            return false;
        }
    }

    private void Start()
    {
        for (int i = 40; i < 240; i++)
        {
                PlaceObjectOnGrid(normalTreePrefab, RandomIndex(), RandomIndex(), 2, 2);
        }
        // spawnNewDomain();
        PlaceObjectOnGrid(palmTreePrefab, RandomIndex(), RandomIndex(), 2, 2); 
    }
    public int RandomIndex()
    {
        return Random.Range(40, 240);
    }

}
