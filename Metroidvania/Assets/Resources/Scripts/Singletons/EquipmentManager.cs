using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class EquipmentManager : MonoBehaviour
{
	public static EquipmentManager			Instance => m_Instance; // Public getter used to access singleton from outside


	private static EquipmentManager			m_Instance;
	public GameObject						m_EquipmentSlotsParent;		// only used once, should not be a member TODO:: Remove
	public GameObject						m_WeaponSlotsParent;		// only used once, should not be a member TODO:: Remove
	public GameObject						m_ConsumableSlotsParent;	// only used once, should not be a member TODO:: Remove
	public ItemSlot[]						m_EquipmentSlots;		// An array of references to all of the inventory slots inside the "EquipmentPanel"-object in the scene. Used to store current equipment.
	public SkinnedMeshRenderer[]			m_EquipmentMeshes;      // An array of the current equipment's meshes

	[SerializeField] private InventoryItem[] m_DefaultSetupArray;
	private Dictionary<EquipmentSlot, InventoryItem> m_DefaultEquipment;

	private ItemSlot	m_SelectedEquipmentSlot;
	public  ItemSlot	SelectedEquipmentSlot => m_SelectedEquipmentSlot;
	private int			m_NumberWeaponSlots;

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
		m_NumberWeaponSlots = m_WeaponSlotsParent.GetComponentsInChildren<ItemSlot>().Length;


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
			Equip( (Equipment)m_DefaultSetupArray[ EquipIndex ], false );
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
	/// Equips an item in the item's specified slot.
	/// 
	/// return value: returns the previously equipped item.
	/// 
	/// parameters:
	/// pr_NewEquipment	: the equipment that should be equipped.
	////////////////////////////////////////////////
	public void Equip( Equipment _NewEquipment, bool _UpdateInventoryUI = true )
	{
		// TODO:: Remake this function. It doesn't work anymore becuase weapons are no longer tied to a specific hand. What this means is that: since there is no longer a LHAND/RHAND_Slot in the Equipslot-enum,
		// this will need to be slightly rworked. TODO:: Remove the right hand / left hand shit, I don't have time for it right now.

		if ( _NewEquipment == null )
			return;

		Equipment CurrentEquipment	= (Equipment)m_SelectedEquipmentSlot.Item;

		// TODO:: Add level or stat requirements here or somewhere else to check whether the character can actually equip the item.

		// If the item we're trying to equip is not the same as the equipped item, try to equip it.
		if ( _NewEquipment != CurrentEquipment )
		{
			Unequip( CurrentEquipment ); // Unequip old item


			m_SelectedEquipmentSlot.AddItemToSlot( _NewEquipment ); // Equip new item
		}

		if ( _UpdateInventoryUI )
			UI_Manager.Instance.rInventoryUI.UpdateEquippedIcon();

		// TODO:: Add stats re-calculation in here
	}


	////////////////////////////////////////////////
	/// Function information - Unequip
	/// 
	/// Unequips an item in the item's specified slot and equips default equipment in its place.
	/// 
	/// return value: void
	/// 
	/// parameters:
	/// pr_EquipmentToUnequip	: the equipment that should be unequipped.
	////////////////////////////////////////////////
	public void Unequip( Equipment _EquipmentToUnequip )
	{
		if ( _EquipmentToUnequip == null )
			return;

		// TODO:: Add check to see if inventory is full before unequipping an item

		m_SelectedEquipmentSlot.RemoveItemFromSlot();
		m_SelectedEquipmentSlot.AddItemToSlot( m_DefaultEquipment[ _EquipmentToUnequip.EquipmentSlots ] );
	}



	////////////////////////////////////////////////
	/// Function information - IsItemEquipped
	/// 
	/// Checks whether or not an item is equipped
	/// 
	/// return value: true if equipped, false if not.
	/// 
	/// parameters:
	/// pr_ItemToCheck	: the item to check
	////////////////////////////////////////////////
	public bool IsItemEquipped( InventoryItem pr_ItemToCheck )
	{
		if ( pr_ItemToCheck as Equipment == null )
			return false;

		foreach ( ItemSlot CurrentEquipSlot in m_EquipmentSlots )
		{
			if ( CurrentEquipSlot.Item == pr_ItemToCheck )
				return true;
		}

		return false;
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
