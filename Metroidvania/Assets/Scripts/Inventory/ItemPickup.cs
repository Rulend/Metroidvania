using UnityEngine;
using UnityEngine.UI;

public class ItemPickup : Interactable
{
	public InventoryItem m_ItemToGive;

	
	//GameManager manager; // Replaced with a local variable futher down. TODO: Remove this.

	private void Awake()
	{
		//manager = GameManager.Instance; // Replaced with a local variable futher down. TODO: Remove this.
		m_InteractableAlertText = "Press E to Pick up item...";
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

		GameManager rGameManager = GameManager.Instance;

		Debug.Log( "Picking up " + m_ItemToGive.m_ItemName );

		if ( rGameManager.Player1.GetInventory.AddItem( m_ItemToGive ) ) // If its possible to pickup the item...
		{
			rGameManager.Player1.InventoryUI.GetComponent<InventoryUI>().ShowItemPickedUpNotice( m_ItemToGive );

			Destroy( gameObject );
		}
		else
		{

			// Add some velocity to the item, so it looks like it got picked up then dropped
		}
		
	}
}
