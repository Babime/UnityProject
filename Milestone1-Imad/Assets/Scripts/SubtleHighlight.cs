using UnityEngine;
using System.Linq;

[DisallowMultipleComponent]
public class SubtleHighlight : MonoBehaviour
{
    [Tooltip("Teinte appliquée en surbrillance")]
    public Color highlightTint = new Color(1f, 1f, 0f, 0.2f);

    private MeshRenderer[] rends;
    private MaterialPropertyBlock mpb;
    private Color[] originalColors;

    void Awake()
    {
        // Récupère tous les MeshRenderer enfants (Lid, Chest)
        rends = GetComponentsInChildren<MeshRenderer>();
        mpb   = new MaterialPropertyBlock();
        originalColors = rends.Select(r => r.sharedMaterial.color).ToArray();
    }

    /// <summary>
    /// Active ou désactive la surbrillance subtile.
    /// </summary>
    public void SetHighlight(bool on)
    {
        for (int i = 0; i < rends.Length; i++)
        {
            var r = rends[i];
            r.GetPropertyBlock(mpb);
            Color baseCol = originalColors[i];
            Color col = on ? baseCol + highlightTint : baseCol;
            mpb.SetColor("_BaseColor", col);
            r.SetPropertyBlock(mpb);
        }
    }
}
