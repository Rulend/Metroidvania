using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class BetterButton : MonoBehaviour, IPointerClickHandler
{


	public void OnPointerClick( PointerEventData pr_EventData )
	{
		if ( pr_EventData.button == PointerEventData.InputButton.Left )
		{
			// Bring up submenu
			Debug.Log( "Hey this is lego city!" );
		}

		else if ( pr_EventData.button == PointerEventData.InputButton.Right )
		{
			// Equip/use an item without submenu
			Debug.Log( "This ain't no lego city!" );
		}
	}

}
