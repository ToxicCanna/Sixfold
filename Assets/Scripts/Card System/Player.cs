using UnityEngine;

public class Player : MonoBehaviour
{
    public int health = 100;
    public int maxHealth = 100;
    public int block = 0;

    public Deck deck;
    public PlayerHand hand;
    public int maxActionPoints = 100;
    public int startingActionPoints = 50;
    public int currentActionPoints;

    public void ResetActionPoints(int amountToRegain)
    {
        currentActionPoints += amountToRegain;
        currentActionPoints = Mathf.Min(currentActionPoints, maxActionPoints);
        Debug.Log($"Player regains Action Points. Current AP: {currentActionPoints}");
    }

    public bool SpendActionPoints(int amount)
    {
        if (currentActionPoints >= amount)
        {
            currentActionPoints -= amount;
            return true;
        }

        return false;
    }

    public void ResetToStartingAP()
    {
        currentActionPoints = startingActionPoints;
    }
}
