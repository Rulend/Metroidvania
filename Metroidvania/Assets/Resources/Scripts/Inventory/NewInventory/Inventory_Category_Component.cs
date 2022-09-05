using UnityEngine;
using UnityEngine.EventSystems;

public class Inventory_Category_Component : MonoBehaviour, ISelectHandler
{

	public ITEMTYPE m_InventoryCategory;

	// This method has 0 references because it is called from a UI button. 
	// This script sits on all the ItemType buttons in the Inventory-screen.
	// To show the Inventory-screen: press Pause, then Inventory.
	public void ShowCategory()
	{
		UI_Manager.Instance.rInventoryUI.ShowInventoryCategory( m_InventoryCategory );
	}


	public void OnSelect( BaseEventData _Data )
	{
		//EquipmentManager.Instance.SelectEquipmentSlot( gameObject.GetComponent<ItemSlot>() );
	}
}
