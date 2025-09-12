using TMPro;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.UI;

public class BuildingManager : MonoBehaviour
{
    private GameObject pendingObj;  
    private Vector3 pos;
    private RaycastHit hit;
    private GameObject targetDirt;
    private PlantController plantController;
    private int currentIndex;
    [SerializeField] private LayerMask layermask; 
    [SerializeField] private LayerMask farmLayerMask; 
    [SerializeField] private LayerMask interactableLayerMask; 
    [SerializeField] private LayerMask unwalkableLayerMask; 
    private float gridSize;
    private float rotateAmount = 45f;
    private bool isSeed = false;
    private bool isValid = false;
    private bool isAFarmArea = false;
    [SerializeField] private GameObject costUI;
    [SerializeField] private Image sprite;
    [SerializeField] private TextMeshProUGUI name;
    [SerializeField] private TextMeshProUGUI theCostText;
    [SerializeField] private GameObject cancellation;
    public static BuildingManager instance;
    [SerializeField] private NavMeshSurface navMeshSurface;
    
    private void Awake()
    {
        if (navMeshSurface == null)
        {
            GameObject.Find("Terrain").GetComponent<NavMeshSurface>();
        }
        instance = this;
        costUI.SetActive(false);
    }
    
    public void CancelPlacement()
    {
        if (pendingObj)
        {
            Destroy(pendingObj); 
            pendingObj = null;   
            costUI.SetActive(false); 
        }
    }
    
    private Vector2 sizeOfThePendingObject;
    
    private void UpdatePendingObjectPosition()
    {
        isValid = false;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (isAFarmArea == false)
        {
            if (Physics.Raycast(ray, out hit, 1000, layermask | farmLayerMask))
            {
                if (((1 << hit.collider.gameObject.layer) & farmLayerMask) != 0)
                {
               
                    if (Physics.Raycast(ray, out hit, 250, farmLayerMask))
                    {
                        if (hit.collider.gameObject.CompareTag("Dirt") && isSeed)
                        {
                            isValid = true;
                            pendingObj.SetActive(true);
                            targetDirt = hit.collider.gameObject;
                            plantController = targetDirt.GetComponent<PlantController>();

                            if (!plantController.IsAvailable)
                            {
                                pendingObj.SetActive(false);
                                isValid = false;
                                ErrorShowing.ShowError("There is already another plant in this dirt", Input.mousePosition, 1f);
                            }
                        }
                        else if (!isSeed)
                        {
                            Collider[] overlappingColliders = Physics.OverlapBox(
                                hit.point,
                                new Vector3(sizeOfThePendingObject.x/2, 1f, sizeOfThePendingObject.y/2),
                                pendingObj.transform.rotation, 
                                farmLayerMask | layermask
                            );
                            int dirtCount = 0;
                            int terrainCount = 0;
                            foreach (Collider collider in overlappingColliders)
                            {
                                if (collider.CompareTag("Dirt"))
                                {
                                    dirtCount++;
                                }
                                else if (LayerMask.LayerToName(collider.gameObject.layer) == "whatIsGround")
                                {
                                    terrainCount++;
                                }
                            }
                            
                            // Debug.Log(terrainCount + "asdfasdfadf" + dirtCount + "Length :" + overlappingColliders.Length);
                            if (dirtCount > 2)
                            {
                                isValid = false;
                                ErrorShowing.ShowError("There is already another dirt block here!", Input.mousePosition, 1f);
                            } else if (overlappingColliders.Length > dirtCount + terrainCount + 1)
                            {
                                isValid = false;
                                ErrorShowing.ShowError("Invalid place, the dirt is colliding with something", Input.mousePosition, 1f);
                            }
                            else
                            {
                                isValid = true; 
                            }
                        }
                        else
                        {
                            isValid = false;
                            pendingObj.SetActive(false);
                            ErrorShowing.ShowError("Place the sapling on a dirt", Input.mousePosition, 1f);
                        }
                    }

                    if (Physics.Raycast(ray, out hit, 1000, layermask))
                    {
                        pos = hit.point;
                        if (isSeed && isValid)
                        {
                            pendingObj.transform.position = new Vector3(
                                RoundToNearestGrid(pos.x),
                                pos.y,
                                RoundToNearestGrid(pos.z)
                            );
                            pendingObj.SetActive(true);
                        }
                        else if (!isSeed)
                        {
                            pendingObj.transform.position = new Vector3(
                                RoundToNearestGrid(pos.x),
                                pos.y,
                                RoundToNearestGrid(pos.z)
                            );
                            pendingObj.SetActive(true);
                        }
                    }
                }
            }
            if(!Physics.Raycast(ray, out hit, 1000, farmLayerMask))
            {
                pendingObj.SetActive(false);
                ErrorShowing.ShowError("Outside of Farm Area", Input.mousePosition, 1f);
            }
        } else if (isAFarmArea == true)
        {
            if (Physics.Raycast(ray, out hit, 1000, layermask))
            {
                pos = hit.point;
                pendingObj.transform.position = new Vector3(
                    RoundToNearestGrid(pos.x),
                    pos.y,
                    RoundToNearestGrid(pos.z)
                );
                Collider[] overlappingColliders = Physics.OverlapBox(
                    hit.point,
                    new Vector3(sizeOfThePendingObject.x / 2, 1f, sizeOfThePendingObject.y / 2),
                    pendingObj.transform.rotation,
                    farmLayerMask | layermask | unwalkableLayerMask | interactableLayerMask
                );
                    
                if (overlappingColliders.Length > 2)
                {
                    isValid = false;
                    ErrorShowing.ShowError("The Farm is Colliding with something!!", Input.mousePosition, 1f);
                }
                else
                {
                    isValid = true;
                   
                }
                pendingObj.SetActive(true);
            }
        }
    }


    private Plants currentPlantToSpawm;
    private FarmArea currentFarmArea;
    
    public void SelectObject(string objectName, string objectType)
    {
        currentPlantToSpawm = null;
        if (pendingObj)
        {
            Destroy(pendingObj);
        }
        if (objectType == "Plant")
        {
            isAFarmArea = false;
            sizeOfThePendingObject = new Vector2(2f, 2f);
            foreach (Plants plant in GameData.Instance.GameResources)
            {
                if (objectName == plant.plantName)
                {
                    if (costUI == null)
                    {
                        Transform parent = GameObject.Find("UI").transform;
                        costUI = parent.Find("Placing").gameObject;
                    }
                    costUI.SetActive(true);
                    sprite.sprite = plant.plantSprite;
                    name.text = plant.plantName;
                    if (plant.plantName != "Dirt")
                    {
                        theCostText.text = "1 "+ plant.plantName +" Sapling";
                    }
                    else
                    {
                        theCostText.text = "There is no Cost";
                    }
                

                    pendingObj = Instantiate(plant.plantsPhase[0], pos, Quaternion.identity);
                    currentPlantToSpawm = plant;
                    break;
                }
            }
            
        } else if (objectType == "FarmArea")
        {
            isAFarmArea = true;
            foreach (FarmArea farm in GameData.Instance.GameFarmAreas)
            {
                if (objectName == farm.farmName)
                {
                    currentFarmArea = farm;
                    pendingObj = Instantiate(farm.farmPrefab, pos, Quaternion.identity);

                    costUI.SetActive(true);
                    sprite.sprite = farm.farmSprite;
                    name.text = farm.farmName;
                    theCostText.text = farm.woodCost +" Wood";
                    
                    if (farm.farmName.Contains("Small"))
                    {
                        sizeOfThePendingObject = new Vector2(18f, 15f);
                    } else if (farm.farmName.Contains("Medium"))
                    {
                        sizeOfThePendingObject = new Vector2(19f, 31f);
                    } else if (farm.farmName.Contains("Large"))
                    {
                        sizeOfThePendingObject = new Vector2(35f, 35f);
                    }
                    break;
                }
            }
        }
        isSeed = pendingObj.CompareTag("Seed");
    }
    public void RotateObject()
    {
        if (pendingObj)
        {
            pendingObj.transform.Rotate(Vector3.up, rotateAmount);
        }
    }
    
    public void PlaceObj()
    {
        if (!isValid && pendingObj != null)
        {
            Destroy(pendingObj.gameObject);
        } else if (isSeed)
        {
            plantController.SetPlantData(currentPlantToSpawm);
            currentPlantToSpawm.seedQuantity -= 1;
            Destroy(pendingObj);
        } else if (isAFarmArea)
        {
            GameData.Instance.GameWood.quantity -= currentFarmArea.woodCost;
            RebuildNavMesh();
        }
        InstantiatePlacingUI.instance.RefreshInventoryUI();
        RemoveGrassInArea();
        costUI.SetActive(false);
        pendingObj = null;
    }
    
    private void RebuildNavMesh()
    {
            navMeshSurface.BuildNavMesh();
    }
    private void RemoveGrassInArea()
    {
        if (pendingObj == null) return;

        Terrain terrain = Terrain.activeTerrain;
        if (terrain == null)
        {
            Debug.LogError("No active terrain found!");
            return;
        }

        TerrainData terrainData = terrain.terrainData;
        Vector3 terrainPos = terrain.transform.position;
        Vector3 farmPos = pendingObj.transform.position;
        Vector2 farmSize = sizeOfThePendingObject;

        int xStart = Mathf.FloorToInt((farmPos.x - farmSize.x / 2 - terrainPos.x) / terrainData.size.x * terrainData.detailWidth);
        int zStart = Mathf.FloorToInt((farmPos.z - farmSize.y / 2 - terrainPos.z) / terrainData.size.z * terrainData.detailHeight);
        int xEnd = Mathf.FloorToInt((farmPos.x + farmSize.x / 2 - terrainPos.x) / terrainData.size.x * terrainData.detailWidth);
        int zEnd = Mathf.FloorToInt((farmPos.z + farmSize.y / 2 - terrainPos.z) / terrainData.size.z * terrainData.detailHeight);

        xStart = Mathf.Clamp(xStart, 0, terrainData.detailWidth - 1);
        zStart = Mathf.Clamp(zStart, 0, terrainData.detailHeight - 1);
        xEnd = Mathf.Clamp(xEnd, 0, terrainData.detailWidth - 1);
        zEnd = Mathf.Clamp(zEnd, 0, terrainData.detailHeight - 1);

        for (int layer = 0; layer < terrainData.detailPrototypes.Length; layer++)
        {
            int[,] detailLayer = terrainData.GetDetailLayer(xStart, zStart, xEnd - xStart, zEnd - zStart, layer);
            for (int x = 0; x < detailLayer.GetLength(0); x++)
            {
                for (int z = 0; z < detailLayer.GetLength(1); z++)
                {
                    detailLayer[x, z] = 0;
                }
            }
            terrainData.SetDetailLayer(xStart, zStart, layer, detailLayer);
        }
    }
    
    private float RoundToNearestGrid(float pos)
    {
        float xDiff = pos % gridSize;
        pos -= xDiff;
        if (xDiff > (gridSize / 2))
        {
            pos += gridSize;
        }
        return pos;
    }
    private void Start()
    {
        gridSize = GridData.Instance.GridSize;
    }
    private bool IsMouseOverCancellation(Vector2 mousePosition)
    {
        RectTransform rectTransform = cancellation.GetComponent<RectTransform>();
        Rect rect = rectTransform.rect;
        Vector2 worldPosition = rectTransform.position;
        Rect worldRect = new Rect(
            worldPosition.x - rect.width / 2,
            worldPosition.y - rect.height / 2,
            rect.width,
            rect.height
        );
        return worldRect.Contains(mousePosition);
    }
    
    void Update()
    { 
        if (navMeshSurface == null)
        {
           navMeshSurface = GameObject.Find("Terrain").GetComponent<NavMeshSurface>();
        }
        if (pendingObj)
        {
            UpdatePendingObjectPosition();
            if (Input.GetKeyDown(KeyCode.R))
            {
                RotateObject();
            }
            if (Input.GetMouseButtonDown(0))
            {
                Vector2 mousePosition = Input.mousePosition;
                if (IsMouseOverCancellation(mousePosition))
                {
                    CancelPlacement();
                }
                else
                {
                    PlaceObj();
                }
            }
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                CancelPlacement();
                costUI.SetActive(false);
            }
        }
    }
}
