using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemList : MonoBehaviour
{
	private List<InventoryItem>			m_HeldItems;
	private List<List_Item>			m_ItemSlots;
	int								m_AmountItemsWidth	= 6;
	float							m_ItemApartDistX	= 50.0f;
	float							m_ItemApartDistY	= 50.0f;




	private void Awake()
	{
		m_HeldItems = new List<InventoryItem>();
		m_ItemSlots = new List<List_Item>();
	}


	// Start is called before the first frame update
	void Start()
    {

		UI_Manager.Instance.rInventoryUI.UpdateDisplayedItemsEvent += UpdateList;
	}

    // Update is called once per frame
    void Update()
    {
        
    }

	// Send in a list of either all items if its a shop, or the categorized items if it's a player's equipment.
	// TODO:: Make it so this doesn't have to be created every time, just do a bunch of them at start and switch them on/off. Do this even if performance is not affected, because this way fucking sucks lol
	public void UpdateList( List<InventoryItem> _NewList )
	{
		m_HeldItems = _NewList;

		foreach ( List_Item currentItem in m_ItemSlots )
		{
			Destroy( currentItem.gameObject );
		}

		m_ItemSlots.Clear();

		Vector2 Size			= new Vector2( 200.0f, 200.0f );
		Vector3 OffsetVector	= Vector3.zero;
		OffsetVector.x			= -Size.x;
		int xChecker			= 0;
		int yChecker			= 0;

		for ( int ItemIndex = 0; ItemIndex < m_HeldItems.Count; ++ItemIndex )
		{
			OffsetVector.x += Size.x + m_ItemApartDistX;

			if ( ItemIndex % ( m_AmountItemsWidth - 1 ) == 0 )
			{
				if ( ItemIndex != 0 )
				{
					OffsetVector.x = m_ItemApartDistX;
					OffsetVector.y -= ( m_ItemApartDistY + Size.y );     // Minus since that is down on the screen
					xChecker = 0;
					yChecker++;
				}
			}	

			InventoryItem CurrentItem = m_HeldItems[ ItemIndex ];



			GameObject NewListItem = new GameObject( $"ItemSlot{ItemIndex}", typeof( RectTransform ), typeof( List_Item ), typeof( CanvasRenderer ), typeof( Image )  );

			NewListItem.GetComponent<CanvasRenderer>().cullTransparentMesh	= true;
			NewListItem.GetComponent<RectTransform>().sizeDelta				= Size;
			NewListItem.GetComponent<RectTransform>().SetParent( GetComponent<RectTransform>() );
			NewListItem.GetComponent<RectTransform>().position				= ( transform.position + OffsetVector );
			NewListItem.GetComponent<Image>().sprite						= ( CurrentItem.m_Icon );


			m_ItemSlots.Add( NewListItem.GetComponent<List_Item>() );

			xChecker++;
		}
	}
}
