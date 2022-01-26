using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
public class InventoryItem : ScriptableObject // If it doesn't inherit from scriptable object, it won't appear inside ItemPickup.cs. Idk why. 
{
	public string m_Name		= "New Item";
	public Sprite m_Icon		= null;
	public bool m_DefaultItem	= false;

}
