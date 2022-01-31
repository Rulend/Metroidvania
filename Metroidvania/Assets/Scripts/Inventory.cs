using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
	public delegate void OnItemsChange();
	public OnItemsChange m_ItemsChangedCallback;

	private int m_AmountOfSlots = 16;
	public int AmountOfSlots { get { return m_AmountOfSlots; } }
	
	public List<InventoryItem> m_Items = new List<InventoryItem>();

	public bool AddItem( InventoryItem pr_ItemToAdd )
	{
		// Is it stackable? Is there space in that stack? Is the inventory full? TODO:: Check these stuff.

		if ( !pr_ItemToAdd.m_DefaultItem )
		{
			if ( m_Items.Count >= m_AmountOfSlots ) 
			{
				Debug.Log( "Couldn't add item to inventory, it's full. \n" );
				return false;
			}

			m_Items.Add( pr_ItemToAdd );

			if ( m_ItemsChangedCallback != null ) 
			{
				m_ItemsChangedCallback.Invoke();
			}

			return true;
		}


		return false;
	}

	public bool RemoveItem( InventoryItem pr_ItemToRemove )
	{

		m_Items.Remove( pr_ItemToRemove );

		if ( m_ItemsChangedCallback != null )
		{
			m_ItemsChangedCallback.Invoke();
		}

		return false;
	}




	// Start is called before the first frame update
	void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
