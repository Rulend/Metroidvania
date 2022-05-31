//using System.Collections;
//using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Interactable : MonoBehaviour
{
	public		float			m_InteractionRadius = 1.5f;
	private		GameObject		m_rInteractableAlert;		// A reference to the Alert that shows up on the screen when the player comes close. Found inside the UI-canvas.
	protected	string			m_InteractableAlertText = "Press E to \n ";	// The text that will be when the player is within InteractionRadius. Add to (+=) this in derived classes. DO NOT OVERWRITE. 


	protected	Player			m_rPlayer;			// A reference to the player.
	private		float			m_DistanceToPlayer;	// Distance to the player.


	// Start is called after creation, if the gameobject or component is not disabled
	void Start()
    {
		m_rPlayer				= GameManager.Instance.rPlayer1;
		m_rInteractableAlert	= UI_Manager.Instance.rInventoryUI.m_InteractableAlert;
	}


	// The function that's called when the player presses E and is within range. Override to get actual functionality.
	public virtual void Interact()
	{
		Debug.Log( string.Format( "Interacting with {0}. \n", gameObject.name ) );
		m_rPlayer.SetCurrentInteractable( null );
		m_rInteractableAlert.SetActive( false );
	}

	// The range to the player has to always be checked, as we don't know how far away the object is from the player otherwise.
	// If the player is within the interactable range, let them interact with the object if they press E.
	// Update is called once per frame
	void Update()
    {
		m_DistanceToPlayer = Vector3.Distance( transform.position, m_rPlayer.transform.position );

		// If distance to player =< InteractionRadius, displpay "Press E to interact".

		if ( m_DistanceToPlayer <= m_InteractionRadius )
		{
			// If player is not currently focusing another interactable, or if this interactable is closer, set this as current and display the Interact text.
			if ( !m_rPlayer.GetCurrentInteractable() || m_DistanceToPlayer < m_rPlayer.GetCurrentInteractable().m_DistanceToPlayer )
			{
				m_rPlayer.SetCurrentInteractable( this );
				m_rInteractableAlert.GetComponentInChildren<Text>().text = m_InteractableAlertText;

				InventoryUI rInventoryUI = UI_Manager.Instance.rInventoryUI;

				// Set position of the interactable text based on whether the player recently picked up an item or not. This is so it can still be seen even if ItemPickedUpNotice is active.
				if ( rInventoryUI.m_ItemPickedUpNotice.activeSelf )
					m_rInteractableAlert.transform.position = rInventoryUI.InteractableAlertStartPos - new Vector3( 0.0f, 200.0f, 0.0f ); // TODO:: Change this from a hard coded value to be the y-size of the itempickupnotice yextbox
				else
					m_rInteractableAlert.transform.position = rInventoryUI.InteractableAlertStartPos;

				m_rInteractableAlert.SetActive( true );
			}

		}
		// If this is the current interactable, but not within range, set current to null. TODO: Fix this later, will yield a small performance gain
		else if ( m_rPlayer.GetCurrentInteractable() == this )
		{
			m_rPlayer.SetCurrentInteractable( null );
			UI_Manager.Instance.rInventoryUI.m_InteractableAlert.SetActive( false ); // Todo:: fix this monstrosity of a mess
		}
	}
}
