using UnityEngine;
using UnityEngine.UI;

public class APBarController : MonoBehaviour
{
    public Player player;
    public Image apBarFill;
    public Text apText;

    void Update()
    {
        Debug.Log($"AP Update: {player.currentActionPoints} / {player.maxActionPoints}");
        float fill = (float)player.currentActionPoints / player.maxActionPoints;
        apBarFill.fillAmount = fill;

        if (apText != null)
            apText.text = $"{player.currentActionPoints} / {player.maxActionPoints}";
    }
}
