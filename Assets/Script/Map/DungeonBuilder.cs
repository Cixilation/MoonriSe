using System;
using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;
using Random = UnityEngine.Random;

public enum Category
{
    floor,
    room,
    empty,
    door
}


public class Node : IHeap<Node>
{
    public Vector3 position;
    public int gridX;
    public int gridY;
    public float gCost; 
    public float hCost;
    public Category nodeCategory;

    public float fCost
    {
        get
        {
            return gCost + hCost;
        }
    } 
    public Node parent;

    public Node(Vector3 _position, int _gridX, int _gridY)
    {
        position = _position;
        gridX = _gridX;
        gridY = _gridY;
        gCost = 0;
        hCost = 0;
        nodeCategory = Category.empty;
    }

    private int heapIndexe;
    public int heapIndex {
        get
        {
            return heapIndexe;
        }
        set
        {
            heapIndexe = value;
        }
    }

    public int CompareTo(Node other)
    {
        int compare = fCost.CompareTo(other.fCost);
        if (compare == 0)
        {
            compare = hCost.CompareTo(other.hCost);
        }
        return -compare;
    }
}

public class DungeonBuilder : MonoBehaviour
{    
    private Node[,] grid;
    private int smallRoomQuantity = 3;
    private int bigRoomQuantity = 2;
    private int mediumRoomQuantity = 2;
    [SerializeField] GameObject smallRoomPrefab;
    [SerializeField] GameObject mediumRoomPrefab;
    [SerializeField] GameObject largeRoomPrefab;
    [SerializeField] GameObject SpawnRoomPrefab;
    [SerializeField] GameObject ExitRoomPrefab;
    [SerializeField] NavMeshSurface navMeshSurface;
    
    
    [SerializeField] private GameObject floor;
    [SerializeField] private GameObject wall;
    
    private List<string> roomNames = new List<string>() {"spawn", "exit", "small", "small", "small", "medium", "medium", "large", "large" };

    private void Start()
    {
        CreateGrid();
        bool isSpawned = false;
        foreach (string room in roomNames)
        {
            while (!isSpawned)
            {
                isSpawned = SpawnRoom(room);
            }
            isSpawned = false;
        }
        FindPathToAllDoors();
        // Testing();
        SpawnFlooring();
        navMeshSurface.BuildNavMesh();
    }
    
    private void Testing()
    {
        grid = new Node[6, 6];
        for (int x = 0; x < 6; x++)
        {
            for (int z = 0; z < 6; z++)
            {
                Vector3 worldPosition = new Vector3(
                    x * GridData.Instance.GridSize, 
                    0, 
                    z * GridData.Instance.GridSize
                );
                grid[x, z] = new Node(worldPosition, x, z);
                grid[x, z].nodeCategory = Category.empty;
            }
        }

        grid[1, 1].nodeCategory = Category.floor;
        grid[2, 2].nodeCategory = Category.floor;
        grid[2, 3].nodeCategory = Category.floor;
        grid[3, 2].nodeCategory = Category.floor;
        grid[4, 2].nodeCategory = Category.floor;
        grid[4, 3].nodeCategory = Category.floor;
        grid[3, 4].nodeCategory = Category.floor;
        grid[2, 4].nodeCategory = Category.floor;
        grid[4, 4].nodeCategory = Category.floor;
    }
    

    private void SpawnFlooring()
    {
        for (int x = 0; x < GridData.Instance.GridWidth / 4; x++)
        {
            for (int z = 0; z < GridData.Instance.GridHeight / 4; z++)
            {
        // for (int x = 0; x < 6; x++)
        // {
        //     for (int z = 0; z < 6; z++)
        //     {
                if (grid[x, z].nodeCategory == Category.floor)
                {
                    Vector3 position = new Vector3(x * GridData.Instance.GridSize + 0.5f, 0f, z * GridData.Instance.GridSize + 0.2f);
                    Instantiate(floor, position - new Vector3(0.3f, 0f, 0f), Quaternion.identity);
                    Instantiate(floor, position + new Vector3(-0.2f, 2.9f, 2.5f), Quaternion.Euler(180, 0, 0));

                    if (grid[x - 1, z].nodeCategory == Category.empty)
                    {
                        //left
                        Instantiate(wall, position + new Vector3(-0.5f, 0f, 0f), Quaternion.Euler(0, 270, 0));
                    } 
                    if (grid[x + 1, z].nodeCategory == Category.empty)
                    {
                        //right
                        Instantiate(wall, position + new Vector3(GridData.Instance.GridSize - 0.25f, 0f, GridData.Instance.GridSize - 0.3f), Quaternion.Euler(0, 90, 0));
                    } 
                    if (grid[x, z - 1].nodeCategory == Category.empty)
                    {
                        //down
                        Instantiate(wall, position + new Vector3(GridData.Instance.GridSize - 0.5f - 0.02f, 0f, -0.288f), Quaternion.Euler(0, 180, 0));
                    }  
                    if (grid[x, z + 1].nodeCategory == Category.empty)
                    {
                        //up
                        Instantiate(wall, position + new Vector3(-0.3f + 0.074f, 0f, GridData.Instance.GridSize), Quaternion.Euler(0, 0, 0));
                    }
                }
            }
        }
    }

    
    private Node spawnRoomNode;

    private List<Node> doors = new List<Node>();
    private List<Node> notFoundPathdoors = new List<Node>();
    private List<Node> foundPathdoors = new List<Node>();
    private void FindPathToAllDoors()
    {
        foreach (Node door in doors)
        {
            notFoundPathdoors.Add(door);
        }

        if (doors == null || doors.Count < 2) return;
        notFoundPathdoors.AddRange(doors);

        while (notFoundPathdoors.Count > 0)
        {
            Node currentNode = notFoundPathdoors[0];

            Node endNode;
            do
            {
                endNode = doors[Random.Range(0, doors.Count)];
            } while (endNode == currentNode);

            bool found = false;
            int attempts = 0;
            int maxAttempts = 100;

            while (!found && attempts < maxAttempts)
            {
                found = FindPath(currentNode, endNode);
                attempts++;
            }

            if (!found)
            {
                Debug.LogWarning($"Failed to find a path from {currentNode} to {endNode} after {maxAttempts} attempts.");
                notFoundPathdoors.Remove(currentNode);
                continue;
            }
            notFoundPathdoors.Remove(currentNode);
            foundPathdoors.Add(currentNode);
        }
    }

    private bool SpawnRoom(string roomSize)
    {
        int randomIndexX = Random.Range(10, GridData.Instance.GridHeight / 4 - 10);
        int randomIndexY = Random.Range(10, GridData.Instance.GridWidth / 4 - 10);
        Room roomDescription = null;
        GameObject roomObject = null;
        switch (roomSize)
        {
            case "spawn":
                roomDescription = SpawnRoomPrefab.GetComponent<Room>();
                roomObject = SpawnRoomPrefab;
                break;
            case "exit":
                roomDescription = ExitRoomPrefab.GetComponent<Room>();
                roomObject = ExitRoomPrefab;
                break;
            case "small":
                roomDescription = smallRoomPrefab.GetComponent<Room>();
                roomObject = smallRoomPrefab;
                break;
            case "medium":
                roomDescription = mediumRoomPrefab.GetComponent<Room>();
                roomObject = mediumRoomPrefab;
                break;
            case "large":
                roomDescription = largeRoomPrefab.GetComponent<Room>();
                roomObject = largeRoomPrefab;
                break;
        }

        for (int i = randomIndexX -3 ; i < randomIndexX + roomDescription.roomSizeX + 2; i++)
        {
            for (int j = randomIndexY - 3; j < randomIndexY + roomDescription.roomSizeY + 2; j++)
            {
                if (i >= GridData.Instance.GridWidth / 4 || j >= GridData.Instance.GridHeight / 4 || grid[i, j].nodeCategory != Category.empty)
                {
                    return false;
                }
            }
        }

        if (roomSize == "exit" && spawnRoomNode != null)
        {
            int manhattanDistance = Mathf.Abs(randomIndexX - spawnRoomNode.gridX) + Mathf.Abs(randomIndexY - spawnRoomNode.gridY);
            if (manhattanDistance < 2)
            {
                return false;
            }
        }
        GameObject instantiatedroomObject = Instantiate(roomObject, new Vector3(randomIndexX * GridData.Instance.GridSize, 0f, randomIndexY * GridData.Instance.GridSize), Quaternion.identity);
        Room instantiatedRoom = instantiatedroomObject.GetComponent<Room>();
        instantiatedRoom.roomPositionX = randomIndexX;
        instantiatedRoom.roomPositionY = randomIndexY;
        for (int i = randomIndexX; i < randomIndexX + roomDescription.roomSizeX; i++)
        {
            for (int j = randomIndexY; j < randomIndexY + roomDescription.roomSizeY; j++)
            {
                grid[i, j].nodeCategory = Category.room;
            }
        }
        if (roomSize == "spawn")
        {
            spawnRoomNode = grid[randomIndexX, randomIndexY];
        }
        grid[randomIndexX + instantiatedRoom.doorPositionX - 1, randomIndexY + instantiatedRoom.doorPositionY - 1].nodeCategory = Category.door;
        doors.Add(grid[randomIndexX + instantiatedRoom.doorPositionX - 1, randomIndexY + instantiatedRoom.doorPositionY - 1]);

        return true;
    }
    
    private void CreateGrid()
    {
        grid = new Node[GridData.Instance.GridWidth / 4, GridData.Instance.GridHeight / 4];
        for (int x = 0; x < GridData.Instance.GridWidth / 4; x++)
        {
            for (int z = 0; z < GridData.Instance.GridHeight / 4; z++)
            {
                Vector3 worldPosition = new Vector3(
                    x * GridData.Instance.GridSize, 
                    0, 
                    z * GridData.Instance.GridSize
                );
                grid[x, z] = new Node(worldPosition, x, z);
            }
        }
    }
    
    public List<Node> GetNeighbours(Node node)
    {
        List<Node> neighbours = new List<Node>();
        for (int i = -1; i <= 1; i++)
        {
            for (int j = -1; j <= 1; j++)
            {
                if ((i == 0 && j == 0) || (Mathf.Abs(i) + Mathf.Abs(j)) == 2)
                {
                    continue;
                }
                int checkX = node.gridX + i;
                int checkY = node.gridY + j;
                if (checkX >= 0 && checkX < GridData.Instance.GridWidth / 4 && checkY >= 0 && checkY < GridData.Instance.GridHeight / 4)
                {
                    neighbours.Add(grid[checkX, checkY]);
                }
            }
        }
        return neighbours;
    }

    // void OnDrawGizmos()
    // {
    //     if (grid != null)
    //     {
    //         foreach (Node node in grid)
    //         {
    //             if (node.nodeCategory == Category.door)
    //             {
    //                 Gizmos.color = Color.green;
    //             } else if (node.nodeCategory == Category.room)
    //             {
    //                 Gizmos.color = Color.red;
    //             } else if (node.nodeCategory == Category.empty)
    //             {
    //                 Gizmos.color = Color.white;
    //             }
    //             else if (node.nodeCategory == Category.floor)
    //             {
    //                 Gizmos.color = Color.blue;
    //             }
    //             Gizmos.DrawCube(
    //                 node.position + new Vector3(GridData.Instance.GridSize / 2, 0, GridData.Instance.GridSize / 2),
    //                 Vector3.one * (GridData.Instance.GridSize - 0.1f)
    //             );
    //         }
    //     }
    // }
    public bool FindPath(Node startNode, Node endNode)
    {
        ResetNodes(grid);
        Heap<Node> openSet = new Heap<Node>(GridData.Instance.GridHeight / 4 * GridData.Instance.GridWidth / 4);
        HashSet<Node> closedSet = new HashSet<Node>();
        openSet.Add(startNode);
        while (openSet.Count > 0)
        {
            Node currentNode = openSet.RemoveFirst();
            closedSet.Add(currentNode);
            if (currentNode == endNode)
            {
                BacktrackPath(startNode, endNode);
                return true;
            }
            foreach (Node neighbor in GetNeighbours(currentNode))
            {
                if (closedSet.Contains(neighbor) || neighbor.nodeCategory == Category.room)
                    continue;

                float newCostToNeighbor = currentNode.gCost + GetDistance(currentNode, neighbor);
                if (newCostToNeighbor < neighbor.gCost || !openSet.Contains(neighbor))
                {
                    neighbor.gCost = newCostToNeighbor;
                    neighbor.hCost = GetDistance(neighbor, endNode);
                    neighbor.parent = currentNode;

                    if (!openSet.Contains(neighbor))
                        openSet.Add(neighbor);
                    else
                        openSet.UpdateItem(neighbor);
                }
            }
        }
        return false;
    }
    void ResetNodes(Node[,] grid)
    {
        for (int x = 0; x < grid.GetLength(0); x++)
        {
            for (int y = 0; y < grid.GetLength(1); y++)
            {
                Node node = grid[x, y];
                node.gCost = float.MaxValue;
                node.hCost = 0;
                node.parent = null;
            }
        }
    }

    private void BacktrackPath(Node startNode, Node endNode)
    {
        List<Node> path = new List<Node>();
        Node currentNode = endNode;

        while (currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.parent;
            currentNode.nodeCategory = Category.floor;
        }
        path.Reverse();
    }

    private int GetDistance(Node startPos, Node endPos)
    {
        int distanceX = Mathf.Abs(startPos.gridX - endPos.gridX);
        int distanceY = Mathf.Abs(startPos.gridY - endPos.gridY);

        return distanceX > distanceY
            ? 14 * distanceY + 10 * (distanceX - distanceY)
            : 14 * distanceX + 10 * (distanceY - distanceX);
    }

    public Node NodeFromWorldPoint(Vector3 worldPosition)
    {
        float percentX = (worldPosition.x - (40 * GridData.Instance.GridSize)) / (GridData.Instance.GridWidth * GridData.Instance.GridSize);
        float percentY = (worldPosition.z - (40 * GridData.Instance.GridSize)) / (GridData.Instance.GridHeight * GridData.Instance.GridSize);
        
        percentX = Mathf.Clamp01(percentX);
        percentY = Mathf.Clamp01(percentY);

        int x = Mathf.RoundToInt((GridData.Instance.GridWidth - 1) * percentX);
        int y = Mathf.RoundToInt((GridData.Instance.GridHeight - 1) * percentY);
        
        return grid[x, y];
    }
}
