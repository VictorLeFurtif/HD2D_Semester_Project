using UnityEngine;

public class AnimationManager : MonoBehaviour
{
    #region Variables
    public Camera cam;
    [SerializeField] private Transform mainTransform;
    [SerializeField] private Animator animator;
    [SerializeField] private SpriteRenderer spriteRenderer;
    
    private static readonly int VelocityHash = Animator.StringToHash("Velocity");
    private static readonly int MoveXHash = Animator.StringToHash("moveX");
    private static readonly int MoveYHash = Animator.StringToHash("moveY");
    
    #endregion

    #region Public Methods

    public void HandleAnimation(float velocity, Vector2 input)
    {
        animator.SetFloat(VelocityHash, velocity);

        if (input.magnitude > 0.1f)
        {
            animator.SetFloat(MoveXHash, Mathf.Lerp(animator.GetFloat(MoveXHash), input.x, 15f * Time.deltaTime));
            animator.SetFloat(MoveYHash, Mathf.Lerp(animator.GetFloat(MoveYHash), input.y, 15f * Time.deltaTime));
        }
    }
    
    #endregion
}