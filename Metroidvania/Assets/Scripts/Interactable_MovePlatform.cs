using UnityEngine;

public class Interactable_MovePlatform : Interactable
{
	[SerializeField] private MovingPlatform[] m_ControlledPillars;

	private void Awake()
	{
		m_InteractableAlertText += "Move pillars";
	}


	public override void Interact()
	{
		base.Interact();

		foreach( MovingPlatform CurrentPillar in m_ControlledPillars )
			CurrentPillar.enabled = true;
	}
}
