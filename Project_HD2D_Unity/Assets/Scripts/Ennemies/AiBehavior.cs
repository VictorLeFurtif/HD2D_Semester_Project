using System.Collections;
using Interface;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class AiBehavior : MonoBehaviour, IDamageable, ICarryable
{
    #region States Instances

        [Header("States Data")] 
        public AiStatic StaticState { get; private set; } = new AiStatic();
        public AiPatrol patrolState { get; private set; } = new AiPatrol();
        public AiChase chaseState = new AiChase();
        public AiAttack attackState = new AiAttack();
        public AiSearch searchState = new AiSearch();
        public AiGoToSpawn goToSpawnState = new AiGoToSpawn();
        public AiKO aiKoState = new AiKO();
        public AiTakeDamage aiTakeDamage = new AiTakeDamage();

        private AiState currentState;
        [HideInInspector] public AiState previousState;
        
        
        [SerializeField] private Rigidbody rb;
        [SerializeField] private NavMeshAgent agent;
        [SerializeField] private Collider mainCollider;

        private bool isFlying = false;
    #endregion

    #region Settings & Triggers
    
        [Header("Triggers")]
        public Trigger viewRangeTrigger;
        public Trigger attackRangeTrigger;

        [Header("Detection")]
        public LayerMask obstacleMask;
        public float eyeHeightOffset = 1.5f;
        
        [Header("Patrol")]
        public Transform[] patrolPoints;
        #endregion

    #region References
        //KO thing , to move later for SRP (if needed)
        public Slider KoSlider;
        public int KoSliderMax = 100;
        public bool isHold = false;
        
        //Name of the state
        public TMP_Text StateTxt;
        
        
        [HideInInspector] public EnnemieMovement movement;
        [HideInInspector] public GameObject target;
        [HideInInspector] public Vector3 spawnPosition;
        [HideInInspector] public Vector3 lastKnownPosition;
        [HideInInspector] public bool isPlayerInAttackRange;
        [HideInInspector] public bool isPlayerInViewRange;
        
    #endregion

    #region Unity LifeCycle
        void Awake()
        {
            movement = GetComponent<EnnemieMovement>();
            spawnPosition = transform.position;
            lastKnownPosition = spawnPosition;
            
            SubscribeEvents();
        }

        void Start()
        {
            //Ko thingy
            KoSlider.maxValue = KoSliderMax;
            
            //Name of state
            StateTxt.text = "DEBUG";
            
            //Default state 
            ChangeState(patrolState);
        }

        void OnEnable()
        {
            if (movement != null) movement.StopMovement();
            transform.position = spawnPosition;
            ChangeState(patrolState);
        }

        void Update() 
        {
            currentState?.UpdateState(this);
            
            Debug.DrawRay(lastKnownPosition, Vector3.up * 2f, Color.yellow);
        }

        void OnDestroy() => UnsubscribeEvents();
        
    #endregion

    #region State Logic
        public void ChangeState(AiState newState)
        {
            currentState?.ExitState(this);
            previousState = currentState;
            
            currentState = newState;
            StateTxt.text = newState.Name;
            currentState.EnterState(this);
        }
        
    #endregion

    #region Detection Logic
        public bool CanSeePlayer()
        {
            if (target == null) return false;

            Vector3 eyePos = transform.position + Vector3.up * eyeHeightOffset;
            Vector3 dir = (target.transform.position - eyePos).normalized;
            float dist = Vector3.Distance(eyePos, target.transform.position);

            if (Physics.Raycast(eyePos, dir, out RaycastHit hit, dist, obstacleMask, QueryTriggerInteraction.Ignore))
            {
                if (hit.transform.CompareTag("Player")) 
                {
                    lastKnownPosition = target.transform.position;
                    Debug.DrawLine(eyePos, hit.point, Color.green);
                    return true;
                }
            }
            
            Debug.DrawLine(eyePos, eyePos + dir * dist, Color.red);
            return false;
        }
        
    #endregion

    #region Event Management
        private void SubscribeEvents()
        {
            if (viewRangeTrigger != null)
            {
                viewRangeTrigger.EnteredTrigger += OnViewEntered;
                viewRangeTrigger.ExitedTrigger += OnViewExited;
            }
            if (attackRangeTrigger != null)
            {
                attackRangeTrigger.EnteredTrigger += OnAttackEntered;
                attackRangeTrigger.ExitedTrigger += OnAttackExited;
            }
        }

        private void UnsubscribeEvents()
        {
            if (viewRangeTrigger != null)
            {
                viewRangeTrigger.EnteredTrigger -= OnViewEntered;
                viewRangeTrigger.ExitedTrigger -= OnViewExited;
            }
            if (attackRangeTrigger != null)
            {
                attackRangeTrigger.EnteredTrigger -= OnAttackEntered;
                attackRangeTrigger.ExitedTrigger -= OnAttackExited;
            }
        }

        private void OnViewEntered(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                target = other.gameObject;
                isPlayerInViewRange = true;
                print("Player is in view range.");
            }
        }

        private void OnViewExited(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                target = null;
                isPlayerInViewRange = false;
            }
        }

        private void OnAttackEntered(Collider other)
        {
            if (other.CompareTag("Player")) isPlayerInAttackRange = true;
        }

        private void OnAttackExited(Collider other)
        {
            if (other.CompareTag("Player")) isPlayerInAttackRange = false;
        }
        
    #endregion

    #region Take Damage

        public void TakeDamage(int value)
        {
            aiTakeDamage.DamageToApply = value;
            
            ChangeState(aiTakeDamage);
        }

    #endregion

    public void Carry(Transform anchor)
    {
        if (agent != null) agent.enabled = false;

        rb.isKinematic = true;
        rb.useGravity = false;
        
        transform.SetParent(anchor);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;

        mainCollider.enabled = false;
    }

    public bool IsCarryable()
    {
        return currentState == aiKoState;
    }
    

    public void Eject()
    {
        
        transform.SetParent(null);

        mainCollider.enabled = true;
        rb.isKinematic = false;
        rb.useGravity = true;
        
        rb.AddForce(-transform.forward * 5f, ForceMode.Impulse);

        isFlying = true;
        StartCoroutine(LandingRoutine());
    }
    
    private IEnumerator LandingRoutine()
    {
        yield return new WaitForSeconds(0.2f);

        bool grounded = false;
        while (!grounded)
        {
            if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, 1.2f))
            {
                if (NavMesh.SamplePosition(hit.point, out NavMeshHit navHit, 0.5f, NavMesh.AllAreas))
                {
                    grounded = true;
                }
            }
            yield return new WaitForFixedUpdate();
        }

        ReactivateAI();
    }
    
    private void ReactivateAI()
    {
        isFlying = false;
        rb.isKinematic = true; 
        
        if (agent != null)
        {
            agent.Warp(transform.position);
            agent.enabled = true;
        }
    }
}