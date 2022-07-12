using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof( Player_Movement ) )]
public class PlayerController : MonoBehaviour
{

	public enum EPlayerControllerState
	{
		PCSTATE_Normal,
		PCSTATE_Inventory,
		PCSTATE_Cutscene,
		PCSTATE_Dialogue,
	}


	// Movement variables.
	private bool	m_ActiveInput	= true;
	private float	m_LRInput		= 0.0f;
	private float	m_UDInput		= 0.0f;

	// Used for controlling stuff, move to controller script later.
	[SerializeField] private InputActionAsset	m_InputActionAsset;

	private InputActionMap	m_ActionMap; // The current actionmap. See to the right how to ser up an array //new InputActionMap[ (int)ControlTypes.CONTROLTYPE_SIZE ];	// The different kinds of actionmaps used.

	private InputAction		m_ActionMovement;
	private InputAction		m_ActionJump;
	private InputAction		m_ActionInteract;
	private InputAction		m_ActionMenuToggle;
	private InputAction		m_ActionReset;


	private Player_Movement m_rPlayerMovement;
	private Player			m_rPlayer;
	private GameObject		m_rInventoryUI;



	void Awake()
    {
		m_ActionMap = m_InputActionAsset.FindActionMap( "MainGameMap" );
		m_ActionMap.Enable();

		m_ActionMovement	= m_ActionMap.FindAction( "Move" );
		m_ActionJump		= m_ActionMap.FindAction( "Jump" );
		m_ActionInteract	= m_ActionMap.FindAction( "Interact" );
		m_ActionMenuToggle	= m_ActionMap.FindAction( "MenuToggle" );
		m_ActionReset		= m_ActionMap.FindAction( "Reset" );

		m_rPlayerMovement	= GetComponent<Player_Movement>();
		m_rPlayer			= GetComponent<Player>();
		//m_rInventoryUI		= UI_Manager.Instance.rInventoryUI.gameObject; // Done in start instead

	}

	private void Start()
	{
		m_rInventoryUI = UI_Manager.Instance.rInventoryUI.gameObject;
	}

	// Update is called once per frame
	void Update()
    {
		TakeInput();
	}

	void FixedUpdate()
	{
		m_rPlayerMovement.Move( m_LRInput );
	}

	void TakeInput(  )
    {

		// TODO: Look up how to handle the input from a joystick based on how much it's being tilted in a direction. Tilting it fully should move the player faster than tilting it just a small bit.
		// TODO: Setup so that the type of buttons with available pressing depends on what kind of control-state it is currently in. If NOJUMP is set, then don't allow jumping, don't even check for jumping.

		//if ( m_ActionMap.FindAction("LeftClick").triggered )
		//{
		//		UI_Manager.Instance.rInventoryUI.m_CurrentSlot.HideItemSlotOptions();
		//}


		// Temporary reset function. Once a loading screen has been implemented, play the loading screen and respawn player at last checkpoint.
		if ( m_ActionReset.triggered )
		{
			m_ActiveInput = true;
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
		if ( m_ActionMovement.triggered )
		{
			//Debug.Log( "Move-action triggered...(-w- ')??" );

			//_LRInput = m_ActionMovement.ReadValue<Vector2>().x;
			m_UDInput = m_ActionMovement.ReadValue<Vector2>().y;
		}

		// Always read this input TODO:: cHECK IF READING ALWAYS IS NECESSARY!	
		m_LRInput = m_ActionMovement.ReadValue<Vector2>().x;


		if ( m_ActionJump.triggered )
		{
			if ( m_UDInput < 0.0f ) 
				m_rPlayerMovement.StartSlide();
			else
				m_rPlayerMovement.Jump();
		}

		// Interact
		if ( m_ActionInteract.triggered/* && m_CurrentlyFocusedInteractable */) 
		{
			if ( CanInteract() )
				m_rPlayer.m_CurrentlyFocusedInteractable.Interact(); // Invoke an event called Interact, which invokes it on the closest one which should be subscribed to the event.
		}

		if ( m_ActionMenuToggle.triggered )
		{
			
			if ( !m_rInventoryUI.activeSelf )
			{
				m_rInventoryUI.SetActive( true );
			}
			else
			{
				m_rInventoryUI.SetActive( false );
			}

		}


		//////////////////////////////////////////////////////////////////////////////////
		//																				//
		// Button ups - what happens when a button is released.							//
		// The actions themselves are defined inside PlayerGameControls.inputactions	//
		//																				//
		//////////////////////////////////////////////////////////////////////////////////

		if ( m_ActionMovement.WasReleasedThisFrame() )
		{
			//Debug.Log( "Move-action released!!(-w- ')!" );
			m_UDInput = 0.0f;
		}


		if ( m_ActionJump.WasReleasedThisFrame() )
			m_rPlayerMovement.StopJump();
	}


	private bool CanInteract()
	{
		if ( m_rPlayer.m_CurrentlyFocusedInteractable == null )
			return false;


		return true;
	}
}
