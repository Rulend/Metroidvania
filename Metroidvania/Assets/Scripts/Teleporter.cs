using UnityEngine;

public class Teleporter : Interactable
{
	//Teleporter[] m_rTeleporters; // gonna use this later when I've made so the player can choose which points to teleport to. Gonna do that after implementing save points.
	[SerializeField] private Teleporter m_TargetLocation;

    // Start is called before the first frame update
    void Awake()
    {
		m_InteractableAlertText += "Take Teleport";
    }

	public override void Interact()
	{
		base.Interact();

		TeleportPlayer();
	}

	private void TeleportPlayer()
	{
		m_rPlayer.transform.position = m_TargetLocation.gameObject.transform.position + new Vector3( 0.0f, 0.5f, 0.0f );
	}

}
