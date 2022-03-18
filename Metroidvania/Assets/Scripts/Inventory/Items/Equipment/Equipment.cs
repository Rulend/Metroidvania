using UnityEngine;

public enum EquipmentSlot
{
	EQUIPMENTSLOT_HEAD = 0	,	// Armor for head.
	EQUIPMENTSLOT_CHEST		,	// Armor for chest.
	EQUIPMENTSLOT_GAUNTLETS	,	// Armor for hands.	
	EQUIPMENTSLOT_LHAND		,	// Weapon for left hand.
	EQUIPMENTSLOT_RHAND		,	// Weapon for right hand.
	EQUIPMENTSLOT_LEGS		,	// Armor for legs.
	EQUIPMENTSLOT_FEET		,	// Armor for feet.

	EQUIPMENTSLOT_SIZE		// Always have this at the bottom to measure the amount of equipment slots.
}


[CreateAssetMenu(fileName = "New Equipment", menuName = "Inventory/Equipment" )]
public class Equipment : InventoryItem
{
	public EquipmentSlot EquipmentSlots => m_Equipmentslots;

	[SerializeField] private EquipmentSlot			m_Equipmentslots;	// The equipment slots that this weapon can be placed in.
	[SerializeField] public SkinnedMeshRenderer		m_Mesh;	// The equipment's mesh - how it looks in the world.
	


	[SerializeField] private int				m_ArmorModifier;
	[SerializeField] private int				m_DamageModifier;
	[SerializeField] private int				m_RequiredStats;


	public override void Use()
	{
		base.Use();

		// Tries to equip this item. Removes it from inventory if it succeeds, also adds previous items back to inventory. Adds this item back to inventory if it was already equipped.
		EquipmentManager.Instance.Equip( this );
	}


	private void Awake()
	{
		m_ItemType = ITEMTYPE.ITEMTYPE_EQUIPMENT;
	}

}

