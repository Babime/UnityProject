using UnityEngine;

public class BillboardUI : MonoBehaviour
{
    void LateUpdate()
    {
        if (Camera.main != null)
            transform.rotation = Quaternion.LookRotation(
                transform.position - Camera.main.transform.position,
                Vector3.up);
    }
}