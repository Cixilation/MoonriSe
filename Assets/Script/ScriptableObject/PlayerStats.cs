using UnityEngine;

[CreateAssetMenu(fileName = "UserStats", menuName = "ScriptableObjects/UserStats", order = 1)]
public class PlayerStats : ScriptableObject
{
    public int level;
    public float maximumHealth;
    public float health;
    public float experience;
    public float maximumExperienceToLevelUp;
    public float money;
    public bool hasDied;
}