using UnityEngine;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour
{
	public InventoryItem	Item => m_Item;         // Getter for the item
	public int				Amount => m_Amount;		//Getter for amount of items

	private InventoryItem	m_Item;					// The item stored in the slot.
	private int				m_Amount;				// How many of the item there are

	private Image			m_Icon;					// The icon of the item when in a slot.
	private Text			m_AmountText;			// The text showing how many of the item there are.

	private InventoryUI		m_rInventoryUI;			// A reference to the InventoryUI


	private void Awake()
	{
		m_Icon					= transform.GetChild( 0 ).GetComponent<Image>();
		m_AmountText			= transform.GetChild( 1 ).GetComponent<Text>();
		m_AmountText.enabled	= false;
	}


	void Start()
	{
		// TODO: Instead of doing player 1, do it the correct way (which includes being able to handle multiple players and not showing each others inventory).

		m_rInventoryUI = UI_Manager.Instance.rInventoryUI;

		// TODO: Add so that the slot menu options buttons get assigned their functions automatically if it's not filled in.
	}




	////////////////////////////////////////////////
	/// Method Information - AddItemToSlot
	/// 
	/// Adds an item to a an itemslot.
	/// 
	/// return value: void
	/// 
	/// parameters:
	/// InventoryItem pr_ItemToAdd	: the item to be added in the slot.
	/// 
	////////////////////////////////////////////////
	public void AddItemToSlot( InventoryItem _ItemToAdd, int _Amount = 1 )
	{
		m_Item = _ItemToAdd;

		m_Icon.sprite	= m_Item.m_Icon;
		m_Icon.enabled	= true;

		m_Amount = _Amount;

		if ( m_Amount > 1 )
		{
			m_AmountText.text		= $"{m_Amount}";
			m_AmountText.enabled	= true;
		}
		else
			m_AmountText.enabled = false;
	}


	////////////////////////////////////////////////
	/// Method Information - RemoveItemFromSlot
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
		m_Item					= null; // TODO: Maybe use destroy in order to destroy it? Or add it to an unload queue.
		m_Icon.sprite			= null;
		m_Icon.enabled			= false;
		m_AmountText.enabled	= false;
		m_AmountText.text		= "0";
		HideItemSlotOptions();
	}


	// Functions to do regarding submenus and other UI-related stuff:


	////////////////////////////////////////////////
	/// Method Information - DisplayItemInfo
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
		{
			HideItemInfo();
			return;
		}

		m_rInventoryUI.ItemInfoPanel.ShowPanel( m_Item );
	}

	public void HideItemInfo()
	{
		m_rInventoryUI.ItemInfoPanel.HidePanel();
	}



	////////////////////////////////////////////////
	/// Method Information - ButtonShowItemSlotOptions
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
				case ITEMTYPE.ITEMTYPE_MISC:
					m_rInventoryUI.SlotMenuCurrent = m_rInventoryUI.SlotMenuMisc;
					break;
				case ITEMTYPE.ITEMTYPE_CONSUMABLE:
					m_rInventoryUI.SlotMenuCurrent = m_rInventoryUI.SlotMenuConsumable;
					break;
				case ITEMTYPE.ITEMTYPE_EQUIPMENT:
					m_rInventoryUI.SlotMenuCurrent = m_rInventoryUI.SlotMenuEquippable;
					break;
				case ITEMTYPE.ITEMTYPE_QUEST:
					m_rInventoryUI.SlotMenuCurrent = m_rInventoryUI.SlotMenuQuest;
					break;
				default:
					Debug.Log( "Unspecified what kind of menu should appear when left-clicking an item of type " + m_Item.m_ItemType.ToString() );
					break;
			}

			PositionUIPanelNextToSlot( m_rInventoryUI.SlotMenuCurrent );

			m_rInventoryUI.SlotMenuCurrent.SetActive( true );

		}
		else
			HideItemSlotOptions();
	}



	////////////////////////////////////////////////
	/// Method Information - HideItemSlotOptions
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

			//Debug.Log( "Hiding the current inventory slot options... (-w- )" );
		}
	}



	////////////////////////////////////////////////
	/// Method Information - PositionUIPanelNextToSlot
	/// 
	/// Desc:	Currently only used to position the ItemInfo-panel,
	///			this method is a relic from when the game was more 
	///			mouse oriented.
	/// 
	/// return value: void
	/// 
	/// parameters:
	///			_PanelToPosition - the panel that should be positioned 
	///			next to the slot, inside the screen.
	////////////////////////////////////////////////
	private void PositionUIPanelNextToSlot( GameObject _PanelToPosition )
	{	
		RectTransform PanelRectTransform = _PanelToPosition.GetComponent<RectTransform>();

		float PanelWidth	= PanelRectTransform.rect.width	 * PanelRectTransform.localScale.x;
		float Panelheight	= PanelRectTransform.rect.height * PanelRectTransform.localScale.y;

		float NewPanelPosX = gameObject.transform.position.x + ( PanelWidth / 2.0f ) + ( gameObject.GetComponent<RectTransform>().rect.width / 2.0f ) + 5.0f;
		float NewPanelPosY = gameObject.transform.position.y - ( Panelheight * 0.75f );

		if ( NewPanelPosX + ( PanelWidth * 0.25f ) > Screen.width ) // If the panel would be put outside the screen on the right by more than 25%
		{
			NewPanelPosX = gameObject.transform.position.x - ( PanelWidth / 2.0f ) - ( gameObject.GetComponent<RectTransform>().rect.width / 2.0f ) - 5.0f; // Put _PanelToPosition to the left of this slot, far enough away to not overlap + 5.0f.
			Debug.Log( $"Had to move xposition of {_PanelToPosition.name}, it would have been outside the screen on the right." );
		}

		if ( NewPanelPosY + m_rInventoryUI.SlotMenuCurrent.GetComponent<RectTransform>().rect.height > Screen.height ) // If panel would go below the screen, position it upwards.
		{
			NewPanelPosY += 100.0f; // TODO: Make this adjustable.
			Debug.Log( $"Had to move yposition of {_PanelToPosition.name}, it would have been below the screen." );
		}

		Vector3 NewSlotMenuPos = new Vector3( NewPanelPosX, NewPanelPosY, 0.0f );

		_PanelToPosition.transform.position = NewSlotMenuPos;
	}


	////////////////////////////////////////////////
	/// Method Information - ButtonUseItem
	/// 
	/// Desc:	Uses the item. For a consumable, this will consume it. 
	///			For an equipment, this will equip it, etc.
	/// 
	/// return value: void
	/// 
	/// parameters:
	/// N/A
	////////////////////////////////////////////////
	// "Uses" the item. For a consumable, this will consume it. For an equipment, this will equip it, etc.
	public void ButtonUseItem()
	{
		if ( m_Item )
		{
			HideItemSlotOptions();

			if ( m_Item.m_DefaultItem )
				return;

			m_Item.Use( m_Amount, GameManager.Instance.rPlayer1 ); // TODO URGENT:: Fix this monstrosity, not a good way to get the player
		}
	}
}
