using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentWheel : MonoBehaviour
{
	private enum EWheelSlots
	{
		Left			,
		Up				,
		Right			,
		Down_1			,	// Down primary
		Down_2			,	// Down secondary
	}


	private		ItemSlot[]				m_WheelSlots;					//	The slots in the bottom left corner of the screen
	private		ItemSlot[]				m_AllConsumableSlots;			//	The bottom row (the potions) when looking at the Equipped screen.
	private		List<ItemSlot>			m_NonEmptyConsumableSlots;		//	List storing slots which have items in them in the Equipped screen.
	private		int						m_DownCurrentSlotIndex;			//	Keeps track of which item is currently equipped in the slot.
//	private		int						m_LeftCurrentSlotIndex;			//	Keeps track of which item is currently equipped in the slot.
//	private		int						m_UpCurrentSlotIndex;			//	Keeps track of which item is currently equipped in the slot.
//	private		int						m_RightCurrentSlotIndex;		//	Keeps track of which item is currently equipped in the slot.

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
			int NextSlotIndex = m_DownCurrentSlotIndex + 1;

			if ( m_DownCurrentSlotIndex < m_NonEmptyConsumableSlots.Count )
			{
				if ( NextSlotIndex >= m_NonEmptyConsumableSlots.Count )
					NextSlotIndex = 0;
			}
			else
			{
				m_DownCurrentSlotIndex = 0;
				NextSlotIndex = 1;
			}


			ItemSlot CurrentSlot	= m_NonEmptyConsumableSlots[ m_DownCurrentSlotIndex ];
			ItemSlot NextSlot		= m_NonEmptyConsumableSlots[ NextSlotIndex ];

			m_WheelSlots[ (int)EWheelSlots.Down_1 ].AddItemToSlot( CurrentSlot.Item, CurrentSlot.Amount );
			m_WheelSlots[ (int)EWheelSlots.Down_2 ].AddItemToSlot( NextSlot.Item, NextSlot.Amount );

		}
		else if ( m_NonEmptyConsumableSlots.Count > 0 )
		{
			m_DownCurrentSlotIndex = 0;
			m_WheelSlots[ (int)EWheelSlots.Down_1 ].AddItemToSlot( m_NonEmptyConsumableSlots[ m_DownCurrentSlotIndex ].Item, m_NonEmptyConsumableSlots[ m_DownCurrentSlotIndex ].Amount );
		}
		else
		{
			// TODO:: Add if statement to check if the slot should be emptied when 0 stacks is reached 
			m_WheelSlots[ (int)EWheelSlots.Down_1 ].RemoveItemFromSlot();
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

		m_DownCurrentSlotIndex += _CycleDirection;
		int NextSlotIndex	= m_DownCurrentSlotIndex + 1;

		//if ( m_CurrentConsumable < 0 )
		//	m_CurrentConsumable = m_EquippedConsumables.Count;
		//else 
		if ( m_DownCurrentSlotIndex >= m_NonEmptyConsumableSlots.Count )
		{
			m_DownCurrentSlotIndex = 0;
			NextSlotIndex = ( m_DownCurrentSlotIndex + 1 ) % m_NonEmptyConsumableSlots.Count;
		}


		if ( NextSlotIndex >= m_NonEmptyConsumableSlots.Count )
			NextSlotIndex = 0;

		ItemSlot CurrentSlot	= m_NonEmptyConsumableSlots[ m_DownCurrentSlotIndex ];
		ItemSlot NextSlot		= m_NonEmptyConsumableSlots[ NextSlotIndex ];

		m_WheelSlots[ (int)EWheelSlots.Down_1 ].AddItemToSlot( CurrentSlot.Item, CurrentSlot.Amount );

		if ( m_DownCurrentSlotIndex != NextSlotIndex )
			m_WheelSlots[ (int)EWheelSlots.Down_2 ].AddItemToSlot( NextSlot.Item, NextSlot.Amount );

		//return m_WheelSlots[ 3 ].Item;
	}

	public void UseCurrentConsumable( Player _Player )
	{
		if ( !m_WheelSlots[ (int)EWheelSlots.Down_1 ].Item )
			return;

		m_WheelSlots[ (int)EWheelSlots.Down_1 ].Item.Use( 1, _Player );
	}
}
