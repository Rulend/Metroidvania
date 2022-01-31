using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
public class InventoryItem : ScriptableObject // If it doesn't inherit from scriptable object, it won't appear inside ItemPickup.cs. Idk why. 
{
	public string		m_ItemName		= "New Item";				// The name for the item.
	public Sprite		m_Icon			= null;						// The icon for the item that will show up in the Inventory.
	public bool			m_DefaultItem	= false;					// Whether or not the item is a default item. Default items cannot be added to the inventory, and will be equipped when everything else is unequipped.
	public ITEMTYPE		m_ItemType		= ITEMTYPE.ITEMTYPE_MISC;	// The type of the item. Used instead of casting to see which inventory window to show it in.

	public enum ITEMTYPE : int
	{
		ITEMTYPE_MISC			= 0,
		ITEMTYPE_CONSUMABLE,
		ITEMTYPE_EQUIPMENT,	// Might split this into armor/weapons later. 
		ITEMTYPE_QUEST,
	}

	public virtual void Use()
	{
		// Use the item, do it differently for every item.
		Debug.Log( "Using " + m_ItemName );


	}

}
