using System;
using UnityEngine;

public class AttackPlayer : MonoBehaviour
{
    [SerializeField] private Zombie zombie;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && zombie.attackPlayerTag)
        {
            // Attack Zombie
            float baseDamage = (float) (20f * Math.Sqrt(zombie.level));
            float criticalMultiplier = UnityEngine.Random.Range(0f, 1f) <= 0.2f ? 2f : 1f; 
            float totalDamage = baseDamage * criticalMultiplier;
            GameData.Instance.GamePlayerStats.health -= Mathf.RoundToInt(totalDamage);
            DamagePopUpGenerator.instance.CreatePopUp(transform.position,  Mathf.RoundToInt(totalDamage).ToString(), Color.red);
        }
    }
}