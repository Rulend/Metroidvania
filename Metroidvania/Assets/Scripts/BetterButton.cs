using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class BetterButton : MonoBehaviour, IPointerClickHandler
{

	public UnityEvent m_LeftMouseButton;
	public UnityEvent m_RightMouseButton;
	public UnityEvent m_MiddleMouseButton;


	public void OnPointerClick( PointerEventData pr_EventData )
	{
		if ( pr_EventData.button == PointerEventData.InputButton.Left )
		{
			m_LeftMouseButton.Invoke();
		}
		else if ( pr_EventData.button == PointerEventData.InputButton.Right )
		{
			m_RightMouseButton.Invoke();
		}
		else
		{
			m_MiddleMouseButton.Invoke();
		}
	}

	public void OnPointer()
	{

	}

}
