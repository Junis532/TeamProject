using UnityEngine;
using UnityEngine.SceneManagement;
using static Globals;

public class Enemy : MonoBehaviour, IDamageable
{
    [Header("피 이펙트")]
    public GameObject bloodEffectPrefab;

    EnemyStatsRuntime currStat;			// 현재 스탯
    EnemySpawner ownerSpawner;
    public EnumType.EnemyState state;   // 현재 상태
    Vector2 lastHitDir = Vector2.right; // 마지막으로 맞은 방향
	[HideInInspector] public bool isDie = false;  // 죽음 여부
	[HideInInspector] public bool isGrounded;     // 땅 체크

	public void Init(EnemySpawner spawner)
    {
        ownerSpawner = spawner;
		isDie = false;
	}

    void OnEnable()
    {
        state = EnumType.EnemyState.Idle;       // 풀에서 다시 나올 때마다 초기화
        currStat = new EnemyStatsRuntime(GameManager.Instance.enemyStats);
    }

	void Start()
    {
        state = EnumType.EnemyState.Idle;   // 상태 초기화
        currStat = new EnemyStatsRuntime(GameManager.Instance.enemyStats);  // 각 스탯 초기화
    }

	public void TakeDamage(int attack)      // 데미지 입히기
    {
        GameManager.Instance.cameraShake.ShakeForSeconds(0.1f); // 카메라 흔들기

        currStat.currentHP -= attack;

		if (currStat.currentHP <= 0)
			DieEnemy();		// 죽음 처리
	}

    public void SetHitDirection(Vector2 hitDir)     // 방향만 저장하는 함수
    {
        lastHitDir = hitDir.normalized;
    }

    void SpawnBloodEffect(Vector2 hitDir)
    {
        if (bloodEffectPrefab == null) return;

        GameObject blood = Instantiate(
            bloodEffectPrefab,
            transform.position,
            Quaternion.identity
        );

        if (blood.TryGetComponent<SpriteRenderer>(out var sr))
        {
            // 오른쪽에서 맞았으면 flip
            sr.flipX = hitDir.x < 0f;
        }

        Destroy(blood, 1f);
    }

    public void DieEnemy()      // 적 죽음처리
	{
		if(!isDie)
		{
			isDie = true;       // 죽은 상태로 변경
			ownerSpawner?.OnEnemyDead(this);    // 스포너가 있으면 스포너 죽음 처리
		}
	}

	public void RemoveEnemy()	// 적 사라짐 처리
	{
		if (!isDie) DieEnemy();	// 적이 죽지 않았다면 죽음 처리
		SpawnBloodEffect(lastHitDir);       // 피 이펙트
		GameManager.Instance.poolManager.ReturnToPool(gameObject);  // 풀로 되돌림
		GameManager.Instance.stageManager?.DecreaseTargetCnt();     // 스테이지 점수 처리
	}
}