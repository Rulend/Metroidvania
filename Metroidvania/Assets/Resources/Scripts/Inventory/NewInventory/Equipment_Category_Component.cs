using UnityEngine;
using UnityEngine.EventSystems;

[ RequireComponent( typeof(ItemSlot) ) ]
public class Equipment_Category_Component : MonoBehaviour, ISelectHandler
{
	public EquipmentSlot m_EquipmentCategory;

	public void ShowCategory()
	{
		UI_Manager.Instance.rInventoryUI.ShowEquipmentCategory( m_EquipmentCategory );
	}


	public void OnSelect( BaseEventData _Data )
	{
		EquipmentManager.Instance.SelectEquipmentSlot( gameObject.GetComponent<ItemSlot>() );
	}
}
