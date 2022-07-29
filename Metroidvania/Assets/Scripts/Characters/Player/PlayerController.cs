using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof( Player_Movement ) )]
public class PlayerController : MonoBehaviour
{

	public enum EPlayerControllerState
	{
		PCSTATE_Normal		,
		PCSTATE_Menu		,
		PCSTATE_Cutscene	,
		PCSTATE_Dialogue	,


		NUM_PCSTATES
	}


	// Movement variables.
	private bool	m_ActiveInput	= true;
	private float	m_LRInput		= 0.0f;
	private float	m_UDInput		= 0.0f;

	// Used for controlling stuff, move to controller script later.
	[SerializeField] private InputActionAsset	m_InputActionAsset;

	private EPlayerControllerState	m_CurrentState;

	private InputActionMap[] m_ActionMaps;

	// Normal game
	private InputAction		m_ActionMovement;
	private InputAction		m_ActionJump;
	private InputAction		m_ActionInteract;
	private InputAction		m_ActionOpenMenu;
	private InputAction		m_ActionReset;

	private Player_Movement m_rPlayerMovement;
	private Player			m_rPlayer;


	// Menu
	private InputAction		m_ActionMenuConfirm;
	private InputAction		m_ActionMenuAlternative1;
	private InputAction		m_ActionMenuAlternative2;
	private InputAction		m_ActionMenuGoBack;
	private InputAction		m_ActionMenuPreviousTab;
	private InputAction		m_ActionMenuNextTab;
	private InputAction		m_ActionMenuExit;

	// Cutscene
	private InputAction m_ActionCutsceneSkip;
	private InputAction m_ActionCutscenePause;

	// Cutscene
	private InputAction m_ActionDialogueConfirm;


	void Awake()
    {
		m_ActionMaps = new InputActionMap[ (int)EPlayerControllerState.NUM_PCSTATES ];
		m_ActionMaps[ (int)EPlayerControllerState.PCSTATE_Normal ]		= m_InputActionAsset.FindActionMap( "MainGame" );
		m_ActionMaps[ (int)EPlayerControllerState.PCSTATE_Menu ]		= m_InputActionAsset.FindActionMap( "Menu" );
		m_ActionMaps[ (int)EPlayerControllerState.PCSTATE_Cutscene ]	= m_InputActionAsset.FindActionMap( "Cutscene" );
		m_ActionMaps[ (int)EPlayerControllerState.PCSTATE_Dialogue ]	= m_InputActionAsset.FindActionMap( "Dialogue" );


		m_ActionMaps[ (int)EPlayerControllerState.PCSTATE_Menu ].Enable();
		m_ActionMenuConfirm			= m_ActionMaps[ (int)EPlayerControllerState.PCSTATE_Menu ].FindAction( "Confirm" );
		m_ActionMenuAlternative1	= m_ActionMaps[ (int)EPlayerControllerState.PCSTATE_Menu ].FindAction( "Alternative1" );
		m_ActionMenuAlternative2	= m_ActionMaps[ (int)EPlayerControllerState.PCSTATE_Menu ].FindAction( "Alternative2" );
		m_ActionMenuGoBack			= m_ActionMaps[ (int)EPlayerControllerState.PCSTATE_Menu ].FindAction( "GoBack" );
		m_ActionMenuPreviousTab		= m_ActionMaps[ (int)EPlayerControllerState.PCSTATE_Menu ].FindAction( "LeftTab" );
		m_ActionMenuNextTab			= m_ActionMaps[ (int)EPlayerControllerState.PCSTATE_Menu ].FindAction( "RightTab" );
		m_ActionMenuExit			= m_ActionMaps[ (int)EPlayerControllerState.PCSTATE_Menu ].FindAction( "ExitMenu" );
		m_ActionMaps[ (int)EPlayerControllerState.PCSTATE_Menu ].Disable();


		m_ActionMaps[ (int)EPlayerControllerState.PCSTATE_Normal ].Enable();
		m_ActionMovement	= 	m_ActionMaps[ (int)EPlayerControllerState.PCSTATE_Normal ].FindAction( "Move" );
		m_ActionJump		= 	m_ActionMaps[ (int)EPlayerControllerState.PCSTATE_Normal ].FindAction( "Jump" );
		m_ActionInteract	= 	m_ActionMaps[ (int)EPlayerControllerState.PCSTATE_Normal ].FindAction( "Interact" );
		m_ActionOpenMenu	= 	m_ActionMaps[ (int)EPlayerControllerState.PCSTATE_Normal ].FindAction( "OpenMenu" );
		m_ActionReset		=	m_ActionMaps[ (int)EPlayerControllerState.PCSTATE_Normal ].FindAction( "Reset" );


		m_rPlayerMovement	= GetComponent<Player_Movement>();
		m_rPlayer			= GetComponent<Player>();
		//m_rInventoryUI		= UI_Manager.Instance.rInventoryUI.gameObject; // Done in start instead
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

	public void SetState( EPlayerControllerState _NewState )
	{
		EPlayerControllerState PreviousState = m_CurrentState;

		m_CurrentState = _NewState;

		m_ActionMaps[ (int)PreviousState ].Disable();
		m_ActionMaps[ (int)m_CurrentState ].Enable();
	}


	void TakeInput(  )
    {

		// TODO: Look up how to handle the input from a joystick based on how much it's being tilted in a direction. Tilting it fully should move the player faster than tilting it just a small bit.
		// TODO: Setup so that the type of buttons with available pressing depends on what kind of control-state it is currently in. If NOJUMP is set, then don't allow jumping, don't even check for jumping.

		// Temporary reset function. Once a loading screen has been implemented, play the loading screen and respawn player at last checkpoint.
		if ( m_ActionReset.triggered )
		{
			m_ActiveInput = true;
			transform.position = new Vector3( 0.0f, 0.0f, 0.0f );
		}

        if ( !m_ActiveInput )
            return;


		switch ( m_CurrentState )
		{
			case EPlayerControllerState.PCSTATE_Normal:
				{
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
						m_UDInput = m_ActionMovement.ReadValue<Vector2>().y;
					}

					// Always read this input TODO:: cHECK IF READING ALWAYS IS NECESSARY!	
					m_LRInput = m_ActionMovement.ReadValue<Vector2>().x;


					if ( m_ActionJump.triggered )
					{
						if ( m_UDInput < 0.0f ) // If player presses down while jumping, slide instead. TODO::
							m_rPlayerMovement.StartSlide();
						else
							m_rPlayerMovement.Jump();
					}

					// Interact
					if ( m_ActionInteract.triggered )
					{
						if ( CanInteract() )
							m_rPlayer.m_CurrentlyFocusedInteractable.Interact(); // Invoke an event called Interact, which invokes it on the closest one which should be subscribed to the event.
					}

					if ( m_ActionOpenMenu.triggered )
					{
						//			if ( CanOpenMenu() ) // TODO:: Implement this function when it becomes necessary.

						m_LRInput = 0.0f;
						UI_Manager.Instance.rMenu.OpenMenu();

						m_ActionMaps[ (int)EPlayerControllerState.PCSTATE_Normal ].Disable();
						m_ActionMaps[ (int)EPlayerControllerState.PCSTATE_Menu ].Enable();
						m_CurrentState = EPlayerControllerState.PCSTATE_Menu;
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
				break;

			case EPlayerControllerState.PCSTATE_Menu:
				{
					if ( m_ActionMenuConfirm.triggered )
					{
					}


					if ( m_ActionMenuAlternative1.triggered )
					{
						UI_Manager.Instance.rMenu.AlternativeButton1();
					}


					if ( m_ActionMenuAlternative2.triggered )
					{
						UI_Manager.Instance.rMenu.AlternativeButton2();
					}


					if ( m_ActionMenuGoBack.triggered )
					{
						UI_Manager.Instance.rMenu.GoBackToPreviousWindow();
					}

					if ( m_ActionMenuPreviousTab.triggered )
					{
						UI_Manager.Instance.rMenu.LeftShoulderButton();
					}

					if ( m_ActionMenuNextTab.triggered )
					{
						UI_Manager.Instance.rMenu.RightShoulderButton();
					}

					if ( m_ActionMenuExit.triggered )
					{
						UI_Manager.Instance.rMenu.CloseMenu();
						//SetState( EPlayerControllerState.PCSTATE_Normal ); // This is done inside CloseMenu, since you can exit that menu by pressing back repeatedly too.
					}
				}
				break;

			case EPlayerControllerState.PCSTATE_Cutscene:

				break;

			case EPlayerControllerState.PCSTATE_Dialogue:

				break;
		}
	}


	private bool CanInteract()
	{
		if ( m_rPlayer.m_CurrentlyFocusedInteractable == null )
			return false;


		return true;
	}
}
