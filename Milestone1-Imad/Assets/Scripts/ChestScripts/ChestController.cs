using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestController : MonoBehaviour
{
    [Header("Interaction")]
    public float interactRange = 2f;
    public KeyCode openKey = KeyCode.E;
    public GameObject interactUI;

    [Header("Loot")]
    public List<GameObject> lootPrefabs;
    public Transform lootSpawnPoint;
    public float lootSpreadRadius = 0.5f;

    [Header("Shrink")]
    public float shrinkDuration = 0.5f;

    private Transform player;
    private bool isOpened = false;
    private Outline[] outlines;

    void Awake()
    {
        var p = GameObject.FindGameObjectWithTag("Player");
        if (p) player = p.transform;

        outlines = GetComponentsInChildren<Outline>(true);
        foreach (var o in outlines)
            o.enabled = false;
    }

    void Start()
    {
        if (interactUI)
            interactUI.SetActive(false);
    }

    void Update()
    {
        if (isOpened || player == null)
            return;

        float dist = Vector3.Distance(transform.position, player.position);
        bool canInteract = dist <= interactRange;

        foreach (var o in outlines)
            o.enabled = canInteract;

        if (interactUI)
            interactUI.SetActive(canInteract);

        if (canInteract && (Input.GetKeyDown(openKey) || Input.GetKeyDown(KeyCode.JoystickButton1)))
            StartCoroutine(OpenAndDisappear());
    }

    private IEnumerator OpenAndDisappear()
    {
        isOpened = true;

        foreach (var o in outlines)
            o.enabled = false;
        if (interactUI)
            interactUI.SetActive(false);

        foreach (var prefab in lootPrefabs)
        {
            Vector2 rnd = Random.insideUnitCircle * lootSpreadRadius;
            Vector3 spawnPos = lootSpawnPoint.position + new Vector3(rnd.x, 0, rnd.y);

            var loot = Instantiate(prefab, spawnPos, Quaternion.identity);

            Vector3 dirFromChest = (spawnPos - transform.position).normalized;
            float spawnOffset = 0.5f;
            Vector3 finalPos = spawnPos + dirFromChest * spawnOffset;

            var fb = loot.AddComponent<FountainBounceEffect>();
            fb.endPosition   = finalPos;
            fb.arcHeight     = 1.5f;   
            fb.arcDuration   = 0.5f;   
            fb.bounceHeight  = 0.2f;
            fb.bounceDuration= 0.2f;
        }

        Vector3 startScale = transform.localScale;
        float t = 0f;
        while (t < shrinkDuration)
        {
            transform.localScale = Vector3.Lerp(startScale, Vector3.zero, t / shrinkDuration);
            t += Time.deltaTime;
            yield return null;
        }
        transform.localScale = Vector3.zero;

        Destroy(gameObject);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, interactRange);
    }
}
