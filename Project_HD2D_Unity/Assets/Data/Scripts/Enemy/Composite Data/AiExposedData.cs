using UnityEngine;

[CreateAssetMenu(fileName = "AiExposedData", menuName = "Enemy/AiExposedData")]
public class AiExposedData : ScriptableObject
{
    [field: SerializeField] public float ExposedTime { get; set; } = 1f;
}