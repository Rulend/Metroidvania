using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemInfoPanel : MonoBehaviour
{
	[SerializeField] private Image	m_ItemIcon;			// Should always exist in one of this object's children. 
	private Text	m_ItemName;			// Should always exist in one of this object's children.
	private Text	m_ItemDescription;  // Optional: add another child with a text component. It will automatically be filled with the item's description.
	private Text	m_ItemStats;        // Optional: only add if you want to see the stats of an equipment. add a third child with a text component. Will automatically be filled with the e


	private void Awake()
	{
		//m_ItemIcon = GetComponentInChildren<Image>( true );

		Text[] TextComponents = GetComponentsInChildren<Text>( true );

		m_ItemName = TextComponents[ 0 ];

		if ( TextComponents.Length > 1 )
			m_ItemDescription = TextComponents[ 1 ];
	}



	public void ShowPanel( InventoryItem _Item )
	{
		gameObject.SetActive( true );

		m_ItemIcon.sprite		= _Item.m_Icon;
		m_ItemName.text			= _Item.m_ItemName;
		m_ItemDescription.text	= _Item.m_ItemDescription;
	}

	public void HidePanel()
	{
		gameObject.SetActive( false );
	}

}
