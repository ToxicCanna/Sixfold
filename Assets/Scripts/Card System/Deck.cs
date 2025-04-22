using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Deck : MonoBehaviour
{
    public List<Card> allCards = new();  // Full collection

    private List<Card> originalDeck = new();

    private void Awake()
    {
        // Save the original full deck when the game first loads
        originalDeck = new List<Card>(allCards);
    }

    public void ResetToOriginal()
    {
        // Restore both allCards and active battle deck to original deck
        allCards = new List<Card>(originalDeck);
    }
}
