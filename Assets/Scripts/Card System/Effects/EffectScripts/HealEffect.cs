using UnityEngine;

[CreateAssetMenu(fileName = "HealEffect", menuName = "CardEffects/Heal")]
public class HealEffect : CardEffect
{
    public override void Apply(Card card, Player player, Enemy enemy)
    {
        player.health += card.value;
        player.health = Mathf.Min(player.health, player.maxHealth);  // Prevent overhealing
        Debug.Log($"Player heals for {card.value}. Current HP: {player.health}");
    }
}
