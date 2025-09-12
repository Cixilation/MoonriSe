using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class FollowPlayer : MonoBehaviour
{
    public AnimationCurve heightCurve;
    private Transform player;
    private float MinModifier = 7;
    private float MaxModifier = 11;
    private Vector3 _velocity = Vector3.zero;
    public string dropObjectName;
    public int expMultiplier;

    private float time;
    private void Start()
    {
        player = GameObject.Find("F_AA_002").transform;
    }
    
    Vector3 originalPosition;
    private void Awake()
    {
        originalPosition = transform.position;
    }

    void Update()
    {
       
        if (time > 2)
        {
            float upwardOffset = 1f;
            Vector3 targetPosition = player.position + Vector3.up * upwardOffset;
            transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref _velocity,
                Time.deltaTime * Random.Range(MinModifier, MaxModifier));
        }
        else
        {
            transform.position = originalPosition + new Vector3(0, heightCurve.Evaluate(time) * 1f, 0);
            time += Time.deltaTime;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Destroy(gameObject, 0.5f);
            if (dropObjectName == "wood")
            {
                GameData.Instance.GameWood.quantity += Random.Range(1, 3);
            } else if (dropObjectName == "Tomato")
            {
                GameData.Instance.GameTomato.quantity += Random.Range(5, 7);
            } else if (dropObjectName == "Bamboo")
            {
                GameData.Instance.GameBamboo.quantity += Random.Range(1, 3);
            } else if (dropObjectName == "exp")
            {
                GameData.Instance.GamePlayerStats.experience += Random.Range(1, 7) * expMultiplier;
            } else if (dropObjectName == "Berries")
            {
                GameData.Instance.GameBerry.quantity += Random.Range(5, 10);
            }
            else
            {
                Debug.Log("What kind of object?");
            }
        }
    }
    
    // Dictionary 
    // wood
    // berry
    // exp
    // bamboo
    // tomato
        
}

