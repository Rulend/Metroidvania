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

	// We use an enum for convenience's sake to access the gameplay actions,
	// though it comes at the cost of an explicit conversion every time.
	// This can be fixed by making use of const values instead, but it's annoying having to adjust them when adding new actions, so will do that later.
	private enum EGameplayActions
	{
		Move = 0,
		UseCurrentConsumable,
		Interact,
		Roll,
		Jump,
		OpenMenu,
		PrevConsumable,
		NextConsumable,
		DebugReset,

		ActionCount
	}

	// Same as above enum, might switch this out
	private enum EMenuActions
	{
		Confirm = 0,
		GoBack,
		Alternative1,
		Alternative2,
		PrevTab,
		NextTab,
		ExitMenu,

		ActionCount
	}


	// TODO:: I know this is the PlayerController but uh, another look at how input is taken is defenitively required. There's so many variables in here that aren't needed.
	// An easy way to solve it would be to have lists with the actions in, and then use an enum to check individual button inputs; another would be to make use of the context-based functions 

	// Movement variables.
	private bool	m_ActiveInput	= true;
	private float	m_LRInput		= 0.0f;
	private float	m_UDInput		= 0.0f;

	// Used for controlling stuff, move to controller script later.
	[SerializeField] private InputActionAsset	m_InputActionAsset;

	private EPlayerControllerState	m_CurrentState;

	private InputActionMap[] m_ActionMaps;

	// Normal game
	// These are const bytes instead of an enum, since otherwise we have to cast the enum every time since it's C#
	InputAction[] gameplayActions;


	// For menu
	InputAction[] menuActions;

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
		m_ActionMaps[ (int)EPlayerControllerState.Gameplay ]		= m_InputActionAsset.FindActionMap( "MainGame" );
		m_ActionMaps[ (int)EPlayerControllerState.Menu ]		= m_InputActionAsset.FindActionMap( "Menu" );
		m_ActionMaps[ (int)EPlayerControllerState.Cutscene ]	= m_InputActionAsset.FindActionMap( "Cutscene" );
		m_ActionMaps[ (int)EPlayerControllerState.Dialogue ]	= m_InputActionAsset.FindActionMap( "Dialogue" );

		// Set up actions for the menu
		m_ActionMaps[ (int)EPlayerControllerState.Menu ].Enable();
		menuActions = new InputAction[ (int)EMenuActions.ActionCount ];
		menuActions[ (int)EMenuActions.Confirm ]			= m_ActionMaps[ (int)EPlayerControllerState.Menu ].FindAction( "Confirm" );
		menuActions[ (int)EMenuActions.GoBack ]			= m_ActionMaps[ (int)EPlayerControllerState.Menu ].FindAction( "GoBack" );
		menuActions[ (int)EMenuActions.Alternative1 ]	= m_ActionMaps[ (int)EPlayerControllerState.Menu ].FindAction( "Alternative1" );
		menuActions[ (int)EMenuActions.Alternative2 ]	= m_ActionMaps[ (int)EPlayerControllerState.Menu ].FindAction( "Alternative2" );
		menuActions[ (int)EMenuActions.PrevTab ]			= m_ActionMaps[ (int)EPlayerControllerState.Menu ].FindAction( "LeftTab" );
		menuActions[ (int)EMenuActions.NextTab ]			= m_ActionMaps[ (int)EPlayerControllerState.Menu ].FindAction( "RightTab" );
		menuActions[ (int)EMenuActions.ExitMenu ]		= m_ActionMaps[ (int)EPlayerControllerState.Menu ].FindAction( "ExitMenu" );
		m_ActionMaps[ (int)EPlayerControllerState.Menu ].Disable();


		// Setup the actions for gameplay
		m_ActionMaps[ (int)EPlayerControllerState.Gameplay ].Enable();
		gameplayActions = new InputAction[ (int)EGameplayActions.ActionCount ];
		gameplayActions[ (int)EGameplayActions.Move ]					= m_ActionMaps[ (int)EPlayerControllerState.Gameplay ].FindAction( "Move" );
		gameplayActions[ (int)EGameplayActions.UseCurrentConsumable ]	= m_ActionMaps[ (int)EPlayerControllerState.Gameplay ].FindAction( "UseCurrentConsumable" );
		gameplayActions[ (int)EGameplayActions.Interact ]				= m_ActionMaps[ (int)EPlayerControllerState.Gameplay ].FindAction( "Interact" );
		gameplayActions[ (int)EGameplayActions.Roll ]					= m_ActionMaps[ (int)EPlayerControllerState.Gameplay ].FindAction( "Roll" );
		gameplayActions[ (int)EGameplayActions.Jump ]					= m_ActionMaps[ (int)EPlayerControllerState.Gameplay ].FindAction( "Jump" );
		gameplayActions[ (int)EGameplayActions.OpenMenu ]				= m_ActionMaps[ (int)EPlayerControllerState.Gameplay ].FindAction( "OpenMenu" );
		gameplayActions[ (int)EGameplayActions.PrevConsumable ]			= m_ActionMaps[ (int)EPlayerControllerState.Gameplay ].FindAction( "EquipmentWheelPreviousConsumable" );
		gameplayActions[ (int)EGameplayActions.NextConsumable ]			= m_ActionMaps[ (int)EPlayerControllerState.Gameplay ].FindAction( "EquipmentWheelNextConsumable" );
		gameplayActions[ (int)EGameplayActions.DebugReset ]				= m_ActionMaps[ (int)EPlayerControllerState.Gameplay ].FindAction( "Reset" );
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


	////////////////////////////////////////////////
	/// Method Information - RemoveItem
	/// 
	/// Removes an item from the player's inventory.
	/// 
	/// parameters:
	/// pr_ItemToRemove		: the item which should be removed from the inventory.
	/// pr_SpawnItemPickup	: whether or not an item pickup should be spawned on the ground after removing the item. Used when dropping an item.
	/// 
	////////////////////////////////////////////////

	void TakeInput(  )
    {

		// TODO: Look up how to handle the input from a joystick based on how much it's being tilted in a direction. Tilting it fully should move the player faster than tilting it just a small bit.
		// TODO: Setup so that the type of buttons with available pressing depends on what kind of control-state it is currently in. If NOJUMP is set, then don't allow jumping, don't even check for jumping.
		// TODO:: Optimize this monster of a mess

		// Temporary reset function. Once a loading screen has been implemented, play the loading screen and respawn player at last checkpoint.
		if ( gameplayActions[ (int)EGameplayActions.DebugReset ].triggered )
		{
			m_ActiveInput = true;
			transform.position = new Vector3( 0.0f, 0.0f, 0.0f );
		}

        if ( !m_ActiveInput )
            return;


		switch ( m_CurrentState )
		{
			case EPlayerControllerState.Gameplay:
				{
					// The movement we want to apply is the input (taken here) multiplied by our chosen speed (done later).

					//////////////////////////////////////////////////////////////////////////////////
					//																				//
					// Button downs - what happens when a button is pressed.						//
					// The actions themselves are defined inside PlayerGameControls.inputactions	//
					//																				//
					//////////////////////////////////////////////////////////////////////////////////
					//if ( m_ActionMovement.triggered )
					//{
					//	//Debug.Log( "Move-action triggered...(-w- ')??" );
					//	m_UDInput = m_ActionMovement.ReadValue<Vector2>().y;
					//}

					// Always read this input
					Vector2 movementInput =  gameplayActions[ (int)EGameplayActions.Move ].ReadValue<Vector2>();
					m_LRInput = movementInput.x;
					m_UDInput = movementInput.y;

					// Jump (button south)
					if ( gameplayActions[ (int)EGameplayActions.Jump ].triggered )
					{
						if ( m_UDInput < 0.0f ) // If player presses down while jumping, slide instead
						{ m_rPlayerMovement.StartSlide(); }
						else 
						{ m_rPlayerMovement.Jump(); }
					}

					// Interact (button north)
					if ( gameplayActions[ (int)EGameplayActions.Interact ].triggered )
					{
						if ( CanInteract() )
						{ m_rPlayer.m_CurrentlyFocusedInteractable.Interact(); } //TODO:: Invoke an event called Interact, which invokes it on the closest one which should be subscribed to the event.
					}

					// Use consumable (button west)
					if ( gameplayActions[ (int)EGameplayActions.UseCurrentConsumable ].triggered )
					{ EquipmentManager.Instance.EquipWheel.UseCurrentConsumable( m_rPlayer ); }

					// Cycle consumables next (dpad down)
					if ( gameplayActions[ (int)EGameplayActions.NextConsumable ].triggered )
					{ EquipmentManager.Instance.EquipWheel.CycleConsumables( 1 ); }

					// Cycle consumables previous (dpad up) (Idk if I'm gonna use this, might do like most souls-likes and have some other type of item in the top slot)
					if ( gameplayActions[ (int)EGameplayActions.PrevConsumable ].triggered )
					{ EquipmentManager.Instance.EquipWheel.CycleConsumables( -1 ); }

					// Roll (button east)
					if ( gameplayActions[ (int)EGameplayActions.Roll ].triggered )
					{ m_rPlayerMovement.StartRoll(); }


					// Open the menu (start button)
					if ( gameplayActions[ (int)EGameplayActions.OpenMenu ].triggered )
					{
						//			if ( CanOpenMenu() ) // TODO:: Implement this function when it becomes necessary.

						m_LRInput = 0.0f;
						UI_Manager.Instance.rMenu.OpenMenu();

						m_ActionMaps[ (int)EPlayerControllerState.Gameplay ].Disable();
						m_ActionMaps[ (int)EPlayerControllerState.Menu ].Enable();
						m_CurrentState = EPlayerControllerState.Menu;
					}


					//////////////////////////////////////////////////////////////////////////////////
					//																				//
					// Button ups - what happens when a button is released.							//
					// The actions themselves are defined inside PlayerGameControls.inputactions	//
					//																				//
					//////////////////////////////////////////////////////////////////////////////////

					if ( gameplayActions[ (int)EGameplayActions.Move ].WasReleasedThisFrame() )
					{
						//Debug.Log( "Move-action released!!(-w- ')!" );
						m_UDInput = 0.0f;
					}

					if ( gameplayActions[ (int)EGameplayActions.Jump ].WasReleasedThisFrame() )
					{ m_rPlayerMovement.StopJump(); }
				}
				break;

			case EPlayerControllerState.Menu:
				{
					// When confirm is pressed, the EventSystem already takes care of it, because it counts as pressing a UI-button, so we don't need to handle this input.
					//if ( menuActions[ACTON_MENU_Confirm].triggered )
					//{
					//}

					// 
					if ( menuActions[ (int)EMenuActions.Alternative1 ].triggered )
					{ UI_Manager.Instance.rMenu.AlternativeButton1(); } 

					if ( menuActions[ (int)EMenuActions.Alternative2 ].triggered )
					{ UI_Manager.Instance.rMenu.AlternativeButton2(); } 

					if ( menuActions[ (int)EMenuActions.GoBack ].triggered )
					{ UI_Manager.Instance.rMenu.GoBackToPreviousWindow(); }

					if ( menuActions[ (int)EMenuActions.PrevTab ].triggered )
					{ UI_Manager.Instance.rMenu.LeftShoulderButton(); }

					if ( menuActions[ (int)EMenuActions.NextTab ].triggered )
					{ UI_Manager.Instance.rMenu.RightShoulderButton(); }

					if ( menuActions[ (int)EMenuActions.ExitMenu ].triggered )
					{ UI_Manager.Instance.rMenu.CloseMenu(); }
				}
				break;

			case EPlayerControllerState.Cutscene:

				break;

			case EPlayerControllerState.Dialogue:

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
