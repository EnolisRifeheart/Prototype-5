using UnityEngine;

[CreateAssetMenu(fileName = "Stats", menuName = "Character Stat Sheet")]
public class Stats : ScriptableObject
{
    [Header("Stats")]
    public int maxHealth;
    public int attackDamage;
    public int defense;
    public int speed;
}