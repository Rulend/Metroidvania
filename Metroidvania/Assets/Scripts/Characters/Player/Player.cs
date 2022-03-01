using UnityEngine;
using UnityEngine.InputSystem;

public class Player : Character
{
	// < Member variables>
	[SerializeField] private LayerMask m_WhatIsGround;                          // A mask determining what is ground to the character

	public Vector3 m_PositionToApply;
	public Vector3 m_GroundCollisionPosition;

//	private Vector3 m_Velocity = new Vector3( 0.0f, 0.0f, 0.0f );
	private Vector3 m_MovementVelocity = new Vector3( 0.0f, 0.0f, 0.0f );
	[SerializeField] private float m_Gravity = 6.0f;

	private bool m_Grounded;			// Whether or not the player is grounded.
	private bool m_FacingRight = true;  // For determining which way the player is currently facing.

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
	[SerializeField] private InputActionAsset ActionAsset;

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

	private PlayerStates m_State;


	// State machine for the controls
	private enum ControlTypes
	{
		CONTROLTYPE_DEFAULT = 0	,
		CONTROLTYPE_NOATTACK	,
		CONTROLTYPE_NOJUMP		,
		CONTROLTYPE_CUTSCENE	,
	}


	// </End of Member variables>

	// Start is called before the first frame update
	void Start()
    {
		// Set character values inherited from Character-class
		m_BaseMovementSpeed = 9.0f;
		//m_CurrentMovementSpeed = m_BaseMovementSpeed /* * speedMultiplier */ ;
		m_CurrentMovementSpeed = 9.0f;


		// Setup other references and stuff
		m_Rigidbody		= gameObject.GetComponent<Rigidbody>(); // Might be able to delete this if not neccessary
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


		if ( m_CoyoteTimeLeft > 0.0f )
		{
			//m_CoyoteTimeLeft -= Time.deltaTime;
		}
		else
		{
			//m_CoyoteTimeLeft = 0.0f;
		}


		DecreaseTimersDT( Time.deltaTime );


	}

    private void FixedUpdate()
    {

		switch( m_State )
		{
			case PlayerStates.PLAYERSTATE_IDLE:

				break;
			case PlayerStates.PLAYERSTATE_RUNNING:

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




		// Add -9.82 multiplied by a modifiable gravity-variable multiplied by time.fixedDeltaTime to make the player fall at a good speed.
		m_GravityVelocity.y += m_Gravity * ( -9.82f ) * Time.fixedDeltaTime;
		m_GravityVelocity.y = Mathf.Clamp( m_GravityVelocity.y, -50.0f, 20.0f );

        // Set to false if player is jumping, set to true if collision is found underneath the player
        m_Grounded = CheckForGround();

        if ( m_Grounded )
        {
			//m_Velocity.y = 0.0f;
			m_GravityVelocity.y	= 0.0f;
            //m_PositionToApply.y = m_GroundCollisionPosition.y; // Not Rigidbody way
			//gameObject.transform.position = new Vector3( transform.position.x, m_GroundCollisionPosition.y, transform.position.z ); // Rigidbody way
			m_JumpWindowTimeLeft	= m_JumpWindowDuration;
            m_CoyoteTimeLeft		= m_CoyoteDuration;
        }
        else if ( m_JumpWindowActive ) // this inside here could be made into a Jump()-function
		{
			m_JumpWindowTimeLeft -= Time.fixedDeltaTime;
			m_GravityVelocity.y = 0.0f; // Set velocity to 0 so player isn't pushed down ( non rigidbody way)
			//m_PositionToApply.y += ( m_MaxJumpHeight / m_JumpWindowDuration ) * Time.fixedDeltaTime; // Set position to move upwards
			//m_MovementVelocity.y += ( m_MaxJumpHeight / m_JumpWindowDuration ) * ( m_JumpWindowTimeLeft * 1.2f ) ;
			m_Rigidbody.AddForce( new Vector3( 0.0f, ( m_MaxJumpHeight / m_JumpWindowDuration ), 0.0f ), ForceMode.VelocityChange ); // Use velocity in order to move the player upwards // This way sucks, because the jump becomes really floaty

			if ( m_JumpWindowTimeLeft < 0.0f )
			{
			    m_JumpWindowActive = false;
			}
		}

		//If not being knocked back by something, allow movement.

		if ( m_KnockBackVelocity.x == 0.0f )
		{
			Vector3 MovementDirection = new Vector3( 0.0f, 0.0f, 0.0f );

			//Vector3 RayStartPos = transform.position + new Vector3( 0.0f, 0.5f, 0.0f ); // Using rigidbody
			//float RayMaxDistance = 0.55f;

			//Vector3 DebugRayStart = transform.position + new Vector3( 0.0f, 0.5f, 0.0f );
			//Vector3 DebugRayEnd = transform.position + new Vector3( 0.0f, -0.05f, 0.0f ); // Just for drawing purposes, max length of raycast has to be changed in if-statement
			//Debug.DrawLine( DebugRayStart, DebugRayEnd, Color.red );ada

			//RaycastHit RaycastHit;
			//Ray GroundCheckRay = new Ray( RayStartPos, Vector3.down );

			//// If ground is detected directly underneath the player:
			//if ( Physics.Raycast( GroundCheckRay, out RaycastHit, RayMaxDistance, m_WhatIsGround ) )
			//{
			//	MovementDirection = Vector3.Cross( new Vector3( m_LRInput, 0.0f, 0.0f ).normalized, RaycastHit.normal );
			//}

			Move( m_LRInput * m_CurrentMovementSpeed );

			//Slide();
		}

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

		InputActionMap ActionMap = ActionAsset.FindActionMap("MainGameMap");
		ActionMap.Enable();

		InputAction Movement			= ActionMap.FindAction( "Move" );
		InputAction Jump				= ActionMap.FindAction( "Jump" );
		InputAction Interact			= ActionMap.FindAction( "Interact" );
		InputAction Reset				= ActionMap.FindAction( "Reset" );
		InputAction MenuToggle			= ActionMap.FindAction( "MenuToggle" );


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

		// Button downs
		if ( Movement.triggered )
		{
			Debug.Log( "Move-action triggered...(-w- ')??" );

			m_LRInput = Movement.ReadValue<Vector2>().x;
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
				m_JumpWindowActive = true;
				m_CoyoteTimeLeft = 0.0f;
			}
		}


		// Interact
		if ( Interact.triggered && m_CurrentlyFocusedInteractable ) 
		{ 
			m_CurrentlyFocusedInteractable.Interact();
		}

		if (MenuToggle.triggered  )
		{
			m_InventoryUI.SetActive( !m_InventoryUI.activeSelf );
		}



        // Button ups

		if ( Movement.WasReleasedThisFrame() )
		{
			Debug.Log( "Move-action released!!(-w- ')!" );
			m_LRInput = 0;
		}

		if ( Jump.WasReleasedThisFrame() ) { m_JumpWindowActive = false; }
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
		//transform.Rotate( new Vector3( m_SlideDirection.x * 10.0f, 0.0f, 0.0f ) );
	}

	private void DecreaseTimersDT( float pr_DeltaTime )
	{
		m_CoyoteTimeLeft			-= pr_DeltaTime;
		m_SlideCooldownTimeLeft		-= pr_DeltaTime;
	}


	public void Move( float pr_Movement )
    {
        // Add this later
        //if ( m_Grounded || m_AirControl ) 
        //{
        //
        //}

        // Turning the character the correct way is the highest priority

        if ( pr_Movement < 0.0f && m_FacingRight )
        {
            Flip();
        }
        else if ( pr_Movement > 0.0f && !m_FacingRight )
        {
            Flip();
        }



        // Set position to apply to player's movement
//		m_PositionToApply.x += pr_Movement * Time.fixedDeltaTime;	// Not Rigidbody way
		m_MovementVelocity.x = pr_Movement * Time.fixedDeltaTime * 50.0f;	// Rigidbody way
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
		if ( m_JumpWindowActive )
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
            if ( m_UDInput < 0.0f && RaycastHit.transform.gameObject.tag == "FallThroughPlatform" )
            {
                return false;
            }

            m_GroundCollisionPosition = RaycastHit.point;
            m_SlideDirection = Vector3.Cross( transform.right.normalized, RaycastHit.normal );
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
			m_GroundCollisionPosition = RaycastHit.point;
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
			m_GroundCollisionPosition = RaycastHit.point;
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
