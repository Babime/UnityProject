using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class EnemySpawner : MonoBehaviour
{
    [System.Serializable]
    public class MobEntry
    {
        public GameObject prefab;
        public Vector3 localPosition;
    }
    public List<MobEntry> mobs = new List<MobEntry>();
    public float respawnDelay   = 10f;

    private bool hasSpawned = false;
    private List<GameObject> spawned = new List<GameObject>();
    private Transform player;

    void Reset()
    {
        var col = GetComponent<Collider>();
        if (col == null) col = gameObject.AddComponent<BoxCollider>();
        col.isTrigger = true;
    }

    void Awake()
    {
        var p = GameObject.FindGameObjectWithTag("Player");
        if (p != null) player = p.transform;
    }

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player") || hasSpawned)
            return;
        SpawnAll();
    }

    void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        StartCoroutine(RespawnDelay());
    }

    private void SpawnAll()
    {
        if (player == null) return;
        hasSpawned = true;
        spawned.Clear();

        foreach (var entry in mobs)
        {
            if (entry.prefab == null) continue;

            Vector3 worldPos = transform.TransformPoint(entry.localPosition);

            Vector3 toPlayer = player.position - worldPos;
            toPlayer.y = 0; 
            Quaternion worldRot = Quaternion.LookRotation(toPlayer.normalized, Vector3.up);

            var go = Instantiate(entry.prefab, worldPos, worldRot);
            spawned.Add(go);
        }
    }

    private IEnumerator RespawnDelay()
    {
        yield return new WaitForSeconds(respawnDelay);
        hasSpawned = false;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        foreach (var entry in mobs)
        {
            Vector3 wp = transform.TransformPoint(entry.localPosition);
            Gizmos.DrawWireSphere(wp, 0.3f);
            Gizmos.DrawLine(transform.position, wp);
        }
    }
}
