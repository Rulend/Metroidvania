using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
	private InventoryItem					m_Item;		// The item stored in the slot.

	[ SerializeField ] private Image		m_Icon;     // The icon of the item.


	InventoryUI m_rInventoryUI;

	void Start()
	{
		// TODO: Instead of doing player 1, do it the correct way (which includes being able to handle multiple players and not showing each others inventory).
		if ( !m_rInventoryUI )
			m_rInventoryUI = GameManager.Instance.Player1.InventoryUI.GetComponent<InventoryUI>();
	}




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
			if ( m_rInventoryUI.SlotMenuMisc.activeSelf )
				m_rInventoryUI.SlotMenuMisc.SetActive( false );

			// Bring up a different menu based on what kind of item it is.	// TODO: Another way to do this, is to assign a different submenu to a slot
			// based on what kind of item is in it. The submenu could be assignd when adding/removing an item from that slot. For now though, this works.
			switch ( m_Item.m_ItemType )
			{
				case InventoryItem.ITEMTYPE.ITEMTYPE_MISC:
					m_rInventoryUI.SlotMenuCurrent = m_rInventoryUI.SlotMenuMisc;
					Debug.Log( "Left-clicking an item of type MISC. " );
					break;
				case InventoryItem.ITEMTYPE.ITEMTYPE_CONSUMABLE:
					m_rInventoryUI.SlotMenuCurrent = m_rInventoryUI.SlotMenuConsumable;
					Debug.Log( "Left-clicking an item of type CONSUMABLE. " );
					break;
				case InventoryItem.ITEMTYPE.ITEMTYPE_EQUIPMENT:
					m_rInventoryUI.SlotMenuCurrent = m_rInventoryUI.SlotMenuEquippable;
					Debug.Log( "Left-clicking an item of type EQUIPMENT. " );
					break;
				case InventoryItem.ITEMTYPE.ITEMTYPE_QUEST:
					m_rInventoryUI.SlotMenuCurrent = m_rInventoryUI.SlotMenuQuest;
					Debug.Log( "Left-clicking an item of type QUEST. " );
					break;
				default:
					Debug.Log( "Unspecified what kind of menu should appear when left-clicking an item of type " + m_Item.m_ItemType.ToString() );
					break;
			}

			Debug.Log( "SlotMenuCurrent: " + m_rInventoryUI.SlotMenuCurrent );
			m_rInventoryUI.SlotMenuCurrent.SetActive( true );
			//m_rInventoryUI.SlotMenuCurrent.SetActive( !m_rInventoryUI.SlotMenuCurrent.activeSelf );

		}
	}


	// Button-functions to be called from the UI


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
