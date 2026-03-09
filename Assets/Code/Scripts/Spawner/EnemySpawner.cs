using System;
using System.Collections;
using UnityEngine;
using EnumType;
using Unity.VisualScripting;

public class EnemySpawner : MonoBehaviour
{
	public enum SpawnerType { Normal, Control };

	[Header("스폰 정보")]
	public EnemyName enemyPoolName;
	public SpawnerType spawnerType;
	public float respawnDelay = 3f;
	public bool isStartSpawn = true;    // 시작 시 스폰시키는지 여부

	private Enemy currentEnemy;
	private bool isSpawning = false;
	private SpawnLinker linker;

	private void Awake()
	{
		linker = GetComponent<SpawnLinker>();
	}

	private void Start()
	{
		// 시작 시 스폰 여부 판단
		if (isStartSpawn)
			Spawn();
	}

	// 스폰 함수
	public void HandleSpawn()
	{
		switch(spawnerType)
		{
		case SpawnerType.Normal:     // 일반: 일정 시간 이후에 리스폰 되게
			StartCoroutine(RespawnRoutine());
			break;

		case SpawnerType.Control:   // 컨트롤: 현재 스포너와 연결된 스포너에서 적 생성
			GetComponent<SpawnLinker>().linkedObj?.GetComponent<EnemySpawner>().Spawn();
			break;
		}
	}

	public void Spawn()
	{
		if (isSpawning) return;

		GameObject obj = GameManager.Instance.poolManager
			.SpawnFromPool(enemyPoolName.ToString(), transform.position, Quaternion.identity);

		if (obj == null) return;

		SpawnLinker objLinker = obj.GetComponent<SpawnLinker>();
		objLinker.ID = linker.ID;
		objLinker.linkedObj = linker.linkedObj;
		currentEnemy = obj.GetComponent<Enemy>();
		currentEnemy.Init(this);

		isSpawning = true;
	}

	public void OnEnemyDead(Enemy enemy)
	{
		if (enemy != currentEnemy) return;

		currentEnemy = null;

		HandleSpawn();

		isSpawning = false;
	}

	private void OnDisable()
	{
		if (currentEnemy != null)
		{
			GameManager.Instance.poolManager?.ReturnToPool(currentEnemy.gameObject);
		}
	}

	IEnumerator RespawnRoutine()
	{
		yield return new WaitForSeconds(respawnDelay);
		Spawn();
	}
}
