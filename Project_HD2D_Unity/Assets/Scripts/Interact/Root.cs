using System;
using UnityEditor;
using UnityEngine;

public class Root : MonoBehaviour, IEnergyLockable
{
    private VATManager vatManager;
    [SerializeField] private Transform pivotPoint;

    #region IEnergyLockable

    public Transform GetLockTransform() => pivotPoint;
    public bool IsLockable() => true;
    public float GetLockPriority() => 1f;

    public bool IsContainingEnergy() => vatManager.IsContainingEnergy();
    public bool IsAtMaximumEnergy() => vatManager.IsAtMaximumEnergy();

    public void AddEnergy() => vatManager.AddEnergy();
    public void RemoveEnergy() => vatManager.RemoveEnergy();

    #endregion

    private void OnDrawGizmos()
    {
        GUIStyle style = new GUIStyle();
        style.normal.textColor = Color.white;
        style.alignment = TextAnchor.MiddleCenter;
        style.fontStyle = FontStyle.Bold;
        
        Handles.Label(transform.position,"ROOT",style);
    }

    public void SetVATManager(VATManager _vatManager)
    {
        vatManager = _vatManager;
    }
}