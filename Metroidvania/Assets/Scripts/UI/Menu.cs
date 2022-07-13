using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Menu : MonoBehaviour
{
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

		if ( GoToPreviousWindowEvent != null ) // If the amount of subscribers is more than 0
		{
			foreach ( var SubscribedFunction in GoToPreviousWindowEvent.GetInvocationList() ) // Unsubscribe all subscribers
				GoToPreviousWindowEvent -= ( SubscribedFunction as GoToPreviousWindowHandler );
		}

		if ( m_OpenedScreen )
		{
			// TODO:: Make an enum switch here on which screen was opened. Otherwise this will crash later.

			m_OpenedScreen.SetActive( false );
			m_OpenedScreen = null;
		}

		GameManager.Instance.rPlayer1.GetComponent<PlayerController>().SetState( PlayerController.EPlayerControllerState.PCSTATE_Normal );
	}


	public void SelectButton( Button _ButtonToSelect )
	{
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
