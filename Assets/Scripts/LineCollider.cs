using System.Collections.Generic;
using UnityEngine;

public class LineCollider : MonoBehaviour
{
    public int count;
    private HashSet<BoxCollider> colliders = new HashSet<BoxCollider>();

    public HashSet<BoxCollider> GetColliders() { return colliders; }

    private void OnCollisionEnter(Collision other)
    {
        BoxCollider hitCollider = other.gameObject.GetComponent<BoxCollider>();
        colliders.Add(hitCollider);
    }

    public void RemoveCollider(BoxCollider collider)
    {
        colliders.Remove(collider);
    }

    public int GetCount()
    {
        return colliders.Count;
    }

    public void ClearColliderSet()
    {
        colliders.Clear();
        count = colliders.Count;
    }

    public int GetDestoyedColliderCount()
    {
        int count = 0;
        foreach (var collider in colliders)
        {
            if (collider.gameObject.CompareTag("Virus"))
            {
                count++;
            }
        }
        return count;
    }

    private void Update()
    {
        count = colliders.Count;
    }
}
