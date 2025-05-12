using UnityEngine;

public class WeaponDamageDealer : MonoBehaviour
{
    [SerializeField]
    public float damage = 10f;

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
