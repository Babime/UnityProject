using System.Collections;
using UnityEngine;

public class PickableObject : MonoBehaviour
{
    public float rotationSpeed = 50f;
    public ParticleSystem pickupEffect;

    private bool playerInRange = false;
    private PlayerInventory playerInventory;
    private ItemData itemData;
    private Outline[] outlines;
    public float pickupDuration = 0.6f;
    public Vector3 pickupOffset = new Vector3(0, 1.5f, 0);
    public float initialBackDistance = 0.3f;


    void Awake()
    {
        outlines = GetComponentsInChildren<Outline>(true);

        ItemDataHolder refComponent = GetComponent<ItemDataHolder>();

        if (refComponent != null)
        {
            itemData = refComponent.itemData;
        }
        else
        {
            Debug.LogError($"PickableObject: No ItemDataHolder found on '{gameObject.name}'. Make sure to add it.");
        }

        SetupCollider();
    }

    void Update()
    {
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime, Space.World);

        if (playerInRange && playerInventory != null && Input.GetKeyDown(KeyCode.JoystickButton3))
        {
            StartCoroutine(PickupRoutine());
        }
        foreach (var o in outlines)
            o.enabled = playerInRange;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (playerInventory == null)
        {
            playerInventory = other.GetComponent<PlayerInventory>();
        }

        if (playerInventory != null)
        {
            playerInRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<PlayerInventory>() != null)
        {
            playerInRange = false;
            playerInventory = null;
        }
    }

    private IEnumerator PickupRoutine()
{
    playerInRange = false;
    foreach (var o in outlines) o.enabled = false;
    if (TryGetComponent<Collider>(out var col)) col.enabled = false;

    Vector3 startPos = transform.position;
    Vector3 endPos   = playerInventory.transform.position + pickupOffset;

    Vector3 awayDir = (startPos - endPos).normalized;
    Vector3 backPos = startPos + awayDir * initialBackDistance;

    float halfDur = pickupDuration * 0.5f;
    Vector3 startScale = transform.localScale;

    float t = 0f;
    while (t < halfDur)
    {
        float u = t / halfDur;
        transform.position = Vector3.Lerp(startPos, backPos, u);
        transform.localScale = Vector3.Lerp(startScale, startScale * 0.8f, u);
        t += Time.deltaTime;
        yield return null;
    }

    transform.position   = backPos;
    transform.localScale = startScale * 0.8f;

    t = 0f;
    while (t < halfDur)
    {
        float u = t / halfDur;
        transform.position = Vector3.Lerp(backPos, endPos, u);
        transform.localScale = Vector3.Lerp(startScale * 0.8f, Vector3.zero, u);
        t += Time.deltaTime;
        yield return null;
    }

    transform.position   = endPos;
    transform.localScale = Vector3.zero;

    playerInventory.AddItem(itemData);
    if (pickupEffect != null)
        Instantiate(pickupEffect, endPos, Quaternion.identity);

    Destroy(gameObject);
}


    private void SetupCollider()
    {
        Collider existingCollider = GetComponent<Collider>();

        if (existingCollider == null)
        {
            Debug.LogWarning("PickableObject: No collider found on the object! Please add a collider manually.");
            return;
        }

        existingCollider.isTrigger = true;
    }
}
