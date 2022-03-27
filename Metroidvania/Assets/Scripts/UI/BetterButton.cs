using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class BetterButton : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{

	public UnityEvent m_LeftMouseButton;
	public UnityEvent m_RightMouseButton;
	public UnityEvent m_MiddleMouseButton;

	public UnityEvent m_PointerEntered;
	public UnityEvent m_PointerExited;

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


	public void OnPointerEnter( PointerEventData pr_EventData )
	{
		m_PointerEntered.Invoke();

	}

	public void OnPointerExit( PointerEventData pr_EventData )
	{
		m_PointerExited.Invoke();
	}



	public void OnPointer()
	{

	}

}
