using UnityEngine;

// This class works like a mirror of InventoryItem. The only difference is that this class can be serialized by a JSON, and has no methods.

public class SerializableItem  // TODO:: Rename this into SerializableEquipment. Then make a class like this for every item class.
{
	// Allow casting of SerializableItem to InventoryItem
	public static explicit operator Equipment( SerializableItem pr_SerializedItem ) 
	{
		Equipment NewEquipment = ScriptableObject.CreateInstance<Equipment>();

		NewEquipment.m_ItemName			= pr_SerializedItem.m_ItemName;
		NewEquipment.m_Icon				= pr_SerializedItem.m_Icon;
		NewEquipment.m_DefaultItem		= pr_SerializedItem.m_DefaultItem;
		NewEquipment.m_ItemType			= pr_SerializedItem.m_ItemType;
		NewEquipment.m_Equipmentslots	= pr_SerializedItem.m_Equipmentslots;
		NewEquipment.m_Mesh				= pr_SerializedItem.m_Mesh;
		NewEquipment.m_ArmorModifier	= pr_SerializedItem.m_ArmorModifier;
		NewEquipment.m_DamageModifier	= pr_SerializedItem.m_DamageModifier;
		NewEquipment.m_RequiredStats	= pr_SerializedItem.m_RequiredStats;

		NewEquipment.name = pr_SerializedItem.m_ItemName;

		return NewEquipment;
	}

	// Allow casting of InventoryItem to SerializableItem
	public static explicit operator SerializableItem( InventoryItem pr_InventoryItem )
	{
		SerializableItem NewItem = new SerializableItem();

		NewItem.m_ItemName		= pr_InventoryItem.m_ItemName;
		NewItem.m_Icon			= pr_InventoryItem.m_Icon;
		NewItem.m_DefaultItem	= pr_InventoryItem.m_DefaultItem;
		NewItem.m_ItemType		= pr_InventoryItem.m_ItemType;

		return NewItem;
	}


	//public static bool operator !( InventoryItem item ) => item == null ? false : true;
	//public static implicit operator bool( InventoryItem item ) => item == null ? false : true;

	public string	m_ItemName		= "New Item";	// The name for the item.
	public Sprite	m_Icon			= null;			// The icon for the item that will show up in the Inventory.
	public bool		m_DefaultItem	= false;		// Whether or not the item is a default item. Default items cannot be added to the inventory, and will be equipped when everything else is unequipped.
//	public int		m_ItemValue		= 0;			// The rarity or "value" of the item. Can be used to prompt a "Are you sure you want to use that"-check.
	public InventoryItem.ITEMTYPE	m_ItemType		= 0;            // The type of the item. Used to sort items in inventory, and also to decide which item slot submenu to use when left clicking the item.

	public EquipmentSlot m_Equipmentslots;    // The equipment slots that this weapon can be placed in.
	public	SkinnedMeshRenderer m_Mesh; // The equipment's mesh - how it looks in the world.
	public int m_ArmorModifier;
	public int m_DamageModifier;
	public int m_RequiredStats;
}
