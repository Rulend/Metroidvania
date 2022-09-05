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
	// Note: Since I wrote the above comment I've changed these to dictionaries; don't think it's really all that practical to change it now. I'll have to look at it sometime soon.
	private Dictionary<InventoryItem, int>	m_Consumables;
	private Dictionary<InventoryItem, int>	m_QuestItems;
	private Dictionary<InventoryItem, int>	m_MiscItems;


	private List<Dictionary<InventoryItem, int>> GearDictionaries;

	private Dictionary<InventoryItem, int> m_WeaponEquipment;
	private Dictionary<InventoryItem, int> m_HeadEquipment;
	private Dictionary<InventoryItem, int> m_ChestEquipment;
	private Dictionary<InventoryItem, int> m_HandEquipment;
	private Dictionary<InventoryItem, int> m_LegEquipment;
	private Dictionary<InventoryItem, int> m_FeetEquipment;

	public delegate void	InventoryUpdateHandler( Dictionary<InventoryItem, int> _Items );
	public event			InventoryUpdateHandler InventoryUpdateEvent;

	private void Start()
	{
		string ItemPickupPrefabFilePath = "Prefabs/Prefab_ItemPickup";

		m_ItemPickupPrefab = (GameObject)Resources.Load( ItemPickupPrefabFilePath );

		if ( m_ItemPickupPrefab == null )
			Debug.LogError( $"Failed to load ItemPickupPrefab at filepath {ItemPickupPrefabFilePath}" );


		m_Consumables	= new Dictionary<InventoryItem, int>();
		m_QuestItems	= new Dictionary<InventoryItem, int>();
		m_MiscItems		= new Dictionary<InventoryItem, int>();



		m_WeaponEquipment	= new Dictionary<InventoryItem, int>();
		m_HeadEquipment		= new Dictionary<InventoryItem, int>();
		m_ChestEquipment	= new Dictionary<InventoryItem, int>();
		m_HandEquipment		= new Dictionary<InventoryItem, int>();
		m_LegEquipment		= new Dictionary<InventoryItem, int>();
		m_FeetEquipment		= new Dictionary<InventoryItem, int>();


		GearDictionaries = new List<Dictionary<InventoryItem, int>>();
		GearDictionaries.Add( m_WeaponEquipment );
		GearDictionaries.Add( m_HeadEquipment );
		GearDictionaries.Add( m_ChestEquipment );
		GearDictionaries.Add( m_HandEquipment );
		GearDictionaries.Add( m_LegEquipment );
		GearDictionaries.Add( m_FeetEquipment );
	}



	////////////////////////////////////////////////
	/// Method Information - AddItem
	/// 
	/// Adds an item to the inventory.
	/// 
	/// return value:	returns false if pr_ItemToAdd
	///					is a default item or if there is not enough
	///					space in inventory, otherwise returns true.
	/// 
	/// parameters:
	/// _ItemToAdd	: the item that should be added to inventory.
	/// 
	////////////////////////////////////////////////
	public bool AddItem( InventoryItem _ItemToAdd )
	{
		// Is it stackable? Is there space in that stack? Is the inventory full? TODO:: Check these stuff.

		if ( _ItemToAdd.m_DefaultItem )
		{
			// TODO:: Add a define for testing vs running the game
			Debug.Log( "Can't add a default item to the inventory." );
			return false;
		}



		Dictionary<InventoryItem, int> DictionaryToPlaceIn = new Dictionary<InventoryItem, int>();


		switch ( _ItemToAdd.m_ItemType )
		{
			case ITEMTYPE.ITEMTYPE_MISC:
				{
					DictionaryToPlaceIn = m_MiscItems;
				}
				break;

			case ITEMTYPE.ITEMTYPE_CONSUMABLE:
				{
					DictionaryToPlaceIn = m_Consumables;
				}
				break;

			case ITEMTYPE.ITEMTYPE_EQUIPMENT:
				{
					Equipment Item = (Equipment)_ItemToAdd;

					switch ( Item.m_Equipmentslots )
					{
						case EquipmentSlot.EQUIPMENTSLOT_WEAPON:		DictionaryToPlaceIn = m_WeaponEquipment;	break;
						case EquipmentSlot.EQUIPMENTSLOT_HEAD:			DictionaryToPlaceIn = m_HeadEquipment;		break;
						case EquipmentSlot.EQUIPMENTSLOT_CHEST:			DictionaryToPlaceIn = m_ChestEquipment;		break;
						case EquipmentSlot.EQUIPMENTSLOT_GAUNTLETS:		DictionaryToPlaceIn = m_HandEquipment;		break;
						case EquipmentSlot.EQUIPMENTSLOT_LEGS:			DictionaryToPlaceIn = m_LegEquipment;		break;
						case EquipmentSlot.EQUIPMENTSLOT_FEET:			DictionaryToPlaceIn = m_FeetEquipment;		break;
					}
				}
				break;

			case ITEMTYPE.ITEMTYPE_QUEST:
				{
					DictionaryToPlaceIn = m_QuestItems;
				}
				break;
		}

		// TODO:: Check if the item is stackable. If it is, search through the inventory to see if it already exists. If it does, increase the stack amount rather than adding it.
		// TODO:: If the item already exists, don't do this:

		if ( _ItemToAdd.m_Stackable )
		{
			if ( DictionaryToPlaceIn.ContainsKey( _ItemToAdd ) )
			{
				DictionaryToPlaceIn[ _ItemToAdd ] += 1;
				Debug.Log( "Item was stackable and already in inventory, increasing count." );
			}
			else
			{
				DictionaryToPlaceIn.Add( _ItemToAdd, 1 );
				Debug.Log( "Item is stackable but the examined instance does not already exist in inventory.,," );
			}

			InventoryUpdateEvent.Invoke( DictionaryToPlaceIn );
			EquipmentManager.Instance.EquipWheel.UpdateWheel();

			return true;
		}

		InventoryItem InstancedItem = Object.Instantiate( _ItemToAdd ); // This needs to be done since otherwise there will only be one instance of the object. That's how it works.

		DictionaryToPlaceIn.Add( InstancedItem, 1 );

		InventoryUpdateEvent.Invoke( DictionaryToPlaceIn );
		EquipmentManager.Instance.EquipWheel.UpdateWheel();

		return true;
		// TODO:: Show the player a prompt that tells them why they couldn't add the item.
	}



	////////////////////////////////////////////////
	/// Method Information - RemoveItem
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
		Dictionary<InventoryItem, int> DictionaryToUpdate = new Dictionary<InventoryItem, int>();


		switch ( _ItemToRemove.m_ItemType )
		{
			case ITEMTYPE.ITEMTYPE_MISC:
				{
					DictionaryToUpdate = m_MiscItems;
				}
				break;

			case ITEMTYPE.ITEMTYPE_CONSUMABLE:
				{
					DictionaryToUpdate = m_Consumables;
				}
				break;

			case ITEMTYPE.ITEMTYPE_EQUIPMENT:
				{
					Equipment Item = (Equipment)_ItemToRemove;

					switch ( Item.m_Equipmentslots )
					{
						case EquipmentSlot.EQUIPMENTSLOT_WEAPON:	DictionaryToUpdate = m_WeaponEquipment;	break;
						case EquipmentSlot.EQUIPMENTSLOT_HEAD:		DictionaryToUpdate = m_HeadEquipment;	break;
						case EquipmentSlot.EQUIPMENTSLOT_CHEST:		DictionaryToUpdate = m_ChestEquipment;	break;
						case EquipmentSlot.EQUIPMENTSLOT_GAUNTLETS: DictionaryToUpdate = m_HandEquipment;	break;
						case EquipmentSlot.EQUIPMENTSLOT_LEGS:		DictionaryToUpdate = m_LegEquipment;	break;
						case EquipmentSlot.EQUIPMENTSLOT_FEET:		DictionaryToUpdate = m_FeetEquipment;	break;
					}

				}
				break;

			case ITEMTYPE.ITEMTYPE_QUEST:
				{
					DictionaryToUpdate = m_QuestItems;
				}
				break;
		}




		// If the item is equipped, save down the equipped slot so we can update it 
		ItemSlot EquippedSlot = EquipmentManager.Instance.IsItemEquipped( _ItemToRemove );

		DictionaryToUpdate[ _ItemToRemove ] -= 1;

		if ( DictionaryToUpdate[ _ItemToRemove ] < 1 )
		{
			DictionaryToUpdate.Remove( _ItemToRemove );

			if ( EquippedSlot )	// Remove item if it's equipped
				EquipmentManager.Instance.Unequip( _ItemToRemove );
		}
		else if ( EquippedSlot )	// Since item wasn't removed, update the equipped slot if there is one
			EquippedSlot.AddItemToSlot( _ItemToRemove, DictionaryToUpdate[ _ItemToRemove ] );



		if ( _SpawnItemPickup )
		{
			m_ItemPickupPrefab.GetComponent<ItemPickup>().m_ItemToGive = _ItemToRemove;
			Instantiate( m_ItemPickupPrefab, GameManager.Instance.rPlayer1.transform.position, Quaternion.identity );
		}

		InventoryUpdateEvent.Invoke( DictionaryToUpdate );

		return;
	}


	////////////////////////////////////////////////
	/// Method Information - GetEquipmentGear
	/// 
	/// Desc:	Returns the dictionary that contains gear
	///			of the selected equipmentslot
	/// 
	/// return	value: the dictionary with gear of the selected
	///			equipmentslot
	/// 
	/// parameters:
	/// _ItemEquipSlot: which type of dictionary to get
	/// 
	////////////////////////////////////////////////
	public Dictionary<InventoryItem, int> GetEquipmentGear( EquipmentSlot _ItemEquipSlot ) 
	{
		Dictionary<InventoryItem, int> ReturnedDictionary = new Dictionary<InventoryItem, int>();

		switch ( _ItemEquipSlot )
		{
			case EquipmentSlot.EQUIPMENTSLOT_WEAPON:		ReturnedDictionary = m_WeaponEquipment;		break;
			case EquipmentSlot.EQUIPMENTSLOT_HEAD:			ReturnedDictionary = m_HeadEquipment;		break;
			case EquipmentSlot.EQUIPMENTSLOT_CHEST:			ReturnedDictionary = m_ChestEquipment;		break;
			case EquipmentSlot.EQUIPMENTSLOT_GAUNTLETS:		ReturnedDictionary = m_HandEquipment;		break;
			case EquipmentSlot.EQUIPMENTSLOT_LEGS:			ReturnedDictionary = m_LegEquipment;		break;
			case EquipmentSlot.EQUIPMENTSLOT_FEET:			ReturnedDictionary = m_FeetEquipment;		break;
			case EquipmentSlot.EQUIPMENTSLOT_Consumable:	ReturnedDictionary = m_Consumables;			break;
		}

		return ReturnedDictionary;
	}

	public Dictionary<InventoryItem, int> GetItemsInCategory( ITEMTYPE _ItemCategory )
	{
		Dictionary<InventoryItem, int> ReturnedDictionary = new Dictionary<InventoryItem, int>();

		switch ( _ItemCategory )
		{
			case ITEMTYPE.ITEMTYPE_MISC:		ReturnedDictionary = m_MiscItems;		break;
			case ITEMTYPE.ITEMTYPE_CONSUMABLE:	ReturnedDictionary = m_Consumables;		break;
			case ITEMTYPE.ITEMTYPE_EQUIPMENT:
				{
					foreach ( var EquipmentDictionary in GearDictionaries )
					{
						foreach ( var CurrentEquipment in EquipmentDictionary )
							ReturnedDictionary.Add( CurrentEquipment.Key, CurrentEquipment.Value );
					}
				}
				break;

			case ITEMTYPE.ITEMTYPE_QUEST: ReturnedDictionary = m_QuestItems;	break;
		}

		return ReturnedDictionary;
	}
}
