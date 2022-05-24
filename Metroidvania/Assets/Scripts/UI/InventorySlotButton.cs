using UnityEngine.EventSystems;

public class InventorySlotButton : BetterButton, IBeginDragHandler, IDragHandler, IEndDragHandler
{
	private ItemSlot m_ItemSlot;

	private void Awake()
	{
		m_ItemSlot = GetComponent<ItemSlot>();
	}

	public override void OnPointerClick( PointerEventData pr_EventData )
	{
		if ( pr_EventData.button == PointerEventData.InputButton.Left )
			m_ItemSlot.ButtonShowItemSlotOptions();
		else if ( pr_EventData.button == PointerEventData.InputButton.Right )
			m_ItemSlot.ButtonUseItem();
		else
			m_ItemSlot.HideItemSlotOptions();
	}

	public override void OnPointerEnter( PointerEventData pr_EventData )
	{
		m_ItemSlot.DisplayItemInfo();
	}

	public override void OnPointerExit( PointerEventData pr_EventData )
	{
		m_ItemSlot.HideItemInfo();
	}



	// Todo:: implement these.
	public void OnBeginDrag( PointerEventData pr_EventData )
	{

	}


	public void OnDrag( PointerEventData pr_EventData )
	{
		
	}


	public void OnEndDrag( PointerEventData pr_EventData )
	{
		
	}
}
