using System.Collections;
using UnityEngine;

public class FountainBounceEffect : MonoBehaviour
{
    [Tooltip("Position finale (sans Y) où doit arriver l'objet")]
    public Vector3 endPosition;

    [Tooltip("Hauteur maximale de l'arc (en unités)")]
    public float arcHeight = 2f;

    [Tooltip("Durée totale de l'arc (montée + descente) en secondes")]
    public float arcDuration = 0.6f;

    [Tooltip("Hauteur du rebond final")]
    public float bounceHeight = 0.3f;

    [Tooltip("Durée du rebond final en secondes")]
    public float bounceDuration = 0.2f;

    void Start()
    {
        StartCoroutine(FountainAndBounce());
    }

    private IEnumerator FountainAndBounce()
    {
        Vector3 startPos = transform.position;
        Vector3 horizontalDelta = new Vector3(
            endPosition.x - startPos.x,
            0,
            endPosition.z - startPos.z
        );
        float t = 0f;
        // Phase arc (parabole : y = 4h * T*(1-T) )
        while (t < 1f)
        {
            float T = t;
            // horizontal interpolation
            Vector3 horiz = startPos + horizontalDelta * T;
            // vertical parabola
            float yOffset = 4f * arcHeight * T * (1f - T);
            transform.position = horiz + Vector3.up * yOffset;
            t += Time.deltaTime / arcDuration;
            yield return null;
        }
        // s'assurer
        transform.position = endPosition;

        // Phase de rebond
        t = 0f;
        while (t < 1f)
        {
            // sin(pi * t) fait un up puis down
            float yBounce = Mathf.Sin(Mathf.PI * t) * bounceHeight;
            transform.position = endPosition + Vector3.up * yBounce;
            t += Time.deltaTime / bounceDuration;
            yield return null;
        }
        // fin
        transform.position = endPosition;
    }
}
