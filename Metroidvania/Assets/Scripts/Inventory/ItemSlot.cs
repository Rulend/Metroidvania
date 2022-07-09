using UnityEngine;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour
{
	public InventoryItem					Item => m_Item;			// Getter for the item
	[SerializeField]private InventoryItem					m_Item;					// The item stored in the slot.

	private Image							m_Icon;					// The icon of the item when in a slot.

	private InventoryUI						m_rInventoryUI;				// A reference to the InventoryUI
	private Image							m_rItemHoverIcon;			// A reference to the item-icon part of the item slot hover panel
	private Text							m_rItemHoverName;			// A reference to the name part of the item slot hover panel
	private Text							m_rItemHoverDescription;	// A reference to the description part of the item slot hover panel

	void Start()
	{
		m_Icon = transform.GetChild( 0 ).GetComponent<Image>();

		// TODO: Instead of doing player 1, do it the correct way (which includes being able to handle multiple players and not showing each others inventory).

		m_rInventoryUI			= UI_Manager.Instance.rInventoryUI;

		m_rItemHoverIcon		= m_rInventoryUI.ItemInfoDisplay.transform.GetChild( 0 ).transform.GetChild( 0 ).GetComponent<Image>();

		m_rItemHoverName		= m_rInventoryUI.ItemInfoDisplay.transform.GetChild( 1 ).transform.GetChild( 0 ).transform.GetChild( 0 ).GetComponent<Text>();

		m_rItemHoverDescription = m_rInventoryUI.ItemInfoDisplay.transform.GetChild( 1 ).transform.GetChild( 1 ).transform.GetChild( 0 ).GetComponent<Text>();

		// TODO: Add so that the slot menu options buttons get assigned their functions automatically if it's not filled in.
	}




	////////////////////////////////////////////////
	/// Function information - AddItemToSlot
	/// 
	/// Adds an item to a an itemslot.
	/// 
	/// return value: void
	/// 
	/// parameters:
	/// InventoryItem pr_ItemToAdd	: the item to be added in the slot.
	/// 
	////////////////////////////////////////////////
	public void AddItemToSlot( InventoryItem pr_ItemToAdd )
	{
		m_Item = pr_ItemToAdd;

		m_Icon.sprite	= m_Item.m_Icon;
		m_Icon.enabled	= true;
	}


	////////////////////////////////////////////////
	/// Function information - RemoveItemFromSlot
	/// 
	/// Removes the item from the slot, sets the slot icon to null, and disables the icon.
	/// 
	/// return value: void
	/// 
	/// parameters:
	/// bool pr_SpawnItemPickup	: whether or not to spawn the item on the ground when dropping it.
	/// 
	////////////////////////////////////////////////
	public void RemoveItemFromSlot()
	{
		m_Item				= null; // TODO: Maybe use destroy in order to destroy it? Or add it to an unload queue.
		m_Icon.sprite		= null;
		m_Icon.enabled		= false;
		HideItemSlotOptions();
	}


	// Functions to do regarding submenus and other UI-related stuff:


	////////////////////////////////////////////////
	/// Function information - DisplayItemInfo
	/// 
	/// Displays information about an item while hovering over it with the mouse.
	/// 
	/// return value: void
	/// 
	/// parameters:
	/// N/A
	////////////////////////////////////////////////
	public void DisplayItemInfo()
	{
		if ( !m_Item || m_Item.m_DefaultItem )
			return;

		m_rItemHoverIcon.sprite		= m_Item.m_Icon; // TODO:: Save this monstrocisy of a way to do this.
		m_rItemHoverName.text		= m_Item.m_ItemName;

		if ( m_Item.m_ItemType == InventoryItem.ITEMTYPE.ITEMTYPE_EQUIPMENT )
		{
			Equipment EquipmentItem = (Equipment)m_Item;

			// TODO:: Display this in a more interesting way, add an icon or something. Also add some kind of info about the item itself to be displayed here. Also make the display box move (maybe).

			m_rItemHoverDescription.text = 
				$" Damage: {EquipmentItem.m_DamageModifier} \n " +
				$" Armor: {EquipmentItem.m_ArmorModifier} ";
		}
		else
		{
			m_rItemHoverDescription.text	= m_Item.m_ItemDescription;
		}

		//PositionUIPanelNextToSlot( m_rInventoryUI.ItemInfoDisplay );
		m_rInventoryUI.ItemInfoDisplay.SetActive( true );
	}

	public void HideItemInfo()
	{
		m_rInventoryUI.ItemInfoDisplay.SetActive( false );
	}



	////////////////////////////////////////////////
	/// Function information - ButtonShowItemSlotOptions
	/// 
	/// Shows a submenu for when left clicking on an item slot that contains an item. 
	///	Brings up a different submenu for different item-types.
	/// 
	/// return value: void
	/// 
	/// parameters:
	/// N/A
	////////////////////////////////////////////////
	public void ButtonShowItemSlotOptions()
	{
		if ( m_Item )	// Check item type to decide which menu to bring up.
		{
			HideItemSlotOptions();

			if ( m_Item.m_DefaultItem ) // Don't show any menu if left clicking a default item, since those can only be in the equipment slots if there's nothing else there.
				return;


			// Bring up a different menu based on what kind of item it is.	// TODO: Another way to do this, is to assign a different submenu to a slot
			// based on what kind of item is in it. The submenu could be assignd when adding/removing an item from that slot. For now though, this works.
			switch ( m_Item.m_ItemType )
			{
				case InventoryItem.ITEMTYPE.ITEMTYPE_MISC:
					m_rInventoryUI.SlotMenuCurrent = m_rInventoryUI.SlotMenuMisc;
					break;
				case InventoryItem.ITEMTYPE.ITEMTYPE_CONSUMABLE:
					m_rInventoryUI.SlotMenuCurrent = m_rInventoryUI.SlotMenuConsumable;
					break;
				case InventoryItem.ITEMTYPE.ITEMTYPE_EQUIPMENT:
					m_rInventoryUI.SlotMenuCurrent = m_rInventoryUI.SlotMenuEquippable;
					break;
				case InventoryItem.ITEMTYPE.ITEMTYPE_QUEST:
					m_rInventoryUI.SlotMenuCurrent = m_rInventoryUI.SlotMenuQuest;
					break;
				default:
					Debug.Log( "Unspecified what kind of menu should appear when left-clicking an item of type " + m_Item.m_ItemType.ToString() );
					break;
			}

			m_rInventoryUI.m_CurrentSlot							= this;
			m_rInventoryUI.m_CurrentSlotBorder.transform.position	= transform.position;
			m_rInventoryUI.m_CurrentSlotBorder.SetActive( true );

			PositionUIPanelNextToSlot( m_rInventoryUI.SlotMenuCurrent );

			m_rInventoryUI.SlotMenuCurrent.SetActive( true );

		}
		else
			HideItemSlotOptions();
	}

	////////////////////////////////////////////////
	/// Function information - HideItemSlotOptions
	/// 
	/// Hides the current ItemSlotsSubmenu.
	/// 
	/// return value: void
	/// 
	/// parameters:
	/// N/A
	////////////////////////////////////////////////
	public void HideItemSlotOptions()
	{
		if ( m_rInventoryUI.SlotMenuCurrent.activeSelf )
		{
			m_rInventoryUI.SlotMenuCurrent.SetActive( false );
			m_rInventoryUI.m_CurrentSlotBorder.SetActive( false );

			Debug.Log( "Hiding the current inventory slot options... (-w- )" );
		}
	}


	private void PositionUIPanelNextToSlot( GameObject pr_PanelToPosition )
	{	
		RectTransform PanelRectTransform = pr_PanelToPosition.GetComponent<RectTransform>();

		float PanelWidth	= PanelRectTransform.rect.width		* PanelRectTransform.localScale.x;
		float Panelheight	= PanelRectTransform.rect.height	* PanelRectTransform.localScale.y;

		float NewPanelPosX = gameObject.transform.position.x + ( PanelWidth / 2.0f ) + ( gameObject.GetComponent<RectTransform>().rect.width / 2.0f ) + 5.0f;
		float NewPanelPosY = gameObject.transform.position.y - ( Panelheight * 0.75f );

		if ( NewPanelPosX + ( PanelWidth * 0.25f ) > Screen.width ) // If the panel would be put outside the screen by more than 25%
		{
			NewPanelPosX = gameObject.transform.position.x - ( PanelWidth / 2.0f ) - ( gameObject.GetComponent<RectTransform>().rect.width / 2.0f ) - 5.0f;
			Debug.Log( $"Had to move xposition of {pr_PanelToPosition.name}, it would have been outside the screen on the right." );
		}

		if ( NewPanelPosY + m_rInventoryUI.SlotMenuCurrent.GetComponent<RectTransform>().rect.height > Screen.height )
		{
			NewPanelPosY += 100.0f; // TODO: Don't let this be static.
			Debug.Log( $"Had to move yposition of {pr_PanelToPosition.name}, it would have been below the screen." );
		}

		Vector3 NewSlotMenuPos = new Vector3( NewPanelPosX, NewPanelPosY, 0.0f );

		pr_PanelToPosition.transform.position = NewSlotMenuPos;
	}


	// Button-functions to be called from the UI


	// "Uses" the item. For a consumable, this will consume it. For an equipment, this will equip it, etc.
	public void ButtonUseItem()
	{
		if ( m_Item )
		{
			HideItemSlotOptions();

			if ( m_Item.m_DefaultItem )
				return;

			m_Item.Use();
		}
	}
}
