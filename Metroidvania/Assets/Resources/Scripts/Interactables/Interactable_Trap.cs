using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable_Trap : Interactable
{

	//[SerializeField] private OverTimeEffect m_EffectsToApply;
	[SerializeField] private OverTimeEffect m_EffectToApply;

	public override void Interact()
	{
		base.Interact();

		//foreach ( OverTimeEffect CurrentEffect in m_EffectsToApply )
		//m_rPlayer.AddOverTimeEffect( Object.Instantiate<OverTimeEffect>( CurrentEffect ) );
		//m_rPlayer.AddOverTimeEffect( CurrentEffect );
		m_rPlayer.AddOverTimeEffect( m_EffectToApply );
	}
}
