using System.Collections.Generic;
using UnityEngine;

public class ItemHandler : MonoBehaviour
{
    public Transform handAnchor; 

    private Dictionary<string, GameObject> inHandObjects = new Dictionary<string, GameObject>();
    private GameObject currentInHandObject;

    void Awake()
    {
        foreach (Transform child in handAnchor)
        {
            inHandObjects.Add(child.name, child.gameObject);
            child.gameObject.SetActive(false);
        }
        if (inHandObjects.Count > 0)
        {
            currentInHandObject = inHandObjects.Values.GetEnumerator().Current;
            currentInHandObject.SetActive(true);
        }
    }

    public void EquipItem(ItemData itemData)
    {
        if (currentInHandObject != null)
        {
            currentInHandObject.SetActive(false);
        }

        if (itemData != null && inHandObjects.ContainsKey(itemData.inHandObjectName))
        {
            currentInHandObject = inHandObjects[itemData.inHandObjectName];
            currentInHandObject.SetActive(true);
        }
        else
        {
            currentInHandObject = null;
        }
    }

    public void UnequipItem()
    {
        if (currentInHandObject != null)
        {
            currentInHandObject.SetActive(false);
            currentInHandObject = null;
        }
    }
}
