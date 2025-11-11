using UnityEngine;

public class TankShoot : MonoBehaviour
{
    [Header("Prefab et Spawn")]
    public GameObject bullet;
    public GameObject spawnPosObj;

    [Header("Game Feel")]
    public AudioClip shootSound;
    public float bulletSpeed = 30f;
    public float fireRate = 0.5f;

    private AudioSource audioSource;
    private float lastShotTime = 0f;

    // ✅ Mobile
    private bool shootHeldMobile = false;

    void Start()
    {
        // --- 🎧 AUDIO ---
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();

        audioSource.spatialBlend = 0f; // Son 2D
        audioSource.volume = 0.7f;

        // --- 📱 UI MOBILE ---
        // Désactiver le Canvas Mobile sur PC et dans l’éditeur
        #if !(UNITY_ANDROID || UNITY_IOS || UNITY_WEBGL)
            GameObject mobileUI = GameObject.Find("MobileUI");
            if (mobileUI != null)
                mobileUI.SetActive(false);
        #endif
    }


    void Update()
    {
        bool canShoot = Time.time - lastShotTime >= fireRate;

        // ✅ Clavier + Mobile (maintenir le bouton mobile ou espace)
        if ((Input.GetKey(KeyCode.Space) || shootHeldMobile) && canShoot)
        {
            Shoot();
            lastShotTime = Time.time;
        }
    }

    void Shoot()
    {
        // Créer la balle
        GameObject newBullet = Instantiate(bullet, spawnPosObj.transform.position, transform.rotation);

        // Appliquer la vitesse si la balle a un Rigidbody
        Rigidbody rb = newBullet.GetComponent<Rigidbody>();
        if (rb != null)
            rb.linearVelocity = transform.forward * bulletSpeed;

        // Feedback sonore
        if (shootSound != null)
            audioSource.PlayOneShot(shootSound);
    }

    // ✅ Fonctions publiques pour EventTrigger
    public void StartShooting()
    {
        shootHeldMobile = true;
    }

    public void StopShooting()
    {
        shootHeldMobile = false;
    }
}
