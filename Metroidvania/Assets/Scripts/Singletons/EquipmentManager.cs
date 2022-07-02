using UnityEngine;
using System.IO;

public class EquipmentManager : MonoBehaviour
{
	public static EquipmentManager			Instance => m_Instance; // Public getter used to access singleton from outside


	private static EquipmentManager			m_Instance;
	public GameObject						m_EquipmentSlotsParent;		// only used once, should not be a member TODO:: Remove
	public ItemSlot[]						m_EquipmentSlots;		// An array of references to all of the inventory slots inside the "EquipmentPanel"-object in the scene. Used to store current equipment.
	public SkinnedMeshRenderer[]			m_EquipmentMeshes;		// An array of the current equipment's meshes

	[SerializeField]private Equipment[]		m_DefaultEquipment;	// SerializeFielded because I don't know how to create them from script.


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
	}

	private void Start()
	{
		m_EquipmentSlots = m_EquipmentSlotsParent.GetComponentsInChildren<ItemSlot>();

		// How to write to JSON
		SerializableEquipmentArray TestArray = new SerializableEquipmentArray( m_DefaultEquipment.Length );

		for ( int i = 0; i < m_DefaultEquipment.Length; ++i )
		{
			TestArray.ItemArray[ i ] = (SerializableEquipment)m_DefaultEquipment[ i ];
		}

		string JsonString = JsonUtility.ToJson( TestArray );

		File.WriteAllText( Application.dataPath + "/Resources/Items(Json)/DefaultItems.json", JsonString );

		// How to read from JSON
		string DefaultItemsFilePath = Application.dataPath + "/Resources/Items(Json)/DefaultItems.json";

		SerializableEquipmentArray ArrayFromJson = JsonUtility.FromJson<SerializableEquipmentArray>( File.ReadAllText( DefaultItemsFilePath ) ); // Create equipment from Json-object

		foreach ( SerializableEquipment CurrentSerializedItem in ArrayFromJson.ItemArray )
		{
			Equipment CurrentEquip = (Equipment)CurrentSerializedItem;
			Equip( CurrentEquip );
		}
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
	public void Equip( Equipment pr_NewEquipment )
	{
		if ( pr_NewEquipment == null )
			return;

		int				NewEquipmentSlots	= (int)pr_NewEquipment.EquipmentSlots;
		Equipment		CurrentEquipment	= (Equipment)m_EquipmentSlots[ NewEquipmentSlots ].Item;

		// TODO:: Add level or stat requirements here or somewhere else to check whether the character can actually equip the item.

		// If the item we're trying to equip is not the same as the equipped item, try to equip it.
		if ( pr_NewEquipment != CurrentEquipment )
		{
			GameManager.Instance.rPlayer1.GetInventory.RemoveItem( pr_NewEquipment, false ); // Remove the item that's about to be equipped from the inventory

			Unequip( CurrentEquipment ); // Unequip old item

			m_EquipmentSlots[ NewEquipmentSlots ].AddItemToSlot( pr_NewEquipment ); // Equip new item
		}
		else	// If equip was called while item was already equipped, unequip it
		{
			Unequip( CurrentEquipment );
		}
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
	public void Unequip( Equipment pr_EquipmentToUnequip )
	{
		if ( pr_EquipmentToUnequip == null )
			return;

		// TODO:: Add check to see if inventory is full before unequipping an item

		int EquipmentSlot = (int)pr_EquipmentToUnequip.EquipmentSlots;

		m_EquipmentSlots[ EquipmentSlot ].RemoveItemFromSlot( false );
		m_EquipmentSlots[ EquipmentSlot ].AddItemToSlot( m_DefaultEquipment[ EquipmentSlot ] );

		GameManager.Instance.rPlayer1.GetInventory.AddItem( pr_EquipmentToUnequip );
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
}
