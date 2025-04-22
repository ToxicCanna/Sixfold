using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "CardDatabase", menuName = "Card Database")]
public class CardDatabase : ScriptableObject
{
    public List<Card> allCards;
}