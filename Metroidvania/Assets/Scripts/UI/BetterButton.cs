using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public abstract class BetterButton : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
	public abstract void OnPointerClick( PointerEventData pr_EventData );

	public abstract void OnPointerEnter( PointerEventData pr_EventData );

	public abstract void OnPointerExit( PointerEventData pr_EventData );
}
