using UnityEngine;

public enum ITEMTYPE
{
	ITEMTYPE_CONSUMABLE = 0,
	ITEMTYPE_QUEST			,
	ITEMTYPE_EQUIPMENT		, // Might split this into armor/weapons later. 
	ITEMTYPE_MISC			,

	NumItemTypes
}


[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
public class InventoryItem : ScriptableObject // If it doesn't inherit from scriptable object, it won't appear inside ItemPickup.cs. Idk why. 
{
	public int			m_ID;											// Database ID for the item.
	public int			m_InventoryID;									// The inventory ID for the item. This makes it so that if there are duplicates of an item, this can be used to tell them apart.
	public string		m_ItemName			= "New Item";				// The name for the item.
	public bool			m_DefaultItem		= false;					// Whether or not the item is a default item. Default items cannot be added to the inventory, and will be equipped when everything else is unequipped.
	public bool			m_Stackable			= true;						// Whether or not the item is stackable
	public Sprite		m_Icon				= null;						// The icon for the item that will show up in the Inventory.
	public ITEMTYPE		m_ItemType			= ITEMTYPE.ITEMTYPE_MISC;   // The type of the item. Used to sort items in inventory, and also to decide which item slot submenu to use when left clicking the item.
	[TextArea( 5, 10 )]
	public string		m_ItemDescription	= "No item description yet."; // The description that shows up when hovering an item.
																		  //	public int			m_ItemValue			= 0;						// The rarity or "value" of the item. Can be used to prompt a "Are you sure you want to use that"-check.

	public virtual void Use( int _Amount = 1, Character _User = null )
	{
		// Use the item, but do something different depending on what type it is. That's why this function is virtual
		Debug.Log( "Using " + m_ItemName );
	}


	public void ConvertToJSON()
	{

	}
}
