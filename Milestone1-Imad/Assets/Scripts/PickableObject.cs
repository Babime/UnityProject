using UnityEngine;

public class PickableObject : MonoBehaviour
{
    public float rotationSpeed = 50f;
    public ParticleSystem pickupEffect;

    private bool playerInRange = false;
    private PlayerInventory playerInventory;
    private ItemData itemData;
    private Outline[] outlines;


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
            PickUp();
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

    private void PickUp()
    {
        if (itemData == null) return;

        playerInventory.AddItem(itemData);

        if (pickupEffect != null)
        {
            Instantiate(pickupEffect, transform.position, Quaternion.identity);
        }

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
