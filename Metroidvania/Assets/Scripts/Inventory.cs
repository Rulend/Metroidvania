using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{

	static private int m_AmountOfSlots = 16;

	private int m_CurrentAmountOfItems = 0; // Set this to read from a save file rather than set it o 0.
	public int AmountOfSlots { get { return m_AmountOfSlots; } }


	public GameObject		m_InventorySlotsParent;	// only used once, should not be a member
	public InventorySlot[]	m_InventorySlots;       // An array of references to all of the inventory slots inside the "InventoryPanel"-object in the scene.


	private void Start()
	{
		m_InventorySlots = m_InventorySlotsParent.GetComponentsInChildren<InventorySlot>();
	}

	public bool AddItem( InventoryItem pr_ItemToAdd )
	{
		// Is it stackable? Is there space in that stack? Is the inventory full? TODO:: Check these stuff.

		if ( !pr_ItemToAdd.m_DefaultItem )
		{
			foreach ( InventorySlot rCurrentSlot in m_InventorySlots )
			{
				if ( !rCurrentSlot.Item )
				{
					rCurrentSlot.AddItemToSlot( pr_ItemToAdd );

					return true;
				}
			}

		}

		Debug.Log( "Couldn't add item to inventory, it's full. \n" );
		return false;
	}

	public bool RemoveItem( InventoryItem pr_ItemToRemove )
	{
		foreach ( InventorySlot rCurrentSlot in m_InventorySlots )
		{
			if ( rCurrentSlot.Item == pr_ItemToRemove )
			{
				rCurrentSlot.RemoveItemFromSlot();

				return true;
			}
		}

		return false;
	}
}
