using UnityEngine;
using UnityEngine.EventSystems;

public class Inventory_Category_Component : MonoBehaviour, ISelectHandler
{
	public ITEMTYPE m_InventoryCategory;

	public void ShowCategory()
	{
		UI_Manager.Instance.rInventoryUI.ShowInventoryCategory( m_InventoryCategory );
	}


	public void OnSelect( BaseEventData _Data )
	{
		//EquipmentManager.Instance.SelectEquipmentSlot( gameObject.GetComponent<ItemSlot>() );
	}
}
