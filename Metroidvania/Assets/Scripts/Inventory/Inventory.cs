using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{

	static private int m_AmountOfSlots = 16;	// Doesn't do anything, the size of the array is based on the number of slots in the scene.

	private int m_CurrentAmountOfItems = 0; // Set this to read from a save file rather than to 0.

	public int AmountOfSlots { get { return m_AmountOfSlots; } }

	private GameObject m_ItemPickupPrefab;      // A prefab used for instantiating the item on the ground when discarding it


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

	public delegate void	InventoryUpdateHandler( List<InventoryItem> _Items );
	public event			InventoryUpdateHandler InventoryUpdateEvent;

	private void Start()
	{
		string ItemPickupPrefabFilePath = "Prefabs/ItemPickup";

		m_ItemPickupPrefab = (GameObject)Resources.Load( ItemPickupPrefabFilePath );

		if ( m_ItemPickupPrefab == null )
			Debug.LogError( $"Failed to load ItemPickupPrefab at filepath {ItemPickupPrefabFilePath}" );


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
	public bool AddItem( InventoryItem _ItemToAdd )
	{
		// Is it stackable? Is there space in that stack? Is the inventory full? TODO:: Check these stuff.


		if ( !_ItemToAdd.m_DefaultItem )
		{
			List<InventoryItem> NewItemList = new List<InventoryItem>();


			if ( _ItemToAdd.m_ItemType == InventoryItem.ITEMTYPE.ITEMTYPE_EQUIPMENT )
			{
				Equipment Item = (Equipment)_ItemToAdd;

				// TODO:: Remove the L_Hand/R_Hand equipslots. They aren't needed in order to identify weapon type anymore, just give it a generic weapon type. Then it will be equippable by both hands.
				switch ( Item.m_Equipmentslots )
				{
					case EquipmentSlot.EQUIPMENTSLOT_HEAD:		NewItemList = m_HeadGear;	break;
					case EquipmentSlot.EQUIPMENTSLOT_CHEST:		NewItemList = m_ChestGear;	break;
					case EquipmentSlot.EQUIPMENTSLOT_GAUNTLETS:	NewItemList = m_HandGear;	break;
					case EquipmentSlot.EQUIPMENTSLOT_LEGS:		NewItemList = m_LegGear;	break;
					case EquipmentSlot.EQUIPMENTSLOT_FEET:		NewItemList = m_FeetGear;	break;
				}
			}

			// TODO:: Check if the item is stackable. If it is, search through the inventory to see if it already exists. If it does, increase the stack amount rather than adding it.
			// TODO:: If the item already exists, don't do this:
			InventoryItem InstancedItem = Object.Instantiate( _ItemToAdd ); // This needs to be done since otherwise there will only be one instance of the object. That's how it works.

			NewItemList.Add( InstancedItem );

			return true;
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

	public void RemoveItem( InventoryItem _ItemToRemove, bool _SpawnItemPickup = true )
	{
		List<InventoryItem> ListToUpdate = new List<InventoryItem>();


		if ( _ItemToRemove.m_ItemType == InventoryItem.ITEMTYPE.ITEMTYPE_EQUIPMENT )
		{
			Equipment Item = (Equipment)_ItemToRemove;

			// TODO:: Remove the L_Hand/R_Hand equipslots. They aren't needed in order to identify weapon type anymore, just give it a generic weapon type. Then it will be equippable by both hands.
			switch ( Item.m_Equipmentslots )
			{
				case EquipmentSlot.EQUIPMENTSLOT_HEAD:		ListToUpdate =	m_HeadGear;		break;
				case EquipmentSlot.EQUIPMENTSLOT_CHEST:		ListToUpdate =	m_ChestGear;	break;
				case EquipmentSlot.EQUIPMENTSLOT_GAUNTLETS:	ListToUpdate =	m_HandGear;		break;
				case EquipmentSlot.EQUIPMENTSLOT_LEGS:		ListToUpdate =	m_LegGear;		break;
				case EquipmentSlot.EQUIPMENTSLOT_FEET:		ListToUpdate =	m_FeetGear;		break;
			}
		}



		//ListToUpdate.Remove( _ItemToRemove );

		int RemovedItemIndex = 0;

		for ( int ItemIndex = 0; ItemIndex < ListToUpdate.Count; ++ItemIndex )
		{
			if ( ListToUpdate[ ItemIndex ] == _ItemToRemove )
			{
				ListToUpdate.RemoveAt( ItemIndex );
				RemovedItemIndex = ItemIndex;
				break;
			}
		}

		for ( int ListIndex = RemovedItemIndex; ListIndex < ListToUpdate.Count - 1; ++ListIndex ) // Reorganize the rest of the inventory to adjust for the removed item
			ListToUpdate[ ListIndex ] = ListToUpdate[ ListIndex + 1 ];

		if ( _SpawnItemPickup )
		{
			m_ItemPickupPrefab.GetComponent<ItemPickup>().m_ItemToGive = _ItemToRemove;
			Instantiate( m_ItemPickupPrefab, GameManager.Instance.rPlayer1.transform.position, Quaternion.identity );
		}

		InventoryUpdateEvent.Invoke( ListToUpdate );

		return;
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
