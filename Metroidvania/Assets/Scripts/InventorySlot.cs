using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
	private InventoryItem					m_Item;		// The item stored in the slot.

	[ SerializeField ] private Image		m_Icon;		// The icon of the item.



	public Button m_EquipButton;
	public Button m_DiscardButton;

	


	// Add item to slot, set image to that item's icon, then enable the image component.
	public void AddItemToSlot( InventoryItem pr_ItemToAdd )
	{
		m_Item = pr_ItemToAdd;

		m_Icon.sprite = m_Item.m_Icon;
		m_Icon.enabled = true;
	}

	// Remove item from slot, set image to null, and disable the image component.
	public void RemoveItemFromSlot()
	{
		m_Item = null;

		m_Icon.sprite = null;
		m_Icon.enabled = false;
	}


	// Brings up the submenu for an item.
	public void ShowItemSlotOptions()
	{
		if ( m_Item )	// Check item type to decide which menu to bring up.
		{
			// Bring up a different menu based on what kind of item it is.	// TODO: Fix this ASAP. 31/01/2022
			switch ( m_Item.m_ItemType )
			{
				case InventoryItem.ITEMTYPE.ITEMTYPE_MISC:
					GameManager.Instance.Player1.GetInventoryUI.GetComponent<InventoryUI>().SlotMenuMisc.SetActive( true );
					Debug.Log( "Left-clicking an item of type MISC. " );
					break;
				case InventoryItem.ITEMTYPE.ITEMTYPE_CONSUMABLE:
					Debug.Log( "Left-clicking an item of type CONSUMABLE. " );
					break;
				case InventoryItem.ITEMTYPE.ITEMTYPE_EQUIPMENT:
					Debug.Log( "Left-clicking an item of type EQUIPMENT. " );
					break;
				case InventoryItem.ITEMTYPE.ITEMTYPE_QUEST:
					Debug.Log( "Left-clicking an item of type QUEST. " );
					break;
				default:
					Debug.Log( "Unspecified what kind of menu should appear when left-clicking an item of type " + m_Item.m_ItemType.ToString() );
					break;
			}



		}

		m_EquipButton.interactable = true;
		m_DiscardButton.interactable = true;
	}


	// Removes the item from your inventory. TODO:: Removing an item from your inventory should either destroy it or leave it on the ground. Decide which one to go with, or make a toggle to switch between them.
	public void OnRemoveButton()
	{
		GameManager.Instance.Player1.GetInventory.RemoveItem( m_Item );
	}


	// "Uses" the item. For a consumable, this will consume it. For an equipment, this will equip it, etc.
	public void OnUseItemButton()
	{
		if ( m_Item )
		{
			m_Item.Use();
		}
	}
}
