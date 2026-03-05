using UnityEngine;
using System.Collections.Generic;
using static Globals;
using tagName = Globals.TagName;    // 태그

/// <summary>
/// 몬스터 컨트롤러
/// 몬스터의 행동 담당
/// </summary>
public class EnemyController : MonoBehaviour
{
    Rigidbody2D rigid;
	[HideInInspector] Enemy tEnemy;

    void Awake()
	{
        rigid = GetComponent<Rigidbody2D>();
		tEnemy = GetComponent<Enemy>();
    }

    void Update()
	{
        if (tEnemy.isGrounded && rigid.linearVelocity == Vector2.zero)
            gameObject.tag = tagName.enemy;

		// 현재 태그에 따라 레이어마스크 대상 변경하기
        if (gameObject.CompareTag(tagName.throwingEnemy))
            gameObject.layer = LayerMask.NameToLayer(tagName.throwingEnemy);
        else if(gameObject.CompareTag(tagName.enemy))
            gameObject.layer = LayerMask.NameToLayer(tagName.enemy);
    }
    public void CheckGround(Collision2D collision)
    {
        foreach (var contact in collision.contacts)     // 바닥 체크
        {
            if (contact.normal.y > 0.7f &&
                contact.point.y < transform.position.y)
            {
				tEnemy.isGrounded = true;
                break;
            }
        }

        if (tEnemy.isGrounded && rigid.linearVelocityY < 0f)       // y값 보정 (바닥 뚫림 방지)
            rigid.linearVelocity = new Vector2(rigid.linearVelocity.x, 0f);
    }

    void OnCollisionEnter2D(Collision2D collision)
	{
        CheckGround(collision);     // 바닥 체크

        if (gameObject.CompareTag(tagName.enemy))
		{
			if (collision.gameObject.CompareTag(tagName.throwingEnemy))     // 적과 닿았을 경우
			{
				if (collision.gameObject.TryGetComponent<Enemy>(out var target))
				{
					Vector2 hitDir = (target.transform.position - transform.position).normalized;
					target.SetHitDirection(hitDir);

					target.TakeDamage(1);       // 닿은 적 죽이기
				}
			}
			// 오브젝트와 닿았을 경우
			// TODO: 코드 정리 - target 정보 확인 후 가능하면 지우기
			else if (collision.gameObject.CompareTag(tagName.throwingObj))
			{
				if (collision.gameObject.TryGetComponent<Enemy>(out var target))
				{
					Vector2 hitDir = (target.transform.position - transform.position).normalized;
					target.SetHitDirection(hitDir);

					target.TakeDamage(1);       // 닿은 적에게 데미지 주기
				}
			}
		}

		// 훅으로 잡혀서(죽어서) 던져진 상태일 경우 or 던져진 적 또는 던져진 오브젝트에게 맞았을 경우
		if ((tEnemy.isDie && gameObject.CompareTag(tagName.throwingEnemy)) || collision.gameObject.CompareTag(tagName.throwingEnemy) || collision.gameObject.CompareTag(tagName.throwingObj))
		{
			if (collision.gameObject.TryGetComponent<Enemy>(out var target))
			{
				Vector2 hitDir = (target.transform.position - transform.position).normalized;
				target.SetHitDirection(hitDir);
			}
			tEnemy.RemoveEnemy();   // 적 사라짐 처리
		}
	}

    void OnCollisionStay2D(Collision2D collision)
    {
        CheckGround(collision);     // 바닥 체크
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(tagName.ground))
			tEnemy.isGrounded = false;
    }
}