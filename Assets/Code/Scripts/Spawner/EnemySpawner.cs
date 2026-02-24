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
	public bool isStartSpawn = true;	// 시작 시 스폰시키는지 여부

    private Enemy currentEnemy;
    private bool isSpawning = false;
	private EnemySpawnLinker linker;

	private void Awake()
	{
		linker = GetComponent<EnemySpawnLinker>();
	}

	private void Start()
	{
		if (isStartSpawn)
			Spawn();
	}

	private void Update()
	{
		//switch (spawnerType)
		//{
		//case SpawnerType.Control:
		//	// 플레이어가 잡은 적의 스폰ID와 자기자신과 연결된 Linker의 스폰ID가 같을 경우 스폰
		//	//if (currentEnemy?.GetComponent<EnemySpawnLinker>().ID == linker.linkedObj.GetComponent<EnemySpawnLinker>().ID)
		//	//	Spawn();
		//	break;
		//}
	}

	public void Spawn()
    {
        if (isSpawning) return;

        GameObject obj = GameManager.Instance.poolManager
            .SpawnFromPool(enemyPoolName.ToString(), transform.position, Quaternion.identity);

        if (obj == null) return;

		EnemySpawnLinker objLinker = obj.GetComponent<EnemySpawnLinker>();
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
		}
	}

	IEnumerator RespawnRoutine()
    {
        yield return new WaitForSeconds(respawnDelay);
        Spawn();
    }
}
