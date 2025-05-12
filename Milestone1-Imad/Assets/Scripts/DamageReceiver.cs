using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DamageReceiver : MonoBehaviour
{
    [System.Serializable]
    public class DamageEvent : UnityEvent<float> { }

    [SerializeField]
    private DamageEvent onTakeDamage;

    public void TakeDamage(float damage)
    {
        if (onTakeDamage != null)
        {
            onTakeDamage.Invoke(damage);
        }
    }
}
