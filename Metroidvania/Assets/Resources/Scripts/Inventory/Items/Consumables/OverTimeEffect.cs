using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu( fileName = "NewOverTimeEffect", menuName = "OverTimeEffect" )]
public class OverTimeEffect : ScriptableObject
{
	public enum EOverTimeEffect
	{
		RestoreHealth	,
		RestoreMana		,
		DecreaseHealth	,
		DecreaseMana	,

		NumEffects
	}

	public delegate void EffectOverTimeDelegate( Character _Character, float _Amount );
	public event EffectOverTimeDelegate m_EffectOverTimeEvent;

	//private Effect m_Effect;


	[Tooltip( "Which effect to apply." )]
	[SerializeField]	private EOverTimeEffect m_EffectType;

	[SerializeField]	private float	m_Duration;
						private float	m_TimeLeft;

	[Tooltip("The interval at which the effect is applied. Ex: 0.16 = 10 times every second.")]
	[SerializeField]	private float	m_EffectCooldownDuration;
						private float	m_EffectCooldownTimeLeft;
	[SerializeField]	private float	m_EffectStrength;

	// TODO:: Change this so you can choose between specifying the power per tick or the total power over the duration

	//[Tooltip("Whether or not the above value is a percentage.")]
	//[SerializeField]	private bool	m_PercentageValue;

	private void Awake()
	{
		switch ( m_EffectType )
		{
			case EOverTimeEffect.RestoreHealth:
				m_EffectOverTimeEvent += Effect.RestoreHealth;
				break;
			case EOverTimeEffect.RestoreMana:
				break;
			case EOverTimeEffect.DecreaseHealth:
				m_EffectOverTimeEvent += Effect.DecreaseHealth;
				break;
			case EOverTimeEffect.DecreaseMana:
				break;
			default:
				break;
		}

		m_TimeLeft = m_Duration;
		m_EffectCooldownTimeLeft = m_EffectCooldownDuration;
	}



	public void Activate( Character _Character )
	{
		_Character.AddOverTimeEffect( this );
	}

// TODO:: Add a choice to add an effect at the end of the duration

	public void TickDownTime( Character _Character )
	{
		m_TimeLeft					-= Time.deltaTime;
		m_EffectCooldownTimeLeft	-= Time.deltaTime;

		if ( m_EffectCooldownTimeLeft < 0.0f )
		{
			m_EffectCooldownTimeLeft = m_EffectCooldownDuration;
			//m_Effect.ApplyEffect( _Character, m_EffectStrength );
			m_EffectOverTimeEvent.Invoke( _Character, m_EffectStrength );

			if ( m_TimeLeft < 0.0f )
			{
				_Character.RemoveOverTimeEffect( this );
			}
		}
	}
}


// TODO:: Make this into it's own file
//public interface Effect
//{
//	public void ApplyEffect( Character _Character, float _Amount );
//}


//public class RestoreHealth : Effect
//{
//	public void ApplyEffect( Character _Character, float _Amount )
//	{
//		_Character.Heal( _Amount );
//	}
//}

public class Effect
{
	public static void RestoreHealth( Character _Character, float _Amount )
	{
		_Character.Heal( _Amount );
	}

	public static void DecreaseHealth( Character _Character, float _Amount )
	{
		Damage NewDamage = new Damage();
		NewDamage.m_Type = DamageTypes.DT_TRUEDAMAGE;
		NewDamage.m_Amount = _Amount;

		_Character.TakeDamage( NewDamage );
	}
}
