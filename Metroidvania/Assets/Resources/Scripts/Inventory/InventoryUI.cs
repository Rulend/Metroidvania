using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
	private Inventory m_Inventory;

	//[ SerializeField ] private GameObject		m_InventoryUI;			// A reference to the "Inventory"-object inside the UI-canvas in the scene. // (Not needed since we do this inside Player.cs instead)

	//private InventorySlot[][]					m_InventorySlots;		// An array of references to all of the inventory slots inside the "InventoryPanel"-object in the scene.

	[ SerializeField ] private GameObject		m_DraggedItem;			// The object which follows the mouse and displays an icon when dragging an item.
	[ SerializeField ] private ItemInfoPanel	m_ItemInfoPanel;		// A gameobject used to display information about an item while hovering over it in the inventory.

	[ SerializeField ] private GameObject		m_SlotMenuCurrent;		//	The current slot-menu.
	[ SerializeField ] private GameObject		m_SlotMenuMisc;			//	The Slot-Menu for ITEMTYPE_MISC items. 
	[ SerializeField ] private GameObject		m_SlotMenuConsumable;	//	The Slot-Menu for ITEMTYPE_CONSUMABLE items. 
	[ SerializeField ] private GameObject		m_SlotMenuEquippable;	//	The Slot-Menu for ITEMTYPE_EQUIPPABLE items. 
	[ SerializeField ] private GameObject		m_SlotMenuQuest;		//	The Slot-Menu for ITEMTYPE_QUEST items. 

	private ItemSlot							m_SelectedInventorySlot;    // The slot was lastly left-clicked.
	[SerializeField] private GameObject			m_ItemPickedUpAlert;		// After picking up an item, this panel will be shown in order to let the player see what they've picked up.

	// TODO:: Clean this document up. So messy, and most of these menus aren't really needed. Only 1 is needed, and then you adjust it based on what you want it to do.

	public ItemInfoPanel ItemInfoPanel => m_ItemInfoPanel;
	public GameObject DraggedItem => m_DraggedItem;

	public GameObject SlotMenuCurrent	
	{
		get { return m_SlotMenuCurrent; } 
		set { if ( value != null ) m_SlotMenuCurrent = value; } 
	}
	public GameObject SlotMenuMisc			{ get { return m_SlotMenuMisc;			} }
	public GameObject SlotMenuConsumable	{ get { return m_SlotMenuConsumable;	} }
	public GameObject SlotMenuEquippable	{ get { return m_SlotMenuEquippable;	} }
	public GameObject SlotMenuQuest			{ get { return m_SlotMenuQuest;			} }



	public delegate void	UpdateDisplayedItemsHandler ( List<InventoryItem> _NewItems );
	public event			UpdateDisplayedItemsHandler UpdateDisplayedItemsEvent;
	[Space]
	[ SerializeField ] private GameObject m_DisplayedItemsParent;
	[ SerializeField ] private GameObject m_CurrentEquipmentWindow;
	[ SerializeField ] private GameObject m_EquippedIcon; // Red icon used to show which item is equipped while browsing the inventory.
	[Space]
	[SerializeField] private GameObject m_InventoryItemTypesParent;
	private Image[]						m_InventoryItemTypesImages;
	[SerializeField] private Color		m_InventoryTabSelectedColor;
	[SerializeField] private Color		m_InventoryTabUnselectedColor;
	private ITEMTYPE					m_CurrentItemCategory;
	private EquipmentSlot				m_CurrentEquipmentCategory;
	[Space]
	[SerializeField] private Text m_InventoryTabName;

	//public delegate void InteractableAlertHandler();
	//public event InteractableAlertHandler InteractableAlert;

	// Start is called before the first frame update
	void Start()
	{
		m_Inventory		= GameManager.Instance.rPlayer1.GetInventory;
		m_Inventory.InventoryUpdateEvent += UpdateDisplayedEquipment;
		m_Inventory.InventoryUpdateEvent += UpdateDisplayedItems;

		// Example
		//for ( int RowIndex = 0; RowIndex < 4; ++RowIndex)
		//{
		//	for ( int ColumnIndex = 0; ColumnIndex < 4; ++ColumnIndex )
		//	{
		//		m_InventorySlots[RowIndex][ColumnIndex] = new InventorySlot();
		//	}
		//}
		// End of Example


		// TODO: Switch out the below for an array with the menus, and use a for loop with a switch case to do this instead.

		if ( !m_SlotMenuMisc )
		{
			Debug.Log( "m_SlotMenuMisc is not dragged into the InventoryUI-field for it in the inspector. Now Finding it via code... ('w' ) " );

			if ( m_SlotMenuMisc = transform.Find( "Inventory" ).gameObject.transform.Find( "SlotMenuMisc" ).gameObject )
			{
				Debug.Log( " Successfully found m_SlotMenuMisc after a PAINFUL and EXPENSIVE search... (´w` ) " );
			}
			else
			{
				Debug.Log( " Failed to find m_SlotMenuMisc even after a PAINFUL and EXPENSIVE search... (`w´ ).. It probably doesn't exist yet... (-n -' ) " );
			}
		}

		if ( !m_SlotMenuConsumable )
		{
			Debug.Log( "m_SlotMenuConsumable is not dragged into the InventoryUI-field for it in the inspector. Now Finding it via code... ('w' ) " );

			if ( m_SlotMenuMisc = transform.Find( "Inventory" ).gameObject.transform.Find( "SlotMenuConsumable" ).gameObject )
			{
				Debug.Log( " Successfully found m_SlotMenuConsumable after a PAINFUL and EXPENSIVE search... (´w` ) " );
			}
			else
			{
				Debug.Log( " Failed to find m_SlotMenuConsumable even after a PAINFUL and EXPENSIVE search... (`w´ ).. It probably doesn't exist yet... (-n -' ) " );
			} 
		}

		if ( !m_SlotMenuEquippable )
		{
			Debug.Log( "m_SlotMenuEquippable is not dragged into the InventoryUI-field for it in the inspector. Now Finding it via code... ('w' ) " );

			if ( m_SlotMenuMisc = transform.Find( "Inventory" ).gameObject.transform.Find( "SlotMenuEquippable" ).gameObject )
			{
				Debug.Log( " Successfully found m_SlotMenuEquippable after a PAINFUL and EXPENSIVE search... (´w` ) " );
			}
			else
			{
				Debug.Log( " Failed to find m_SlotMenuEquippable even after a PAINFUL and EXPENSIVE search... (`w´ ).. It probably doesn't exist yet... (-n -' ) " );
			}
		}

		if ( !m_SlotMenuQuest )
		{
			Debug.Log( "m_SlotMenuQuest is not dragged into the InventoryUI-field for it in the inspector. Now Finding it via code... ('w' ) " );

			if ( m_SlotMenuMisc = transform.Find( "Inventory" ).gameObject.transform.Find( "SlotMenuQuest" ).gameObject )
			{
				Debug.Log( " Successfully found m_SlotMenuEquippable after a PAINFUL and EXPENSIVE search... (´w` ) " );
			}
			else
			{
				Debug.Log( " Failed to find m_SlotMenuEquippable even after a PAINFUL and EXPENSIVE search... (`w´ ).. It probably doesn't exist yet... (-n -' ) " );
			}
		}

		m_SlotMenuCurrent = m_SlotMenuMisc;
		m_SlotMenuMisc.SetActive( false );
		SlotMenuConsumable.SetActive( false );
		SlotMenuEquippable.SetActive( false );
		SlotMenuQuest.SetActive( false );

		m_InventoryItemTypesImages = m_InventoryItemTypesParent.GetComponentsInChildren<Image>();

		gameObject.SetActive( false );
	}


	// This function has 0 references because it gets called from a UI button press.
	public void RemoveCurrentlySelectedItem() // An ugly workaround to the problem where the item slot menus need to target a specific slot in order to trigger their functions. Since they can't access this gameobject in their button functions, this was the only way I found.
	{
		m_Inventory.RemoveItem( m_SelectedInventorySlot.Item ); // Change this later so it brings up an "Are you sure?"-menu.
	}


	public void ShowItemPickedUpAlert( InventoryItem _PickedUpItem )
	{
		m_ItemPickedUpAlert.transform.GetChild( 0 ).gameObject.transform.GetChild(0).GetComponent<Image>().sprite	= _PickedUpItem.m_Icon; // TODO:: Save this monstrocisy of a way to do this.
		m_ItemPickedUpAlert.GetComponentInChildren<Text>().text														= _PickedUpItem.m_ItemName;
		m_ItemPickedUpAlert.GetComponent<HideUIAfterDuration>().ResetAliveTimeLeft();
		m_ItemPickedUpAlert.SetActive( true );

		UI_Manager.Instance.LowerInteractablePrompt();
	}

	public void HideItemPickedUpAlert()
	{
		UI_Manager.Instance.RaiseInteractablePrompt();
	}



	// Updates the text in the box above the inventory.
	public void UpdateTabName( bool _SelectingEquipment = false )
	{
		string NewTabName = "NoNameYet";

		switch ( m_CurrentItemCategory )
		{
			case ITEMTYPE.ITEMTYPE_CONSUMABLE:
				NewTabName = "Consumables";
				break;

			case ITEMTYPE.ITEMTYPE_QUEST:
				NewTabName = "Key Items";
				break;

			case ITEMTYPE.ITEMTYPE_EQUIPMENT:

				if ( !_SelectingEquipment )
					NewTabName = "Equipment";
				else
				{
					switch ( m_CurrentEquipmentCategory )
					{
						case EquipmentSlot.EQUIPMENTSLOT_WEAPON:
							{
								// TODO:: Add some sort of check to see which weaponslot is selected between 1,2 or 3.
								NewTabName = "Weapon";
							}
							break;

						case EquipmentSlot.EQUIPMENTSLOT_HEAD:			NewTabName = "Head";		break;
						case EquipmentSlot.EQUIPMENTSLOT_CHEST:			NewTabName = "Chest";		break;
						case EquipmentSlot.EQUIPMENTSLOT_GAUNTLETS:		NewTabName = "Gauntlets";	break;
						case EquipmentSlot.EQUIPMENTSLOT_LEGS:			NewTabName = "Legs";		break;
						case EquipmentSlot.EQUIPMENTSLOT_FEET:			NewTabName = "Feet";		break;
						case EquipmentSlot.EQUIPMENTSLOT_Consumable:	NewTabName = "Consumables";	break;
						case EquipmentSlot.EQUIPMENTSLOT_EQUIPPEDTAB:	NewTabName = "Equipment";	break;
					}
				}
				break;

			case ITEMTYPE.ITEMTYPE_MISC:
				NewTabName = "Misc. Items";
				break;
		}

		m_InventoryTabName.text = NewTabName;
	}


	public void ShowInventoryCategory( ITEMTYPE _Category )
	{

		Dictionary<InventoryItem, int> Items = GameManager.Instance.rPlayer1.GetInventory.GetItemsInCategory( _Category );
		UpdateDisplayedItems( Items );

		m_InventoryItemTypesImages[ (int)m_CurrentItemCategory ].color = m_InventoryTabUnselectedColor;
		m_CurrentItemCategory = _Category;
		m_InventoryItemTypesImages[ (int)m_CurrentItemCategory ].color = m_InventoryTabSelectedColor;

		UpdateTabName();
	}


	public void ShowPreviousInventoryCategory()
	{
		ITEMTYPE NewCategory	= (m_CurrentItemCategory - 1) < 0 ? ITEMTYPE.NumItemTypes - 1 : m_CurrentItemCategory - 1; // Set category to current category -1, or max category if -1 would be less than 0

		ShowInventoryCategory( NewCategory );
	}


	public void ShowNextInventoryCategory()
	{
		ITEMTYPE NewCategory	= m_CurrentItemCategory + 1 >= ITEMTYPE.NumItemTypes ? 0 : m_CurrentItemCategory + 1;

		ShowInventoryCategory( NewCategory );
	}


	public void UpdateDisplayedItems( Dictionary<InventoryItem, int> _ItemDictionary )
	{
		m_CurrentEquipmentWindow.SetActive( false );
		m_DisplayedItemsParent.SetActive( true );
		m_InventoryItemTypesParent.SetActive( true );

		// Do these in order to reset them. The equipped icon might come in use later, when the item wheel for consumables is implemented, and then these should show up on those items.
		m_EquippedIcon.transform.SetParent( gameObject.transform, false );		// Reset transform of the equipped icon
		m_EquippedIcon.GetComponent<Image>().enabled	= false;				// Disable image component in case nothing is equipped in this category


		ItemSlot[] Slots = m_DisplayedItemsParent.GetComponentsInChildren<ItemSlot>();


		if ( _ItemDictionary.Count > Slots.Length )
			Debug.LogWarning( "Inventory needs to be expanded to show all items." );


		int SlotIndex = 0;

		foreach ( var CurrentItem in _ItemDictionary )
		{
			Slots[ SlotIndex ].AddItemToSlot( CurrentItem.Key, CurrentItem.Value );

			SlotIndex++;

			if ( SlotIndex >= Slots.Length )
			{
				Debug.LogWarning( $"More items in inventory than can currently be displayed. Only displaying the first {Slots.Length}." );
				break;
			}
		}

		while ( SlotIndex < Slots.Length )
		{
			Slots[ SlotIndex ].RemoveItemFromSlot();
			SlotIndex++;
		}
		
		// If slot 0 is already selected, OnSelect methods won't be called.
		// That's why we select slot1, then slot 0.
		Slots[ 1 ].GetComponent<Button>().Select(); // Select the first slot
		Slots[ 0 ].GetComponent<Button>().Select(); // Select the first slot
	}




	public void ShowEquippedEquipment()
	{
		UI_Manager.Instance.rMenu.SetMenuState( Menu.EMenuState.EquipmentScreen );	// Call this in order to set correct actions to the controller's buttons

		m_CurrentEquipmentWindow.SetActive( true );
		m_DisplayedItemsParent.SetActive( false );
		m_InventoryItemTypesParent.SetActive( false ); // Call false on this here in case it is still open

		EquipmentManager.Instance.SelectedEquipmentSlot.GetComponent<Button>().Select();

		m_CurrentItemCategory		= ITEMTYPE.ITEMTYPE_EQUIPMENT;
		m_CurrentEquipmentCategory	= EquipmentSlot.EQUIPMENTSLOT_EQUIPPEDTAB;

		UpdateTabName();
	}


	// This function is called from a UI-button press. That's why it has 0 references.
	public void ShowEquipmentCategory( EquipmentSlot _Category )
	{
		UI_Manager.Instance.rMenu.SetMenuState( Menu.EMenuState.EquipmentBrowse );

		Dictionary<InventoryItem, int> Equipments = GameManager.Instance.rPlayer1.GetInventory.GetEquipmentGear( _Category );
		UpdateDisplayedEquipment( Equipments );

		m_CurrentEquipmentCategory = _Category;

		UpdateTabName( true );
	}


	public void UpdateDisplayedEquipment( Dictionary<InventoryItem, int> _EquipmentDictionary )
	{
		m_CurrentEquipmentWindow.SetActive( false );
		m_DisplayedItemsParent.SetActive( true );

		m_EquippedIcon.transform.SetParent( gameObject.transform, false );		// Reset transform of the equipped icon
		m_EquippedIcon.GetComponent<Image>().enabled	= false;				// Disable image component in case nothing is equipped in this category

		ItemSlot[] Slots =  m_DisplayedItemsParent.GetComponentsInChildren<ItemSlot>();

		InventoryItem EquippedItem	= EquipmentManager.Instance.SelectedEquipmentSlot.Item; // Get the item in the selected slot, so that we can put the equipped icon there, and also later scroll to there if the inventory is too big to fit
		bool FoundEquippedItem		= false;



		int SlotIndex = 0;

		foreach ( var CurrentItem in _EquipmentDictionary )
		{
			Slots[ SlotIndex ].AddItemToSlot( CurrentItem.Key, CurrentItem.Value );

			if ( Slots[ SlotIndex ].Item == EquippedItem )
			{
				FoundEquippedItem = true;

				Slots[ SlotIndex ].GetComponent<Button>().Select();
				UpdateEquippedIcon();
			}

			SlotIndex++;

			if ( SlotIndex >= Slots.Length )
			{
				Debug.LogWarning( $"More items in inventory than can currently be displayed. Only displaying the first {Slots.Length}." );
				break;
			}
		}

		while ( SlotIndex < Slots.Length )
		{
			Slots[ SlotIndex ].RemoveItemFromSlot();
			SlotIndex++;
		}

		if ( !FoundEquippedItem )
			Slots[ 0 ].GetComponent<Button>().Select(); // Select the first slot
	}



	public void SelectInventorySlot( ItemSlot _SlotToSelect )
	{
		m_SelectedInventorySlot	= _SlotToSelect;
	}

	public void UpdateEquippedIcon()
	{
		m_EquippedIcon.transform.SetParent( m_SelectedInventorySlot.transform );
		m_EquippedIcon.transform.localPosition			= new Vector3( 25.0f, 25.0f, 0.0f );
		m_EquippedIcon.GetComponent<Image>().enabled	= true;
	}
}
