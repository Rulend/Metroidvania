using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class EquipmentManager : MonoBehaviour
{
	public static EquipmentManager			Instance => m_Instance; // Public getter used to access singleton from outside


						private static EquipmentManager			m_Instance;
	[SerializeField]	private GameObject						m_EquipmentSlotsParent;		// only used once, should not be a member TODO:: Remove
	[SerializeField]	private GameObject						m_WeaponSlotsParent;		// only used once, should not be a member TODO:: Remove
	[SerializeField]	private GameObject						m_ConsumableSlotsParent;	// only used once, should not be a member TODO:: Remove
						private ItemSlot[]						m_EquipmentSlots;		// An array of references to all of the inventory slots inside the "EquipmentPanel"-object in the scene. Used to store current equipment.
						private SkinnedMeshRenderer[]			m_EquipmentMeshes;      // An array of the current equipment's meshes
	[SerializeField]	private SkinnedMeshRenderer				m_PlayerMesh;

	[SerializeField]	private InventoryItem[]					m_DefaultSetupArray;
						private Dictionary<EquipmentSlot, InventoryItem> m_DefaultEquipment; // TODO:: Get rid of this later

						private ItemSlot						m_SelectedEquipmentSlot;
						public  ItemSlot						SelectedEquipmentSlot => m_SelectedEquipmentSlot;
						private int								m_NumberWeaponSlots;

	[SerializeField]	private EquipmentWheel					m_EquipWheel;
						public EquipmentWheel					EquipWheel => m_EquipWheel;

	private void Awake()
	{
		// Will set this to be the instance the first time, and delete any later attempts to create more.
		if ( m_Instance && m_Instance != this )
		{
			Destroy( this.gameObject );
		}
		else
		{
			m_Instance = this;
		}

		// Create default equipment
		//m_DefaultEquipment = new Equipment[]
		//{

		// dragged inside from inspector (cheating)
		//};


		m_EquipmentSlots	= m_EquipmentSlotsParent.GetComponentsInChildren<ItemSlot>();
		m_EquipmentMeshes	= new SkinnedMeshRenderer[ m_EquipmentSlots.Length ];


		m_NumberWeaponSlots = m_WeaponSlotsParent.GetComponentsInChildren<ItemSlot>().Length;

		m_EquipWheel.SetupWheel( m_ConsumableSlotsParent.GetComponentsInChildren<ItemSlot>() );

		// TODO:: Instead of just loading in the default items and equipping them like below,
		// Setup a save system so that the player's last equipped items will be equipped instead

		m_DefaultEquipment = new Dictionary<EquipmentSlot, InventoryItem>();
		InventoryItem[] DefaultItems = Resources.LoadAll<InventoryItem>( "Scripts/Inventory/Items/Equipment/Default" );


		foreach ( InventoryItem CurrentItem in DefaultItems )
		{
			Equipment CurrentEquipment = (Equipment)CurrentItem;
			m_DefaultEquipment.Add( CurrentEquipment.EquipmentSlots, CurrentItem );
		}

		// Equip the items
		for ( int EquipIndex = 0; EquipIndex < m_DefaultSetupArray.Length; ++EquipIndex )
		{
			m_SelectedEquipmentSlot = m_EquipmentSlots[ EquipIndex ]; // Set a current equipmentslot in order to make the next line work
			Equip( (Equipment)m_DefaultSetupArray[ EquipIndex ], 1, false, false );
		}
	}

	private void Start()
	{
		//m_EquipmentSlots	= m_EquipmentSlotsParent.GetComponentsInChildren<ItemSlot>();
		
		//m_NumberWeaponSlots = m_WeaponSlotsParent.GetComponentsInChildren<ItemSlot>().Length;


		// This is how I used to load the equipment from a JSON file. It doesn't work good beacuse the ID for the images don't match between sessions. Gonna make a database to fix this later.
		// TODO:: Make a database to store and load items like this.
		//// How to write to JSON
		//SerializableEquipmentArray TestArray = new SerializableEquipmentArray( m_DefaultEquipment.Length );

		//// TODO:: Fix it so that the order of the default items is correct
		//for ( int i = 0; i < m_DefaultEquipment.Length; ++i )
		//{
		//	TestArray.ItemArray[ i ] = (SerializableEquipment)m_DefaultEquipment[ i ];
		//}

		//string JsonString = JsonUtility.ToJson( TestArray );

		//File.WriteAllText( Application.dataPath + "/Resources/Items(Json)/DefaultItems.json", JsonString );

		//// How to read from JSON
		//string DefaultItemsFilePath = Application.dataPath + "/Resources/Items(Json)/DefaultItems.json";

		//SerializableEquipmentArray ArrayFromJson = JsonUtility.FromJson<SerializableEquipmentArray>( File.ReadAllText( DefaultItemsFilePath ) ); // Create equipment from Json-object

		//for ( int EquipIndex = 0; EquipIndex < ArrayFromJson.ItemArray.Length; ++EquipIndex )
		//{
		//	Equipment CurrentEquip = (Equipment)ArrayFromJson.ItemArray[EquipIndex];

		//	m_SelectedEquipmentSlot = m_EquipmentSlots[ EquipIndex ];
		//	Equip( CurrentEquip, false );
		//}

		m_SelectedEquipmentSlot = m_EquipmentSlots[ 0 ];
	}

	////////////////////////////////////////////////
	/// Function information - Equip
	/// 
	/// Equips an item in the selected slot.
	/// 
	/// return value: void
	/// 
	/// parameters:
	/// _NewItem : the item that should be equipped.
	////////////////////////////////////////////////
	public void Equip( InventoryItem _ItemToEquip, int _Amount = 1, bool _UpdateEquipmentWheel = false, bool _UpdateInventoryUI = true )
	{
		if ( _ItemToEquip == null )
			return;

		// TODO:: Add level or stat requirements here or somewhere else to check whether the character can actually equip the item.


		// Maybe optimize this? It works but might come back to bite in ass later. What it does: Swaps places on two items if the current slot has an item and the equipped item is already equipped somewhere else
		if ( _ItemToEquip.m_ItemType == ITEMTYPE.ITEMTYPE_CONSUMABLE )
		{
			ItemSlot EquippedSlot = IsItemEquipped( _ItemToEquip );

			if ( EquippedSlot ) // If the item that the player wants to equip is already equipped
			{
				if ( m_SelectedEquipmentSlot.Item ) // If ItemToEquip is already equipped in another slot, set the item of selected slot to that slot, which reuslts in a swap. 
					EquippedSlot.AddItemToSlot( m_SelectedEquipmentSlot.Item, m_SelectedEquipmentSlot.Amount );
				else
					Unequip( _ItemToEquip );
			}
		}

		if ( _ItemToEquip != m_SelectedEquipmentSlot.Item )
		{
			Unequip( m_SelectedEquipmentSlot.Item, true ); // Unequip old item


			m_SelectedEquipmentSlot.AddItemToSlot( _ItemToEquip, _Amount ); // Equip new item

			if ( !_ItemToEquip.m_DefaultItem )
			{
				m_SelectedEquipmentSlot.DisplayItemInfo();
			}


			//if ( !_ItemToEquip.m_DefaultItem && _ItemToEquip.m_ItemType == ITEMTYPE.ITEMTYPE_EQUIPMENT )
			//{
			//	Equipment			NewEquipment			= _ItemToEquip as Equipment;
			//	SkinnedMeshRenderer NewSkinnedMeshRenderer	= Instantiate<SkinnedMeshRenderer>( NewEquipment.m_Mesh );
			//	NewSkinnedMeshRenderer.bones				= m_PlayerMesh.bones;
			//	NewSkinnedMeshRenderer.rootBone				= m_PlayerMesh.rootBone;
			//	NewSkinnedMeshRenderer.transform.SetParent( m_PlayerMesh.gameObject.transform );

			//	m_EquipmentMeshes[ (int)NewEquipment.EquipmentSlots ] = NewSkinnedMeshRenderer;
			//}
		}


		if ( _UpdateInventoryUI )
			UI_Manager.Instance.rInventoryUI.UpdateEquippedIcon();

		// Update equipment wheels
		if ( _UpdateEquipmentWheel )
			m_EquipWheel.UpdateWheel(); // Send in type of thing to update here, like weapon/shield or consumable.

		// TODO:: Add stats re-calculation in here
	}


	////////////////////////////////////////////////
	/// Function information - Unequip
	/// 
	/// Unequips an item in the item's specified slot and equips a default item in its place.
	/// This has to be InventoryItem rather than Equipment, because consumables can be equipped as well.
	/// 
	/// return value: void
	/// 
	/// parameters:
	/// pr_EquipmentToUnequip	: the equipment that should be unequipped.
	////////////////////////////////////////////////
	public void Unequip( InventoryItem _ItemToUnequip, bool _RemovingItemFromMenu = false )
	{
		if ( _ItemToUnequip == null )
			return;

		// TODO:: Add check to see if inventory is full before unequipping an item

		if ( _ItemToUnequip.m_ItemType == ITEMTYPE.ITEMTYPE_EQUIPMENT )
		{
			m_SelectedEquipmentSlot.RemoveItemFromSlot();
			m_SelectedEquipmentSlot.HideItemInfo();

			Equipment EquipmentToUnequip = (Equipment)_ItemToUnequip;
			m_SelectedEquipmentSlot.AddItemToSlot( m_DefaultEquipment[ EquipmentToUnequip.EquipmentSlots ] );
		}
		else // Consumables can be unequipped after using them from the EquipmentWheel, so some extra steps are necessary to make sure the correct slot is emptied
		{
			if ( _RemovingItemFromMenu ) // If we're removing or swapping an item from the menu, then the swap won't work if we cycle through the items and are trying to move an item from an earlier slot; for example from slot 1 to 3. That's because the item from slot 3 has already been put into slot 1, so trying to unequip that item from slot 3 would unequip the one from slot 1 instead of we cycled through them.
			{
				m_SelectedEquipmentSlot.RemoveItemFromSlot();
				m_SelectedEquipmentSlot.AddItemToSlot( m_DefaultEquipment[ EquipmentSlot.EQUIPMENTSLOT_Consumable ] );
				m_EquipWheel.UpdateWheel();
				return;
			}


			ItemSlot[] ConsumableSlots = m_ConsumableSlotsParent.GetComponentsInChildren<ItemSlot>();

			foreach ( ItemSlot CurrentItemslot in ConsumableSlots )
			{
				if ( CurrentItemslot.Item == _ItemToUnequip )
				{
					CurrentItemslot.RemoveItemFromSlot();
					CurrentItemslot.AddItemToSlot( m_DefaultEquipment[ EquipmentSlot.EQUIPMENTSLOT_Consumable ] );
					m_EquipWheel.UpdateWheel();
					break;
				}
			}
		}
	}



	////////////////////////////////////////////////
	/// Function information - IsItemEquipped
	/// 
	/// Checks whether or not an item is equipped
	/// 
	/// return value: null if item isn't equipped, the item's ItemSlot if it is equipped
	/// 
	/// parameters:
	/// pr_ItemToCheck	: the item to check
	////////////////////////////////////////////////
	public ItemSlot IsItemEquipped( InventoryItem _ItemToCheck )
	{
		foreach ( ItemSlot CurrentEquipSlot in m_EquipmentSlots )
		{
			if ( CurrentEquipSlot.Item == _ItemToCheck )
				return CurrentEquipSlot;
		}

		return null;
	}



	public void SelectEquipmentSlot( ItemSlot _SlotToSelect )
	{
		m_SelectedEquipmentSlot = _SlotToSelect;
		// Call UI manager to show new slot category of items
	}



	public void UnselectEquipmentSlot() // Set the selected equipmentslot as the first one.
	{
		m_SelectedEquipmentSlot = m_EquipmentSlots[0];
	}
}
