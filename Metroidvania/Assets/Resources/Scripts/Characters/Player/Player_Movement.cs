using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Movement : Character_Movement
{
	
	// < Member variables>
	// Used for measuring fall damage
	private float m_DistanceFallen;
	[SerializeField] private float m_FallDamageThreshold = 10.0f; // Distance fallen before damage should be dealt

	// The player's jump
	[SerializeField] private float	m_MaxJumpHeight			= 3.3f;
	[SerializeField] private float	m_JumpWindowDuration	= 0.214f; // Static as this value only needs to be set once
	private float					m_JumpWindowTimeLeft	= 0.214f;
	private const float				m_CoyoteDuration		= 0.2f;
	private float					m_CoyoteTimeLeft		= 0.2f;

	// The player's slide move.
	private float					m_SlideCooldownDuration	= 0.75f;
	private float					m_SlideCooldownTimeLeft	= 0.0f;
	[SerializeField] private float	m_SlideDuration			= 0.5f;
	[SerializeField] private float	m_SlideDistanceTotal	= 2.0f;
	private float					m_SlideDistanceLeft		= 0.0f;
	private Vector3					m_SlideDirection		= Vector3.zero;


	// References to other components
	private Player m_rPlayer;

	// Hurtboxes - used for detecting hits.
	[SerializeField] private BoxCollider m_FullBodyCollider;
	[SerializeField] private BoxCollider m_SlideCollider;


	// State machine for player
	private enum EPlayerState
	{
		PlayerState_Idle	= 0	,
		PlayerState_Running		,
		PlayerState_Rolling		,
		PlayerState_Jumping		,
		PlayerState_Falling		,
		PlayerState_Sliding		,
	}


	private EPlayerState m_CurrentState;
	private EPlayerState m_PreviousState;
	// </End of Member variables>


	protected override void Awake()
	{
		base.Awake();

		m_BaseMovementSpeed		= 350.0f;
		m_CurrentMovementSpeed	= m_BaseMovementSpeed;

		m_rPlayer = GetComponent<Player>();
	}

	private void Update()
	{
		float DeltaTime = Time.deltaTime;

		//m_CoyoteTimeLeft			-= DeltaTime;	// Only needs to be decreased inside STATE_Falling.
		m_SlideCooldownTimeLeft		-= DeltaTime;
	}


    private void FixedUpdate()
    {
		// Add -9.82 multiplied by a modifiable gravity-variable multiplied by time.fixedDeltaTime to make the player fall at a good speed.
		m_GravityVelocity.y += m_Gravity * ( -9.82f ) * Time.fixedDeltaTime;
		m_GravityVelocity.y = Mathf.Clamp( m_GravityVelocity.y, -50.0f, 20.0f );

		// Set to false if player is Jumping, set to true if collision is found underneath the player
		m_Grounded = CheckForGround();
		m_rAnimator.SetBool( "Grounded", m_Grounded );

		if ( m_Grounded )
		{
			m_GravityVelocity.y		= 0.0f;
			m_JumpWindowTimeLeft	= m_JumpWindowDuration;
			m_CoyoteTimeLeft		= m_CoyoteDuration;
		}



		// Temporarily like this until I make it better
		switch ( m_CurrentState )
		{
			case EPlayerState.PlayerState_Idle:
				{
					// Idle animation will play.
				}
				break;

			case EPlayerState.PlayerState_Running:
				{
					// Running animation will play.
				}
				break;

			case EPlayerState.PlayerState_Rolling:
				{

				}
				break;

			case EPlayerState.PlayerState_Jumping:
				{
					m_JumpWindowTimeLeft -= Time.fixedDeltaTime;

					m_GravityVelocity.y = 0.0f; // Set velocity to 0 so player isn't pushed down
					m_rRigidbody.AddForce(new Vector3(0.0f, (m_MaxJumpHeight / m_JumpWindowDuration), 0.0f), ForceMode.VelocityChange); // Use velocity in order to move the player upwards // This way sucks, because the jump becomes really floaty

					if (m_JumpWindowTimeLeft <= 0.0f)
						SetState(EPlayerState.PlayerState_Falling);
				}
				break;

			case EPlayerState.PlayerState_Falling:
				{
					m_CoyoteTimeLeft -= Time.fixedDeltaTime; // Decrease this in here

					m_DistanceFallen += -0.01f * m_GravityVelocity.y;   // Will revisit this later revisit this amount later, for now it works.

					// TODO: Remove this, it ain't a good way to use a state machine.
					if ( m_Grounded )
					{
						if ( m_DistanceFallen > m_FallDamageThreshold )
						{
							Damage FallDamage = new Damage();
							FallDamage.m_Type = DamageTypes.DT_TRUEDAMAGE;
							FallDamage.m_Amount = (m_DistanceFallen - m_FallDamageThreshold);

							m_rPlayer.TakeDamage( FallDamage );   // Take damage equal to distance fallen - the threshold
							Debug.Log($"Fell { m_DistanceFallen } and took { m_DistanceFallen - m_FallDamageThreshold } damage from Falling. \n");
						}

						m_DistanceFallen = 0.0f;
						SetState(EPlayerState.PlayerState_Idle);
					}
				}
				break;

			case EPlayerState.PlayerState_Sliding:
				{
					if ( m_SlideDistanceLeft > 0.0f )
					{
						Slide();
						m_FullBodyCollider.enabled = false;
					}
					else
					{
						SetState(EPlayerState.PlayerState_Idle);
						m_FullBodyCollider.enabled = true;
					}

				}
				break;

			default:
				break;
		}

		if ( m_GravityVelocity.y < 0.0f )
			SetState( EPlayerState.PlayerState_Falling );

		m_rRigidbody.velocity = ( m_MovementVelocity + m_GravityVelocity );
	}


	private void SetState( EPlayerState _State )
	{
		m_PreviousState = m_CurrentState;
		m_CurrentState	= _State;

		// Switch on previous state
		switch( m_PreviousState )
		{
			case EPlayerState.PlayerState_Idle:
				{

				}
				break;

			case EPlayerState.PlayerState_Running:
				{

				}
				break;

			case EPlayerState.PlayerState_Jumping:
				{

					m_rAnimator.SetBool( "Jumping", false ); // Has to be set here instead of above, because this state can also be entered by releasing the space key
				}
				break;

			case EPlayerState.PlayerState_Falling:
				{
					m_rAnimator.SetBool( "Falling", false );
				}
				break;

			case EPlayerState.PlayerState_Sliding:
				{
					m_rAnimator.SetBool( "Sliding", false );

					m_SlideCollider.enabled		= false;
					m_FullBodyCollider.enabled	= true;
				}
				break;
		}

		// Switch on new state
		switch ( _State )
		{
			case EPlayerState.PlayerState_Idle:
				{
				}
				break;

			case EPlayerState.PlayerState_Running:
				{
				}
				break;

			case EPlayerState.PlayerState_Jumping:
				{
					m_rAnimator.SetBool( "Jumping", true );
				}
				break;

			case EPlayerState.PlayerState_Falling:
				{
					m_rAnimator.SetBool( "Falling", true );
				}
				break;

			case EPlayerState.PlayerState_Sliding:
				{
					m_SlideCooldownTimeLeft		= m_SlideCooldownDuration;
					m_SlideDistanceLeft			= m_SlideDistanceTotal;
					m_CurrentState				= EPlayerState.PlayerState_Sliding;
					m_FullBodyCollider.enabled	= false;
					m_SlideCollider.enabled		= true;

				}
				break;
		}

	}


	public void StartSlide()
    {
		if ( m_SlideCooldownTimeLeft > 0.0f )
			return;

		SetState( EPlayerState.PlayerState_Sliding );
    }

	private void Slide()
	{
		// Change this formula later, the dash should have more speed during ~80% of the duration, and close to the end it should halt quickly.
		m_SlideDistanceLeft -= m_SlideDistanceTotal / ( m_SlideDuration / Time.fixedDeltaTime );

		Vector3 DesiredSlidePosition = m_SlideDistanceLeft * m_SlideDirection;
		DesiredSlidePosition.z = 0.0f;

		//m_rAnimator.SetFloat( "Movement", Mathf.Abs( pr_LRInput ), 0.05f, Time.deltaTime );   // TODO: Switch the string out for ID later.
		m_rRigidbody.AddForce( DesiredSlidePosition * 50.0f, ForceMode.VelocityChange );
	}


	// Timers that always need to be decreased, regardless of state
	private void DecreaseTimersDT( float pr_DeltaTime )
	{
//		m_CoyoteTimeLeft			-= pr_DeltaTime;	// Only needs to be decreased inside STATE_Falling.
		m_SlideCooldownTimeLeft		-= pr_DeltaTime;
	}


	public void Move( float pr_LRInput )
    {
		if ( m_KnockBackVelocity.x != 0.0f )
			return;

		// Turning the character the correct way is the highest priority
		if ( pr_LRInput < 0.0f && m_FacingRight )
            Flip();
        else if ( pr_LRInput > 0.0f && !m_FacingRight )
            Flip();

		m_MovementDirection = Vector3.Cross( ( new Vector3( 0.0f, 0.0f, -pr_LRInput ) ), m_GroundNormal );

		// Update animator and apply movement
		m_rAnimator.SetFloat( "Moving", Mathf.Abs( pr_LRInput ), 0.05f, Time.deltaTime );			// TODO: Switch the string out for ID later.
		m_MovementVelocity = m_MovementDirection * m_CurrentMovementSpeed * Time.fixedDeltaTime;	// Rigidbody way

		//m_rRigidbody.velocity = ( m_MovementVelocity + m_GravityVelocity );
		//m_rRigidbody.MovePosition()
	}



    private void Flip()
    {
        // Switch the way the player is labelled as facing.
        m_FacingRight = !m_FacingRight;

        // My own version of rotating, instead of changing the scale as above.
        float CurrentYRotation = gameObject.transform.localRotation.y;

        Quaternion DesiredRotation = new Quaternion( gameObject.transform.localRotation.x, -CurrentYRotation, gameObject.transform.localRotation.z, gameObject.transform.localRotation.w );

        gameObject.transform.localRotation = DesiredRotation;
    }



	public void Jump()
	{
		if ( m_CoyoteTimeLeft > 0.0f )
		{
			SetState( EPlayerState.PlayerState_Jumping );
			m_CoyoteTimeLeft	= 0.0f;
		}
	}



	public void StopJump()
	{
		if ( m_CurrentState == EPlayerState.PlayerState_Jumping )
			SetState( EPlayerState.PlayerState_Falling ); 
	}


	// TODO:: Rework this mess
    bool CheckForGround()
    {
		m_GroundNormal = Vector3.up;

		if ( m_CurrentState == EPlayerState.PlayerState_Jumping )
			return false;

		// if ( DownInput && Down.Raycast == fallthrough )
		//		return false;

		Vector3 RayStartPos		= transform.position + new Vector3( 0.0f, 0.5f, 0.0f ); 
        float RayMaxDistance	= 0.55f;

#if UNITY_EDITOR
		Vector3 DebugRayStart = transform.position + new Vector3( 0.0f, 0.5f, 0.0f );
        Vector3 DebugRayEnd   = transform.position + new Vector3( 0.0f, -0.05f, 0.0f ); // Just for drawing purposes, max length of raycast has to be changed in if-statement
        Debug.DrawLine( DebugRayStart, DebugRayEnd, Color.red );
#endif

		RaycastHit	RaycastHit;
        Ray			GroundCheckRay = new Ray( RayStartPos, Vector3.down );

		// If ground is detected directly underneath the player:
		if ( Physics.Raycast( GroundCheckRay, out RaycastHit, RayMaxDistance, m_WhatIsGround ) )
		{
			m_GroundNormal		= RaycastHit.normal;
			m_SlideDirection	= Vector3.Cross( transform.right.normalized, RaycastHit.normal );

			return true;
		}

		// Add offset to ray, in order to check slightly BEHIND the player for ground collision.
		RayStartPos = transform.position + new Vector3( 0.0f, 0.5f, 0.0f ) + ( ( m_SlideCollider.size.z / 2.0f - 0.01f ) * -transform.forward );

#if UNITY_EDITOR
		DebugRayStart	= RayStartPos;
		DebugRayEnd		= RayStartPos - new Vector3( 0.0f, RayMaxDistance, 0.0f );
		Debug.DrawLine( DebugRayStart, DebugRayEnd, Color.blue );
#endif

		GroundCheckRay = new Ray( RayStartPos, Vector3.down );

		if ( Physics.Raycast( GroundCheckRay, out RaycastHit, RayMaxDistance, m_WhatIsGround ) ) // if no ground directly underneath, check behind
		{
			m_GroundNormal = RaycastHit.normal;
			m_SlideDirection	= Vector3.Cross( transform.right.normalized, RaycastHit.normal );
			return true;
		}

		// Add offset to ray, in order to check slightly in FRONT of the player for ground collision.
		RayStartPos = transform.position + new Vector3( 0.0f, 0.5f, 0.0f ) + ( ( m_SlideCollider.size.z / 2.0f - 0.01f) * transform.forward );

#if UNITY_EDITOR
		DebugRayStart	= RayStartPos;
		DebugRayEnd		= RayStartPos - new Vector3( 0.0f, RayMaxDistance, 0.0f );
		Debug.DrawLine( DebugRayStart, DebugRayEnd, Color.yellow );
#endif

		GroundCheckRay = new Ray( RayStartPos, Vector3.down );

		if ( Physics.Raycast( GroundCheckRay, out RaycastHit, RayMaxDistance, m_WhatIsGround ) ) // if no ground directly underneath, check in front
		{
			m_GroundNormal		= RaycastHit.normal;
			m_SlideDirection	= Vector3.Cross( transform.right.normalized, RaycastHit.normal );
			return true;
		}

		return false;
    }


}
