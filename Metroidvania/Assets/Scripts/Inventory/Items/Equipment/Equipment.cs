using UnityEngine;

public enum EquipmentSlot
{
	EQUIPMENTSLOT_HEAD = 0	,
	EQUIPMENTSLOT_CHEST		,
	EQUIPMENTSLOT_LHAND		,
	EQUIPMENTSLOT_RHAND		,
	EQUIPMENTSLOT_LEGS		,
	EQUIPMENTSLOT_FEET		,

	EQUIPMENTSLOT_SIZE		// Always have this at the bottom to measure the amount of equipment slots.
}


[CreateAssetMenu(fileName = "New Equipment", menuName = "Inventory/Equipment" )]
public class Equipment : InventoryItem
{
	public EquipmentSlot EquipmentSlots => m_Equipmentslots;


	[SerializeField] private EquipmentSlot		m_Equipmentslots;	// The equipment slots that this weapon can be placed in.

	[SerializeField] private int					m_ArmorModifier;
	[SerializeField] private int					m_DamageModifier;
	[SerializeField] private int					m_RequiredStats;


	public override void Use()
	{
		base.Use();

		// Equip item.
		EquipmentManager.Instance.Equip( this );
		// Remove item from inventory.
		GameManager.Instance.rPlayer1.GetInventory.RemoveItem( this, false );

	}


	private void Awake()
	{
		m_ItemType = ITEMTYPE.ITEMTYPE_EQUIPMENT;
	}

}

