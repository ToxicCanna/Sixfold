using UnityEngine;
using UnityEngine.UI;

public class CardDisplay : MonoBehaviour
{
    public Text cardNameText;
    public Text descriptionText;
    public Text costText;
    public Button playButton;

    private Card card;
    private BattleSystem battleSystem;
    private GameObject cardUIRef;

    public void SetupSelectable(Card card, BattleSystem system, GameObject cardGO)
    {
        this.card = card;
        this.battleSystem = system;

        cardNameText.text = card.cardName;
        descriptionText.text = card.description;
        costText.text = card.cost.ToString();

        playButton.onClick.RemoveAllListeners();
        playButton.onClick.AddListener(() => battleSystem.SelectCardForBattle(card, cardGO));

        EnablePlayButton();
    }

    public void SetupPlayable(Card card, BattleSystem system, GameObject cardGO)
    {
        this.card = card;
        this.battleSystem = system;
        this.cardUIRef = cardGO;

        cardNameText.text = card.cardName;
        descriptionText.text = card.description;
        costText.text = card.cost.ToString();

        playButton.onClick.RemoveAllListeners();
        playButton.onClick.AddListener(() => battleSystem.PlayerTurn(card, cardUIRef));

        EnablePlayButton();
    }

    public void DisablePlayButton()
    {
        playButton.interactable = false;
        SetVisualAlpha(0.5f);
    }

    public void EnablePlayButton()
    {
        playButton.interactable = true;
        SetVisualAlpha(1f);
    }
    private void SetVisualAlpha(float alpha)
    {
        SetAlpha(cardNameText, alpha);
        SetAlpha(descriptionText, alpha);
        SetAlpha(costText, alpha);
    }

    private void SetAlpha(Graphic graphic, float alpha)
    {
        Color c = graphic.color;
        c.a = alpha;
        graphic.color = c;
    }

    public void SetupRewardCard(Card card, System.Action onClickAction)
    {
        this.card = card;

        cardNameText.text = card.cardName;
        descriptionText.text = card.description;
        costText.text = card.cost.ToString();

        EnablePlayButton();

        // Add listener to handle when the card is chosen during the reward phase
        playButton.onClick.RemoveAllListeners();
        playButton.onClick.AddListener(() => onClickAction?.Invoke());
    }
}
