using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
	private InventoryItem					m_Item;         // The item stored in the slot.
	[SerializeField] private GameObject		m_ItemPickupPrefab; // Used for instantiating items on the ground when dropping them

	public InventoryItem					Item => m_Item;

	[ SerializeField ] private Image		m_Icon;     // The icon of the item.


	InventoryUI								m_rInventoryUI;	// A reference to the InventoryUI

	void Start()
	{
		// TODO: Instead of doing player 1, do it the correct way (which includes being able to handle multiple players and not showing each others inventory).
		if ( !m_rInventoryUI )
			m_rInventoryUI = GameManager.Instance.rPlayer1.InventoryUI.GetComponent<InventoryUI>();

		// TODO: Add so that the slot menu options buttons get assigned their functions automatically if it's not filled in.
	}




	// Add item to slot, set image to that item's icon, then enable the image component.
	public void AddItemToSlot( InventoryItem pr_ItemToAdd )
	{
		m_Item = pr_ItemToAdd;

		m_Icon.sprite = m_Item.m_Icon;
		m_Icon.enabled = true;


	}

	// Remove item from slot, set image to null, and disable the image component.
	public void RemoveItemFromSlot( bool pr_SpawnItemPickup )
	{
		if ( pr_SpawnItemPickup )
		{
			m_ItemPickupPrefab.GetComponent<ItemPickup>().m_ItemToGive = m_Item;
			Instantiate( m_ItemPickupPrefab, GameManager.Instance.rPlayer1.transform.position, Quaternion.identity );
		}

		m_Item = null; // TODO: Maybe use destroy in order to destroy it? Or add it to an unload queue.

		m_Icon.sprite	 = null;
		m_Icon.enabled	 = false;
		HideItemSlotOptions();
	}


	// Brings up the submenu for an item.
	public void ButtonShowItemSlotOptions()
	{
		if ( m_Item )	// Check item type to decide which menu to bring up.
		{
			HideItemSlotOptions();

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

			m_rInventoryUI.m_CurrentSlot = this;
			m_rInventoryUI.m_CurrentSlotBorder.transform.position = gameObject.transform.position;
			m_rInventoryUI.m_CurrentSlotBorder.SetActive( true );

			PositionSlotMenuCurrent();

			m_rInventoryUI.SlotMenuCurrent.SetActive( true );

		}
		else
		{
			HideItemSlotOptions();
		}
	}

	public void HideItemSlotOptions()
	{
		if ( m_rInventoryUI.SlotMenuCurrent.activeSelf )
		{
			m_rInventoryUI.SlotMenuCurrent.SetActive( false );
			m_rInventoryUI.m_CurrentSlotBorder.SetActive( false );

			Debug.Log( "Hiding the current inventory slot options... (-w- )" );

		}
	}


	private void PositionSlotMenuCurrent()
	{
		RectTransform SlotMenuCurrentRectTransform = m_rInventoryUI.SlotMenuCurrent.GetComponent<RectTransform>();

		float SlotMenuWidth		= SlotMenuCurrentRectTransform.rect.width	* SlotMenuCurrentRectTransform.localScale.x;
		float SlotMenuHeight	= SlotMenuCurrentRectTransform.rect.height	* SlotMenuCurrentRectTransform.localScale.y;

		float NewSlotMenuXPos	= gameObject.transform.position.x + ( SlotMenuWidth / 2.0f ) + ( gameObject.GetComponent<RectTransform>().rect.width / 2.0f ) + 5.0f;
		float NewSlotMenuYPos	= gameObject.transform.position.y - ( SlotMenuHeight * 0.75f );

		if ( NewSlotMenuXPos + ( SlotMenuWidth * 0.25f ) > Screen.width )
		{
			NewSlotMenuXPos = gameObject.transform.position.x - ( SlotMenuWidth / 2.0f ) - ( gameObject.GetComponent<RectTransform>().rect.width / 2.0f ) - 5.0f;
			Debug.Log( "Had to move xposition of slotmenu options, it would have been outside the screen on the right." );
		}

		if ( NewSlotMenuYPos + m_rInventoryUI.SlotMenuCurrent.GetComponent<RectTransform>().rect.height > Screen.height )
		{
			NewSlotMenuYPos += 100.0f; // TODO: Don't let this be static.
			Debug.Log( "Had to move yposition of slotmenu options, it would have been below the screen." );
		}

		Vector3 NewSlotMenuPos = new Vector3( NewSlotMenuXPos, NewSlotMenuYPos, 0.0f );

		m_rInventoryUI.SlotMenuCurrent.transform.position = NewSlotMenuPos;
	}


	// Button-functions to be called from the UI


	// Removes the item from your inventory. TODO:: Removing an item from your inventory should either destroy it or leave it on the ground. Decide which one to go with, or make a toggle to switch between them.
	public void ButtonRemoveItem()
	{
		RemoveItemFromSlot( true ); // Remove item from inventory, and since the Discard button was pressed, drop an item pickup on the ground.
	}


	// "Uses" the item. For a consumable, this will consume it. For an equipment, this will equip it, etc.
	public void ButtonUseItem()
	{
		if ( m_Item )
		{
			m_Item.Use();
			HideItemSlotOptions();
		}
	}
}
