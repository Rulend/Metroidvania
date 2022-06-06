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
	void Start()
    {
		// Set character values inherited from Character-class. Read these from save file later, once a save system has been set up.
		//m_BaseMovementSpeed = 350.0f;
		//m_CurrentMovementSpeed = m_BaseMovementSpeed /* * speedMultiplier */ ;
		m_CurrentMovementSpeed = m_BaseMovementSpeed;
		m_MaxHealth = 10.0f;
		m_CurrentHealth = m_MaxHealth;

		// Get inventory UI
		m_rInventoryUI = UI_Manager.Instance.rInventoryUI.gameObject;
	}

	void Awake()
	{
		m_Inventory = gameObject.GetComponent<Inventory>();
	}
}
