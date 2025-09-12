using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridData
{
    private static GridData _instance;
    public static GridData Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new GridData(); 
            }
            return _instance;
        }
    }
    
    private GridData() { }
  
    private float gridSize = 2.5f;
    private int gridWidth = 220;
    private int gridHeight = 220;
    private int borderOffset = 40; 
    private float objectHeightOffset = 0f;
    
    public float GridSize { get { return gridSize; } }
    public int GridWidth { get { return gridWidth; } }
    public int GridHeight { get { return gridHeight; } }
    public int BorderOffset { get { return borderOffset; } }
    public float ObjectHeightOffset { get { return objectHeightOffset; } }
}