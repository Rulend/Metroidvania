using UnityEngine;

[ RequireComponent( typeof(ItemSlot) ) ]
public class Equipment_Category_Component : MonoBehaviour
{
	public EquipmentSlot m_EquipmentCategory;

	public void SetSlotAsSelected()
	{
		EquipmentManager.Instance.SelectEquipmentSlot( gameObject.GetComponent<ItemSlot>(), m_EquipmentCategory );
	}
}
