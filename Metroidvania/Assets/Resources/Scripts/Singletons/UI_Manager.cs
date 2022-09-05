using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Manager : MonoBehaviour
{
	// Singleton setup
	private static UI_Manager m_Instance;
	public	static UI_Manager Instance => m_Instance;


	// 
	public enum EButtonPromptCombo
	{
		None				,
		MenuRegular			,	// When you first open the menu
		EquipmentScreen		,	// When you press equipment
		EquipmentBrowse		,	// When you select an equipment slot in order to equip something
	}


	[SerializeField] private InventoryUI	m_rInventoryUI;
	[SerializeField] private Menu			m_rMenu;
	[SerializeField] private HealthBar		m_HealthBar;


	[Space]
	[SerializeField] private ButtonPrompt m_PromptInteract;


	private Vector3 m_PromptInteractHighPos;
	private Vector3 m_PromptInteractLowPos;



	public InventoryUI rInventoryUI => m_rInventoryUI;
	public Menu rMenu => m_rMenu;
	public ButtonPrompt PromptInteract => m_PromptInteract;


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


		m_PromptInteractHighPos	= UI_Manager.Instance.PromptInteract.transform.position;
		m_PromptInteractLowPos	= m_PromptInteractHighPos - new Vector3( 0.0f, 200.0f, 0.0f );
	}


	private void Start()
	{
		
	}



	private void OnDestroy()
	{
		if ( m_Instance == this )
			m_Instance = null;
	}


	private void Update()
	{
		
	}


	public void LowerInteractablePrompt()
	{
		m_PromptInteract.transform.position = m_PromptInteractLowPos;
	}

	public void RaiseInteractablePrompt()
	{
		m_PromptInteract.transform.position = m_PromptInteractHighPos;
	}


	public void UpdateHealth( float _CurrentHeatlh, float _MaxHealth )
	{
		m_HealthBar.AdjustHealth( _CurrentHeatlh, _MaxHealth );
	}
}
