using UnityEngine;

[CreateAssetMenu(fileName = "BlockEffect", menuName = "CardEffects/Block")]
public class BlockEffect : CardEffect
{

    public override void Apply(Card card, Player player, Enemy enemy)
    {
        Debug.Log($"Gaining block: {card.value}");
        player.block += card.value;
    }
}