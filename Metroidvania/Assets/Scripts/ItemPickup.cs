using UnityEngine;

public class ItemPickup : Interactable
{
	public InventoryItem m_ItemToGive;
	//GameManager manager; // Replaced with a local variable futher down. TODO: Remove this.

	private void Awake()
	{
		//manager = GameManager.Instance; // Replaced with a local variable futher down. TODO: Remove this.
	}

	public override void Interact()
	{
		base.Interact();
		// Pick up item

		PickUp();
	}

	private void PickUp()
	{
		// Check if there is space for the item in inventory
		// If there is enough space, add item to inventory
		// If added to inventory, remove from scene

		GameManager manager = GameManager.Instance;

		Debug.Log( "Picking up " + m_ItemToGive.m_ItemName );

		if ( manager.Player1.GetInventory.AddItem( m_ItemToGive ) )
		{
			Destroy( gameObject );
		}
		else
		{
			// Add some velocity to the item, so it looks like it got picked up then dropped
		}
		
	}
}
