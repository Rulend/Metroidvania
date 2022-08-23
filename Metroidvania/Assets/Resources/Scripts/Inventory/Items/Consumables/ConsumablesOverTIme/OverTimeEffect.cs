using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu( fileName = "NewOverTimeEffect", menuName = "OverTimeEffect" )]
public class OverTimeEffect : ConsumableEffect
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
	[SerializeField] private EOverTimeEffect			m_EffectType;

	[SerializeField]					private float	m_Duration;
	[SerializeField] [HideInInspector]	private float	m_TimeLeft;

	[Tooltip("The interval at which the effect is applied. Ex: 0.16 = 10 times every second.")]
	[SerializeField]					private float	m_EffectCooldownDuration;
	[SerializeField] [HideInInspector]	private float	m_EffectCooldownTimeLeft;

	[Tooltip( "How much the effect will be in total. Use numbers between 0-1 in order to make it percentage based. Effect is applied evenly over the duration." )]
	[SerializeField]					private float	m_TotalEffectStrength;
	[SerializeField] [HideInInspector]	private float	m_EffectPerTick; // How strong the effect is per tick. 

	// TODO:: Change this so you can choose between specifying the power per tick or the total power over the duration


	// This method is called from within Character.cs. All you have to do in order to use an OverTimeEffect is call ChosenCharacter.AddOverTimeEffect( OTE_To_Add ), and everything else will fix itself.
	public override void Activate( Character _Affected )
	{
		float m_AmountAffectedStat = 0.0f;

		switch ( m_EffectType )
		{
			case EOverTimeEffect.RestoreHealth:
				m_EffectOverTimeEvent += Effect.RestoreHealth;

				m_AmountAffectedStat = _Affected.MaxHealth;
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

		if ( m_TotalEffectStrength < 1 )
			m_TotalEffectStrength = m_AmountAffectedStat * m_TotalEffectStrength;

		// If EfectCooldownDuration is left at 0 when creating an OTE, then it should be applied every frame. To do that, we need to substitute it's value for the framerate.
		float DivisionValue = ( m_EffectCooldownDuration == 0.0f ? Time.deltaTime : m_EffectCooldownDuration );

		m_EffectPerTick = m_TotalEffectStrength / ( m_Duration / DivisionValue );
	}

// TODO:: Add a choice to add an effect at the end of the duration

	public void TickDownTime( Character _Character )
	{
		m_TimeLeft					-= Time.deltaTime;
		m_EffectCooldownTimeLeft	-= Time.deltaTime;

		if ( m_EffectCooldownTimeLeft < 0.0f )
		{
			m_EffectCooldownTimeLeft = m_EffectCooldownDuration;

			m_EffectOverTimeEvent.Invoke( _Character, m_EffectPerTick );

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
		_Character.RestoreHealth( _Amount );
	}

	public static void DecreaseHealth( Character _Character, float _Amount )
	{
		Damage NewDamage	= new Damage();
		NewDamage.m_Type	= DamageTypes.DT_TRUEDAMAGE;
		NewDamage.m_Amount	= _Amount;

		_Character.TakeDamage( NewDamage );
	}

	//public static void RestoreMana( Character _Character, float _Amount )
	//{
	//	_Character.RestoreMana( _Amount );
	//}

	//public static void DecreaseMana( Character _Character, float _Amount )
	//{
	//	_Character.RestoreMana( _Amount );
	//}
}
