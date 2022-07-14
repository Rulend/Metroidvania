using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Manager : MonoBehaviour
{
	// Singleton setup
	private static UI_Manager m_Instance;
	public	static UI_Manager Instance => m_Instance;



	public enum EButtonPromptCombo
	{
		None				,
		MenuRegular			,	// When you first open the menu
		EquipmentScreen		,	// When you press equipment
		EquipmentBrowse		,	// When you select an equipment slot in order to equip something
	}


	[SerializeField] private InventoryUI m_rInventoryUI;
	[SerializeField] private Menu m_rMenu;


	[Space]
	[SerializeField] private ButtonPrompt m_PromptConfirm;
	[SerializeField] private ButtonPrompt m_PromptAlternative;
	[SerializeField] private ButtonPrompt m_PromptInteract;
	[SerializeField] private ButtonPrompt m_PromptGoBack;


	private Vector3 m_PromptInteractHighPos;
	private Vector3 m_PromptInteractLowPos;



	public InventoryUI rInventoryUI => m_rInventoryUI;
	public Menu rMenu => m_rMenu;
	public ButtonPrompt PromptConfirm => m_PromptConfirm;
	public ButtonPrompt PromptAlternative => m_PromptAlternative;
	public ButtonPrompt PromptInteract => m_PromptInteract;
	public ButtonPrompt PromptGoBack => m_PromptGoBack;


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


	public void UpdateDisplayedButtonPrompts( EButtonPromptCombo _ButtonPromptsCombo )
	{
		switch ( _ButtonPromptsCombo )
		{
			case EButtonPromptCombo.None:
				{
					PromptConfirm.Deactivate();
					PromptAlternative.Deactivate();
					PromptInteract.Deactivate();
					PromptGoBack.Deactivate();
				}
				break;

			case EButtonPromptCombo.MenuRegular:
				{
					PromptConfirm.Activate( "Select" );
					PromptAlternative.Deactivate();
					PromptInteract.Deactivate();
					PromptGoBack.Activate( "Cancel" );
				}
				break;

			case EButtonPromptCombo.EquipmentScreen:
				{
					PromptConfirm.Activate( "Select" );
					PromptAlternative.Activate( "Unequip" );
					PromptInteract.Deactivate();
					PromptGoBack.Activate( "Cancel" );
				}
				break;

			case EButtonPromptCombo.EquipmentBrowse:
				{
					PromptConfirm.Activate( "Equip" );
					PromptAlternative.Activate( "Discard" );
					PromptInteract.Deactivate();
					PromptGoBack.Activate( "Cancel" );
				}
				break;
		}
	}


	public void LowerInteractablePrompt()
	{
		m_PromptInteract.transform.position = m_PromptInteractLowPos;
	}

	public void RaiseInteractablePrompt()
	{
		m_PromptInteract.transform.position = m_PromptInteractHighPos;
	}
}
