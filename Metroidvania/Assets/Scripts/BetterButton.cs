using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class BetterButton : MonoBehaviour, IPointerClickHandler
{


	public void OnPointerClick( PointerEventData pr_EventData )
	{
		if ( pr_EventData.button == PointerEventData.InputButton.Left )
		{
			GetComponent<InventorySlot>().ShowItemSlotOptions(); // TODO: Make some sort of "CanInteractWith"-class that uses the mouse, and so this button can be used for other stuff too. Or rename this class. Whichever.
		}
		else if ( pr_EventData.button == PointerEventData.InputButton.Right )
		{
			// Equip/use an item without submenu
			Debug.Log( "Directly equip or use item." );
			GetComponent<InventorySlot>().OnUseItemButton();
		}
		else
		{
			GetComponent<InventorySlot>().HideItemSlotOptions();
		}
	}

	public void OnPointer()
	{

	}

}
