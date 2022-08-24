using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu( fileName = "New Consumable", menuName = "Inventory/Consumable" )]
public class Consumable : InventoryItem
{
	public enum EConsumableEffect
	{
		EFFECT_OverTimeEffect	,
		EFFECT_ThrowItem		,
	}


	[SerializeField] public SkinnedMeshRenderer m_Mesh; // The equipment's mesh - how it looks in the world.
														// TODO:: Add an animation to play here too when using it

	[SerializeField] private ConsumableEffect m_Effect;

	public override void Use( int _Amount, Character _User )
	{
		base.Use( _Amount );


		if ( UI_Manager.Instance.rMenu.CurrentState == Menu.EMenuState.EquipmentBrowse )
			EquipmentManager.Instance.Equip( this, _Amount, true );
		else
		{
			Debug.Log( $"Using consumable {m_ItemName}." );

			// NOTE:: I know this code underneath is a shit way to do this, but hear me out: this is the best way.
			// This cast is safe, since OverTimEffect inherits from ConsumableEffect, and by doing this, we get to skip repeating a switch statement inside OverTimeEffect.
			// If we instead wanted to add the OverTimeEffect to the target inside that class itself, it would require us to implement its Awake-method, in which we would have to switch
			// on the type of effect it is. This is bad, since we are already doing a switch like that inside the Activate function, which we need to do if we want the effect to scale with 
			// a percentage of any kind. So to summarize: doing it this way lets us skip implementing the awake method of OverTimeEffect, where we would need an unnecessary switch statement on the type.

			if ( m_Effect as OverTimeEffect != null )
				_User.AddOverTimeEffect( m_Effect as OverTimeEffect );
			else
				m_Effect.Activate( _User );

			GameManager.Instance.rPlayer1.GetInventory.RemoveItem( this, false );
			EquipmentManager.Instance.EquipWheel.UpdateWheel();
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


public abstract class ConsumableEffect : ScriptableObject
{
	public abstract void Activate( Character _Affected );
}
