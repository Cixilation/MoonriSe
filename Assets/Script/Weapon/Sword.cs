using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Sword : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy" && PlayerData.Instance.isAttacking)
        {
            Zombie zombie = other.gameObject.GetComponent<Zombie>();
            //Player Sword Attack
            int damage =  (int) (50 * GameData.Instance.GamePlayerStats.level * UnityEngine.Random.Range(1, 4));
            zombie.TakeDamage(damage);
        }
    }
}
