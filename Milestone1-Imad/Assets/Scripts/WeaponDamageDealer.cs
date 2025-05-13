using UnityEngine;
using UnityEngine.UIElements;

public class WeaponDamageDealer : MonoBehaviour
{
    [SerializeField]
    public float damage = 10f;
    private BoxCollider weaponCollider;

    private void Awake()
    {
        weaponCollider = GetComponent<BoxCollider>();
        DisableWeapon();
    }

    public void EnableWeapon()
    {
        weaponCollider.enabled = true;
    }

    public void DisableWeapon()
    {
        weaponCollider.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == this.tag) return;
        DamageReceiver receiver = other.GetComponent<DamageReceiver>();
        if (receiver != null)
        {
            receiver.TakeDamage(damage);
        }
    }
}
