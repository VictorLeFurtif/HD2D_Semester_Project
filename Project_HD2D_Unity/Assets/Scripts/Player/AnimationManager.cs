using UnityEngine;

public class AnimationManager : MonoBehaviour
{
    #region Variables

    private static readonly int Velocity = Animator.StringToHash("Velocity");
    
    [SerializeField] private Animator animator;

    #endregion

    #region Public Methods

    public void HandleAnimation(float velocity)
    {
        animator.SetFloat(Velocity, velocity);
    }

    #endregion
}