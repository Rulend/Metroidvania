using UnityEngine;
using System.Collections.Generic;

public class Character : MonoBehaviour
{
    // Protected member variables
    [SerializeField] protected float	m_MaxHealth;
    protected float						m_CurrentHealth;

//	protected float					m_MaxStamina;
//	protected float					m_CurrentStamina;

//	protected float					m_MaxSpellResource;
//	protected float					m_CurrentSpellResource;

    protected float					m_BaseMovementSpeed;
    protected float					m_CurrentMovementSpeed;

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

	public void Attack()
	{


		// Call TakeDamage for every damage type of the equipment
		//foreach( Damage CurrentDamageType in Weapon.Damages )
		//{
		//	target.TakeDamage( CurrentDamageType );
		//}
	}

    
	public virtual void TakeDamage( Damage pr_IncomingDamage )
	{
		float FinalDamage = pr_IncomingDamage.m_Amount;

		if ( pr_IncomingDamage.m_Type != DamageTypes.DT_TRUEDAMAGE )
			FinalDamage -= m_Resistances[ pr_IncomingDamage.m_Type ];

		if ( FinalDamage < 1.0f ) // used instead of clamp
			FinalDamage = 1.0f;

		m_CurrentHealth -= FinalDamage;
	}

	public void TakeDamage( float pr_IncomingDamage )	// Not really neccessary, but it allows to deal damage to a character without having to create a new Dmage-instance with TrueDamage as the type.
	{
		m_CurrentHealth -= pr_IncomingDamage;
	}


	public virtual void Die()
	{

	}




    // Start is called before the first frame update
    void Start()
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
			{ DamageTypes.DT_POISON		, 2	}
		};

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // FixedUpdate is called 50 times a second
    private void FixedUpdate()
    {
        
    }

}
