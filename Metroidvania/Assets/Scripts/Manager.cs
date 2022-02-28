using UnityEngine;
using System;

public class Manager : MonoBehaviour
{
	// Make this static, so that only one can exist at a time.
	private static Manager m_Instance;
	public static Manager Instance => m_Instance;	// Cast this to the type that it's supposed to be 



	// Start is called before the first frame update
	void Start()
    {
		// Will set this to be the instance the first time, and delete any later attempts to create more.
		if ( m_Instance && m_Instance != this )
		{
			Destroy( this.gameObject );
			return;
		}
		else
		{
			m_Instance = this;
		}

		DontDestroyOnLoad( gameObject );
	}

    // Update is called once per frame
    void Update()
    {
        
    }


	void PreventDuplicates( Manager pr_rDerived )
	{
		if ( m_Instance && m_Instance != this )
		{
			Debug.Log( "Error, more than 1 instance of: " + pr_rDerived.GetType().ToString() );

			Destroy( this.gameObject );

			return;
		}
		else
		{
			m_Instance = this;
		}
	}

}
