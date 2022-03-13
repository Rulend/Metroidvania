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


	[SerializeField] private EquipmentSlot		m_Equipmentslots;	// The equipment slots that this weapon can be placed in.

	[SerializeField] private int					m_ArmorModifier;
	[SerializeField] private int					m_DamageModifier;
	[SerializeField] private int					m_RequiredStats;


	public override void Use()
	{
		base.Use();

		// Try to equip item and check whether or not it was possible.
		if ( EquipmentManager.Instance.Equip( this ) )
		{
			// Remove item from inventory since we equipped it.
			GameManager.Instance.rPlayer1.GetInventory.RemoveItem( this, false );
		}

	}


	private void Awake()
	{
		m_ItemType = ITEMTYPE.ITEMTYPE_EQUIPMENT;
	}

}

