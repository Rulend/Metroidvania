using UnityEngine;
using UnityEngine.InputSystem;

public class Player : Character
{
	// < Member variables>

	// Player's stats
	// Derived from Character

	// The currently focused interactable.
	public Interactable m_CurrentlyFocusedInteractable { get; set; }

	private Inventory	m_Inventory;
	public Inventory	GetInventory => m_Inventory;
	private GameObject	m_rInventoryUI;

	// </End of Member variables>

	// Start is called before the first frame update
	public override void Start()
    {
		// Set character values inherited from Character-class. Read these from save file later, once a save system has been set up.
		//m_BaseMovementSpeed = 350.0f;
		//m_CurrentMovementSpeed = m_BaseMovementSpeed /* * speedMultiplier */ ;
		base.Start();

		m_CurrentMovementSpeed = m_BaseMovementSpeed;
		m_CurrentHealth = m_MaxHealth;

		// Get inventory UI
		m_rInventoryUI = UI_Manager.Instance.rInventoryUI.gameObject;
	}

	void Awake()
	{
		m_Inventory = gameObject.GetComponent<Inventory>();
	}


	public override void Heal( float _Amount )
	{
		base.Heal( _Amount );

		UI_Manager.Instance.UpdateHealth( m_CurrentHealth, m_MaxHealth );
	}


	public override void TakeDamage( Damage pr_IncomingDamage )
	{
		base.TakeDamage( pr_IncomingDamage );

		UI_Manager.Instance.UpdateHealth( m_CurrentHealth, m_MaxHealth );
	}
}
