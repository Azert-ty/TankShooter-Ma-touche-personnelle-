


using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ScoreManager : MonoBehaviour
{
    [Header("Références UI")]
    public Text scoreText;
    public GameObject gameOverPrefab; // prefab UI du texte/panel "Game Over"

    [Header("Feedbacks sonores")]
    public AudioClip scoreSound;
    public AudioClip gameOverSound;

    [Header("Paramètres de jeu")]
    public int targetScore = 1200;
    public float delayBeforeMainMenu = 1f;

    private int score = 0;
    private AudioSource audioSource;
    private bool gameEnded = false;

    void Start()
    {
        // Récupération du texte si non assigné
        if (scoreText == null)
        {
            GameObject textObj = GameObject.FindGameObjectWithTag("ScoreText");
            if (textObj != null)
                scoreText = textObj.GetComponent<Text>();
            else
                Debug.LogWarning("⚠️ Aucun objet avec le tag 'ScoreText' trouvé dans la scène.");
        }

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();

        UpdateScoreUI();
    }

    public void IncreaseScore(int amount = 1)
    {
        if (gameEnded) return;

        score += amount;
        UpdateScoreUI();

        // Feedback sonore
        if (scoreSound != null)
            audioSource.PlayOneShot(scoreSound);

        // Vérifie si le score max est atteint
        if (score >= targetScore)
            TriggerGameOver();
    }

    //private void TriggerGameOver()
    //{
    //    gameEnded = true;

    //    // Instancier le texte "Game Over"
    //    if (gameOverPrefab != null)
    //    {
    //        GameObject go = Instantiate(gameOverPrefab);
    //        Canvas canvas = FindObjectOfType<Canvas>();
    //        if (canvas != null)
    //            go.transform.SetParent(canvas.transform, false);
    //    }
    //    else
    //    {
    //        Debug.LogWarning("⚠️ Aucun GameOverPrefab assigné dans le ScoreManager.");
    //    }

    //    // Son de fin
    //    if (gameOverSound != null)
    //        audioSource.PlayOneShot(gameOverSound);

    //    // Retour au menu après délai
    //    Invoke(nameof(LoadMainMenu), delayBeforeMainMenu);
    //}
    //private void TriggerGameOver()
    //{
    //    gameEnded = true;



    //    // Informe le menu qu’on doit afficher “Game Over”
    //    PlayerPrefs.SetInt("ShowGameOver", 1);

    //    // Sauvegarde immédiate
    //    PlayerPrefs.Save();

    //    // Retourne au menu
    //    Invoke(nameof(LoadMainMenu), delayBeforeMainMenu);
    //    if (gameOverSound != null)
    //        audioSource.PlayOneShot(gameOverSound);
    //}


    private void TriggerGameOver()
    {
        gameEnded = true;

        // Sauvegarde du flag GameOver
        PlayerPrefs.SetInt("ShowGameOver", 1);
        PlayerPrefs.Save();

        // Son de fin
        if (gameOverSound != null)
            audioSource.PlayOneShot(gameOverSound);

        // Transition douce
        if (SceneTransition.Instance != null)
            SceneTransition.Instance.FadeToScene("MainMenu");
        else
            SceneManager.LoadScene("MainMenu");
    }


    private void LoadMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    private void UpdateScoreUI()
    {
        if (scoreText != null)
            scoreText.text = "Score: " + score;
    }

    public int GetScore() => score;
}
