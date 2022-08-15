using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu( fileName = "New Consumable", menuName = "Inventory/Consumable" )]
public class Consumable : InventoryItem
{
	[SerializeField] public SkinnedMeshRenderer m_Mesh; // The equipment's mesh - how it looks in the world.
														// TODO:: Add an animation to play here too when using it

	[SerializeField] private ConsumableEffect m_Effect;

	public override void Use()
	{
		base.Use();


		if ( UI_Manager.Instance.rMenu.CurrentState == Menu.EMenuState.EquipmentBrowse )
			EquipmentManager.Instance.Equip( this );
		else
		{
			Debug.Log( "Not adding to slot, but actually using the item." );

			// TODO:: Either remove the character requirement for these calls and use the player reference from game manager, or make the character parameter on all calls up through the chain as well.
			m_Effect.Activate( GameManager.Instance.rPlayer1 );
		}
		// Tries to equip this item. Removes it from inventory if it succeeds, also adds previous items back to inventory. Adds this item back to inventory if it was already equipped.
		//EquipmentManager.Instance.Equip( this );
	}


	private void Awake()
	{
		m_ItemType = ITEMTYPE.ITEMTYPE_CONSUMABLE;
		m_Stackable = true;
	}
}


public class ConsumableEffect : ScriptableObject
{
	public virtual void Activate( Character _Character )
	{

	}
}
