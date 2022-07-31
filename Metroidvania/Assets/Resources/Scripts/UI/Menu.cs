using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Menu : MonoBehaviour
{
	public enum EMenuState
	{
		Closed			,	// When menu is closed
		Opened			,	// When menu is open and no other window has been gone to yet
		EquipmentScreen	,	// When the currently equipped equipment tab has ben opened
		EquipmentBrowse	,	// When an equipment slot has been selected, and the player is selecting an equipment to put in that slot
		InventoryScreen	,	// When the inventory tab has been opened
	}



	// Event used to handle going back in menus via pressing the back button 
	public delegate void GoBackButtonHandler();
	public GoBackButtonHandler GoBackButtonEvent; // Button East on gamepad

	//public delegate void AlternativeButtonHandler();
	//public AlternativeButtonHandler AlternativeButton1Event;	// Button West on gamepad
	//public AlternativeButtonHandler AlternativeButton2Event;	// Button North on gamepad

	[SerializeField] private Button		m_SelectedButton;
	[SerializeField] private GameObject	m_SelectedButtonBorder;

	[Space]
	[SerializeField] private ButtonPrompt m_PromptConfirm;			// Gamepad South / Keyboard Enter
	[SerializeField] private ButtonPrompt m_PromptAlternative1;		// Gamepad West  / Idk yet lol
	[SerializeField] private ButtonPrompt m_PromptAlternative2;		// Gamepad North / Idk yet lol
	[SerializeField] private ButtonPrompt m_PromptGoBack;			// Gamepad East  / Keyboard backspace


	private GameObject m_OpenedScreen;  // The gameobject of the currently selected screen. Screen meaning the button pressed in the menu.

	private EMenuState m_CurrentState;

	public EMenuState CurrentState => m_CurrentState;


	private void Start()
	{
		gameObject.SetActive( false );
	}


	public void OpenMenu()
	{
		gameObject.SetActive( true );
		m_SelectedButtonBorder.SetActive( true );

		GetComponentInChildren<Button>().Select();

		SetMenuState( EMenuState.Opened );
	}

	public void CloseMenu()
	{
		gameObject.SetActive( false );
		m_SelectedButtonBorder.SetActive( false );

		if ( m_OpenedScreen )
		{
			// TODO:: Make an enum switch here on which screen was opened. Otherwise this will crash later.

			m_OpenedScreen.SetActive( false );
			m_OpenedScreen = null;
		}

		SetMenuState( EMenuState.Closed );

		GameManager.Instance.rPlayer1.GetComponent<PlayerController>().SetState( PlayerController.EPlayerControllerState.PCSTATE_Normal );
	}


	public void SelectButton( Button _ButtonToSelect )
	{
		m_SelectedButton = _ButtonToSelect;

		RectTransform ButtonTransform = _ButtonToSelect.GetComponent<RectTransform>();
		RectTransform BorderTransform =  m_SelectedButtonBorder.GetComponent<RectTransform>();

		BorderTransform.sizeDelta	= ButtonTransform.sizeDelta;
		BorderTransform.position	= ButtonTransform.position;
	}



	// NOTE!!
	// These functions are called by UI buttons, which is why they have 0 refereces.

	public void ButtonOpenEquipmentScreen()
	{
		gameObject.SetActive( false );

		InventoryUI rInventoryUI	= UI_Manager.Instance.rInventoryUI;
		m_OpenedScreen				= rInventoryUI.gameObject;

		m_OpenedScreen.SetActive( true );
		EquipmentManager.Instance.UnselectEquipmentSlot(); // We need to call this here in order for it to always start on the first slot.
		rInventoryUI.ShowEquippedEquipment();


		//UpdateButtonPrompts( UI_Manager.EButtonPromptCombo.EquipmentScreen ); // This is called inside ShowEquippedEquipment instead
	}

	public void ButtonOpenInventoryScreen()
	{
		gameObject.SetActive( false );

		SetMenuState( EMenuState.InventoryScreen );

		InventoryUI rInventoryUI	= UI_Manager.Instance.rInventoryUI;
		m_OpenedScreen				= rInventoryUI.gameObject;

		m_OpenedScreen.SetActive( true );
		rInventoryUI.ShowInventoryCategory( 0 ); // Show the category of items at index 0 of enumerator

		//UpdateButtonPrompts( UI_Manager.EButtonPromptCombo.InventoryScreen ); // This is called inside ShowEquippedEquipment instead
	}

	public void ButtonOpenOptionsScreen()
	{

	}


	public void CloseOpenedScreen()
	{
		m_OpenedScreen.SetActive( false );
		gameObject.SetActive( true );

		m_OpenedScreen = null;

		GetComponentInChildren<Button>().Select();

		SetMenuState( EMenuState.Opened );
	}


	public void GoBackToPreviousWindow()
	{
		// TODO:: If the events end up being unnecessary, do the same switch case as in AlternativeButton1 and 2
		GoBackButtonEvent.Invoke();
	}


	public void AlternativeButton1()
	{
		switch ( m_CurrentState )
		{
			case EMenuState.Closed:break;

			case EMenuState.Opened:break;

			case EMenuState.EquipmentScreen: EquipmentManager.Instance.Unequip( (Equipment)m_SelectedButton.GetComponent<ItemSlot>().Item ); break;

			case EMenuState.EquipmentBrowse: break;
		}
	}


	public void AlternativeButton2()
	{
		switch ( m_CurrentState )
		{
			case EMenuState.Closed:break;

			case EMenuState.Opened:break;

			case EMenuState.EquipmentScreen:break;

			case EMenuState.EquipmentBrowse:break;
		}
	}


	public void LeftShoulderButton()
	{
		switch ( m_CurrentState )
		{
			case EMenuState.Closed: break;

			case EMenuState.Opened: break;

			case EMenuState.EquipmentScreen: break;

			case EMenuState.EquipmentBrowse: break;

			case EMenuState.InventoryScreen: UI_Manager.Instance.rInventoryUI.ShowPreviousInventoryCategory(); break;
		}
	}


	public void RightShoulderButton()
	{
		switch ( m_CurrentState )
		{
			case EMenuState.Closed: break;

			case EMenuState.Opened: break;

			case EMenuState.EquipmentScreen: break;

			case EMenuState.EquipmentBrowse: break;

			case EMenuState.InventoryScreen: UI_Manager.Instance.rInventoryUI.ShowNextInventoryCategory(); break;
		}
	}




	public void SetMenuState( EMenuState _NewState )
	{
		m_CurrentState = _NewState;

		UpdateButtonPrompts( m_CurrentState ); 

		switch ( m_CurrentState )
		{
			case EMenuState.Closed:
				{
					if ( GoBackButtonEvent != null ) // If the amount of subscribers is more than 0
					{
						foreach ( var SubscribedFunction in GoBackButtonEvent.GetInvocationList() ) // Unsubscribe all subscribers
							GoBackButtonEvent -= ( SubscribedFunction as GoBackButtonHandler );
					}
				}
				break;

			case EMenuState.Opened:
				{
					GoBackButtonEvent -= CloseOpenedScreen;
					GoBackButtonEvent += CloseMenu;
				}
				break;

			case EMenuState.EquipmentScreen:
				{
					GoBackButtonEvent -= CloseMenu;
					GoBackButtonEvent -= UI_Manager.Instance.rInventoryUI.ShowEquippedEquipment;	// Unsubscribe this event so that pressing back won't show the equipped equipment
					GoBackButtonEvent += CloseOpenedScreen;                                         // Subscribe this event so that if the player presses back, they go back to the menu

				}
				break;

			case EMenuState.EquipmentBrowse:
				{
					GoBackButtonEvent -= CloseOpenedScreen;											// Unsubscribe this event so that pressing back won't go back to the menu
					GoBackButtonEvent += UI_Manager.Instance.rInventoryUI.ShowEquippedEquipment;    // Subscribe this event so that if the player presses back, they go back to the equipment screen
				}
				break;

			case EMenuState.InventoryScreen:
				{
					GoBackButtonEvent -= CloseMenu;
					GoBackButtonEvent += CloseOpenedScreen;                                         // Subscribe this event so that if the player presses back, they go back to the menu
				}
				break;
		}
	}


	public void UpdateButtonPrompts( EMenuState _MenuState )
	{
		switch ( _MenuState )
		{
			case EMenuState.Closed:
				{
					m_PromptConfirm.Deactivate();
					m_PromptAlternative1.Deactivate();
					m_PromptAlternative2.Deactivate();
					m_PromptGoBack.Deactivate();
				}
				break;

			case EMenuState.Opened:
				{
					m_PromptConfirm.Activate( "Select" );
					m_PromptAlternative1.Deactivate();
					m_PromptAlternative2.Deactivate();
					m_PromptGoBack.Activate( "Cancel" );
				}
				break;

			case EMenuState.EquipmentScreen:
				{
					m_PromptConfirm.Activate( "Select" );
					m_PromptAlternative1.Activate( "Unequip" );
					m_PromptAlternative2.Deactivate();
					m_PromptGoBack.Activate( "Cancel" );
				}
				break;

			case EMenuState.EquipmentBrowse:
				{
					m_PromptConfirm.Activate( "Equip" );
					m_PromptAlternative1.Activate( "Discard" );
					m_PromptAlternative2.Deactivate();
					m_PromptGoBack.Activate( "Cancel" );
				}
				break;
			case EMenuState.InventoryScreen:
				{
					m_PromptConfirm.Activate( "Use?" );
					m_PromptAlternative1.Activate( "Discard" );
					m_PromptAlternative2.Deactivate();
					m_PromptGoBack.Activate( "Cancel" );
				}
				break;
		}
	}
}
