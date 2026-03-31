using System;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

public class Root : MonoBehaviour
{
    #region Variables

    [Header("Link VAT Managers")] 
    public List<VATManager> vatManagers;
    
    [Header("Link Flaws")]
    public List<Flaw> flaws;
    
    [Header("Current State")]
    [SerializeField] private int currentEnergy = 0;
    [SerializeField] private int maxEnergy = 0;
    
    public int CurrentEnergy => currentEnergy;

    #endregion
    
    #region Unity Lifecycle

    private void Awake()
    {
        InitFlaws();
        InitVatManagers();
    }

    #endregion
    
    #region Init

    private void InitFlaws()
    {
        foreach (Flaw flaw in flaws) flaw.SetRoot(this);
    }

    private void InitVatManagers()
    {
        foreach (VATManager vatManager in vatManagers) vatManager.SetRoot(this);
    }

    #endregion
    
    #region Link Flaws

    private void SetEnergy(int energy)
    {
        currentEnergy = Mathf.Clamp(energy, 0, maxEnergy);
    }

    public void AddEnergy()
    {
        SetEnergy(currentEnergy + 1);
        
        foreach (var vatManager in vatManagers)
        {
            if (vatManager.IsAtMaximumEnergy()) continue;
        }
    }

    public void RemoveEnergy()
    {
        SetEnergy(currentEnergy - 1);
        
        foreach (var vatManager in vatManagers)
        {
            if (!vatManager.IsContainingEnergy()) continue;
              
        }
    }

    #endregion
    
    #region Helper 

    public bool IsContainingEnergy() => currentEnergy > 0;
    public bool IsAtMaximumEnergy()  => currentEnergy >= maxEnergy;

    #endregion
    
    
    #region Debug Gizmos

    private void OnDrawGizmos()
    {
#if UNITY_EDITOR
        
        GUIStyle debugStyle = new GUIStyle
        {
            normal =
            {
                textColor = Color.red
            },
            fontSize = 30,
            fontStyle = FontStyle.Bold,
            alignment = TextAnchor.MiddleCenter
        };

        Handles.zTest = UnityEngine.Rendering.CompareFunction.Always;
        Handles.Label(transform.position, "ENERGY: " + currentEnergy, debugStyle);
#endif
        
        if (flaws != null)
        {
            Gizmos.color = Color.yellow;
            foreach (Flaw flaw in flaws)
            {
                if (flaw != null)
                {
                    Gizmos.DrawLine(transform.position, flaw.transform.position);
                    Gizmos.DrawWireSphere(flaw.transform.position, 0.3f);
                }
            }
        }

        if (vatManagers != null)
        {
            Gizmos.color = Color.cyan;
            foreach (VATManager vatManager in vatManagers)
            {
                if (vatManager != null)
                {
                    Gizmos.DrawLine(transform.position, vatManager.transform.position);
                    Gizmos.DrawWireCube(vatManager.transform.position, Vector3.one * 0.5f);
                }
            }
        }
        
    }
    

    #endregion
}
