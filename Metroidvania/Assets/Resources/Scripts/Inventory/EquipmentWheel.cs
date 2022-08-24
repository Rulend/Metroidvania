using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentWheel : MonoBehaviour
{
	private ItemSlot[]				m_WheelSlots;				// The slots in the bottom left corner of the screen
	private ItemSlot[]				m_AllConsumableSlots;			// The bottom row when looking at the Equipped screen.
	private List<ItemSlot>			m_NonEmptyConsumableSlots;

	private int m_CurrentSlotIndex;

	// Setup the wheel; providing a reference to the slots of the equipmentmanager that has the consumables in them.
	public void SetupWheel( ItemSlot[] _ConsumableSlots )
	{
		m_WheelSlots				= GetComponentsInChildren<ItemSlot>( true );
		m_AllConsumableSlots		= _ConsumableSlots;
		m_NonEmptyConsumableSlots	= new List<ItemSlot>();
	}


	public void UpdateWheel()
	{
		// Loop through the slots and fix the list of consumables accordingly´, since not all slots will have items in them
		Debug.Log( "Updating equipment-wheel" );
		m_NonEmptyConsumableSlots.Clear();

		foreach ( ItemSlot CurrentSlot in m_WheelSlots )
			CurrentSlot.RemoveItemFromSlot();

		foreach ( ItemSlot CurrentSlot in m_AllConsumableSlots )
		{
			if ( CurrentSlot.Item && !CurrentSlot.Item.m_DefaultItem )
				m_NonEmptyConsumableSlots.Add( CurrentSlot );
		}

		if ( m_NonEmptyConsumableSlots.Count > 1 )
		{
			int NextSlotIndex = m_CurrentSlotIndex + 1;

			if ( m_CurrentSlotIndex < m_NonEmptyConsumableSlots.Count )
			{
				if ( NextSlotIndex >= m_NonEmptyConsumableSlots.Count )
					NextSlotIndex = 0;
			}
			else
			{
				m_CurrentSlotIndex = 0;
				NextSlotIndex = 1;
			}


			ItemSlot CurrentSlot	= m_NonEmptyConsumableSlots[ m_CurrentSlotIndex ];
			ItemSlot NextSlot		= m_NonEmptyConsumableSlots[ NextSlotIndex ];

			m_WheelSlots[ 3 ].AddItemToSlot( CurrentSlot.Item, CurrentSlot.Amount );
			m_WheelSlots[ 4 ].AddItemToSlot( NextSlot.Item, NextSlot.Amount );

		}
		else if ( m_NonEmptyConsumableSlots.Count > 0 )
		{
			m_CurrentSlotIndex = 0;
			m_WheelSlots[ 3 ].AddItemToSlot( m_NonEmptyConsumableSlots[ m_CurrentSlotIndex ].Item, m_NonEmptyConsumableSlots[ m_CurrentSlotIndex ].Amount );
		}
		else
		{
			// TODO:: Add if statement to check if the slot should be emptied when 0 stacks is reached 
			m_WheelSlots[ 3 ].RemoveItemFromSlot();
		}
	} 

	private void UpdateVisuals()
	{

	}


	// Changes which item is the currently 
	public void CycleConsumables( int _CycleDirection )
	{
		if ( m_NonEmptyConsumableSlots.Count == 0 )
		{
			Debug.LogWarning( "No shit equipped in the equipment wheel" );
			//return null;
			return;
		}

		m_CurrentSlotIndex += _CycleDirection;
		int NextSlotIndex	= m_CurrentSlotIndex + 1;

		//if ( m_CurrentConsumable < 0 )
		//	m_CurrentConsumable = m_EquippedConsumables.Count;
		//else 
		if ( m_CurrentSlotIndex >= m_NonEmptyConsumableSlots.Count )
		{
			m_CurrentSlotIndex = 0;
			NextSlotIndex = ( m_CurrentSlotIndex + 1 ) % m_NonEmptyConsumableSlots.Count;
		}


		if ( NextSlotIndex >= m_NonEmptyConsumableSlots.Count )
			NextSlotIndex = 0;

		ItemSlot CurrentSlot	= m_NonEmptyConsumableSlots[ m_CurrentSlotIndex ];
		ItemSlot NextSlot		= m_NonEmptyConsumableSlots[ NextSlotIndex ];

		m_WheelSlots[ 3 ].AddItemToSlot( CurrentSlot.Item, CurrentSlot.Amount );

		if ( m_CurrentSlotIndex != NextSlotIndex )
			m_WheelSlots[ 4 ].AddItemToSlot( NextSlot.Item, NextSlot.Amount );

		//return m_WheelSlots[ 3 ].Item;
	}

	public void UseCurrentConsumable( Player _Player )
	{
		if ( !m_WheelSlots[ 3 ].Item )
			return;

		m_WheelSlots[ 3 ].Item.Use( 1, _Player );
	}
}
