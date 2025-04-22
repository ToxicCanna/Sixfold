using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class BattleSystem : MonoBehaviour
{
    public Player player;
    public Enemy enemy;

    public GameObject cardUIPrefab;
    public Transform selectionPanel;
    public Transform leftHandPanel;
    public Transform rightHandPanel;
    public Button endTurnButton;
    public int enemiesDefeated = 0;
    public RewardManager rewardManager;
    public UIManager uiManager;
    public GameObject SelectionUIGroup;


    public int cardsToSelect = 6;
    private List<Card> selectedCards = new List<Card>();
    private HashSet<GameObject> cardsPlayedThisTurn = new();

    private void Start()
    {
        ResetCardBuffs();
        uiManager.UpdateUI();
        ShowCardSelection();
    }

    void ShowCardSelection()
    {
        Debug.Log(">> Showing card selection...");
        Debug.Log($"Deck has {player.deck.allCards.Count} cards.");

        SelectionUIGroup.SetActive(true);

        Dictionary<Card, int> cardCounts = new Dictionary<Card, int>();

        // Count the occurrences of each card in the deck
        foreach (Card card in player.deck.allCards)
        {
            if (!cardCounts.ContainsKey(card))
            {
                cardCounts[card] = 0;
            }
            cardCounts[card]++;
        }

        // Instantiate UI elements for each card copy
        foreach (var entry in cardCounts)
        {
            Card card = entry.Key;
            int count = entry.Value;

            // Instantiate for each copy
            for (int i = 0; i < count; i++)
            {
                GameObject cardGO = Instantiate(cardUIPrefab, selectionPanel);
                cardGO.GetComponent<CardDisplay>().SetupSelectable(card, this, cardGO);
            }
        }
    }

    public void SelectCardForBattle(Card card, GameObject cardGO)
    {
        selectedCards.Add(card);
        Debug.Log($"Selected: {card.cardName} ({selectedCards.Count}/{cardsToSelect})");

        cardGO.GetComponent<CardDisplay>().DisablePlayButton();

        if (selectedCards.Count >= cardsToSelect)
        {
            StartBattleWithSelectedCards();
        }
    }

    void StartBattleWithSelectedCards()
    {
        SelectionUIGroup.SetActive(false);
        endTurnButton.onClick.RemoveAllListeners();
        endTurnButton.onClick.AddListener(EndPlayerTurn);

        // Clear selection panel
        foreach (Transform child in selectionPanel)
        {
            Destroy(child.gameObject);
        }

        player.hand.ClearHand();

        // Add all selected cards to hand directly
        for (int i = 0; i < selectedCards.Count; i++)
        {
            Card originalCard = selectedCards[i];
            Card cardInstance = originalCard.Clone();

            // Set up clean baseline
            cardInstance.SetOriginalValues();

            // Apply temp mods to instance only
            ApplyHandBuffsAndDebuffs(cardInstance, i);

            // Add this instance to the hand
            player.hand.AddCard(cardInstance);

            // Instantiate the card UI prefab
            GameObject cardGO = Instantiate(cardUIPrefab);

            // Set up the card display
            cardGO.GetComponent<CardDisplay>().SetupPlayable(cardInstance, this, cardGO);

            // Assign the card to the left or right hand based on the index
            if (i < 3)
            {
                // Left panel for the first three cards
                cardGO.transform.SetParent(leftHandPanel, false);
            }
            else
            {
                // Right panel for the last three cards
                cardGO.transform.SetParent(rightHandPanel, false);
            }
        }

        Debug.Log("Battle started with selected hand!");
    }

    void ApplyHandBuffsAndDebuffs(Card card, int index)
    {
        if (index == 0) // Top left card
        {
            card.ApplyTemporaryBuffs(5, 5);  // Buff value, debuff cost
        }
        else if (index == 1) // Middle left card
        {
            // No change
        }
        else if (index == 2) // Bottom left card
        {
            card.ApplyTemporaryBuffs(-5, -5);  // Debuff value, buff cost
        }

        else if (index == 3) // Top right card
        {
            card.ApplyTemporaryBuffs(5, 5);  // Buff value, debuff cost
        }
        else if (index == 4) // Middle right card
        {
            // No change
        }
        else if (index == 5) // Bottom right card
        {
            card.ApplyTemporaryBuffs(-5, -5);  // Debuff value, buff cost
        }
    }

    public void PlayerTurn(Card selectedCard, GameObject cardUI)
    {
        // Prevent using the same card more than once in a turn
        if (cardsPlayedThisTurn.Contains(cardUI))
        {
            Debug.Log("This card has already been played this turn.");
            return;
        }

        // Check if the player has enough AP to play the card
        if (!player.SpendActionPoints(selectedCard.cost))
        {
            Debug.Log("Not enough AP to play this card!");
            return;
        }

        // Apply the card effect
        bool played = player.hand.PlayCard(selectedCard, player, enemy);
        if (!played) return;

        // Mark card as used this turn
        cardsPlayedThisTurn.Add(cardUI);

        // disable the card's play button
        cardUI.GetComponent<CardDisplay>()?.DisablePlayButton();

        uiManager.UpdateUI();

        // Check if enemy is defeated
        if (enemy.health <= 0)
        {
            Debug.Log("Enemy defeated!");
            enemiesDefeated++;

            Enemy.IncrementDifficulty(); // Scale difficulty
            Debug.Log($"Enemies defeated: {enemiesDefeated}. New enemy level: {Enemy.enemyLevel}");
            rewardManager.ShowRewards();

            return;
        }
    }

    void EndPlayerTurn()
    {
        endTurnButton.interactable = false;
        EnemyTurn();
    }


    void EnemyTurn()
    {
        enemy.PlayTurn(player);
        uiManager.UpdateUI();

        if (player.health <= 0)
        {
            Debug.Log("Player defeated!");
            ResetGame();
            return;
        }

        StartPlayerTurn();
    }

    void StartPlayerTurn()
    {
        Debug.Log($"Start of Turn: Current AP: {player.currentActionPoints}");

        player.ResetActionPoints(20); // can be changed for balance

        cardsPlayedThisTurn.Clear();

        // Reactivate all card buttons
        foreach (Transform cardGO in leftHandPanel)
        {
            cardGO.GetComponent<CardDisplay>().EnablePlayButton();
        }
        foreach (Transform cardGO in rightHandPanel)
        {
            cardGO.GetComponent<CardDisplay>().EnablePlayButton();
        }

        endTurnButton.interactable = true;
        
    }

    public void StartNextBattle()
    {
        Debug.Log("Starting next battle...");

        // Reset selection phase
        selectedCards.Clear();

        // Clear both hand panels
        foreach (Transform child in leftHandPanel)
        {
            Destroy(child.gameObject);
        }
        foreach (Transform child in rightHandPanel)
        {
            Destroy(child.gameObject);
        }

        // Reset player action points and hand
        player.ResetToStartingAP();
        player.hand.ClearHand();

        // Reset any temporary buffs or debuffs applied to cards in the previous battle
        ResetCardBuffs();

        // Reset enemy health based on difficulty level
        enemy.ResetEnemy();
        Debug.Log($"Enemy's full health: {enemy.health}");

        ShowCardSelection();
        uiManager.UpdateUI();
    }

    void ResetCardBuffs()
    {
        foreach (Card card in player.hand.handCards)
        {
            if (card != null)
            {
                // Reset buffs/debuffs after the battle
                card.SetOriginalValues();
            }
        }
    }

    public void ResetGame()
    {
        Debug.Log("Resetting game...");

        // Clear all cards from UI
        foreach (Transform child in leftHandPanel)
            Destroy(child.gameObject);
        foreach (Transform child in rightHandPanel)
            Destroy(child.gameObject);
        foreach (Transform child in selectionPanel)
            Destroy(child.gameObject);

        // Reset enemy difficulty
        Enemy.enemyLevel = 1;

        // Reset player stats
        player.maxHealth = 100;
        player.health = player.maxHealth;
        player.currentActionPoints = player.startingActionPoints;
        player.block = 0;

        // Reset player's deck to original
        player.deck.ResetToOriginal();

        // Reset enemy
        enemy.ResetEnemy();

        // Reset selected cards list
        selectedCards.Clear();
        cardsPlayedThisTurn.Clear();

        endTurnButton.interactable = true;
        // Go back to card selection
        ShowCardSelection();

        // Update UI
        FindAnyObjectByType<UIManager>().UpdateUI();
    }
}
