using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class BetterButton : MonoBehaviour, ISelectHandler
{
	public virtual void OnSelect( BaseEventData _EventData )
	{
		UI_Manager.Instance.rMenu.SelectButton( GetComponent<Button>() );
	}
}
