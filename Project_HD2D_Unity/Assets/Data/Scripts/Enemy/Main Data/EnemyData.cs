using UnityEngine;

[CreateAssetMenu(fileName = "EnemyData", menuName = "Enemy/EnemyData")]
public class EnemyData : ScriptableObject
{
    [field: SerializeField] public AiNavigationData AiNavigationData { get; private set; } // AJOUT
    [field: SerializeField] public AiAttackData AiAttackData { get; private set; }
    [field: SerializeField] public AiKOData AiKOData { get; private set; }
    [field: SerializeField] public AiSearchData AiSearchData { get; private set; }
    [field: SerializeField] public AiTakeDamageData AiTakeDamageData { get; private set; }
    [field: SerializeField] public AiDataFeedBack AiDataFeedBack { get; private set; }
    [field: SerializeField] public AiExposedData AiExposedData { get; private set; }
    
    public EnemyDataInstance Init() => new EnemyDataInstance(this);
}

public class EnemyDataInstance
{
    public float PatrolSpeed;
    public float ChaseSpeed;
    public float Acceleration;
    public float StoppingDistance;
    public float DetectionRange;
    public float ViewAngle;

    public float AttackCooldown;
    public float AnticipationTime;      
    public float HitboxActiveDuration; 
    public float AttackDashSpeed;     
    public float AttackDashDuration;
    
    public float KoTime;
    public int MaxKo;
    public int CurrentKo;

    public float SearchDuration;
    public float SearchRadius;

    public int DamageToApply;
    public float StunDuration;

    public Sprite SpriteSearch;
    public Sprite SpriteAttackStart;
    public Sprite SpriteAttackEnd;
    public Sprite SpriteChase;
    public Sprite SpritePatrol;
    public Sprite SpriteKo;
    public Sprite SpriteFall;
    public Sprite SpriteTakeDamage;
    public Sprite SpriteExposed;
    
    public float ExposedTime;

    public EnemyDataInstance(EnemyData data)
    {
        PatrolSpeed = data.AiNavigationData.PatrolSpeed;
        ChaseSpeed = data.AiNavigationData.ChaseSpeed;
        StoppingDistance = data.AiNavigationData.StoppingDistance;
        DetectionRange = data.AiNavigationData.DetectionRange;
        ViewAngle = data.AiNavigationData.ViewAngle;
        Acceleration = data.AiNavigationData.Acceleration;

        AttackCooldown = data.AiAttackData.AttackCooldown;
        AnticipationTime = data.AiAttackData.AnticipationTime;
        HitboxActiveDuration = data.AiAttackData.HitboxActiveDuration;
        AttackDashSpeed = data.AiAttackData.AttackDashSpeed;
        AttackDashDuration = data.AiAttackData.AttackDashDuration;
        
        KoTime = data.AiKOData.KoTime;
        MaxKo = data.AiKOData.MaxKo;
        CurrentKo = 0;

        SearchDuration = data.AiSearchData.searchDuration;
        SearchRadius = data.AiSearchData.searchRadius;

        DamageToApply = data.AiTakeDamageData.DamageToApply;
        StunDuration = data.AiTakeDamageData.StunDuration;
        
        SpriteSearch = data.AiDataFeedBack.SpriteSearch;
        SpriteAttackStart = data.AiDataFeedBack.SpriteAttackStart;
        SpriteChase = data.AiDataFeedBack.SpriteChase;
        SpritePatrol =  data.AiDataFeedBack.SpritePatrol;
        SpriteKo =  data.AiDataFeedBack.SpriteKo;
        SpriteFall = data.AiDataFeedBack.SpriteFall;
        SpriteTakeDamage = data.AiDataFeedBack.SpriteTakeDamage;
        SpriteExposed = data.AiDataFeedBack.SpriteExposed;
        
        ExposedTime = data.AiExposedData.ExposedTime;
    }
    
    public bool IsKoFull() => CurrentKo >= MaxKo;
    
    public void ResetKo() => CurrentKo = 0;

    public Sprite GetSprite(AiState currentState)
    {
        switch (currentState.Name)
        {
            case "Chase":
                return SpriteChase;
            case "Searching":
                return SpriteSearch;
            case "Attacking":
                return SpriteAttackStart; 
            case "Patrol":
                return SpritePatrol;
            case "K-O":
                return SpriteKo;
            case "Falling":
                return SpriteFall;
            case "Taking Damage":
                return SpriteTakeDamage;
            case "Exposed":
                return SpriteExposed;
            default:
                return null;
        }
    }
    
}

