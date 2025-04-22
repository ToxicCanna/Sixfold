using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;

public class RewardManager : MonoBehaviour
{
    public GameObject rewardPanel;
    public Button healButton;
    public Button upgradeButton;
    public Button newCardButton;
    public Button maxHpButton;
    public CardDatabase cardDatabase;
    public Transform rewardCardPanel;
    public GameObject cardUIPrefab;

    public BattleSystem battleSystem;
    public Player player;

    public void ShowRewards()
    {
        rewardPanel.SetActive(true);

        healButton.onClick.RemoveAllListeners();
        upgradeButton.onClick.RemoveAllListeners();
        newCardButton.onClick.RemoveAllListeners();
        maxHpButton.onClick.RemoveAllListeners();

        healButton.onClick.AddListener(() =>
        {
            int healAmount = player.maxHealth / 3;
            player.health = Mathf.Min(player.health + healAmount, player.maxHealth);
            Debug.Log($"Healed for {healAmount}, new HP: {player.health}");
            FinishReward();
        });

        upgradeButton.onClick.AddListener(() =>
        {
            ShowUpgradeableCards();
        });

        newCardButton.onClick.AddListener(() =>
        {
            ShowCardChoices();
            Debug.Log("Card selection coming soon...");
        });

        maxHpButton.onClick.AddListener(() =>
        {
            player.maxHealth += 15;
            player.health += 15;
            Debug.Log("Max HP increased by 15.");
            FinishReward();
        });
    }
    void ShowCardChoices()
    {
        healButton.gameObject.SetActive(false);
        upgradeButton.gameObject.SetActive(false);
        newCardButton.gameObject.SetActive(false);
        maxHpButton.gameObject.SetActive(false);

        // Choose 3 random cards from the database
        List<Card> randomChoices = cardDatabase.allCards
            .OrderBy(c => Random.value)
            .Take(3)
            .ToList();

        foreach (Card card in randomChoices)
        {
            GameObject cardGO = Instantiate(cardUIPrefab, rewardCardPanel);
            var display = cardGO.GetComponent<CardDisplay>();

            // Pass AddCardToPlayerDeck as the action to be invoked on button click
            display.SetupRewardCard(card, () =>
            {
                AddCardToPlayerDeck(card);
                FinishReward();
            });
        }
    }

    void AddCardToPlayerDeck(Card card)
    {
        player.deck.allCards.Add(card);
        Debug.Log($"Added {card.cardName} to player's deck!");
    }

    void ShowUpgradeableCards()
    {
        // Hide the main reward buttons
        healButton.gameObject.SetActive(false);
        upgradeButton.gameObject.SetActive(false);
        newCardButton.gameObject.SetActive(false);
        maxHpButton.gameObject.SetActive(false);

        // Clear previous card displays
        foreach (Transform child in rewardCardPanel)
            Destroy(child.gameObject);

        // Get all cards that have an upgrade available
        List<Card> upgradeableCards = player.deck.allCards
            .Where(c => c.upgradedVersion != null)
            .Distinct()
            .ToList();

        if (upgradeableCards.Count == 0)
        {
            Debug.Log("No upgradeable cards available.");
            FinishReward();
            return;
        }

        foreach (Card card in upgradeableCards)
        {
            GameObject cardGO = Instantiate(cardUIPrefab, rewardCardPanel);
            var display = cardGO.GetComponent<CardDisplay>();

            display.SetupRewardCard(card, () =>
            {
                UpgradeCardInDeck(card);
                FinishReward();
            });
        }
    }

    void UpgradeCardInDeck(Card cardToUpgrade)
    {
        if (cardToUpgrade.upgradedVersion == null)
        {
            Debug.LogWarning($"{cardToUpgrade.cardName} has no upgraded version assigned!");
            return;
        }

        int index = player.deck.allCards.IndexOf(cardToUpgrade);
        if (index >= 0)
        {
            player.deck.allCards[index] = cardToUpgrade.upgradedVersion;
            Debug.Log($"Upgraded {cardToUpgrade.cardName} to {cardToUpgrade.upgradedVersion.cardName}");
        }
    }


    void FinishReward()
    {
        healButton.gameObject.SetActive(true);
        upgradeButton.gameObject.SetActive(true);
        newCardButton.gameObject.SetActive(true);
        maxHpButton.gameObject.SetActive(true);
        foreach (Transform child in rewardCardPanel)
            Destroy(child.gameObject);

        rewardPanel.SetActive(false);
        battleSystem.StartNextBattle();
    }
}
