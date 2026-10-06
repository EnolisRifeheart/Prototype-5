using UnityEngine;

public class Battler : MonoBehaviour
{
    [Header("Character Data")]
    public Stats stats;

    [Header("Runtime")]
    public int currentHealth;

    private void Awake()
    {
        if (stats != null)

        {
            currentHealth = stats.maxHealth;
        }

        // Making sure if Stats isn't assigned, character can't participate in battle.

        else

        {
            Debug.LogError(gameObject.name + " does not have a Stats asset assigned.");
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        // Health should not go under 0.

        currentHealth = Mathf.Max(currentHealth, 0);

        Debug.Log(gameObject.name + " received " + damage + " damage. Health: " + currentHealth);
    }

    public void Heal(int amount)
    {
        currentHealth += amount;

        // Healing shouldn't go increase past character's MAX HP.

        currentHealth = Mathf.Min(currentHealth, stats.maxHealth);
    }

    public bool IsDead()
    {
        return currentHealth <= 0;
    }
}