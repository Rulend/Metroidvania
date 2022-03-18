using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentManager : MonoBehaviour
{
	private static EquipmentManager		m_Instance;
	public static EquipmentManager		Instance => m_Instance; // Public getter used to access singleton from outside

	public GameObject					m_EquipmentSlotsParent;		// only used once, should not be a member TODO:: Remove
	public InventorySlot[]				m_EquipmentSlots;			// An array of references to all of the inventory slots inside the "EquipmentPanel"-object in the scene.
	public SkinnedMeshRenderer[]		m_EquipmentMeshes;			


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
	/// return value: returns the previously equipped item.
	/// 
	/// parameters:
	/// pr_NewEquipment	: the equipment that should be equipped.
	/// 
	////////////////////////////////////////////////
	public void Equip( Equipment pr_NewEquipment )
	{
		EquipmentSlot	SlotsToCheck		= pr_NewEquipment.EquipmentSlots;
		Equipment		CurrentEquipment	= (Equipment)m_EquipmentSlots[ (int)SlotsToCheck ].Item;
		Inventory		PlayerInventory = GameManager.Instance.rPlayer1.GetInventory;

		// TODO:: Add level or stat requirements here or somewhere else to check whether the character can actually equip the item.


		// If the item we're trying to equip is not already equipped, try to equip it.
		if ( CurrentEquipment != pr_NewEquipment )
		{
			Debug.Log( $"Equipping new item {pr_NewEquipment.name}." );
			m_EquipmentSlots[ (int)pr_NewEquipment.EquipmentSlots ].AddItemToSlot( pr_NewEquipment );

			// Remove newly equipped item from inventory
			PlayerInventory.RemoveItem( pr_NewEquipment, false );
		}
		else	// If the item is already equipped, which means we right clicked it when it's equipped, unequip it.
		{
			Unequip( CurrentEquipment );
		}

		// Add previous equipment to inventory if there was any
		if ( CurrentEquipment )
		{
			PlayerInventory.AddItem( CurrentEquipment );
		}
	}



	public void Unequip( Equipment pr_EquipmentToUnequip )
	{
		m_EquipmentSlots[ (int)pr_EquipmentToUnequip.EquipmentSlots ].RemoveItemFromSlot( false );
	}
}
