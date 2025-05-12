using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FadeMessageUI : MonoBehaviour
{
    [Header("UI References")]
    [Tooltip("L’Image noire du panel qui couvre tout l’écran")]
    public Image fadeImage;
    [Tooltip("Le composant TextMeshProUGUI qui affichera le message")]
    public TextMeshProUGUI messageText;

    [Header("Timings")]
    [Tooltip("Durée du fondu In/Out")]
    public float fadeDuration = 1f;
    [Tooltip("Temps d’affichage complet du texte après le typewriter")]
    public float displayDuration = 3f;
    [Tooltip("Délai entre chaque caractère du typewriter")]
    public float typewriterDelay = 0.05f;


    public IEnumerator Play(string message)
    {
        // 0) Initialisation
        fadeImage.color = new Color(0, 0, 0, 0);
        messageText.text = message;
        messageText.maxVisibleCharacters = 0;
        messageText.gameObject.SetActive(true);

        // 1) Fade In
        float t = 0f;
        while (t < fadeDuration)
        {
            float a = t / fadeDuration;
            fadeImage.color = new Color(0, 0, 0, a);
            t += Time.unscaledDeltaTime;
            yield return null;
        }
        fadeImage.color = Color.black;

        // 2) Typewriter Effect (lettre par lettre)
        int total = messageText.textInfo.characterCount;
        for (int i = 1; i <= total; i++)
        {
            messageText.maxVisibleCharacters = i;
            yield return new WaitForSecondsRealtime(typewriterDelay);
        }

        // 3) Pause sur le texte complet
        yield return new WaitForSecondsRealtime(displayDuration);

        // 4) Fade Out
        t = 0f;
        while (t < fadeDuration)
        {
            float a = 1f - (t / fadeDuration);
            fadeImage.color = new Color(0, 0, 0, a);
            t += Time.unscaledDeltaTime;
            yield return null;
        }
        fadeImage.color = new Color(0, 0, 0, 0);

        // 5) Nettoyage
        Destroy(gameObject);
    }
}
