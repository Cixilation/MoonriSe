using System;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    private void Start()
    {
        GetComponent<Rigidbody>().velocity = transform.forward * 2f;
    }

    private void OnTriggerEnter(Collider other)
    {

        if (!other.CompareTag("Player"))
        {
            Rigidbody rb = GetComponent<Rigidbody>();
            Zombie zombie = other.gameObject.GetComponent<Zombie>();

            if (zombie != null)
            {
                // Player Bow Attack
                zombie.TakeDamage((int) (50 * Math.Round(GameData.Instance.GamePlayerStats.level * 1.5f * UnityEngine.Random.Range(1, 4))));
            }

            if (rb != null)
            {
                rb.isKinematic = true;
            }

            if (other.CompareTag("Enemy"))
            {
                transform.SetParent(other.transform);
            }

            Destroy(gameObject, 10f);
        }
    }
}