using UnityEngine;

[CreateAssetMenu(fileName = "New Card", menuName = "Card")]
public class Card : ScriptableObject
{
    public string cardName;
    public string description;
    public int cost;
    public int value;
    public Sprite artwork;
    public Card upgradedVersion;

    public CardEffect effect;

    private int originalCost;
    private int originalValue;

    // Temporary buffs/debuffs (for the position based changes)
    private int tempValueModifier = 0;
    private int tempCostModifier = 0;

    // This method sets the original values to be used during the battle since scriptables persist through editor sessions
    public void SetOriginalValues()
    {
        originalCost = cost;
        originalValue = value;
    }
    public Card Clone()
    {
        Card clone = ScriptableObject.CreateInstance<Card>();

        clone.cardName = this.cardName;
        clone.description = this.description;
        clone.cost = this.cost;
        clone.value = this.value;
        clone.artwork = this.artwork;
        clone.effect = this.effect;
        clone.upgradedVersion = this.upgradedVersion;

        return clone;
    }
    // Method to apply temporary buffs/debuffs
    public void ApplyTemporaryBuffs(int valueModifier, int costModifier)
    {
        tempValueModifier = valueModifier;
        tempCostModifier = costModifier;

        // Apply the temporary buffs/debuffs
        value = Mathf.Max(originalValue + tempValueModifier, 5);
        cost = originalCost + tempCostModifier;
    }
}
