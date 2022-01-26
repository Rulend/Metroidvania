using UnityEngine;

public class FollowTarget : MonoBehaviour
{
	// Drag the player's transform in here.
	[SerializeField] private Transform m_TargetTransform;	// The target that the camera should follow.

	private Vector3 m_SmoothVelocity = Vector3.zero;		// Used for setting the velocity of the SmoothDamp function. Idk why this is needed honestly.

	public float    m_SmoothSpeed = 0.125f;		// Used for deciding how fast the camera should reach the target's position. Lower number = faster.
	public Vector3  m_CameraOffset;				// Camera's offset from it's focused on target.

	private void Start()
	{
		// Set camera's focus to player by default if no other target has been set
		if ( !m_TargetTransform )
		{
			m_TargetTransform = FindObjectOfType<Player>().transform;
		}
	}

	// The player's movement happens inside FixedUpdate, so the camera's movement has to happen inside FixedUpdate as well.
	private void FixedUpdate()
	{
		Vector3 DesiredPosition = m_TargetTransform.position + m_CameraOffset;
		Vector3 SmoothPosition = Vector3.SmoothDamp(transform.position, DesiredPosition, ref m_SmoothVelocity, m_SmoothSpeed);

		transform.position = SmoothPosition;
	}



	// Switch camera's focus to a new transform.
	public bool SwitchFocus( Transform pr_NewFocus )
	{
		if ( m_TargetTransform = pr_NewFocus )
		{
			print("Succeded in changing camera focus to new target! \n");
			return true;
		}

		print("Failed to set new focus. \n");
		return false;
	}

}
