using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentManager : MonoBehaviour
{
	private static EquipmentManager		m_Instance;
	public static EquipmentManager		Instance => m_Instance; // Public getter used to access singleton from outside

	public GameObject m_EquipmentSlotsParent;   // only used once, should not be a member
	public InventorySlot[] m_EquipmentSlots;    // An array of references to all of the inventory slots inside the "InventoryPanel"-object in the scene.


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
		m_EquipmentSlots = m_EquipmentSlotsParent.GetComponentsInChildren<InventorySlot>();
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
	public bool Equip( Equipment pr_NewEquipment )
	{
		EquipmentSlot	SlotsToCheck	= pr_NewEquipment.EquipmentSlots;
		Equipment		CurrentEquip	= (Equipment)m_EquipmentSlots[ (int)SlotsToCheck ].Item;

		if ( CurrentEquip && !CurrentEquip.m_DefaultItem ) // If there was already an item equipped, and it was not a default item, put it in inventory
		{
			GameManager.Instance.rPlayer1.GetInventory.AddItem( CurrentEquip );
			Debug.Log( $"Adding {CurrentEquip.name} to inventory." );
		}

		// TODO:: Add level or stat requirements here or somewhere else to check whether the character can actually equip the item.

		// If the item we're trying to equip is not already equipped, equip it.
		if ( CurrentEquip != pr_NewEquipment )
		{
			Debug.Log( $"Equipping new item {pr_NewEquipment.name}." );
			m_EquipmentSlots[ (int)pr_NewEquipment.EquipmentSlots ].AddItemToSlot( pr_NewEquipment );
			return true;
		}
		else	// If the item is already equipped, which means we right clicked it when it's equipped, unequip it.
		{
			Unequip( CurrentEquip );
			return false;
		}

		// security
		return false;
	}

	public void Unequip( Equipment pr_EquipmentToUnequip )
	{
		m_EquipmentSlots[ (int)pr_EquipmentToUnequip.EquipmentSlots ].RemoveItemFromSlot( false );
	}

}
