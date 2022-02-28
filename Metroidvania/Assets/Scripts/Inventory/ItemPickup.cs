using UnityEngine;
using UnityEngine.UI;

public class ItemPickup : Interactable
{
	public InventoryItem m_ItemToGive;

	
	//GameManager manager; // Replaced with a local variable futher down. TODO: Remove this.

	private void Awake()
	{
		//manager = GameManager.Instance; // Replaced with a local variable futher down. TODO: Remove this.
		m_InteractableAlertText += "Pick up item...";
	}

	public override void Interact()
	{
		base.Interact();

		// Pick up item and display that it has been picked up.
		PickUp();
	}

	private void PickUp()
	{
		// Check if there is space for the item in inventory
		// If there is enough space, add item to inventory
		// If added to inventory, remove from scene


		Debug.Log( "Picking up " + m_ItemToGive.m_ItemName );

		if ( m_rPlayer.GetInventory.AddItem( m_ItemToGive ) ) // If its possible to pickup the item...
		{
			m_rPlayer.InventoryUI.GetComponent<InventoryUI>().ShowItemPickedUpNotice( m_ItemToGive );

			Destroy( gameObject );
		}
		else
		{

			// Add some velocity to the item, so it looks like it got picked up then dropped
		}
		
	}
}
