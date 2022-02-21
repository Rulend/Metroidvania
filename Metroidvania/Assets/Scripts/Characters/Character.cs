using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    // Protected member variables
    protected float					m_MaxHealth;
    protected float					m_CurrentHealth;

	protected float					m_MaxSpellResource;
	protected float					m_CurrentSpellResource;

    protected float					m_BaseMovementSpeed;
    protected float					m_CurrentMovementSpeed;


	protected uint m_Strength;
//	protected uint m_Dexterity;
	protected uint m_Intelligence;
//	protected uint m_Endurance;
	protected uint m_Adaptability; // For using throwing weapons that are limited, such as knives, bombs, shuriken, etc. 




    protected CharacterController	m_MovementController;

    

    // Start is called before the first frame update
    void Start()
    {
        m_MovementController = gameObject.GetComponent<CharacterController>();
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
