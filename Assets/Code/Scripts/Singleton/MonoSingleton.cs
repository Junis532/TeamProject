using UnityEngine;

public class MonoSingleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static object lockObject = new object();
    private static T instance = null;
    private static bool IsQuitting = false;

    public static T Instance
    {

        // 쓰레드 안정화
        get
        {
            // 한 번에 한 스레드만 lock 블럭 실행
            lock (lockObject)
            {
                //비활성화 됐다면 기존꺼 내비두고 새로 만듦
                if (IsQuitting)
                {
                    return null;
                }
            }
            // instance가 NULL일 때 새로 생성
            if (instance == null)
            {
                instance = GameObject.Instantiate(Resources.Load<T>("MonoSingleton/" + typeof(T).Name));
                DontDestroyOnLoad(instance.gameObject);
            }
            return instance;
        }
    }

    private void OnDisable()
    {
        // 비활성화 된다면 null로 변경
        IsQuitting = true;
        instance = null;
    }
}
