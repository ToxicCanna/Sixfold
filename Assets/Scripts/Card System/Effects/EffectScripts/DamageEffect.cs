using UnityEngine;

[CreateAssetMenu(fileName = "DamageEffect", menuName = "CardEffects/Damage")]
public class DamageEffect : CardEffect
{
    public override void Apply(Card card, Player player, Enemy enemy)
    {
        enemy.health -= card.value;
        Debug.Log($"Enemy takes {card.value} damage. Remaining HP: {enemy.health}");
        FindObjectOfType<UIManager>().UpdateUI();
    }
}
