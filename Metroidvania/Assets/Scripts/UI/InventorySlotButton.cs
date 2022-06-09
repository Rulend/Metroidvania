using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class InventorySlotButton : BetterButton, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
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


	public void OnBeginDrag( PointerEventData pr_EventData )
	{
		if ( m_ItemSlot.Item == null )
			return;

		if ( !m_ItemSlot.Item.m_DefaultItem )
		{

			m_ItemSlotItemImage.color -= new Color( 0.0f, 0.0f, 0.0f, 0.6f );
			m_DraggedItemObject.GetComponent<Image>().enabled = true;
			m_DraggedItemObject.GetComponent<Image>().sprite = m_ItemSlot.Item.m_Icon;
		}

		m_Mouse = Mouse.current;
	}


	public void OnDrag( PointerEventData pr_EventData )
	{
		//We don't need this functionality, we have a separate gameobjet that takes care of that.
		m_DraggedItemObject.transform.position = m_Mouse.position.ReadValue();
	}


	public void OnEndDrag( PointerEventData pr_EventData )
	{
		m_ItemSlotItemImage.color += new Color( 0.0f, 0.0f, 0.0f, 0.6f );
		m_DraggedItemObject.GetComponent<Image>().enabled = false;
		m_DraggedItemObject.GetComponent<Image>().sprite = null;
		m_Mouse = null;
	}


	// Triggers when an item is dropped into THIS slot. The item inside pr_EventData is the dragged item.
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
			if ( rEquipManager.IsItemEquipped( m_ItemSlot.Item ) ) // This will always return true for the equipment slots, as default items also count as equipped items.
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
				else // Switch places on items since neither of them was equipped
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
			{
				rEquipManager.Unequip( (Equipment)DraggedItem ); // Remove item from being equipped
				GameManager.Instance.rPlayer1.GetInventory.RemoveItem( DraggedItem ); // Remove item from inventory, since unequipping it puts it in the first open slot
			}
			else
			{
				pr_EventData.pointerDrag.GetComponent<ItemSlot>().RemoveItemFromSlot( false );
			}

			m_ItemSlot.AddItemToSlot( DraggedItem ); // Put item in dragged to slot // TODO:: Test if this causes any issues.
		}
	}
}
