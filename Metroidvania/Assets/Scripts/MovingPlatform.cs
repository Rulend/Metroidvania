using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
	[SerializeField] private List<Transform>	m_TravelPoints;				
	private List<Vector3>						m_TravelPointsInternal;		// Used for the internal calculations, since you can't drag vector3's inside the inspector
	private Vector3								m_GoalPoint;
	[SerializeField] private Vector3								m_TargetPoint;
	private Vector3								m_PreviousPoint;
	[SerializeField] private float				m_MoveDuration;
	private float								m_MoveTimeLeft;
	private int									m_ListTraversalDirection = 1;

	[SerializeField] bool						m_StartActive = false;

	// Choose one, makea dropdown
	[SerializeField] bool						m_DisableOnReachedLastPoint;
	[SerializeField] bool						m_ReturnOnReachedLastPoint;

    // Start is called before the first frame update
    private void Start()
    {
		m_TravelPointsInternal = new List<Vector3>();

		foreach ( Transform CurrentTransform in m_TravelPoints )
			m_TravelPointsInternal.Add( CurrentTransform.position );

		m_PreviousPoint = m_TravelPointsInternal[ 0 ];
		m_TargetPoint	= m_TravelPointsInternal[ 1 ];
		m_GoalPoint		= m_TravelPointsInternal[ m_TravelPointsInternal.Count - 1 ];

		if ( !m_StartActive )
			enabled = false;
	}

    // Update is called once per frame
    void Update()
    {
		m_MoveTimeLeft -= Time.deltaTime;

		transform.position = Vector3.Lerp( m_TargetPoint, m_PreviousPoint, (m_MoveTimeLeft / m_MoveDuration) ); // Lerp between start and end pos. As timer gets less and less, startpos is the right parameter, and target is the left.

		if ( (m_TargetPoint - transform.position).sqrMagnitude < 0.1f ) // If close enough to targetpoint 
		{
			if ( m_TargetPoint == m_GoalPoint ) // If current target point was goal point
			{
				if ( m_DisableOnReachedLastPoint )
					enabled = false;
				else
					OnEnable();
			}
			else
			{
				m_MoveTimeLeft = m_MoveDuration;

				m_PreviousPoint = m_TargetPoint;
				m_TargetPoint	= m_TravelPointsInternal[ m_TravelPointsInternal.IndexOf( m_TargetPoint ) + m_ListTraversalDirection ];
			}
		}
    }

	private void OnEnable()
	{
		if ( m_TravelPointsInternal == null )
			return;

		m_MoveTimeLeft = m_MoveDuration;
		m_ListTraversalDirection *= -1;

		if ( m_ListTraversalDirection == 1 ) // Traveled from first to last point
		{
			m_GoalPoint		= m_TravelPointsInternal[ 0 ];
			m_TargetPoint	= m_TravelPointsInternal[ m_TravelPointsInternal.Count - 2 ]; // Will not crash, since this script requires at least 2 points to work.
			m_PreviousPoint = m_TravelPointsInternal[ m_TravelPointsInternal.Count - 1 ];
		}
		else if ( m_ListTraversalDirection == -1 )// Traveled from last to first point
		{
			m_GoalPoint		= m_TravelPointsInternal[ m_TravelPointsInternal.Count - 1 ];
			m_TargetPoint	= m_TravelPointsInternal[ 1 ];
			m_PreviousPoint = m_TravelPointsInternal[ 0 ];
		} 
	}
}
