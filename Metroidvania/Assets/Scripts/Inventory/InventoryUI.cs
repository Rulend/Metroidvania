using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
	private Inventory m_Inventory;

	//[ SerializeField ] private GameObject		m_InventoryUI;			// A reference to the "Inventory"-object inside the UI-canvas in the scene. // (Not needed since we do this inside Player.cs instead)

	[ SerializeField ] private GameObject		m_InventorySlotsParent;	// A reference to the "InventoryPanel"-object inside the "Inventory"-object in the scene.


	//private InventorySlot[][]					m_InventorySlots;		// An array of references to all of the inventory slots inside the "InventoryPanel"-object in the scene.
	private ItemSlot[]							m_Slots;				// An array of references to all of the inventory slots inside the "InventoryPanel"-object in the scene.

	[ SerializeField ] private GameObject		m_DraggedItem;		// The object which follows the mouse and displays an icon when dragging an item.
	[ SerializeField ] private GameObject		m_ItemInfoDisplay;		// A gameobject used to display information about an item while hovering over it in the inventory.

	[ SerializeField ] private GameObject		m_SlotMenuCurrent;		//	The current slot-menu.
	[ SerializeField ] private GameObject		m_SlotMenuMisc;			//	The Slot-Menu for ITEMTYPE_MISC items. 
	[ SerializeField ] private GameObject		m_SlotMenuConsumable;	//	The Slot-Menu for ITEMTYPE_CONSUMABLE items. 
	[ SerializeField ] private GameObject		m_SlotMenuEquippable;	//	The Slot-Menu for ITEMTYPE_EQUIPPABLE items. 
	[ SerializeField ] private GameObject		m_SlotMenuQuest;		//	The Slot-Menu for ITEMTYPE_QUEST items. 

	public ItemSlot								m_CurrentSlot;			// The slot was lastly left-clicked.
	public GameObject							m_CurrentSlotBorder;	// The slot was lastly left-clicked.
	public GameObject							m_InteractableAlert;	// A panel that shows up when close to an interactable. Will show different text based on what kind it is. 
	public GameObject							m_ItemPickedUpNotice;	// After picking up an item, this panel will be shown in order to let the player see what they've picked up.

	private Vector3								m_InteractableAlertStartPos;

	public Vector3 InteractableAlertStartPos =>		m_InteractableAlertStartPos;

	// TODO:: Clean this document up. So messy

	public GameObject ItemInfoDisplay => m_ItemInfoDisplay;
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



	public delegate void	UpdateEquippabableListHandler( List<Equipment> _NewItems );
	public event			UpdateEquippabableListHandler UpdateEquippableListEvent;


	//public delegate void InteractableAlertHandler();
	//public event InteractableAlertHandler InteractableAlert;

	// Start is called before the first frame update
	void Start()
	{
		m_Inventory		= GameManager.Instance.rPlayer1.GetInventory;
		m_Slots			= m_Inventory.m_InventorySlots; // I do not enjoy doing things in this manner, I would rather create them via code. TODO:: Do this via code. Example below.

		m_InteractableAlertStartPos = m_InteractableAlert.transform.position;

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

		gameObject.SetActive( false );
	}

	public void UseCurrentlySelectedItem() // An ugly workaround to the problem where the item slot menus need to target a specific slot in order to trigger their functions. Since they can't access this gameobject in their button functions, this was the only way I found.
	{
		m_CurrentSlot.ButtonUseItem();
	}

	public void RemoveCurrentlySelectedItem() // An ugly workaround to the problem where the item slot menus need to target a specific slot in order to trigger their functions. Since they can't access this gameobject in their button functions, this was the only way I found.
	{
		m_CurrentSlot.ButtonRemoveItem(); // Change this later so it brings up an "Are you sure?"-menu. Also change discard to actually put the gameobject back into the scene.
	}


	public void ShowItemPickedUpNotice( InventoryItem pr_PickedUpItem )
	{
		m_ItemPickedUpNotice.transform.GetChild( 0 ).gameObject.transform.GetChild(0).GetComponent<Image>().sprite		= pr_PickedUpItem.m_Icon; // TODO:: Save this monstrocisy of a way to do this.
		m_ItemPickedUpNotice.GetComponentInChildren<Text>().text														= pr_PickedUpItem.m_ItemName;
		m_ItemPickedUpNotice.GetComponent<HideUIAfterDuration>().ResetAliveTimeLeft();
		m_ItemPickedUpNotice.SetActive( true );
	}



	public void UpdateEquippableList( List<Equipment> _NewList )
	{
		// Here we have to get the different inventories


		UpdateEquippableListEvent.Invoke( _NewList );
	}


}
