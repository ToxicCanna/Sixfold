using UnityEngine;

public abstract class CardEffect : ScriptableObject
{
    public abstract void Apply(Card card, Player player, Enemy enemy);
}
