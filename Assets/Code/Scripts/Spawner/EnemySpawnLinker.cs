using UnityEngine;

public class EnemySpawnLinker : MonoBehaviour
{
	[Header("ID")]
	public string ID;

	private void Start()
	{
		if (ID == null)
			Debug.LogWarning("Doesn't exist ID");
	}
}
