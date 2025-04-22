using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Image playerHealthBar;
    public Image enemyHealthBar;

    public Text playerHealthText;
    public Text enemyHealthText;

    public Player player;
    public Enemy enemy;

    public Text playerBlockText;

    public void UpdateUI()
    {
        playerBlockText.text = $"Block: {player.block}";

        playerHealthBar.fillAmount = (float)player.health / player.maxHealth;
        enemyHealthBar.fillAmount = (float)enemy.health / enemy.maxHealth;

        playerHealthText.text = $"Player HP: {player.health}/{player.maxHealth}";
        enemyHealthText.text = $"Enemy HP: {enemy.health}/{enemy.maxHealth}";
    }
}
