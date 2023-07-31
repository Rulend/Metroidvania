using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof( Player_Movement ) )]
public class PlayerController : MonoBehaviour
{

	public enum EPlayerControllerState
	{
		Gameplay	,
		Menu		,
		Cutscene	,
		Dialogue	,


		ControllerStateCount
	}


	// TODO:: I know this is the PlayerController but uh, another look at how input is taken is defenitively required. There's so many variables in here that aren't needed.
	// An easy way to solve it would be to have lists with the actions in, and then use an enum to check individual button inputs; another would be to make use of the context-based functions 

	// Movement variables.
	private float	m_LRInput		= 0.0f;
	private float	m_UDInput		= 0.0f;

	// Used for controlling stuff, move to controller script later.
	[SerializeField] private InputActionAsset	m_InputActionAsset;

	private EPlayerControllerState	m_CurrentState;

	private InputActionMap[] m_ActionMaps;

	// References to the player
	private Player_Movement m_rPlayerMovement;
	private Player			m_rPlayer;

	// Cutscene
	private InputAction m_ActionCutsceneSkip;
	private InputAction m_ActionCutscenePause;

	// Cutscene
	private InputAction m_ActionDialogueConfirm;


	void Awake()
    {
		SetupActions();

		m_rPlayerMovement	= GetComponent<Player_Movement>();
		m_rPlayer			= GetComponent<Player>();
		//m_rInventoryUI		= UI_Manager.Instance.rInventoryUI.gameObject; // Done in start instead
	}

	private void SetupActions()
	{
		// Setup all the action maps
		m_ActionMaps = new InputActionMap[ (int)EPlayerControllerState.ControllerStateCount ];
		m_ActionMaps[ (int)EPlayerControllerState.Gameplay ]	= m_InputActionAsset.FindActionMap( "MainGame" );
		m_ActionMaps[ (int)EPlayerControllerState.Menu ]		= m_InputActionAsset.FindActionMap( "Menu" );
		m_ActionMaps[ (int)EPlayerControllerState.Cutscene ]	= m_InputActionAsset.FindActionMap( "Cutscene" );
		m_ActionMaps[ (int)EPlayerControllerState.Dialogue ]	= m_InputActionAsset.FindActionMap( "Dialogue" );

		// Set up actions for the menu
		m_ActionMaps[ (int)EPlayerControllerState.Menu ].Enable();
		m_ActionMaps[ (int)EPlayerControllerState.Menu ].FindAction( "GoBack" ).started += StartMenuGoBack;
		m_ActionMaps[ (int)EPlayerControllerState.Menu ].FindAction( "Alternative1" ).started += StartMenuAlternative1;
		m_ActionMaps[ (int)EPlayerControllerState.Menu ].FindAction( "Alternative2" ).started += StartMenuAlternative2;
		m_ActionMaps[ (int)EPlayerControllerState.Menu ].FindAction( "LeftTab" ).started += StartMenuPrevTab;
		m_ActionMaps[ (int)EPlayerControllerState.Menu ].FindAction( "RightTab" ).started += StartMenuNextTab;
		m_ActionMaps[ (int)EPlayerControllerState.Menu ].FindAction( "ExitMenu" ).started += StartMenuExit;
		m_ActionMaps[ (int)EPlayerControllerState.Menu ].Disable();


		// Setup the actions for gameplay
		m_ActionMaps[ (int)EPlayerControllerState.Gameplay ].Enable();
		m_ActionMaps[ (int)EPlayerControllerState.Gameplay ].FindAction( "Move" ).performed += PerformMove;
		m_ActionMaps[ (int)EPlayerControllerState.Gameplay ].FindAction( "Move" ).canceled += CancelMove;
		m_ActionMaps[ (int)EPlayerControllerState.Gameplay ].FindAction( "Jump" ).started += StartJump;
		m_ActionMaps[ (int)EPlayerControllerState.Gameplay ].FindAction( "Jump" ).canceled += CancelJump;
		m_ActionMaps[ (int)EPlayerControllerState.Gameplay ].FindAction( "Roll" ).started += StartRoll;
		m_ActionMaps[ (int)EPlayerControllerState.Gameplay ].FindAction( "Interact" ).started += StartInteract;
		m_ActionMaps[ (int)EPlayerControllerState.Gameplay ].FindAction( "UseCurrentConsumable" ).started += StartUseCurrentConsumable;
		m_ActionMaps[ (int)EPlayerControllerState.Gameplay ].FindAction( "EquipmentWheelPreviousConsumable" ).started += StartPrevConsumable;
		m_ActionMaps[ (int)EPlayerControllerState.Gameplay ].FindAction( "EquipmentWheelNextConsumable" ).started += StartNextConsumable;
		m_ActionMaps[ (int)EPlayerControllerState.Gameplay ].FindAction( "OpenMenu" ).started += StartOpenMenu;
		m_ActionMaps[ (int)EPlayerControllerState.Gameplay ].FindAction( "Reset" ).started += StartDebugReset;
	}

	void FixedUpdate()
	{
		m_rPlayerMovement.Move( m_LRInput );
	}

	// Functions subscribed to button press callbacks.
	// Instead of polling actions, we just subscribe these functions to their events.
	private void PerformMove( InputAction.CallbackContext context )
	{
		Debug.Log( "Reading movement" );
		Vector2 movementInput = context.ReadValue<Vector2>();
		m_LRInput = movementInput.x;
		m_UDInput = movementInput.y;
	}

	private void CancelMove( InputAction.CallbackContext context )
	{
		Debug.Log( "Cancelled movement input" );
		m_LRInput = 0.0f;
		m_UDInput = 0.0f;
	}

	private void StartJump( InputAction.CallbackContext context )
	{
		if ( m_UDInput < 0.0f ) // If player presses down while jumping, slide instead
		{ m_rPlayerMovement.StartSlide(); }
		else
		{ m_rPlayerMovement.Jump(); }
	}

	private void CancelJump( InputAction.CallbackContext context )
	{
		m_rPlayerMovement.StopJump();
	}

	private void StartUseCurrentConsumable( InputAction.CallbackContext context )
	{
		EquipmentManager.Instance.EquipWheel.UseCurrentConsumable( m_rPlayer );
	}

	private void StartInteract( InputAction.CallbackContext context )
	{
		if ( CanInteract() )
		{
			m_rPlayer.m_CurrentlyFocusedInteractable.Interact();//TODO:: Invoke an event called Interact, which invokes it on the closest one which should be subscribed to the event.
		}
	}

	private void StartPrevConsumable( InputAction.CallbackContext context )
	{
		EquipmentManager.Instance.EquipWheel.CycleConsumables( -1 );
	}

	private void StartNextConsumable( InputAction.CallbackContext context )
	{
		EquipmentManager.Instance.EquipWheel.CycleConsumables( 1 );
	}

	private void StartRoll( InputAction.CallbackContext context )
	{
		m_rPlayerMovement.StartRoll();
	}

	private void StartOpenMenu( InputAction.CallbackContext context )
	{
		m_LRInput = 0.0f;
		UI_Manager.Instance.rMenu.OpenMenu();

		m_ActionMaps[ (int)EPlayerControllerState.Gameplay ].Disable();
		m_ActionMaps[ (int)EPlayerControllerState.Menu ].Enable();
		m_CurrentState = EPlayerControllerState.Menu;
	}

	private void StartDebugReset( InputAction.CallbackContext context )
	{
		transform.position = new Vector3( 0.0f, 0.0f, 0.0f );
	}




	// Menu functions
	private void StartMenuAlternative1( InputAction.CallbackContext context )
	{
		UI_Manager.Instance.rMenu.AlternativeButton1();
	}

	private void StartMenuAlternative2( InputAction.CallbackContext context )
	{
		UI_Manager.Instance.rMenu.AlternativeButton2();
	}

	private void StartMenuGoBack( InputAction.CallbackContext context )
	{
		UI_Manager.Instance.rMenu.GoBackToPreviousWindow();
	}

	private void StartMenuPrevTab( InputAction.CallbackContext context )
	{
		UI_Manager.Instance.rMenu.LeftShoulderButton();
	}

	private void StartMenuNextTab( InputAction.CallbackContext context )
	{
		UI_Manager.Instance.rMenu.RightShoulderButton();
	}

	private void StartMenuExit( InputAction.CallbackContext context )
	{
		UI_Manager.Instance.rMenu.CloseMenu();
	}


	public void SetState( EPlayerControllerState _NewState )
	{
		EPlayerControllerState PreviousState = m_CurrentState;

		m_CurrentState = _NewState;

		m_ActionMaps[ (int)PreviousState ].Disable();
		m_ActionMaps[ (int)m_CurrentState ].Enable();
	}

	private bool CanInteract()
	{
		if ( m_rPlayer.m_CurrentlyFocusedInteractable == null )
			return false;


		return true;
	}
}
