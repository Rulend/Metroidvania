using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class InventorySlotButton : BetterButton, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
	private ItemSlot	m_ItemSlot;
	private Image		m_ItemSlotItemImage;

	private GameObject	m_DraggedItemObject;
	private Mouse		m_Mouse;

	private void Awake()
	{
		m_ItemSlot			= GetComponent<ItemSlot>();
		m_ItemSlotItemImage = m_ItemSlot.transform.GetChild( 0 ).GetComponent<Image>();
	}

	private void Start()
	{
		m_DraggedItemObject = UI_Manager.Instance.rInventoryUI.DraggedItem;
	}



	public void OnPointerClick( PointerEventData pr_EventData )
	{
		if ( pr_EventData.button == PointerEventData.InputButton.Left )
			m_ItemSlot.ButtonShowItemSlotOptions();
		else if ( pr_EventData.button == PointerEventData.InputButton.Right )
			m_ItemSlot.ButtonUseItem();
		else
			m_ItemSlot.HideItemSlotOptions();
	}

	public void OnPointerEnter( PointerEventData pr_EventData )
	{
		m_ItemSlot.DisplayItemInfo();
	}

	public void OnPointerExit( PointerEventData pr_EventData )
	{
		m_ItemSlot.HideItemInfo();
	}


	public override void OnSelect( BaseEventData _EventData )
	{
		//UI_Manager.Instance.rInventoryUI.SelectInventorySlot( GetComponent<ItemSlot>() );
		base.OnSelect( _EventData );

		UI_Manager.Instance.rInventoryUI.SelectInventorySlot( GetComponent<ItemSlot>() );

		m_ItemSlot.DisplayItemInfo();
	}
}
