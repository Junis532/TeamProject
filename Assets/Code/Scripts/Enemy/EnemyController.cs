using UnityEngine;

public class EnemyController : MonoBehaviour, IDamageable
{
    EnemyStatsRuntime currStat;     // 현재 스탯
    IDamageable damageable;

    private void Awake()
    {
        damageable = GetComponent<IDamageable>();
    }

    private void Start()
    {
        currStat = GameManager.Instance.enemyStatsRuntime;
    }

    void Update()
    { 
        // 몬스터 체력이 없을 경우
        if(currStat.currentHP <= 0)
        {
            // TODO: 몬스터 숨기기 (임시 코드)
            transform.gameObject.SetActive(false);
        }
    }

    // 던져지는 몬스터 또는 오브젝트와 닿았을 경우
    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("ThrowingEnemy") 
            && !other.CompareTag("Object")) 
            return;

        // 몬스터 데미지 입히기
        if (other.CompareTag("ThrowingEnemy") || other.CompareTag("Object"))
        {
            damageable.TakeDamage(1);
        }
    }

    // 적 데미지
    void IDamageable.TakeDamage(int attack)
    {
        currStat.currentHP -= attack;

        Debug.Log("[적 데미지] 적 현재 체력: " + currStat.currentHP);
    }
}
