using UnityEngine;

public class InventoryUI : MonoBehaviour
{
	private Inventory m_Inventory;

	//[ SerializeField ] private GameObject		m_InventoryUI;			// A reference to the "Inventory"-object inside the UI-canvas in the scene. // (Not needed since we do this inside Player.cs instead)

	[ SerializeField ] private GameObject		m_InventorySlotsParent;	// A reference to the "InventoryPanel"-object inside the "Inventory"-object in the scene.


	//private InventorySlot[][]					m_InventorySlots;		// An array of references to all of the inventory slots inside the "InventoryPanel"-object in the scene.
	private InventorySlot[]						m_InventorySlots;		// An array of references to all of the inventory slots inside the "InventoryPanel"-object in the scene.

	[ SerializeField ] private GameObject		m_SlotMenuCurrent;		//	The current slot-menu.
	[ SerializeField ] private GameObject		m_SlotMenuMisc;			//	The Slot-Menu for ITEMTYPE_MISC items. 
	[ SerializeField ] private GameObject		m_SlotMenuConsumable;	//	The Slot-Menu for ITEMTYPE_CONSUMABLE items. 
	[ SerializeField ] private GameObject		m_SlotMenuEquippable;	//	The Slot-Menu for ITEMTYPE_EQUIPPABLE items. 
	[ SerializeField ] private GameObject		m_SlotMenuQuest;		//	The Slot-Menu for ITEMTYPE_QUEST items. 


	public GameObject SlotMenuCurrent	{
		get { return m_SlotMenuCurrent; } 
		set { if ( value != null ) m_SlotMenuCurrent = value; } }
	public GameObject SlotMenuMisc			{ get { return m_SlotMenuMisc;			} }
	public GameObject SlotMenuConsumable	{ get { return m_SlotMenuConsumable;	} }
	public GameObject SlotMenuEquippable	{ get { return m_SlotMenuEquippable;	} }
	public GameObject SlotMenuQuest			{ get { return m_SlotMenuQuest;			} }


	// Start is called before the first frame update
	void Start()
	{
		m_Inventory = GameManager.Instance.Player1.GetInventory;
		m_Inventory.m_ItemsChangedCallback += UpdateUI;

		m_InventorySlots = m_InventorySlotsParent.GetComponentsInChildren<InventorySlot>(); // I do not enjoy doing things in this manner, I would rather create them via code. TODO:: Do this via code. Example below.

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

	}

	// Update is called once per frame
	void Update()
	{

	}

	void UpdateUI()
	{
		Debug.Log("Updating UI!");

		for ( int SlotsIndex = 0; SlotsIndex < m_InventorySlots.Length; ++SlotsIndex )
		{
			if ( SlotsIndex < m_Inventory.m_Items.Count)
			{
				//m_InventorySlots[RowIndex][ColumnIndex].AddItemToSlot( m_Inventory.m_Items[SlotsIndex] );
				m_InventorySlots[SlotsIndex].AddItemToSlot( m_Inventory.m_Items[SlotsIndex] );
			}
			else
			{
				//m_InventorySlots[RowIndex][ColumnIndex].RemoveItemFromSlot();
				m_InventorySlots[SlotsIndex].RemoveItemFromSlot();
			}

		}

	}
}
