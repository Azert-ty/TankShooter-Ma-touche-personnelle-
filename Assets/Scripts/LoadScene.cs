using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour
{
    [Header("Réglages de transition")]
    public string sceneName;
    public float delayBeforeLoad = 0f;
    public AudioClip transitionSound;
    public Animator transitionAnimator;

    private AudioSource audioSource;

    public void Load()
    {
        // Lancement d’une coroutine pour gérer la transition
        StartCoroutine(LoadSceneCoroutine());
    }

    private IEnumerator LoadSceneCoroutine()
    {
        // --- 1️⃣ Feedback sonore ---
        if (transitionSound != null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.PlayOneShot(transitionSound);
        }

        // --- 2️⃣ Animation de transition (ex : fondu, portail, effet de flash) ---
        if (transitionAnimator != null)
        {
            transitionAnimator.SetTrigger("Start");
        }

        // --- 3️⃣ Attente avant le chargement effectif ---
        if (delayBeforeLoad > 0)
            yield return new WaitForSeconds(delayBeforeLoad);

        // --- 4️⃣ Chargement de la nouvelle scène ---
        SceneManager.LoadScene(sceneName);
    }
}
