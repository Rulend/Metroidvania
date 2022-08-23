using UnityEngine;
using System.Collections.Generic;

public class Character : MonoBehaviour
{
	public float MaxHealth => m_MaxHealth;


    // Protected member variables
    [SerializeField] protected float	m_MaxHealth;
    protected float						m_CurrentHealth;

//	protected float					m_MaxStamina;
//	protected float					m_CurrentStamina;

	[SerializeField] protected float	m_MaxMana;
	protected float						m_CurrentMana;

	[SerializeField] protected float	m_BaseMovementSpeed;
    protected float						m_CurrentMovementSpeed;

//	protected uint					m_Strength;		// Increase sword damage.
//	protected uint					m_Dexterity;	// Increase bow damage.
//	protected uint					m_Intelligence;	// Increase magic damage.
//	protected uint					m_Endurance;	// For increasing stamina and health.
//	protected uint					m_Adaptability; // For using throwing weapons that are limited, such as knives, bombs, shuriken, etc. 

	Dictionary<DamageTypes, float> m_Resistances;

//	protected int					m_PhysicalResistance;
//	protected int					m_MagicalResistance;
//	protected int					m_FireResistance;
//	protected int					m_WaterResistance;
//	protected int					m_WindResistance;
//	protected int					m_EarthResistance;
//	protected int					m_LightningResistance;
//	protected int					m_PoisonResistance;
//	protected uint					m_EquippedWeightCapacity;


//	protected CharacterController	m_MovementController;	

	// Take in an attack-struct as a parameter. That struct should have the following:
	// 1. Delay in frames before attack trace starts
	// 2. Two or more points that can be traced between in ord er to check if a target has been hit
	// 3. 

	private HashSet<OverTimeEffect> m_OverTimeEffects;
	//private OverTimeEffect[] m_OverTimeEffects;
	private List<OverTimeEffect>	m_ExpiredEffects; // Effects that have gone through their duration or been cut short, so they should be removed from the list. Has to be a separate list, since removing right away would break enumeration of the list being looped through.

	public void Attack()
	{


		// Call TakeDamage for every damage type of the equipment
		//foreach( Damage CurrentDamageType in Weapon.Damages )
		//{
		//	target.TakeDamage( CurrentDamageType );
		//}
	}


	public virtual void RestoreHealth( float _Amount )
	{
		if ( m_CurrentHealth <= 0 ) // Maybe unnecessary if death is handled in a good way. TODO:: Remove(?)
			return;

		m_CurrentHealth += _Amount;

		if ( m_CurrentHealth > m_MaxHealth )
			m_CurrentHealth = m_MaxHealth;
	}



	public virtual void RestoreMana( float _Amount )
	{
		if ( m_CurrentMana <= 0 ) // Maybe unnecessary if death is handled in a good way. TODO:: Remove(?)
			return;

		m_CurrentMana += _Amount;

		if ( m_CurrentMana > m_MaxMana )
			m_CurrentMana = m_MaxMana;
	}



	public virtual void TakeDamage( Damage _IncomingDamage )
	{
		float FinalDamage = _IncomingDamage.m_Amount;

		if ( _IncomingDamage.m_Type != DamageTypes.DT_TRUEDAMAGE )
			FinalDamage -= m_Resistances[ _IncomingDamage.m_Type ];

		//if ( FinalDamage < 0.05f ) // used instead of clamp
		//	FinalDamage = 0.05f;

		m_CurrentHealth -= FinalDamage;

		if ( m_CurrentHealth <= 0 )
			Die();
	}



	public virtual void Die()
	{
		Debug.Log( $"{gameObject.name} has died." );
	}



    // Start is called before the first frame update
    public virtual void Start()
    {
		//		m_MovementController = gameObject.GetComponent<CharacterController>();
		m_Resistances = new Dictionary<DamageTypes, float>()
		{
			{ DamageTypes.DT_PHYSICAL	, 2	},
			{ DamageTypes.DT_MAGICAL	, 2	},
			{ DamageTypes.DT_FIRE		, 2	},
			{ DamageTypes.DT_WATER		, 2	},
			{ DamageTypes.DT_WIND		, 2	},
			{ DamageTypes.DT_EARTH		, 2	},
			{ DamageTypes.DT_LIGHTNING	, 2	},
			{ DamageTypes.DT_POISON		, 2	},
			{ DamageTypes.DT_TRUEDAMAGE	, 0	}
		};

		m_OverTimeEffects	= new HashSet<OverTimeEffect>();
		m_ExpiredEffects	= new List<OverTimeEffect>();

		// Use this and a variable in the effect so see whether or not it is stackable, and also see which one is more powerful to decide whether or not to use the new effect when supplied with one
		//m_OverTimeEffects = new OverTimeEffect[ (int)OverTimeEffect.EOverTimeEffect.NumEffects ];
	}

	public void AddOverTimeEffect( OverTimeEffect _EffectToAdd )
	{
		OverTimeEffect EffectInstance = Object.Instantiate<OverTimeEffect>( _EffectToAdd );
		//OverTimeEffect EffectInstance = new OverTimeEffect(  );

		EffectInstance.Activate( this );

		m_OverTimeEffects.Add( EffectInstance );
	}

	public void RemoveOverTimeEffect( OverTimeEffect _EffectToRemove )
	{
		m_ExpiredEffects.Add( _EffectToRemove );
	}

	private void RemoveExpiredEffects()
	{
		foreach ( OverTimeEffect CurrentEffect in m_ExpiredEffects )
			m_OverTimeEffects.Remove( CurrentEffect );

		m_ExpiredEffects.Clear();
	}

	// Update is called once per frame
	public virtual void Update()
    {
		foreach ( var CurrentEffect in m_OverTimeEffects )
			CurrentEffect.TickDownTime( this );

		RemoveExpiredEffects();
	}

    // FixedUpdate is called 50 times a second
    private void FixedUpdate()
    {
        
    }

}
