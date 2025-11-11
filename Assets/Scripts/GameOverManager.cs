using UnityEngine;

public class GameOverManager : MonoBehaviour
{
    [Header("Références UI")]
    public GameObject gameOverText; // ton texte "Game Over"

    void Start()
    {
        // Si la scène est chargée suite à un Game Over :
        if (PlayerPrefs.GetInt("ShowGameOver", 0) == 1)
        {
            if (gameOverText != null)
                gameOverText.SetActive(true);

            // On remet à 0 pour la prochaine fois
            PlayerPrefs.SetInt("ShowGameOver", 0);
        }
        else
        {
            if (gameOverText != null)
                gameOverText.SetActive(false);
        }
    }
}
