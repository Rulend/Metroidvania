using UnityEngine;

public class InventoryUI : MonoBehaviour
{
	private Inventory m_Inventory;

	[ SerializeField ] private GameObject		m_InventorySlotsParent;
	private InventorySlot[]						m_InventorySlots;

	// Start is called before the first frame update
	void Start()
	{
		m_Inventory = GameManager.Instance.Player1.GetInventory;
		m_Inventory.m_ItemsChangedCallback += UpdateUI;

		m_InventorySlots = m_InventorySlotsParent.GetComponentsInChildren<InventorySlot>();
	}

	// Update is called once per frame
	void Update()
	{

	}

	void UpdateUI()
	{
		Debug.Log("Updating UI!");

		for( int SlotsIndex = 0; SlotsIndex < m_InventorySlots.Length; ++SlotsIndex )
		{
			if ( SlotsIndex < m_Inventory.m_Items.Count)
			{
				m_InventorySlots[SlotsIndex].AddItemToSlot( m_Inventory.m_Items[SlotsIndex] );
			}
			else
			{
				m_InventorySlots[SlotsIndex].RemoveItemFromSlot();
			}

		}

	}
}
