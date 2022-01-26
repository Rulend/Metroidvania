using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	// One way of making a singleton, left here for future reference: 
	//public sealed class GameManager : MonoBehaviour
	//private static readonly GameManager m_Instance = new GameManager();
	//static GameManager() { }
	//private GameManager() { }
	//public static GameManager Instance => m_Instance;



	/// Start of member variables

	private static GameManager m_Instance;
	public static GameManager Instance => m_Instance;


	[SerializeField] private Player g_Player1;
	public Player Player1 => g_Player1; // Property: this one specifically is like a Getter in c++.


	/// End of member variables

	void Awake()
	{
		// Will set this to be the instance the first time, and delete any later attempts to create more.
		if ( m_Instance && m_Instance != this )
		{
			Destroy( this.gameObject );
		}
		else
		{
			m_Instance = this;
		}



		// Find player if not already set inside the inspector
		if ( !g_Player1 )
		{
			g_Player1 = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
		}
	}

}
