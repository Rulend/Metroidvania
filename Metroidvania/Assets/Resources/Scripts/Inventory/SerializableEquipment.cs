using UnityEngine;

[System.Serializable]
public class SerializableEquipmentArray
{
	public SerializableEquipmentArray( int pr_ArraySize )
	{
		ItemArray = new SerializableEquipment[ pr_ArraySize ];
	}

	public SerializableEquipment[] ItemArray;
}

// This class works like a mirror of InventoryItem. The only difference is that this class can be serialized by a JSON, and has no methods.
[System.Serializable]
public class SerializableEquipment  // TODO:: Rename this into SerializableEquipment. Then make a class like this for every item class.
{
	// Allow casting of SerializableItem to InventoryItem
	public static explicit operator Equipment( SerializableEquipment pr_SerializedItem ) 
	{
		Equipment NewEquipment = ScriptableObject.CreateInstance<Equipment>();

		NewEquipment.m_ItemName			= pr_SerializedItem.m_ItemName;
		NewEquipment.m_ItemDescription	= pr_SerializedItem.m_ItemDescription;
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
	public static explicit operator SerializableEquipment( Equipment pr_InventoryItem )
	{
		SerializableEquipment	NewItem			= new SerializableEquipment();
		Equipment			ProvidedItem	= (Equipment)pr_InventoryItem;

		NewItem.m_ItemName			=	ProvidedItem.m_ItemName;
		NewItem.m_ItemDescription	=	ProvidedItem.m_ItemDescription;
		NewItem.m_Icon				=	ProvidedItem.m_Icon;
		NewItem.m_DefaultItem		=	ProvidedItem.m_DefaultItem;
		NewItem.m_ItemType			=	ProvidedItem.m_ItemType;

		NewItem.m_Equipmentslots	= ProvidedItem.m_Equipmentslots;
		NewItem.m_Mesh				= ProvidedItem.m_Mesh;
		NewItem.m_ArmorModifier		= ProvidedItem.m_ArmorModifier;
		NewItem.m_DamageModifier	= ProvidedItem.m_DamageModifier;
		NewItem.m_RequiredStats		= ProvidedItem.m_RequiredStats;

		return NewItem;
	}


	//public static bool operator !( InventoryItem item ) => item == null ? false : true;
	//public static implicit operator bool( InventoryItem item ) => item == null ? false : true;

	public string	m_ItemName				= "New Item";			// The name for the item.
	public string	m_ItemDescription		= "New Description";	// The description for the item.
	public Sprite	m_Icon					= null;					// The icon for the item that will show up in the Inventory.
	public bool		m_DefaultItem			= false;				// Whether or not the item is a default item. Default items cannot be added to the inventory, and will be equipped when everything else is unequipped.
//	public int		m_ItemValue				= 0;					// The rarity or "value" of the item. Can be used to prompt a "Are you sure you want to use that"-check.
	public ITEMTYPE	m_ItemType				= 0;	// The type of the item. Used to sort items in inventory, and also to decide which item slot submenu to use when left clicking the item.

	public EquipmentSlot m_Equipmentslots;    // The equipment slots that this weapon can be placed in.
	public SkinnedMeshRenderer m_Mesh; // The equipment's mesh - how it looks in the world.
	public int m_ArmorModifier;
	public int m_DamageModifier;
	public int m_RequiredStats;
}
