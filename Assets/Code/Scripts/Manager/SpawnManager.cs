using System.Collections.Generic;
using UnityEngine;
using static EnemySpawner;

public class SpawnManager : MonoBehaviour
{
	//[Header("적 스포너")]
	//public List<EnemySpawner> spawnerList;

	//private void Update()
	//{
	//	switch (spawnerType)
	//	{
	//	case SpawnerType.Control:
	//		if (grappling.hookingList.Count <= 0) return;
	//		// 플레이어가 잡은 적의 스폰ID와 자기자신과 연결된 Linker의 스폰ID가 같을 경우 스폰
	//		if (grappling.hookingList[0].GetComponent<EnemySpawnLinker>()?.ID == linker.linkedObj.GetComponent<EnemySpawnLinker>()?.ID)
	//			Spawn();
	//		break;
	//	}
	//}

	//public void SpawnEnemy(int ID)
	//{
	//	foreach(EnemySpawner t in spawnerList)
	//	{
	//		EnemySpawnLinker tLinker = t.GetComponent<EnemySpawnLinker>();
	//		if(tLinker?.ID == ID)
	//		{
	//			tLinker.linkedObj.GetComponent<EnemySpawnLinker>().ID;
	//		}
	//	}
	//}
}
