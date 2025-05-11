using UnityEngine;

public class WeaponDamageDealer : MonoBehaviour
{
    [SerializeField]
    public float damage = 10f;

    private void OnTriggerEnter(Collider other)
    {
        DamageReceiver receiver = other.GetComponent<DamageReceiver>();
        if (receiver != null)
        {
            receiver.TakeDamage(damage);
        }
    }
}
