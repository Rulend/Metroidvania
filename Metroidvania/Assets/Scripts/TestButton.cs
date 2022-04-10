using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestButton : Interactable
{
	[SerializeField] private Pillar[] m_ControlledPillars;

	private void Awake()
	{
		//manager = GameManager.Instance; // Replaced with a local variable futher down. TODO: Remove this.
		m_InteractableAlertText += "Move pillars";
	}


	public override void Interact()
	{
		base.Interact();

		foreach( Pillar CurrentPillar in m_ControlledPillars )
		{
			CurrentPillar.MovePillar();
		}
	}

}
