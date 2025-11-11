using UnityEngine;

public class BulletHit : MonoBehaviour
{
    [Header("Feedback visuel et sonore")]
    public GameObject hitEffectPrefab;
    public AudioClip hitSound;

    [Header("Gameplay")]
    public int baseScore = 100;

    private ScoreManager scoreManager;

    private void Start()
    {
        // Récupérer le ScoreManager
        scoreManager = FindObjectOfType<ScoreManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Target")) return;

        // --- Feedback visuel ---
        if (hitEffectPrefab != null)
        {
            GameObject effect = Instantiate(hitEffectPrefab, other.transform.position, Quaternion.identity);
            // Optionnel : détruire l'effet après 2 secondes pour éviter l'accumulation
            Destroy(effect, 2f);
        }

        // --- Feedback sonore 2D ---
        if (hitSound != null)
        {
            PlaySound2D(hitSound, 1f); // volume à 100%
        }

        // --- Mise à jour du score ---
        if (scoreManager != null)
        {
            scoreManager.IncreaseScore(baseScore);
        }

        // --- Nettoyage ---
        Destroy(other.gameObject); // détruit la cible
        Destroy(gameObject);       // détruit le projectile
    }

    // Fonction pour jouer un son 2D fort et clair
    private void PlaySound2D(AudioClip clip, float volume)
    {
        GameObject tempGO = new GameObject("TempAudio");
        AudioSource audioSource = tempGO.AddComponent<AudioSource>();
        audioSource.clip = clip;
        audioSource.volume = Mathf.Clamp01(volume);
        audioSource.spatialBlend = 0f; // 0 = son 2D, pas de perte avec distance
        audioSource.Play();
        Destroy(tempGO, clip.length);
    }
}
