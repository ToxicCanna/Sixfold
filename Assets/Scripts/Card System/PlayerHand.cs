using System.Collections.Generic;
using UnityEngine;

public class PlayerHand : MonoBehaviour
{
    public List<Card> handCards = new();

    public void AddCard(Card card)
    {
        handCards.Add(card);
    }

    public void RemoveCard(Card card)
    {
        handCards.Remove(card);
    }

    public void ClearHand()
    {
        handCards.Clear();
    }

    public bool PlayCard(Card card, Player player, Enemy enemy)
    {
        card.effect.Apply(card, player, enemy);
        return true;
    }
}
