using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button_EquipmentCategory : MonoBehaviour
{


	//[SerializeField] InventoryItem.ITEMTYPE m_ItemType;

	[SerializeField] EquipmentSlot m_EquipmentType;


	public void ShowItemsInCategory()
	{
		List<InventoryItem> ListOfItems = new List<InventoryItem>();


		switch ( m_EquipmentType )
		{
			case EquipmentSlot.EQUIPMENTSLOT_HEAD:			ListOfItems = GameManager.Instance.rPlayer1.GetInventory.m_HeadGear;	break;
			case EquipmentSlot.EQUIPMENTSLOT_CHEST:			ListOfItems = GameManager.Instance.rPlayer1.GetInventory.m_ChestGear;	break;
			case EquipmentSlot.EQUIPMENTSLOT_GAUNTLETS:		ListOfItems = GameManager.Instance.rPlayer1.GetInventory.m_HandGear;	break;
			case EquipmentSlot.EQUIPMENTSLOT_LEGS:			ListOfItems = GameManager.Instance.rPlayer1.GetInventory.m_LegGear;		break;
			case EquipmentSlot.EQUIPMENTSLOT_FEET:			ListOfItems = GameManager.Instance.rPlayer1.GetInventory.m_FeetGear;	break;
		}
	}
}
