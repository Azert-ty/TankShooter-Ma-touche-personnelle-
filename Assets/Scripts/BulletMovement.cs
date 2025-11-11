using UnityEngine;

public class BulletMovement : MonoBehaviour
{
    [Header("Paramètres de déplacement")]
    public float speed = 30f;
    public float lifetime = 5f;
    public float acceleration = 0f; // permet d'accélérer les balles si besoin

    [Header("Feedbacks visuels")]
    public AudioClip shootSound;

    private float currentSpeed;
    private AudioSource audioSource;

    void Start()
    {
        currentSpeed = speed;

  

        if (shootSound != null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.PlayOneShot(shootSound);
        }

        // --- Nettoyage automatique ---
        Destroy(gameObject, lifetime);
    }

    void Update()
    {
        // --- Déplacement + accélération optionnelle ---
        currentSpeed += acceleration * Time.deltaTime;
        transform.Translate(Vector3.forward * currentSpeed * Time.deltaTime, Space.Self);
    }
}
