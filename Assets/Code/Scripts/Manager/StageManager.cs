using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{
	[Header("목표 피격 수")]
	public int targetKillCnt;
	[Header("클리어 시 나타낼 오브젝트 리스트")]
	public List<Transform> activeObjectList;
	[Header("클리어 시 사라질 오브젝트 리스트")]
	public List<Transform> disableObjectList;

	private bool isClear = false;

	private void Update()
	{
		// 목표 피격 수가 0 이하일 경우 게임 클리어 처리
		if(targetKillCnt <= 0) isClear = true;

		// 클리어 시 오브젝트 업데이트
		if(isClear)
		{
			// 나타내기
			foreach(Transform t in activeObjectList)
				t?.gameObject.SetActive(true);
			// 없애기
			foreach (Transform t in disableObjectList)
				t?.gameObject.SetActive(false);

			isClear = false;
		}
	}

	// 목표 피격 수 감소
	public void DecreaseTargetCnt()
	{
		--targetKillCnt;
	}
	public void DecreaseTargetCnt(int cnt)
	{
		targetKillCnt -= cnt;
	}
}
