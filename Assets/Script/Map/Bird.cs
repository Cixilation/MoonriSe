using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bird : MonoBehaviour
{

    private bool playerInSightRange;
    private float sightRange = 4f;
    private float wanderRange = 20f;
    
    
    
    public LayerMask landingLayer, whatIsPlayer;
    private Animator BirdAnimator;

    private void Awake()
    {
        
        landingLayer = LayerMask.GetMask("whatIsGround", "Unwalkable");
        whatIsPlayer = LayerMask.GetMask("whatIsPlayer");
        BirdAnimator = GetComponent<Animator>();

    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
