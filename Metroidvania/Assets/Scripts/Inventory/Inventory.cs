using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{

	static private int m_AmountOfSlots = 16;	// Doesn't do anything, the size of the array is based on the number of slots in the scene.

	private int m_CurrentAmountOfItems = 0; // Set this to read from a save file rather than to 0.

	public int AmountOfSlots { get { return m_AmountOfSlots; } }


	public GameObject		m_InventorySlotsParent;	// only used once, should not be a member
	public ItemSlot[]		m_InventorySlots;       // An array of references to all of the inventory slots inside the "InventoryPanel"-object in the scene.


	private void Start()
	{
		m_InventorySlots = m_InventorySlotsParent.GetComponentsInChildren<ItemSlot>();
	}

	////////////////////////////////////////////////
	/// Function information - AddItem
	/// 
	/// Adds an item to the inventory.
	/// 
	/// return value:	returns false if pr_ItemToAdd
	///					is a default item or if there is not enough
	///					space in inventory, otherwise returns true.
	/// 
	/// parameters:
	/// pr_ItemToAdd	: the item that should be added to inventory.
	/// 
	////////////////////////////////////////////////
	public bool AddItem( InventoryItem pr_ItemToAdd )
	{
		// Is it stackable? Is there space in that stack? Is the inventory full? TODO:: Check these stuff.

		if ( !pr_ItemToAdd.m_DefaultItem )
		{
			foreach ( ItemSlot rCurrentSlot in m_InventorySlots )
			{
				if ( !rCurrentSlot.Item )
				{
					rCurrentSlot.AddItemToSlot( pr_ItemToAdd );

					return true;
				}
			}

		}

		// TODO:: Show the player a prompt that tells them why they couldn't add the item.
		Debug.Log( "Couldn't add item to inventory. \n" );
		return false;
	}

	////////////////////////////////////////////////
	/// Function information - RemoveItem
	/// 
	/// Removes an item from the player's inventory.
	/// 
	/// parameters:
	/// pr_ItemToRemove		: the item which should be removed from the inventory.
	/// pr_SpawnItemPickup	: whether or not an item pickup should be spawned on the ground after removing the item. Used when dropping an item.
	/// 
	////////////////////////////////////////////////

	public bool RemoveItem( InventoryItem pr_ItemToRemove, bool pr_SpawnItemPickup = true )
	{
		foreach ( ItemSlot rCurrentSlot in m_InventorySlots )
		{
			if ( rCurrentSlot.Item == pr_ItemToRemove )
			{
				rCurrentSlot.RemoveItemFromSlot( pr_SpawnItemPickup );

				return true;
			}
		}

		return false;
	}
}
