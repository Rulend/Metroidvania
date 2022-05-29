using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlotButton : BetterButton, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
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
		//if ( m_ItemSlot.Item != null )
		//	GameManager.Instance.rPlayer1.InventoryUI.GetComponent<InventoryUI>().DraggedItem.GetComponent<Image>().sprite = m_ItemSlot.Item.m_Icon;
	}


	public void OnDrag( PointerEventData pr_EventData )
	{
		//if ( m_ItemSlot.Item != null )
		//	GameManager.Instance.rPlayer1.InventoryUI.GetComponent<InventoryUI>().DraggedItem.transform.position = Camera.main.ScreenToWorldPoint( Input.mousePosition );
	}


	public void OnEndDrag( PointerEventData pr_EventData )
	{

		//Inventory_Item_Mover.Instance.MoveItems( GetComponent<ItemSlot>() );
		//Debug.Log( $"Ended drag, object that handled that was: {pr_EventData.pointerDrag.name}" );
	}


	public void OnDrop( PointerEventData pr_EventData )
	{
		InventoryItem DraggedItem = pr_EventData.pointerDrag.GetComponent<ItemSlot>().Item; // The item that was dragged to this slot

		if ( DraggedItem == null )
			return;
		if ( DraggedItem.m_DefaultItem )
			return;

		EquipmentManager rEquipManager = EquipmentManager.Instance;

		if ( m_ItemSlot.Item != null ) // If this slot is not empty
		{
			if ( rEquipManager.IsItemEquipped( m_ItemSlot.Item ) ) // This will always return true for the equipment slots, since they count the default items as equipped items.
			{
				if ( DraggedItem as Equipment != null )
					rEquipManager.Equip( (Equipment)DraggedItem );
			}
			else
			{
				if ( rEquipManager.IsItemEquipped( DraggedItem ) )
				{
					if ( m_ItemSlot.Item as Equipment != null )
						rEquipManager.Equip( (Equipment)m_ItemSlot.Item );
					else
						rEquipManager.Unequip( (Equipment)DraggedItem );
				}
				else
				{
					InventoryItem TempItemHolder = m_ItemSlot.Item;
					m_ItemSlot.AddItemToSlot( pr_EventData.pointerDrag.GetComponent<ItemSlot>().Item );
					pr_EventData.pointerDrag.GetComponent<ItemSlot>().AddItemToSlot( TempItemHolder );
				}
			}
		}
		else
		{
			if ( rEquipManager.IsItemEquipped( DraggedItem ) )
				rEquipManager.Unequip( (Equipment)DraggedItem );
			else
			{
				m_ItemSlot.AddItemToSlot( DraggedItem );
				pr_EventData.pointerDrag.GetComponent<ItemSlot>().RemoveItemFromSlot( false );
			}
		}
	}
}
