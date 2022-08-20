using UnityEngine;

public enum EquipmentSlot
{
	EQUIPMENTSLOT_WEAPON = 0,	// Weapon.
	EQUIPMENTSLOT_HEAD		,	// Armor for head.
	EQUIPMENTSLOT_CHEST		,	// Armor for chest.
	EQUIPMENTSLOT_GAUNTLETS	,	// Armor for hands.	
	EQUIPMENTSLOT_LEGS		,	// Armor for legs.
	EQUIPMENTSLOT_FEET		,	// Armor for feet.
	EQUIPMENTSLOT_Consumable,	// For the consumables that the player can put in their quick access bar.

	EQUIPMENTSLOT_SIZE		,	// Always have under all the equipslots to measure how many they are.
	EQUIPMENTSLOT_EQUIPPEDTAB	// This is used in the inventory UI to represent the Current-tab. Needed in order to not make stuff messy.
}


[CreateAssetMenu(fileName = "New Equipment", menuName = "Inventory/Equipment" )]
public class Equipment : InventoryItem
{
	[SerializeField] public EquipmentSlot EquipmentSlots => m_Equipmentslots;

	[SerializeField] public EquipmentSlot			m_Equipmentslots;	// The equipment slots that this weapon can be placed in.
	[SerializeField] public SkinnedMeshRenderer		m_Mesh;	// The equipment's mesh - how it looks in the world.
	


	[SerializeField] public int				m_ArmorModifier;
	[SerializeField] public int				m_DamageModifier;
	[SerializeField] public int				m_RequiredStats;


	public override void Use( int _Amount, Character _User )
	{
		base.Use();

		// Tries to equip this item. Removes it from inventory if it succeeds, also adds previous items back to inventory. Adds this item back to inventory if it was already equipped.
		EquipmentManager.Instance.Equip( this );
	}


	private void Awake()
	{
		m_ItemType	= ITEMTYPE.ITEMTYPE_EQUIPMENT;
		m_Stackable = false;
	}
}

