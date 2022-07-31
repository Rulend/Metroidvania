using UnityEngine;

public class Singleton<T> : MonoBehaviour
	where T : Component
{

	public static T Instance { get; private set; }

	protected virtual void Awake()
	{
		if ( Instance == null )
		{
			Instance = this as T;
		}
		else
		{
			Debug.LogError( $"More than one singleton of type { this }" );
			Destroy( gameObject );
		}
	}
}

public class MonoBehaviourSingletonPersistent<T> : MonoBehaviour
	where T : Component
{
	public static T Instance { get; private set; }

	public virtual void Awake()
	{
		if ( Instance == null )
		{
			Instance = this as T;
			DontDestroyOnLoad( this );
		}
		else
		{
			Destroy( gameObject );
		}
	}
}
