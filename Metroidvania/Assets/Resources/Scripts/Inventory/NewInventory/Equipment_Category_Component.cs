using UnityEngine;
using UnityEngine.EventSystems;

[ RequireComponent( typeof(ItemSlot) ) ]
public class Equipment_Category_Component : MonoBehaviour, ISelectHandler
{
	public EquipmentSlot m_EquipmentCategory;

	// This method has 0 references because it is called from a UI button. 
	// This script sits on all the EquipmentSlots in the Equipped-screen.
	// To show the equipped-screen: press Pause, then Equipment.
	public void ShowCategory()
	{
		UI_Manager.Instance.rInventoryUI.ShowEquipmentCategory( m_EquipmentCategory );
	}


	// This is called automatically when you use a controller and tilt the stick to get to this button.
	// Also called upon
	public void OnSelect( BaseEventData _Data )
	{
		EquipmentManager.Instance.SelectEquipmentSlot( gameObject.GetComponent<ItemSlot>() );
	}
}
