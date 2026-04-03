using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class UiManager : MonoBehaviour
{
    #region Variables

    [Header("Energy Settings")]
    [SerializeField] private Transform energyContainer; 
    [SerializeField] private GameObject energyPointPrefab; 
    private List<Image> energyIcons = new List<Image>();

    [Header("Sap Settings")]
    [SerializeField] private Transform sapContainer; 
    [SerializeField] private GameObject sapPointPrefab; 
    private List<Image> sapIcons = new List<Image>();

    #endregion
    

    #region Lifecycle

    private void OnDestroy()
    {
        DOTween.KillAll();
    }

    #endregion

    #region Energy Logic

    public void SetupEnergyBar(int maxEnergy) => SetupBar(energyContainer, energyIcons, energyPointPrefab, maxEnergy);
    public void UpdateEnergyDisplay(int currentEnergy) => UpdateDisplay(energyIcons, currentEnergy);

    #endregion

    #region Sap Logic

    public void SetupSapBar(int maxSap) => SetupBar(sapContainer, sapIcons, sapPointPrefab, maxSap);
    public void UpdateSapDisplay(int currentSap) => UpdateDisplay(sapIcons, currentSap);

    #endregion

    #region Generic Bar Logic 

    private void SetupBar(Transform container, List<Image> icons, GameObject prefab, int maxCount)
    {
        ClearContainer(container, icons);
        
        for (int i = 0; i < maxCount; i++)
        {
            GameObject obj = Instantiate(prefab, container);
            if (obj.TryGetComponent(out Image img))
            {
                img.raycastTarget = false;
                icons.Add(img);
                PlaySpawnAnimation(obj.transform, i);
            }
        }
    }

    private void UpdateDisplay(List<Image> icons, int currentCount)
    {
        for (int i = 0; i < icons.Count; i++)
        {
            Image icon = icons[i];
            bool shouldBeActive = (i < currentCount);

            if (icon.enabled != shouldBeActive)
            {
                if (shouldBeActive) 
                    AnimateGain(icon);
                else 
                    AnimateLoss(icon);
            }
        }
    }

    private void ClearContainer(Transform container, List<Image> icons)
    {
        foreach (Transform child in container) 
        {
            child.DOKill(); 
            Destroy(child.gameObject);
        }
        icons.Clear();
    }

    #endregion

    #region Animations

    private void PlaySpawnAnimation(Transform target, int index)
    {
        target.localScale = Vector3.zero;
        target.DOScale(Vector3.one, 0.5f)
            .SetEase(Ease.OutBack)
            .SetDelay(index * 0.05f);
    }

    private void AnimateLoss(Image icon)
    {
        icon.transform.DOPunchRotation(new Vector3(0, 0, 15), 0.3f);
        icon.DOFade(0.2f, 0.2f).OnComplete(() => {
            icon.enabled = false;
        });
    }

    private void AnimateGain(Image icon)
    {
        icon.enabled = true;
        icon.transform.DOKill(); 
        icon.transform.DOScale(1.2f, 0.1f).OnComplete(() => {
            icon.transform.DOScale(1.0f, 0.1f);
        });
        icon.DOFade(1f, 0.2f);
    }

    #endregion
}