using UnityEngine;

/// <summary>
/// MonoBehaviour 상속받는 싱글톤 클래스 
/// </summary>
public abstract class SingletonMonoBehaviour<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance;

    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                // 씬 내에서 찾기
                instance = FindObjectOfType<T>();

                // 없으면 새로 생성
                if (instance == null)
                {
                    GameObject obj = new GameObject(typeof(T).Name);
                    instance = obj.AddComponent<T>();
                    DontDestroyOnLoad(obj); // 씬 전환에도 유지
                }
            }

            return instance;
        }
    }

    protected virtual void Awake()
    {
        // 중복 인스턴스 제거
        if (instance == null)
        {
            instance = this as T;
            DontDestroyOnLoad(this.gameObject);
        }
        else if (instance != this)
        {
            Destroy(gameObject); // 중복 제거
        }
    }
}
