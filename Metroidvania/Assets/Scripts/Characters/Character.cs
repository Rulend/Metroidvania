using UnityEngine;

public class Character : MonoBehaviour
{
    // Protected member variables
    protected float					m_MaxHealth;
    protected float					m_CurrentHealth;

//	protected float					m_MaxStamina;
//	protected float					m_CurrentStamina;

//	protected float					m_MaxSpellResource;
//	protected float					m_CurrentSpellResource;

    protected float					m_BaseMovementSpeed;
    protected float					m_CurrentMovementSpeed;

//	protected uint					m_Strength;
//	protected uint					m_Dexterity;
//	protected uint					m_Intelligence;
//	protected uint					m_Endurance;
//	protected uint					m_Adaptability; // For using throwing weapons that are limited, such as knives, bombs, shuriken, etc. 

//	protected int					m_PhysicalResistance;
//	protected int					m_MagicalResistance;
//	protected int					m_FireResistance;
//	protected int					m_WaterResistance;
//	protected int					m_WindResistance;
//	protected int					m_EarthResistance;
//	protected int					m_LightningResistance;
//	protected int					m_NatureResistance;
//	protected int					m_PoisonResistance;
//	protected uint					m_EquippedWeightCapacity;


//	protected CharacterController	m_MovementController;	

	// Take in an attack-struct as a parameter. That struct should have the following:
	// 1. Delay in frames before attack trace starts
	// 2. Two or more points that can be traced between in ord er to check if a target has been hit
	// 3. 

	public void Attack()
	{

	}

    
	public void TakeDamage( float pr_IncomingDamage )
	{

		float FinalDamage = pr_IncomingDamage;


		// Reduce FinalDamage by stats gained from levels and equipment here.


		m_CurrentHealth -= FinalDamage;
	}


	public void Die()
	{

	}




    // Start is called before the first frame update
    void Start()
    {
//		m_MovementController = gameObject.GetComponent<CharacterController>();
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
