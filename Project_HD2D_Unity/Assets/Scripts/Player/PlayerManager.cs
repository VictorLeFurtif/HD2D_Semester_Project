using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    #region Variables

    [SerializeField] private InputManager     inputManager;
    [SerializeField] private PlayerController playerController;
    [SerializeField] private AnimationManager animationManager;
    [SerializeField] private LockOnSystem     lockOnSystem;

    [SerializeField] private Transform cameraTransform;
    [SerializeField] private Transform visualTransform;
    [SerializeField] private Transform shootOriginPoint;

    [SerializeField] private PlayerCursor    playerCursor;
    [SerializeField] private Rigidbody       rb;

    [Tooltip("Minimum stick magnitude to update rotation (deadzone)")]
    [SerializeField] private float inputDeadzone = 0.8f;

    private Vector3 m_targetDirection = Vector3.zero;
    private Vector2 m_blendInput      = Vector2.zero;
    private Vector3 m_shootDirection  = Vector3.zero;

    [SerializeField] private ShootingSystem m_shootingSystem;

    #endregion

    #region Unity Lifecycle


    private void OnEnable()
    {
        inputManager.OnLockToggle  += lockOnSystem.ToggleLock;
        inputManager.OnAttackMelee += playerController.TryAttackMelee;
        inputManager.OnJumpPressed += playerController.TryJump;

        inputManager.OnShootStart += m_shootingSystem.HandleStartTryShoot;
        inputManager.OnShootStop += m_shootingSystem.HandleStopTryShoot;

        playerController.OnAttackMelee += animationManager.AttackMelee;
        playerController.OnJump        += HandleJump;
    }

    private void OnDisable()
    {
        inputManager.OnLockToggle  -= lockOnSystem.ToggleLock;
        inputManager.OnAttackMelee -= playerController.TryAttackMelee;
        inputManager.OnJumpPressed -= playerController.TryJump;

        inputManager.OnShootStart -= m_shootingSystem.HandleStartTryShoot;
        inputManager.OnShootStop -= m_shootingSystem.HandleStopTryShoot;

        playerController.OnAttackMelee -= animationManager.AttackMelee;
        playerController.OnJump        -= HandleJump;
    }

    private void Update()
    {
        lockOnSystem.CalculLockRotation();
        CalculateTargetDirection();

        playerController.SetLockMode(lockOnSystem.IsLocked);
        playerController.UpdatePlayerController(cameraTransform, inputManager.MoveInput);

        m_blendInput = GetBlendTreeInput(playerController.IsAttacking);
        animationManager.HandleAnimation(
            playerController.Rb.linearVelocity.magnitude,
            m_blendInput,
            playerController.IsGrounded);

        m_shootDirection = CalculateShootDirection(inputManager.ShootInput);
        playerCursor.HandleRotation(m_shootDirection);
        m_shootingSystem.SetShootDirection(m_shootDirection);

        Debug.DrawRay(shootOriginPoint.position, m_shootDirection * 5f, Color.red, 0.5f);
    }

    private void FixedUpdate()
    {
        playerController.UpdatePlayerControllerPhysics(m_targetDirection);
        lockOnSystem.HandleRotationLock(rb);
    }

    private void LateUpdate()
    {
        playerCursor.FollowPlayer();
    }

    #endregion

    #region Private Methods

    private void CalculateTargetDirection()
    {
        Vector3 camForward = cameraTransform.forward;
        camForward.y = 0; camForward.Normalize();

        Vector3 camRight = cameraTransform.right;
        camRight.y = 0; camRight.Normalize();

        m_targetDirection = camForward * inputManager.MoveInput.y +
                            camRight   * inputManager.MoveInput.x;

        if (m_targetDirection.magnitude > 0.1f)
            m_targetDirection.Normalize();
    }

    private Vector2 GetBlendTreeInput(bool isAttacking)
    {
        if (isAttacking) return m_blendInput;

        Vector3 camR = cameraTransform.right;
        camR.y = 0f; camR.Normalize();

        Vector3 camF = cameraTransform.forward;
        camF.y = 0f; camF.Normalize();

        if (lockOnSystem.IsLocked)
        {
            Vector3 enemyDir = lockOnSystem.CurrentTarget.GetLockTransform().position
                               - transform.position;
            enemyDir.y = 0f; enemyDir.Normalize();

            return new Vector2(
                Vector3.Dot(enemyDir, camR),
                Vector3.Dot(enemyDir, camF));
        }

        if (m_targetDirection.magnitude < 0.1f) return Vector2.zero;

        Vector3 d = m_targetDirection;
        d.y = 0f; d.Normalize();

        return new Vector2(
            Vector3.Dot(d, camR),
            Vector3.Dot(d, camF));
    }

    private Vector3 CalculateShootDirection(Vector2 shootInput)
    {
        if (shootInput.magnitude < inputDeadzone) return m_shootDirection;

        Vector3 camRight = cameraTransform.right;
        camRight.y = 0f; camRight.Normalize();

        Vector3 camForward = cameraTransform.forward;
        camForward.y = 0f; camForward.Normalize();

        Vector3 worldDirection = camRight * shootInput.x + camForward * shootInput.y;
        worldDirection.y = 0f;

        return worldDirection;
    }

    #endregion

    #region Jump

    private void HandleJump() => animationManager.Jump();

    #endregion
}