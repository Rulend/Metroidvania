using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	// Notation explanation:
	//	g_		: global
	//	m_		: a member variable of own or parent class
	//	_r		: a reference, meaning it exists somwhere else
	//	

	// One way of making a singleton, left here for future reference: 
	//public sealed class GameManager : MonoBehaviour
	//private static readonly GameManager m_Instance = new GameManager();
	//static GameManager() { }
	//private GameManager() { }
	//public static GameManager Instance => m_Instance;

	/// Start of member variables

	private static	GameManager m_Instance;
	public	static	GameManager Instance => m_Instance;

	private TeleporterManager m_rTeleporterManager;


	[SerializeField] private Player g_rPlayer1;
	public Player rPlayer1 => g_rPlayer1; // Readonly propery


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


		// TODO: Instead of doing this, search through an array or vector after the players, and assign them from there. This will make it easier for multiplayer.
		if ( !g_rPlayer1 )
		{
			g_rPlayer1 = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
		}



		m_rTeleporterManager = (TeleporterManager)TeleporterManager.Instance;


	}

}
