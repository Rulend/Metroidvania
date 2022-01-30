using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
	private InventoryItem	m_Item;		// The item stored in the slot.

	[ SerializeField ] private Image			m_Icon;     // The icon of the item.



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


	public void ShowItemSlotOptions()
	{
		m_EquipButton.interactable = true;
		m_DiscardButton.interactable = true;
	}


	public void OnRemoveButton()
	{
		GameManager.Instance.Player1.GetInventory.RemoveItem( m_Item );
	}

	public void OnUseItem()
	{
		if ( m_Item )
		{
			m_Item.Use();
		}
	}
}
