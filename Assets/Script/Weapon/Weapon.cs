using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] private string weaponName; 
    [SerializeField] private float damageMultiplier;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            //Todo : get the player stats and make a formula for the damage 
            // Debug.Log($"{weaponName} hit {other.name} for {damage} damage!");
        }
    }
}
