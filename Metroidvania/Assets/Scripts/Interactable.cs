//using System.Collections;
//using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
	public float	m_InteractionRadius = 1.5f;

	private Player	m_rPlayer;

	private float	m_DistanceToPlayer;

	private bool	m_IsFocused = false;
	

	// The range to the player has to always be checked, as we don't know how far away the object is from the player otherwise.
	// If the player is within the interactable range, let them interact with the object if they press E.

	// The function that's called when the player presses E and is within range. Override to get actual functionality.
	public virtual void Interact()
	{
		Debug.Log( string.Format( "Interacting with {0}. \n", gameObject.name ) );
	}




	void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.blue;
		Gizmos.DrawWireSphere( transform.position, m_InteractionRadius );
	}




	// Start is called after creation, if the gameobject or component is not disabled
	void Start()
    {
		m_rPlayer			= GameManager.Instance.Player1;
	}

    // Update is called once per frame
    void Update()
    {
		m_DistanceToPlayer = Vector3.Distance( transform.position, m_rPlayer.transform.position );

		// If distance to player =< InteractionRadius, displpay "Press E to interact".

		if ( m_DistanceToPlayer <= m_InteractionRadius )
		{
			//Debug.Log( "Within interaction range \n" );

			// If player is not currently focusing another interactable, or if this interactable is closer, set this as current
			if ( !m_rPlayer.GetCurrentInteractable() || m_DistanceToPlayer < m_rPlayer.GetCurrentInteractable().m_DistanceToPlayer )
			{
				m_rPlayer.SetCurrentInteractable( this );
				m_rPlayer.InventoryUI.GetComponent<InventoryUI>().m_InteractableAlert.SetActive( true );
			}
			else if ( m_rPlayer.GetCurrentInteractable() )
			{
				m_rPlayer.InventoryUI.GetComponent<InventoryUI>().m_InteractableAlert.SetActive( true );
			}


			if ( m_rPlayer.GetCurrentInteractable() == this )
			{
				// Display interactable button and a message. "E To pick up." or "E To pull lever." or "E to talk" 
				// If this is a weapon, display the stats windows or something here.
				// Debug.Log( string.Format( "Player is closest to {0} . \n", gameObject.name) );
			}

		}
		// If this is the current interactable, but not within range, set current to null. TODO: Fix this later, will improve performance if needed.
		else if ( m_rPlayer.GetCurrentInteractable() == this )
		{
			m_rPlayer.SetCurrentInteractable( null );
		}



	}
}
