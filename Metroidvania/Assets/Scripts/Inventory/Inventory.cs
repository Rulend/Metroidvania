using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{

	static private int m_AmountOfSlots = 16;	// Doesn't do anything, the size of the array is based on the number of slots in the scene.

	private int m_CurrentAmountOfItems = 0; // Set this to read from a save file rather than to 0.

	public int AmountOfSlots { get { return m_AmountOfSlots; } }


	public GameObject		m_InventorySlotsParent;	// only used once, should not be a member
	public ItemSlot[]		m_InventorySlots;       // An array of references to all of the inventory slots inside the "InventoryPanel"-object in the scene.

	// TODO:: Replace all of these lists with indices instead, so that all of them can use the same list.
	// In other words, a start index and a count of how many of that item type exists.
	public List<InventoryItem> m_Consumables;
	public List<InventoryItem> m_QuestItems;
	public List<InventoryItem> m_MiscItems;

	//private List<InventoryItem>[] GearLists;
	public List<InventoryItem> m_WeaponGear;
	public List<InventoryItem> m_HeadGear;
	public List<InventoryItem> m_ChestGear;
	public List<InventoryItem> m_HandGear;
	public List<InventoryItem> m_LegGear;
	public List<InventoryItem> m_FeetGear;

	private void Start()
	{
		m_InventorySlots = m_InventorySlotsParent.GetComponentsInChildren<ItemSlot>();


		m_WeaponGear	= new List<InventoryItem>();
		m_HeadGear		= new List<InventoryItem>();
		m_ChestGear		= new List<InventoryItem>();
		m_HandGear		= new List<InventoryItem>();
		m_LegGear		= new List<InventoryItem>();
		m_FeetGear		= new List<InventoryItem>();

		//GearLists = new List<InventoryItem>[ 6 ];
		//GearLists[0] = m_WeaponGear;
		//GearLists[1] = m_HeadGear;
		//GearLists[2] = m_ChestGear;
		//GearLists[3] = m_HandGear;
		//GearLists[4] = m_LegGear;
		//GearLists[5] = m_FeetGear;
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
			if ( pr_ItemToAdd.m_ItemType == InventoryItem.ITEMTYPE.ITEMTYPE_EQUIPMENT )
			{
				Equipment Item = (Equipment)pr_ItemToAdd;

				// TODO:: Remove the L_Hand/R_Hand equipslots. They aren't needed in order to identify weapon type anymore, just give it a generic weapon type. Then it will be equippable by both hands.
				switch ( Item.m_Equipmentslots )
				{
					case EquipmentSlot.EQUIPMENTSLOT_HEAD:			m_HeadGear.Add( pr_ItemToAdd );		break;
					case EquipmentSlot.EQUIPMENTSLOT_CHEST:			m_ChestGear.Add( pr_ItemToAdd );	break;
					case EquipmentSlot.EQUIPMENTSLOT_GAUNTLETS:		m_HandGear.Add( pr_ItemToAdd );		break;
					case EquipmentSlot.EQUIPMENTSLOT_LEGS:			m_LegGear.Add( pr_ItemToAdd );		break;
					case EquipmentSlot.EQUIPMENTSLOT_FEET:			m_FeetGear.Add( pr_ItemToAdd );		break;
				}

				return true;
			}
		}





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



	public List<InventoryItem> GetEquipmentGear( EquipmentSlot _ItemEquiSlot ) 
	{
		List<InventoryItem> ReturnedList = new List<InventoryItem>();

		switch ( _ItemEquiSlot )
		{
			case EquipmentSlot.EQUIPMENTSLOT_WEAPON:		ReturnedList = m_WeaponGear;	break;
			case EquipmentSlot.EQUIPMENTSLOT_HEAD:			ReturnedList = m_HeadGear;		break;
			case EquipmentSlot.EQUIPMENTSLOT_CHEST:			ReturnedList = m_ChestGear;		break;
			case EquipmentSlot.EQUIPMENTSLOT_GAUNTLETS:		ReturnedList = m_HandGear;		break;
			case EquipmentSlot.EQUIPMENTSLOT_LEGS:			ReturnedList = m_LegGear;		break;
			case EquipmentSlot.EQUIPMENTSLOT_FEET:			ReturnedList = m_FeetGear;		break;
		}

		return ReturnedList;
		//return GearLists[ _EquipSlotIndex ];
	}
}
