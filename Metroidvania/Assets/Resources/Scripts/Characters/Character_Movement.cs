using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character_Movement : MonoBehaviour
{
	// < Member variables>
	[SerializeField] protected LayerMask	m_WhatIsGround;      // A mask determining what is ground to the character. Used to optimize collision detection
	[SerializeField] protected float		m_BaseMovementSpeed;
	[SerializeField] protected float		m_CurrentMovementSpeed;
	[SerializeField] protected float		m_Gravity = 6.0f;

	protected Vector3		m_MovementVelocity = new Vector3( 0.0f, 0.0f, 0.0f );
	protected bool			m_Grounded;
	protected bool			m_FacingRight = true;  // For determining the currently faced position

	// Rigidbody used for movement
	protected Rigidbody		m_rRigidbody;
	protected Vector3		m_KnockBackVelocity;
	protected Vector3		m_GravityVelocity = new Vector3( 0.0f, 0.0f, 0.0f );

	// Movement variables.
	protected Vector3		m_MovementDirection;
	protected Vector3		m_GroundNormal; // Not needed by flying characters, but whatever

	// Animator:
	protected Animator		m_rAnimator;


	protected virtual void Awake()
	{
		m_rRigidbody = GetComponent<Rigidbody>();
		m_rAnimator = GetComponent<Animator>();

	}
}
