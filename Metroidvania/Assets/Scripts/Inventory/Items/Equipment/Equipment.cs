using UnityEngine;

public enum EquipmentSlot
{
	EQUIPMENTSLOT_HEAD = 0	,
	EQUIPMENTSLOT_CHEST		,
	EQUIPMENTSLOT_LHAND		,
	EQUIPMENTSLOT_RHAND		,
	EQUIPMENTSLOT_LEGS		,
	EQUIPMENTSLOT_FEET		,
}


[CreateAssetMenu(fileName = "New Equipment", menuName = "Inventory/Equipment" )]
public class Equipment : InventoryItem
{
	[SerializeField] private EquipmentSlot[]		m_Equipmentslots;

	[SerializeField] private int					m_ArmorModifier;
	[SerializeField] private int					m_DamageModifier;


	private void Awake()
	{
		m_ItemType = ITEMTYPE.ITEMTYPE_EQUIPMENT;
	}

}

