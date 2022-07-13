using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Manager : MonoBehaviour
{
	// Singleton setup
	private static UI_Manager m_Instance;
	public	static UI_Manager Instance => m_Instance;



	[SerializeField] private InventoryUI m_rInventoryUI;
	[SerializeField] private Menu m_rMenu;

	
	public InventoryUI rInventoryUI => m_rInventoryUI;
	public Menu rMenu => m_rMenu;


	private void Awake()
	{
		if ( m_Instance == null )
		{
			m_Instance = this;
		}
		else
		{
			Debug.LogError( "More than once instance of UI_Manager detected. Deleting the extra..." );
			Destroy( gameObject );
		}
	}


	// Start is called before the first frame update
	void Start()
    {
        
    }

	private void OnDestroy()
	{
		if ( m_Instance == this )
			m_Instance = null;
	}
}
