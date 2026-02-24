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

    private Enemy currentEnemy;
    private bool isSpawning = false;
	private TestGrapplingHook grappling;
	private EnemySpawnLinker linker;

	private void Awake()
	{
		grappling = GameManager.Instance.grapplingHook;
		linker = GetComponent<EnemySpawnLinker>();
	}

	private void Start()
	{
		switch(spawnerType)
		{
		case SpawnerType.Normal:
			Spawn();
			break;
		}
	}

	private void Update()
	{
		switch (spawnerType)
		{
		case SpawnerType.Control:
			if (grappling.hookingList.Count <= 0) return;
			if (grappling.hookingList[0].GetComponent<EnemySpawnLinker>().ID == linker.ID)
				Spawn();
			break;
		}
	}

	public void Spawn()
    {
        if (isSpawning) return;

        GameObject obj = GameManager.Instance.poolManager
            .SpawnFromPool(enemyPoolName.ToString(), transform.position, Quaternion.identity);

        if (obj == null) return;

		obj.GetComponent<EnemySpawnLinker>().ID = linker.ID;
        currentEnemy = obj.GetComponent<Enemy>();
        currentEnemy.Init(this);

        isSpawning = true;
    }

    public void OnEnemyDead(Enemy enemy)
    {
        if (enemy != currentEnemy) return;

        currentEnemy = null;

		switch(spawnerType)
		{
		case SpawnerType.Normal:	// 일반: 리스폰 되게
			StartCoroutine(RespawnRoutine());
			break;
		}
		isSpawning = false;
    }

	private void OnDisable()
	{
		if(currentEnemy != null)
		{
			GameManager.Instance.poolManager.ReturnToPool(currentEnemy.gameObject);
			Debug.Log("Success Return");
		}
	}

	IEnumerator RespawnRoutine()
    {
        yield return new WaitForSeconds(respawnDelay);
        Spawn();
    }
}
