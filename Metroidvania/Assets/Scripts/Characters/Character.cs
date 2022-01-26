using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    // Protected member variables
    protected ushort  m_MaxHealth;
    protected short   m_CurrentHealth;

    protected float m_BaseMovementSpeed;
    protected float m_CurrentMovementSpeed;

    protected CharacterController m_MovementController;

    

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
