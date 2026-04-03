using UnityEngine;

public class Sap : MonoBehaviour, ISapLockable
{
    [SerializeField] private Transform lockTransform;

    private bool empty = false;
    
    public Transform GetLockTransform()
    {
        return lockTransform != null ? lockTransform : transform;
    }

    public bool IsLockable()
    {
        return !empty;
    }

    public float GetLockPriority()
    {
        return 1f;
    }

    public void GiveSap()
    {
        empty = true;
    }
}