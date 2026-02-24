using UnityEngine;
/// <summary>
/// 전역 유틸리티
/// 자주 사용되는 기능들을 클래스로 모아놓은 파일
/// </summary>
public class GlobalUtil
{
	public void CheckGround(Transform transform, Collision2D collision, Rigidbody2D rigid)
	{
		bool isGrounded = false;

		foreach (var contact in collision.contacts)     // 바닥 체크
		{
			if (contact.normal.y > 0.7f &&
				contact.point.y < transform.position.y)
			{
				isGrounded = true;
				break;
			}
		}

		if (isGrounded && rigid.linearVelocityY < 0f)       // y값 보정 (바닥 뚫림 방지)
			rigid.linearVelocity = new Vector2(rigid.linearVelocity.x, 0f);
	}
	public static void EnemySpawnUpdate(Enemy e)
	{
		// 죽은 적 스포너의 반대편 스포너에서 적 생성되게 설정
		e?.GetComponent<EnemySpawnLinker>().linkedObj.GetComponent<EnemySpawner>()?.Spawn();
	}
}
