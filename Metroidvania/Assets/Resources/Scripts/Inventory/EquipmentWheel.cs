using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentWheel : MonoBehaviour
{
	private ItemSlot[]				m_WheelSlots;				// The slots in the bottom left corner of the screen
	private ItemSlot[]				m_ConsumableSlots;			// The bottom row when looking at the Equipped screen.
	private List<InventoryItem>		m_EquippedConsumables;		

	private int m_CurrentConsumable;

	private void Awake()
	{
		m_WheelSlots			= GetComponentsInChildren<ItemSlot>();
		m_EquippedConsumables	= new List<InventoryItem>();
	}


	public void SetupWheel( ItemSlot[] _ConsumableSlots )
	{
		m_ConsumableSlots = _ConsumableSlots;
	}


	public void UpdateWheel()
	{
		m_CurrentConsumable = 0;
		// Loop through the slots and fix the list of consumables accordingly´, since not all slots will have items in them
		Debug.Log( "Updating equipment-wheel" );
		m_EquippedConsumables.Clear();

		foreach ( ItemSlot CurrentSlot in m_ConsumableSlots )
		{
			if ( CurrentSlot.Item && !CurrentSlot.Item.m_DefaultItem )
				m_EquippedConsumables.Add( CurrentSlot.Item );
		}

		if ( m_EquippedConsumables.Count > 1 )
		{
			m_WheelSlots[ 3 ].AddItemToSlot( m_EquippedConsumables[ m_CurrentConsumable ] );
			m_WheelSlots[ 4 ].AddItemToSlot( m_EquippedConsumables[ m_CurrentConsumable + 1 ] );

		}
		else if ( m_EquippedConsumables.Count > 0 )
			m_WheelSlots[ 3 ].AddItemToSlot( m_EquippedConsumables[ m_CurrentConsumable ] );
	} 

	private void UpdateVisuals()
	{

	}


	// Changes which item is the currently 
	public InventoryItem CycleConsumables( int _CycleWay )
	{
		if ( m_EquippedConsumables.Count == 0 )
		{
			Debug.LogWarning( "No shit equipped in the equipment wheel" );
			return null;
		}

		m_CurrentConsumable += _CycleWay;
		int NextConsumable	= m_CurrentConsumable + 1;

		//if ( m_CurrentConsumable < 0 )
		//	m_CurrentConsumable = m_EquippedConsumables.Count;
		//else 
		if ( m_CurrentConsumable >= m_EquippedConsumables.Count )
		{
			m_CurrentConsumable = 0;
			NextConsumable = ( m_CurrentConsumable + 1 ) % m_EquippedConsumables.Count;
		}

		if ( NextConsumable >= m_EquippedConsumables.Count )
			NextConsumable = 0;

		m_WheelSlots[ 3 ].AddItemToSlot( m_EquippedConsumables[ m_CurrentConsumable ] );
		m_WheelSlots[ 4 ].AddItemToSlot( m_EquippedConsumables[ NextConsumable ] );

		return m_WheelSlots[ 3 ].Item;
	}
}
