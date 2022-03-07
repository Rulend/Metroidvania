using UnityEngine;
using UnityEngine.InputSystem;

public class Player : Character
{
	// < Member variables>
	[SerializeField] private LayerMask m_WhatIsGround;                          // A mask determining what is ground to the character

	public Vector3 m_PositionToApply;

//	private Vector3 m_Velocity = new Vector3( 0.0f, 0.0f, 0.0f );
	private Vector3 m_MovementVelocity = new Vector3( 0.0f, 0.0f, 0.0f );
	[SerializeField] private float m_Gravity = 6.0f;

	private bool m_Grounded;			// Whether or not the player is grounded.
	private bool m_FacingRight = true;  // For determining which way the player is currently facing.

	// Used for measuring fall damage
	private float m_DistanceFallen;

	// Player's stats
	// Derived from Character

	// Rigidbody used for movement
	Rigidbody m_Rigidbody;
	Vector3 m_KnockBackVelocity;
	//Vector3 m_MovementVelocity;
	[SerializeField] private Vector3 m_GravityVelocity = new Vector3(0.0f, 0.0f, 0.0f);


	// Movement variables.
	private bool	m_ActiveInput = true;
	private float	m_LRInput = 0.0f;
	private float	m_UDInput = 0.0f;

	private Vector3 m_MovementDirection;
	private Vector3 m_GroundNormal;

	// The player's jump
	[SerializeField] private float m_MaxJumpHeight		= 3.3f;
	private bool	m_JumpWindowActive					= false;
	[SerializeField] private float m_JumpWindowDuration = 0.214f; // Static as this value only needs to be set once
	private float	m_JumpWindowTimeLeft				= 0.214f;
	private float	m_CoyoteDuration					= 0.2f;
	private float	m_CoyoteTimeLeft					= 0.2f;

	// The player's slide move.
	private float	m_SlideCooldownDuration				= 0.75f;
	private float	m_SlideCooldownTimeLeft				= 0.0f;
	[SerializeField] private float m_SlideDuration		= 0.5f;
	[SerializeField] private float m_SlideDistanceTotal	= 2.0f;
	private float	m_SlideDistanceLeft					= 0.0f;
	private Vector3 m_SlideDirection					= Vector3.zero;

	// The currently focused interactable.
	[SerializeField] private Interactable m_CurrentlyFocusedInteractable;

	[SerializeField] private Inventory m_Inventory;
	public Inventory GetInventory => m_Inventory;

	[SerializeField] private GameObject m_InventoryUI;

	public GameObject InventoryUI { get { return m_InventoryUI; } }

	// Used for controlling stuff, move to controller script later.
	[SerializeField] private InputActionAsset	m_InputActionAsset;

	private InputActionMap	m_ActionMap; // The current actionmap. See to the right how to ser up an array //new InputActionMap[ (int)ControlTypes.CONTROLTYPE_SIZE ];	// The different kinds of actionmaps used.
	private InputAction[]	m_InputActions; // The different actions for the action map.

	// Used for controlling the character's animations
	[SerializeField] private Animator m_Animator;

	// Hurtboxes - used for detecting hits.
	[SerializeField] private BoxCollider m_UpperCollider;
	[SerializeField] private BoxCollider m_LowerCollider;


	// State machine for player
	private enum PlayerStates
	{
		PLAYERSTATE_IDLE	= 0	,
		PLAYERSTATE_RUNNING		,
		PLAYERSTATE_JUMPING		,
		PLAYERSTATE_FALLING		,
		PLAYERSTATE_SLIDING		,
	}

	// State machine for the controls
	private enum ControlTypes : int // Used to decide which InputActionMap to use. 
	{
		CONTROLTYPE_DEFAULT = 0	,
		CONTROLTYPE_NOATTACK	,
		CONTROLTYPE_NOJUMP		,
		CONTROLTYPE_CUTSCENE	,


		CONTROLTYPE_SIZE,
	}

	enum GameplayActions :int
	{
		GAMEPLAYACTIONS_MOVE	= 0	,
		GAMEPLAYACTIONS_JUMP		,
		GAMEPLAYACTIONS_INTERACT	,
		//GAMEPLAYACTIONS_SLIDE		, // Slide is registered as a jump performed while pressing the down button. Therefore, its not here.
		GAMEPLAYACTIONS_ATTACK		,
		GAMEPLAYACTIONS_MENUTOGGLE	,

		// Debug-actions
		GAMEPLAYACTIONS_RESET		,

		//! Not a debug action. Used to measure the size of the Actions-array. 
		GAMEPLAYACTIONS_SIZE
	}


	private PlayerStates m_State;
	// </End of Member variables>

	// Start is called before the first frame update
	void Start()
    {
		// Set character values inherited from Character-class. Read these from save file later, once a save system has been set up.
		m_BaseMovementSpeed = 450.0f;
		//m_CurrentMovementSpeed = m_BaseMovementSpeed /* * speedMultiplier */ ;
		m_CurrentMovementSpeed = 450.0f;
		m_MaxHealth = 10.0f;
		m_CurrentHealth = m_MaxHealth;


		// Setup other references and stuff
		m_Rigidbody				= gameObject.GetComponent<Rigidbody>(); // Might be able to delete this if not neccessary


		// Move the below to the player controller.

		// Setup the action map
		//m_ActionMaps[ (int)ControlTypes.CONTROLTYPE_DEFAULT ] = GameplayActionMap;
		m_ActionMap = m_InputActionAsset.FindActionMap( "MainGameMap" );
		m_ActionMap.Enable();

		// Setup the actions
		m_InputActions = new InputAction[ (int)GameplayActions.GAMEPLAYACTIONS_SIZE ];

		m_InputActions[ (int)GameplayActions.GAMEPLAYACTIONS_MOVE ]			= m_ActionMap.FindAction( "Move" );
		m_InputActions[ (int)GameplayActions.GAMEPLAYACTIONS_JUMP ]			= m_ActionMap.FindAction( "Jump" );
		m_InputActions[ (int)GameplayActions.GAMEPLAYACTIONS_INTERACT ]		= m_ActionMap.FindAction( "Interact" );
		m_InputActions[ (int)GameplayActions.GAMEPLAYACTIONS_MENUTOGGLE ]	= m_ActionMap.FindAction( "MenuToggle" );
		m_InputActions[ (int)GameplayActions.GAMEPLAYACTIONS_RESET ]		= m_ActionMap.FindAction( "Reset" );

	}

	void Awake()
	{
		if ( !m_Inventory )
		{
			m_Inventory = gameObject.GetComponent<Inventory>();
		}

	}

	// Update is called once per frame
	void Update()
    {
		// Takes input from the player
		DecideInput();

		DecreaseTimersDT( Time.deltaTime );
	}

    private void FixedUpdate()
    {

		// Add -9.82 multiplied by a modifiable gravity-variable multiplied by time.fixedDeltaTime to make the player fall at a good speed.
		m_GravityVelocity.y += m_Gravity * ( -9.82f ) * Time.fixedDeltaTime;
		m_GravityVelocity.y = Mathf.Clamp( m_GravityVelocity.y, -50.0f, 20.0f );

		// Set to false if player is jumping, set to true if collision is found underneath the player
		m_Grounded = CheckForGround();
		m_Animator.SetBool( "IsGrounded", m_Grounded );

		if ( m_Grounded )
		{
			//m_Velocity.y = 0.0f;
			m_GravityVelocity.y = 0.0f;
			m_JumpWindowTimeLeft = m_JumpWindowDuration;
			m_CoyoteTimeLeft = m_CoyoteDuration;
		}

		//If not being knocked back by something, allow movement.
		if ( m_KnockBackVelocity.x == 0.0f )
		{

			m_MovementDirection = Vector3.Cross( (new Vector3(0.0f, 0.0f, -m_LRInput) ), m_GroundNormal );
			Move( m_MovementDirection * m_CurrentMovementSpeed ); // If no input is provided, the function will still run, but the player won't move.
		}


		//PlayerStates InputState = PlayerStates.PLAYERSTATE_IDLE; Do input here



		// Temporarily like this until I make it better
		switch ( m_State )
		{
			case PlayerStates.PLAYERSTATE_IDLE:
				// Play idle animation

				break;
			case PlayerStates.PLAYERSTATE_RUNNING:

				break;
			case PlayerStates.PLAYERSTATE_JUMPING:
				m_JumpWindowTimeLeft -= Time.deltaTime;
				m_Animator.SetBool( "IsJumping", true );

				m_GravityVelocity.y = 0.0f; // Set velocity to 0 so player isn't pushed down
				m_Rigidbody.AddForce( new Vector3( 0.0f, ( m_MaxJumpHeight / m_JumpWindowDuration ), 0.0f ), ForceMode.VelocityChange ); // Use velocity in order to move the player upwards // This way sucks, because the jump becomes really floaty


				if ( m_JumpWindowTimeLeft <= 0.0f )
				{
					m_State = PlayerStates.PLAYERSTATE_FALLING;
				}

				break;
			case PlayerStates.PLAYERSTATE_FALLING:

				m_DistanceFallen += 0.1f;
				m_Animator.SetBool( "IsJumping", false ); // Has to be set here instead of above, because this state can also be entered by releasing the space key
				m_Animator.SetBool( "IsFalling", true );

				// TODO: Remove this, it ain't a good way to use a state machine.
				if ( m_Grounded )
				{
					m_State = PlayerStates.PLAYERSTATE_IDLE;
					m_Animator.SetBool( "IsFalling", false );


					if ( m_DistanceFallen > 2.0f )
					{
						TakeDamage( m_DistanceFallen );
						Debug.Log( $"Took { m_DistanceFallen } damage from falling. \n" );
					}
					Debug.Log( string.Format( "Current health: {0}. \n", m_CurrentHealth ) );

					m_DistanceFallen = 0.0f;
				}

				break;
			case PlayerStates.PLAYERSTATE_SLIDING:

				if ( m_SlideDistanceLeft > 0.0f ) 
				{
					Slide();
					m_UpperCollider.enabled = false;
				}
				else
				{
					m_State = PlayerStates.PLAYERSTATE_IDLE;
					m_UpperCollider.enabled = true;
				}

				break;

			default:
				break;
		}

		if ( m_GravityVelocity.y < 0.0f )
			m_State = PlayerStates.PLAYERSTATE_FALLING;

		m_Animator.SetFloat( "Movement", Mathf.Abs( m_LRInput ), 0.05f, Time.deltaTime );	// TODO: Switch the string out for ID later.

		//Debug.Log( "Current Player state: " + m_State.ToString() );


		// Add velocity to the position.
		//m_PositionToApply += ( m_GravityVelocity * Time.fixedDeltaTime );

		// Apply the position to the player.
		//gameObject.transform.position = m_PositionToApply; // The way to do it if Rigidbodies were not needed. They are needed though, since for whatever reason colliders need a rigidbody in order to do collision against other colliders.

		// Below is the rigidbody way
		m_Rigidbody.velocity = ( m_MovementVelocity + m_GravityVelocity );
    }

    void DecideInput()
    {

		// TODO: Look up how to handle the input from a joystick based on how much it's being tilted in a direction. Tilting it fully should move the player faster than tilting it just a small bit.
		// TODO: Setup so that the type of buttons with available pressing depends on what kind of control-state it is currently in. If NOJUMP is set, then don't allow jumping, don't even check for jumping.

		InputAction Movement		= m_InputActions[ (int)GameplayActions.GAMEPLAYACTIONS_MOVE ];
		InputAction Jump			= m_InputActions[ (int)GameplayActions.GAMEPLAYACTIONS_JUMP ];
		InputAction Interact		= m_InputActions[ (int)GameplayActions.GAMEPLAYACTIONS_INTERACT ];
		InputAction MenuToggle		= m_InputActions[ (int)GameplayActions.GAMEPLAYACTIONS_MENUTOGGLE ];
		InputAction Reset			= m_InputActions[ (int)GameplayActions.GAMEPLAYACTIONS_RESET];



		// Temporary reset function. Once a loading screen has been implemented, play the loading screen and respawn player at last checkpoint.
		if ( Reset.triggered )
		{
			m_ActiveInput = true;
			//m_PositionToApply = new Vector3( 0.0f, 0.0f, 0.0f );
			transform.position = new Vector3( 0.0f, 0.0f, 0.0f );
		}

        if ( !m_ActiveInput )
            return;

		// The movement we want to apply is the input (taken here) multiplied by our chosen speed (done later).

		//////////////////////////////////////////////////////////////////////////////////
		//																				//
		// Button downs - what happens when a button is pressed.						//
		// The actions themselves are defined inside PlayerGameControls.inputactions	//
		//																				//
		//////////////////////////////////////////////////////////////////////////////////
		if ( Movement.triggered )
		{
			Debug.Log( "Move-action triggered...(-w- ')??" );

			//m_LRInput = Movement.ReadValue<Vector2>().x;
			m_UDInput = Movement.ReadValue<Vector2>().y;
		}

		if ( Jump.triggered )
		{
			if ( m_UDInput < 0.0f && m_Grounded /*&& m_SlideCooldownTimeLeft <= 0.0f*/ ) // Don't put this here.
			{
				if ( m_SlideCooldownTimeLeft <= 0.0f ) // This approach makes it so we don't have to check for down press again in the if statement below.
				{
					StartSlide();
				}
			}
			else if ( m_CoyoteTimeLeft > 0.0f )
			{
				//m_JumpWindowActive = true;
				m_State				= PlayerStates.PLAYERSTATE_JUMPING;
				m_CoyoteTimeLeft	= 0.0f;
			}
		}


		// Interact
		if ( Interact.triggered && m_CurrentlyFocusedInteractable ) 
		{ 
			m_CurrentlyFocusedInteractable.Interact();
		}

		if ( MenuToggle.triggered  )
		{
			m_InventoryUI.SetActive( !m_InventoryUI.activeSelf );
		}



		//////////////////////////////////////////////////////////////////////////////////
		//																				//
		// Button ups - what happens when a button is released.							//
		// The actions themselves are defined inside PlayerGameControls.inputactions	//
		//																				//
		//////////////////////////////////////////////////////////////////////////////////

		if ( Movement.WasReleasedThisFrame() )
		{
			Debug.Log( "Move-action released!!(-w- ')!" );
			//m_LRInput = 0.0f;
			m_UDInput = 0.0f;
		}

		m_LRInput = Movement.ReadValue<Vector2>().x;
		
		//print( $" LRinput = {m_LRInput}" );

		//if ( Jump.WasReleasedThisFrame() ) { m_JumpWindowActive = false; }
		if ( Jump.WasReleasedThisFrame() && m_State == PlayerStates.PLAYERSTATE_JUMPING ) { m_State = PlayerStates.PLAYERSTATE_FALLING; }
	}


    void StartSlide()
    {
        m_SlideCooldownTimeLeft = m_SlideCooldownDuration;
        m_SlideDistanceLeft		= m_SlideDistanceTotal;
		m_State					= PlayerStates.PLAYERSTATE_SLIDING;
    }

	void Slide()
	{
		Debug.Log( "Sliding!" );

		// Change this formula later, the dash should have more speed during ~80% of the duration, and close to the end it should halt quickly.
		m_SlideDistanceLeft -= m_SlideDistanceTotal / ( m_SlideDuration / Time.fixedDeltaTime );

		Vector3 DesiredSlidePosition = m_SlideDistanceLeft * m_SlideDirection;
		DesiredSlidePosition.z = 0.0f;

		//m_PositionToApply += DesiredSlidePosition;
		m_Rigidbody.AddForce( DesiredSlidePosition * 50.0f, ForceMode.VelocityChange );
	}

	void Jump()
	{

	}

	void Land()
	{
		// TODO on Thursday 4th of March 2022: move movement-related stuff over to the player controller. Make a velocity component.
	}


	// Timers that always need to be decreased, regardless of state
	private void DecreaseTimersDT( float pr_DeltaTime )
	{
//		m_CoyoteTimeLeft			-= pr_DeltaTime;	// Only needs to be decreased inside STATE_FALLING.
//		m_JumpWindowTimeLeft		-= pr_DeltaTime;	// Only needs to be decreased inside STATE_JUMPING.
		m_SlideCooldownTimeLeft		-= pr_DeltaTime;
	}


	public void Move( Vector3 pr_Movement )
    {
        // Add this later
        //if ( m_Grounded || m_AirControl ) 
        //{
        //
        //}

        // Turning the character the correct way is the highest priority

        if ( m_LRInput < 0.0f && m_FacingRight )
        {
            Flip();
        }
        else if ( m_LRInput > 0.0f && !m_FacingRight )
        {
            Flip();
        }

		// Set position to apply to player's movement
		//		m_PositionToApply.x += pr_Movement * Time.fixedDeltaTime;	// Not Rigidbody way
		m_MovementVelocity = pr_Movement * Time.fixedDeltaTime;	// Rigidbody way
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



    bool CheckForGround()
    {
		m_GroundNormal = Vector3.up;


		if ( m_State == PlayerStates.PLAYERSTATE_JUMPING )
			return false;


//		Vector3 RayStartPos = m_PositionToApply + new Vector3( 0.0f, 0.5f, 0.0f ); // Not using rigidbody
		Vector3 RayStartPos = transform.position + new Vector3( 0.0f, 0.5f, 0.0f ); // Using rigidbody
        float RayMaxDistance = 0.55f;

        Vector3 DebugRayStart = transform.position + new Vector3( 0.0f, 0.5f, 0.0f );
        Vector3 DebugRayEnd   = transform.position + new Vector3( 0.0f, -0.05f, 0.0f ); // Just for drawing purposes, max length of raycast has to be changed in if-statement
        Debug.DrawLine( DebugRayStart, DebugRayEnd, Color.red );

        RaycastHit	RaycastHit;
        Ray			GroundCheckRay = new Ray( RayStartPos, Vector3.down );

		// If ground is detected directly underneath the player:
		if ( Physics.Raycast( GroundCheckRay, out RaycastHit, RayMaxDistance, m_WhatIsGround ) )
		{
			// TODO WARNING ERROR HEY! Fix this later so it uses a bool in the same way JumpWindowActive works.
			//if ( m_UDInput < 0.0f && RaycastHit.transform.gameObject.tag == "FallThroughPlatform" )
			//{
			//    return false;
			//}
			m_GroundNormal		= RaycastHit.normal;
			m_SlideDirection	= Vector3.Cross( transform.right.normalized, RaycastHit.normal );
			//m_MovementDirection = Vector3.Cross( transform.right.normalized, RaycastHit.normal );

			return true;
		}

		// Add offset to ray, in order to check slightly BEHIND the player for ground collision.
		RayStartPos = transform.position + new Vector3( 0.0f, 0.5f, 0.0f ) + ( ( m_LowerCollider.size.z / 2.0f - 0.01f ) * -transform.forward );

		DebugRayStart	= RayStartPos;
		DebugRayEnd		= RayStartPos - new Vector3( 0.0f, RayMaxDistance, 0.0f );
		Debug.DrawLine( DebugRayStart, DebugRayEnd, Color.blue );

		GroundCheckRay = new Ray( RayStartPos, Vector3.down );

		if ( Physics.Raycast( GroundCheckRay, out RaycastHit, RayMaxDistance, m_WhatIsGround ) ) // if no ground directly underneath, check behind
		{
			m_GroundNormal = RaycastHit.normal;
			m_SlideDirection	= Vector3.Cross( transform.right.normalized, RaycastHit.normal );
			//m_MovementDirection = Vector3.Cross( transform.right.normalized, RaycastHit.normal );
			return true;
		}

		// Add offset to ray, in order to check slightly in FRONT of the player for ground collision.
		RayStartPos = transform.position + new Vector3( 0.0f, 0.5f, 0.0f ) + ( ( m_LowerCollider.size.z / 2.0f - 0.01f) * transform.forward );

		DebugRayStart	= RayStartPos;
		DebugRayEnd		= RayStartPos - new Vector3( 0.0f, RayMaxDistance, 0.0f );
		Debug.DrawLine( DebugRayStart, DebugRayEnd, Color.yellow );

		GroundCheckRay = new Ray( RayStartPos, Vector3.down );

		if ( Physics.Raycast( GroundCheckRay, out RaycastHit, RayMaxDistance, m_WhatIsGround ) ) // if no ground directly underneath, check in front
		{
			m_GroundNormal = RaycastHit.normal;
			m_SlideDirection	= Vector3.Cross( transform.right.normalized, RaycastHit.normal );
			//m_MovementDirection = Vector3.Cross( transform.right.normalized, RaycastHit.normal );
			return true;
		}

		return false;
    }



	public Interactable GetCurrentInteractable()
	{
		return m_CurrentlyFocusedInteractable;
	}



	public void SetCurrentInteractable( Interactable pr_NewFocusedInteractable )
	{
		m_CurrentlyFocusedInteractable = pr_NewFocusedInteractable;
	}

}
