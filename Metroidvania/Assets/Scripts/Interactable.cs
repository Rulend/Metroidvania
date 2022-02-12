//using System.Collections;
//using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Interactable : MonoBehaviour
{
	public float	m_InteractionRadius = 1.5f;

	private Player	m_rPlayer;

	private float	m_DistanceToPlayer;

	private GameObject m_rInteractableAlert;
	protected string m_InteractableAlertText = "Interactable \n Text";
	


	// Start is called after creation, if the gameobject or component is not disabled
	void Start()
    {
		m_rPlayer				= GameManager.Instance.Player1;
		m_rInteractableAlert	= m_rPlayer.InventoryUI.GetComponent<InventoryUI>().m_InteractableAlert;
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
				m_rInteractableAlert.SetActive( true );
			}

		}
		// If this is the current interactable, but not within range, set current to null. TODO: Fix this later, will yield a small performance gain
		else if ( m_rPlayer.GetCurrentInteractable() == this )
		{
			m_rPlayer.SetCurrentInteractable( null );
			m_rPlayer.InventoryUI.GetComponent<InventoryUI>().m_InteractableAlert.SetActive( false );
		}
	}
}
