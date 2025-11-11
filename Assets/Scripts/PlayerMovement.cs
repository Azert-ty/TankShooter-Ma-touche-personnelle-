//using UnityEngine;

//public class PlayerMovement : MonoBehaviour
//{
//    [Header("Mouvement de base")]
//    public float speed = 10.5f;
//    public float rotSpeed = 90f;

//    [Header("Game Feel")]
//    public float acceleration = 5f;       // pour lisser la montée de vitesse
//    public float deceleration = 5f;       // pour ralentir naturellement
//    public float fovSpeedEffect = 5f;     // effet de vitesse sur la caméra

//    [Header("Feedback sonore")]
//    public AudioClip engineIdleSound;     // bruit à l’arrêt
//    public AudioClip engineMoveSound;     // bruit en mouvement
//    public float pitchVariation = 0.3f;   // variation de ton selon la vitesse

//    private float currentSpeed = 0f;
//    private AudioSource audioSource;
//    private Camera playerCam;
//    private float baseFOV;

//    private bool isMoving = false;

//    void Start()
//    {
//        // --- Audio ---
//        audioSource = GetComponent<AudioSource>();
//        if (audioSource == null)
//            audioSource = gameObject.AddComponent<AudioSource>();

//        audioSource.loop = true;
//        audioSource.spatialBlend = 0f; // son 2D pour éviter l’effet de distance
//        audioSource.volume = 0.6f;

//        // Démarrer le son moteur au ralenti
//        if (engineIdleSound != null)
//        {
//            audioSource.clip = engineIdleSound;
//            audioSource.Play();
//        }

//        // --- Caméra ---
//        playerCam = Camera.main;
//        if (playerCam != null)
//            baseFOV = playerCam.fieldOfView;
//    }

//    void Update()
//    {
//        HandleMovement();
//        HandleRotation();
//        HandleCameraFOV();
//        HandleAudio();
//    }

//    void HandleMovement()
//    {
//        bool movingForward = Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow);
//        bool movingBackward = Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow);

//        // Accélération / Décélération
//        if (movingForward || movingBackward)
//        {
//            currentSpeed = Mathf.Lerp(currentSpeed, speed, Time.deltaTime * acceleration);
//            isMoving = true;
//        }
//        else
//        {
//            currentSpeed = Mathf.Lerp(currentSpeed, 0, Time.deltaTime * deceleration);
//            isMoving = currentSpeed > 0.1f; // encore en glissement
//        }

//        // Mouvement
//        if (movingForward)
//            transform.Translate(Vector3.forward * currentSpeed * Time.deltaTime);
//        else if (movingBackward)
//            transform.Translate(Vector3.back * currentSpeed * 0.5f * Time.deltaTime);
//    }

//    void HandleRotation()
//    {
//        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
//            transform.Rotate(Vector3.down * rotSpeed * Time.deltaTime);
//        else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
//            transform.Rotate(Vector3.up * rotSpeed * Time.deltaTime);
//    }

//    void HandleCameraFOV()
//    {
//        if (playerCam == null) return;

//        float targetFOV = baseFOV + (currentSpeed / speed) * fovSpeedEffect;
//        playerCam.fieldOfView = Mathf.Lerp(playerCam.fieldOfView, targetFOV, Time.deltaTime * 2);
//    }

//    void HandleAudio()
//    {
//        if (isMoving && engineMoveSound != null)
//        {
//            if (audioSource.clip != engineMoveSound)
//            {
//                audioSource.clip = engineMoveSound;
//                audioSource.Play();
//            }

//            // Variation du ton selon la vitesse
//            audioSource.pitch = 1f + (currentSpeed / speed) * pitchVariation;
//        }
//        else if (!isMoving && engineIdleSound != null)
//        {
//            if (audioSource.clip != engineIdleSound)
//            {
//                audioSource.clip = engineIdleSound;
//                audioSource.Play();
//            }

//            // Ralenti doux du son
//            audioSource.pitch = Mathf.Lerp(audioSource.pitch, 1f, Time.deltaTime * 2);
//        }
//    }
//}


using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Mouvement de base")]
    public float speed = 10.5f;
    public float rotSpeed = 90f;

    [Header("Game Feel")]
    public float acceleration = 5f;
    public float deceleration = 5f;
    public float fovSpeedEffect = 5f;

    [Header("Feedback sonore")]
    public AudioClip engineIdleSound;
    public AudioClip engineMoveSound;
    public float pitchVariation = 0.3f;

    private float currentSpeed = 0f;
    private AudioSource audioSource;
    private Camera playerCam;
    private float baseFOV;

    private bool isMoving = false;

    // ✅ Ajout mobile
    private bool moveForwardMobile = false;
    private bool moveBackwardMobile = false;
    private bool turnLeftMobile = false;
    private bool turnRightMobile = false;

    void Start()
    {
        // --- 🎧 AUDIO ---
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();

        audioSource.loop = true;
        audioSource.spatialBlend = 0f; // Son 2D (pas affecté par la distance)
        audioSource.volume = 0.6f;

        if (engineIdleSound != null)
        {
            audioSource.clip = engineIdleSound;
            audioSource.Play();
        }

        // --- 🎥 CAMÉRA ---
        playerCam = Camera.main;
        if (playerCam != null)
            baseFOV = playerCam.fieldOfView;

        // --- 📱 UI MOBILE ---
        // Désactive le Canvas mobile sur PC ou dans l'éditeur
        #if !(UNITY_ANDROID || UNITY_IOS || UNITY_WEBGL)
            GameObject mobileUI = GameObject.Find("MobileUI");
            if (mobileUI != null)
                mobileUI.SetActive(false);
        #endif
    }


    void Update()
    {
        HandleMovement();
        HandleRotation();
        HandleCameraFOV();
        HandleAudio();
    }

    void HandleMovement()
    {
        // ✅ Contrôles clavier + mobile
        bool movingForward = Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow) || moveForwardMobile;
        bool movingBackward = Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow) || moveBackwardMobile;

        if (movingForward || movingBackward)
        {
            currentSpeed = Mathf.Lerp(currentSpeed, speed, Time.deltaTime * acceleration);
            isMoving = true;
        }
        else
        {
            currentSpeed = Mathf.Lerp(currentSpeed, 0, Time.deltaTime * deceleration);
            isMoving = currentSpeed > 0.1f;
        }

        if (movingForward)
            transform.Translate(Vector3.forward * currentSpeed * Time.deltaTime);
        else if (movingBackward)
            transform.Translate(Vector3.back * currentSpeed * 0.5f * Time.deltaTime);
    }

    void HandleRotation()
    {
        // ✅ Clavier + mobile
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow) || turnLeftMobile)
            transform.Rotate(Vector3.down * rotSpeed * Time.deltaTime);
        else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow) || turnRightMobile)
            transform.Rotate(Vector3.up * rotSpeed * Time.deltaTime);
    }

    void HandleCameraFOV()
    {
        if (playerCam == null) return;

        float targetFOV = baseFOV + (currentSpeed / speed) * fovSpeedEffect;
        playerCam.fieldOfView = Mathf.Lerp(playerCam.fieldOfView, targetFOV, Time.deltaTime * 2);
    }

    void HandleAudio()
    {
        if (isMoving && engineMoveSound != null)
        {
            if (audioSource.clip != engineMoveSound)
            {
                audioSource.clip = engineMoveSound;
                audioSource.Play();
            }
            audioSource.pitch = 1f + (currentSpeed / speed) * pitchVariation;
        }
        else if (!isMoving && engineIdleSound != null)
        {
            if (audioSource.clip != engineIdleSound)
            {
                audioSource.clip = engineIdleSound;
                audioSource.Play();
            }
            audioSource.pitch = Mathf.Lerp(audioSource.pitch, 1f, Time.deltaTime * 2);
        }
    }

    // ✅ Fonctions publiques pour les boutons tactiles (à lier dans l’Inspector)
    public void SetMoveForward(bool pressed) => moveForwardMobile = pressed;
    public void SetMoveBackward(bool pressed) => moveBackwardMobile = pressed;
    public void SetTurnLeft(bool pressed) => turnLeftMobile = pressed;
    public void SetTurnRight(bool pressed) => turnRightMobile = pressed;
}
