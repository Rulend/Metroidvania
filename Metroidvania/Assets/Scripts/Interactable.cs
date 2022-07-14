//using System.Collections;
//using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Interactable : MonoBehaviour
{
	public		float			m_InteractionRadius = 1.5f;
	protected	string			m_InteractableAlertText = "";	// The text that will be when the player is within InteractionRadius. 


	protected	Player				m_rPlayer;				// A reference to the player.
	private		float				m_DistanceToPlayer;		// Distance to the player.


	// Start is called after creation, if the gameobject or component is not disabled
	void Start()
    {
		m_rPlayer				= GameManager.Instance.rPlayer1;
	}


	// The function that's called when the player presses E and is within range. Override to get actual functionality.
	public virtual void Interact()
	{
		Debug.Log( string.Format( $"Interacting with { gameObject.name }. \n" ) );
		m_rPlayer.m_CurrentlyFocusedInteractable = null;
		UI_Manager.Instance.PromptInteract.Deactivate();
	}



	// The range to the player has to always be checked, as we don't know how far away the object is from the player otherwise.
	// If the player is within the interactable range, and player's currently selected interactable is null or further away than self,
	// set self as the currently selected interactable and display the interactable text
	void Update()
    {
		m_DistanceToPlayer = Vector3.Distance( transform.position, m_rPlayer.transform.position );

		if ( m_DistanceToPlayer <= m_InteractionRadius )
		{
			if ( !m_rPlayer.m_CurrentlyFocusedInteractable || m_DistanceToPlayer < m_rPlayer.m_CurrentlyFocusedInteractable.m_DistanceToPlayer ) 
			{
				m_rPlayer.m_CurrentlyFocusedInteractable = this;
				UI_Manager.Instance.PromptInteract.Activate( m_InteractableAlertText );
			}
		}
		else if ( m_rPlayer.m_CurrentlyFocusedInteractable == this ) // If this is the current interactable, but not within range, set current to null.
		{
			m_rPlayer.m_CurrentlyFocusedInteractable = null;
			UI_Manager.Instance.PromptInteract.Deactivate();
		}
	}
}
