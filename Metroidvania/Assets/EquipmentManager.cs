using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentManager : MonoBehaviour
{
	private static EquipmentManager		m_Instance;
	public static EquipmentManager		Instance => m_Instance; // Public getter used to access singleton from outside

	Equipment[] m_CurrentEquipment;


	private void Awake()
	{
		// Will set this to be the instance the first time, and delete any later attempts to create more.
		if ( m_Instance && m_Instance != this )
		{
			Destroy( this.gameObject );
		}
		else
		{
			m_Instance = this;
		}
	}

	private void Start()
	{
		m_CurrentEquipment = new Equipment[ (int)EquipmentSlot.EQUIPMENTSLOT_SIZE ];
	}

	////////////////////////////////////////////////
	/// Function information - Equip
	/// 
	/// Equips an item in the item's specified slot.
	/// 
	/// parameters:
	/// pr_NewEquipment	: the equipment that should be equipped.
	/// 
	////////////////////////////////////////////////
	public void Equip( Equipment pr_NewEquipment )
	{
		EquipmentSlot	SlotsToCheck		= pr_NewEquipment.EquipmentSlots;
		Equipment		PreviousEquip	= m_CurrentEquipment[ (int)SlotsToCheck ];

		if ( PreviousEquip && !PreviousEquip.m_DefaultItem ) // If there was already an item equipped, and it was not a default item, put it in inventory
		{
			GameManager.Instance.rPlayer1.GetInventory.AddItem( PreviousEquip );
		}

		m_CurrentEquipment[ (int)pr_NewEquipment.EquipmentSlots ] = pr_NewEquipment;
	}

}
