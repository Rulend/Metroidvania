using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Menu : MonoBehaviour
{
	public enum EMenuState
	{
		MENUSTATE_Closed,
		MENUSTATE_Open,
		MENUSTATE_EquipmentTab,
		MENUSTATE_ChoosingEquipment,
		MENUSTATE_InventoryTab,
		MENUSTATE_InventoryOptions,
		MENUSTATE_Options,
		MENUSTATE_AdjustingOptions,
	}

	private Button		m_SelectedButton;

	public delegate void GoToPreviousWindowHandler();
	public GoToPreviousWindowHandler GoToPreviousWindowEvent;

	[SerializeField] private GameObject	m_SelectedButtonBorder;

	private GameObject m_OpenedScreen;


	private void Start()
	{
		gameObject.SetActive( false );
	}


	public void OpenMenu()
	{
		gameObject.SetActive( true );
		m_SelectedButtonBorder.SetActive( true );

		GetComponentInChildren<Button>().Select();
	}

	public void CloseMenu()
	{
		gameObject.SetActive( false );
		m_SelectedButtonBorder.SetActive( false );

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

		m_OpenedScreen = UI_Manager.Instance.rInventoryUI.gameObject;

		m_OpenedScreen.SetActive( true );
		m_OpenedScreen.GetComponentInChildren<Button>().Select();
	}

	public void ButtonOpenInventoryScreen()
	{

	}

	public void ButtonOpenOptionsScreen()
	{

	}


	public void CloseOpenedScreen()
	{
		GoToPreviousWindowEvent -= CloseOpenedScreen;


		m_OpenedScreen.SetActive( false );
		gameObject.SetActive( true );

		m_OpenedScreen = null;

		GetComponentInChildren<Button>().Select();
	}


	public void GoBackToPreviousWindow()
	{
		if ( GoToPreviousWindowEvent?.GetInvocationList().Length > 0 ) // This is more than 0 if any screen has been opened (equipment, inventory, options ,etc)
			GoToPreviousWindowEvent.Invoke();
		else
			CloseMenu();
	}
}
