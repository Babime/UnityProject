using System.Collections;
using UnityEngine;

public class SpecialChestController : MonoBehaviour
{
    [Header("Interaction")]
    public float interactRange = 2f;
    public KeyCode openKey = KeyCode.E;

    [Header("Highlight")]
    [Tooltip("Composants Outline à activer quand le joueur s'approche")]
    public Outline[] outlines;

    [Header("Animation")]
    public Animator animator;
    public string openTrigger = "Open";

    [Header("Shrink Effect")]
    public float shrinkDuration = 0.5f;

    [Header("Portal")]
    public GameObject portalPrefab;
    [Tooltip("Décalage vertical pour que le portail n'apparaisse pas dans le sol")]
    public Vector3 portalSpawnOffset = new Vector3(0, 0.5f, 0);
    public float portalDelay = 0.5f;

    [Header("Player Pull")]
    [Tooltip("Durée pendant laquelle le joueur est aspiré")]
    public float pullDuration = 1f;
    [Tooltip("Vitesse de rotation du joueur pendant l'aspiration (°/s)")]
    public float pullSpinSpeed = 720f;

    private Transform player;
    private bool hasActivated = false;

    void Awake()
    {
        player = GameObject.FindWithTag("Player")?.transform;
        if (animator != null)
            animator.updateMode = AnimatorUpdateMode.UnscaledTime;

        if (outlines == null || outlines.Length == 0)
            outlines = GetComponentsInChildren<Outline>(true);

        foreach (var o in outlines)
            o.enabled = false;
    }

    void Update()
    {
        if (hasActivated || player == null) return;

        float dist = Vector3.Distance(transform.position, player.position);
        bool canInteract = dist <= interactRange;

        foreach (var o in outlines)
            o.enabled = canInteract;

        if (canInteract && Input.GetKeyDown(openKey))
            StartCoroutine(OpenAndSpawnPortal());
    }

    private IEnumerator OpenAndSpawnPortal()
    {
        hasActivated = true;
        foreach (var o in outlines) o.enabled = false;

        // 1) Animation d'ouverture
        if (animator != null)
            animator.SetTrigger(openTrigger);

        // 2) Shrink
        Vector3 startScale = transform.localScale;
        float tShrink = 0f;
        while (tShrink < shrinkDuration)
        {
            transform.localScale = Vector3.Lerp(startScale, Vector3.zero, tShrink / shrinkDuration);
            tShrink += Time.unscaledDeltaTime;
            yield return null;
        }
        transform.localScale = Vector3.zero;

        // 3) Attendre avant le portail
        yield return new WaitForSecondsRealtime(portalDelay);

        // 4) Spawn portail
        Vector3 portalPos = transform.position + portalSpawnOffset;
        GameObject portal = Instantiate(portalPrefab, portalPos, Quaternion.identity);

        // 5) Attirer et faire tourner le joueur dans le portail
        yield return StartCoroutine(PullPlayerInto(portalPos, portal.transform));

        // 6) Détruire le coffre
        Destroy(gameObject);
    }

    private IEnumerator PullPlayerInto(Vector3 targetPos, Transform portalTransform)
    {
        // Désactiver le controller et movement
        var cc = player.GetComponent<CharacterController>();
        if (cc != null) cc.enabled = false;
        var pm = player.GetComponent<MonoBehaviour>(); // remplace par ton script de movement
        if (pm != null && pm.enabled) pm.enabled = false;

        Vector3 startPos = player.position;
        float elapsed = 0f;

        while (elapsed < pullDuration)
        {
            float t = elapsed / pullDuration;
            // Déplacement
            player.position = Vector3.Lerp(startPos, targetPos, t);
            // Rotation autour de l'axe Y
            player.Rotate(Vector3.up, pullSpinSpeed * Time.deltaTime, Space.World);
            elapsed += Time.deltaTime;
            yield return null;
        }

        // Assurer la position et rotation finale
        // player.position = targetPos;
        // On peut ici aligner son orientation face à l'intérieur du portail
        // player.rotation = portalTransform.rotation;

        // (Optionnel) parenté pour rester dans le portail
        player.SetParent(portalTransform);

        // Réactiver controller et movement après un bref délai ou condition
        yield return null;
        if (cc != null) cc.enabled = true;
        if (pm != null) pm.enabled = true;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, interactRange);
    }
}
