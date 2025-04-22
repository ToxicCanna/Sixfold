using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int baseHealth = 100;
    public int health;
    public int maxHealth
    {
        get { return baseHealth + (enemyLevel - 1) * 20; }
    }

    public static int enemyLevel = 1;
    public UIManager uiManager;

    private void Start()
    {
        health = maxHealth;
        Debug.Log($"Enemy Level: {enemyLevel}, Health: {health}");

        if (uiManager != null)
        {
            uiManager.UpdateUI();  // Update health bar on start
        }
    }

    public void PlayTurn(Player player)
    {
        int baseDamage = Random.Range(5, 10);
        int scaledDamage = baseDamage + (enemyLevel - 1) * Random.Range(2, 4);

        int damageBlocked = Mathf.Min(player.block, scaledDamage);
        int finalDamage = scaledDamage - damageBlocked;

        player.health -= finalDamage;
        Debug.Log($"Enemy attacks for {scaledDamage}. Player HP: {player.health}");
        player.block = 0;

        if (uiManager != null)
        {
            uiManager.UpdateUI();
        }
    }

    public static void IncrementDifficulty()
    {
        enemyLevel++;
    }
    public void ResetEnemy()
    {
        health = maxHealth;  // Reset health based on maxHealth
        Debug.Log($"Enemy reset with health: {health}");

        // Update health bars when resetting enemy
        if (uiManager != null)
        {
            uiManager.UpdateUI();
        }
    }

}
