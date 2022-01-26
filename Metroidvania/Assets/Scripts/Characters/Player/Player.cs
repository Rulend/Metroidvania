using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
	// < Member variables>
	[SerializeField] private LayerMask			m_WhatIsGround;                          // A mask determining what is ground to the character

    public Vector3								m_PositionToApply;
    public Vector3								m_GroundCollisionPosition;

    private Vector3								m_Velocity = new Vector3(0.0f, 0.0f, 0.0f);
	[SerializeField] private float				m_Gravity = 6.0f;

    private bool								m_Grounded;            // Whether or not the player is grounded.
    private bool								m_FacingRight = true;  // For determining which way the player is currently facing.

    // Player's stats
    private ushort								m_MaxHealth = 5;
    private short								m_CurrentHealth = 5;

    private float								m_BaseMovementSpeed = 5.0f;
    private float								m_CurrentMovementSpeed = 9.0f;

    // Movement variables.
    private bool								m_ActiveInput = true;
    private float								m_LRInput = 0.0f;
    private float								m_UDInput = 0.0f;

	// The player's jump
	[SerializeField] private float				m_MaxJumpHeight = 3.3f;
    private bool								m_JumpWindowActive = false;
    [SerializeField] private float				m_JumpWindowDuration = 0.214f; // Static as this value only needs to be set once
    private float								m_JumpWindowTimeLeft = 0.214f;
    private float								m_CoyoteDuration = 0.2f;
    private float								m_CoyoteTimeLeft = 0.2f;

    // The player's slide move.
    private float								m_SlideCooldownDuration = 0.75f;
    private float								m_SlideCooldownTimeLeft = 0.0f;
	[SerializeField] private float				m_SlideDuration = 0.5f;
	[SerializeField] private float				m_SlideDistanceTotal = 2.0f;
    private float								m_SlideDistanceLeft = 0.0f;
    private										Vector3 m_SlideDirection = Vector3.zero;

	// The currently focused interactable.
	[SerializeField] private  Interactable		m_CurrentlyFocusedInteractable;



	[SerializeField] private  Inventory			m_Inventory;
	public Inventory GetInventory =>			m_Inventory;



	// </End of Member variables>

    // Start is called before the first frame update
    void Start()
    {
        
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
        DecideInput();


        if ( m_CoyoteTimeLeft > 0.0f )
		{
            m_CoyoteTimeLeft -= Time.deltaTime;
		}
		else
		{
            m_CoyoteTimeLeft = 0.0f;
		}
    }

    private void FixedUpdate()
    {
        // Add -9.82 multiplied by a modifiable gravity-variable multiplied by time.fixedDeltaTime to make the player fall at a good speed.
        m_Velocity.y += m_Gravity * ( -9.82f ) * Time.fixedDeltaTime;
        m_Velocity.y = Mathf.Clamp( m_Velocity.y, -50.0f, 20.0f );

        // Set to false if player is jumping, set to true if collision is found underneath the player
        m_Grounded = CheckForGround();

        if ( m_Grounded )
        {
            m_Velocity.y = 0.0f;
            m_PositionToApply.y = m_GroundCollisionPosition.y;
            m_JumpWindowTimeLeft = m_JumpWindowDuration;
            m_CoyoteTimeLeft = m_CoyoteDuration;
        }
        else if ( m_JumpWindowActive ) // this inside here could be made into a Jump()-function
        {
            m_JumpWindowTimeLeft -= Time.fixedDeltaTime;
            m_Velocity.y = 0.0f; // Set new velocity so player goes upwards
            m_PositionToApply.y += ( m_MaxJumpHeight / m_JumpWindowDuration ) * Time.fixedDeltaTime; // Set new velocity so player goes upwards

            if ( m_JumpWindowTimeLeft < 0.0f )
            {
                m_JumpWindowActive = false;
            }
        }

        // If not being knocked back by something, allow movement.
        if ( m_Velocity.x == 0.0f ) // Can check for exactly 0.0f, as velocity.x is set to exactly 0.0f if within a threshold.
		{
            Move( m_LRInput * m_CurrentMovementSpeed );
            Slide();
		}

        // Add velocity to the position.
        m_PositionToApply += ( m_Velocity * Time.fixedDeltaTime );

        // Apply the position to the player.
        gameObject.transform.position = m_PositionToApply;

    }

    void DecideInput()
    {
        if ( Keyboard.current.rKey.wasPressedThisFrame )
        {
            m_ActiveInput = true;
            m_PositionToApply = new Vector3( 0.0f, 0.0f, 0.0f );
		}

        if ( !m_ActiveInput )
            return;

        // The movement we want to apply is the input multiplied by our chosen speed.
        // As we want to apply the movement at a fixed rate, we calculate the movement here based on input, and apply it do it inside FixedUpdate.
        //m_LRInput = Input.GetAxisRaw("Horizontal") * m_CurrentMovementSpeed;

        // Button downs

		// Movement
        if ( Keyboard.current.aKey.wasPressedThisFrame ) { m_LRInput -= 1.0f; }
        if ( Keyboard.current.dKey.wasPressedThisFrame ) { m_LRInput += 1.0f; }
        if ( Keyboard.current.wKey.wasPressedThisFrame ) { m_UDInput += 1.0f; }
        if ( Keyboard.current.sKey.wasPressedThisFrame ) { m_UDInput -= 1.0f; }


		// Interact
		if ( Keyboard.current.eKey.wasPressedThisFrame && m_CurrentlyFocusedInteractable ) 
		{ 
			m_CurrentlyFocusedInteractable.Interact(); 
		}


		// Jump / slide
		if ( Keyboard.current.spaceKey.wasPressedThisFrame )
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

        // if (Input.GetAxisRaw("Vertical") < 0.0f)    { m_Crouching = true; }


        // Button ups
        if ( Keyboard.current.aKey.wasReleasedThisFrame )       { m_LRInput += 1.0f; }
        if ( Keyboard.current.dKey.wasReleasedThisFrame )       { m_LRInput -= 1.0f; }
        if ( Keyboard.current.wKey.wasReleasedThisFrame )       { m_UDInput -= 1.0f; }
        if ( Keyboard.current.sKey.wasReleasedThisFrame )       { m_UDInput += 1.0f; }

        if ( Keyboard.current.spaceKey.wasReleasedThisFrame )   { m_JumpWindowActive = false; }


    }


    void StartSlide()
    {
        m_SlideCooldownTimeLeft = m_SlideCooldownDuration;
        m_SlideDistanceLeft		= m_SlideDistanceTotal;
    }

    void Slide()
    {
        // Dividing the distance by (duration / fixedDeltaTime) will make sure the dash is complete by the specified time.
        // The further you want the character to go during the time frame, the faster they will go to get there.

        if ( m_SlideDistanceLeft > 0.0f ) // Doing this is better than a clamp, as a clamp has 2 if-statements, this has one.
        {
            Debug.Log( "Sliding!" );

            // Change this formula later, the dash should have more speed during ~80% of the duration, and close to the end it should halt quickly.
            m_SlideDistanceLeft -= m_SlideDistanceTotal / ( m_SlideDuration / Time.fixedDeltaTime );

            Vector3 DesiredSlidePosition = m_SlideDistanceLeft * m_SlideDirection;
            DesiredSlidePosition.z = 0.0f;

            m_PositionToApply += DesiredSlidePosition;
        }

        // Tick down the cooldown by the same amount every time if it's more than 0.
        if (m_SlideCooldownTimeLeft > 0.0f)
        {
            m_SlideCooldownTimeLeft -= Time.fixedDeltaTime;
            Debug.Log("SlideCooldown: " + m_SlideCooldownTimeLeft);
            // Cooldown has to reach 0 in combined time of itself + duration.
            // If duration is 0.5f, and cooldown is 0.5f, then we need to have:
            // Awake() m_CoolDown = 1;
            // FixedUpdate() m_CurrentCooldown -= (m_CoolDown * time.fixedDeltaTime) /time.fixedDeltaTime;
        }
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
        m_PositionToApply.x += pr_Movement * Time.fixedDeltaTime;
    }


    private void Flip()
    {
        // Switch the way the player is labelled as facing.
        m_FacingRight = !m_FacingRight;

        // Multiply the player's x local scale by -1.
        //Vector3 theScale = transform.localScale;
        //theScale.z *= -1;
        //transform.localScale = theScale;

        // My own version of rotating, instead of changing the scale as above.
        float currentYRotation = gameObject.transform.localRotation.y;

        Quaternion desiredRotation = new Quaternion(gameObject.transform.localRotation.x, -currentYRotation, gameObject.transform.localRotation.z, gameObject.transform.localRotation.w);

        gameObject.transform.localRotation = desiredRotation;
    }

    bool CheckForGround()
    {
        if ( m_JumpWindowActive )
		{
			return false;
		}

        Vector3 RayStartPos = m_PositionToApply + new Vector3( 0.0f, 0.5f, 0.0f );
        float RayMaxDistance = 0.55f;

        Vector3 DebugRayStart = m_PositionToApply + new Vector3( 0.0f, 0.5f, 0.0f );
        Vector3 DebugRayEnd   = m_PositionToApply + new Vector3( 0.0f, -0.05f, 0.0f ); // Just for drawing purposes, max length of raycast has to be changed in if-statement
        Debug.DrawLine( DebugRayStart, DebugRayEnd, Color.red );

        RaycastHit RaycastHit;
        Ray GroundCheckRay = new Ray( RayStartPos, Vector3.down );

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
        RayStartPos = m_PositionToApply + new Vector3( 0.0f, 0.5f, 0.0f ) + ( transform.forward * -0.5f );

        DebugRayStart = m_PositionToApply + new Vector3( 0.0f, 0.5f, 0.0f ) + ( transform.forward * -0.5f );
        DebugRayEnd = m_PositionToApply + new Vector3( 0.0f, -0.05f, 0.0f ) + ( transform.forward * -0.5f );
        Debug.DrawLine( DebugRayStart, DebugRayEnd, Color.blue );

        GroundCheckRay = new Ray( RayStartPos, Vector3.down );

        if ( Physics.Raycast( GroundCheckRay, out RaycastHit, RayMaxDistance, m_WhatIsGround) ) // if no ground directly underneath, check behind
        {
            m_GroundCollisionPosition = RaycastHit.point;
            return true;
        }

        // Add offset to ray, in order to check slightly in FRONT of the player for ground collision.
        RayStartPos = m_PositionToApply + new Vector3( 0.0f, 0.5f, 0.0f ) + ( transform.forward * 0.5f );

        DebugRayStart = m_PositionToApply + new Vector3( 0.0f, 0.5f, 0.0f ) + ( transform.forward * 0.5f );
        DebugRayEnd = m_PositionToApply + new Vector3( 0.0f, -0.05f, 0.0f ) + ( transform.forward * 0.5f );
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
