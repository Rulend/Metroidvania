using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pillar : MonoBehaviour
{
	private Vector3 m_StartPos;
	private Vector3 m_OffsetPos;
	[SerializeField] private float m_Offset = 5.0f;
	private Vector3 m_TargetPos;
	private bool m_ShouldMove = false;

    // Start is called before the first frame update
    void Start()
    {
		m_StartPos = transform.position;

		m_OffsetPos = m_StartPos + new Vector3( 0.0f, m_Offset, 0.0f );


		m_TargetPos = m_StartPos;
	}

    // Update is called once per frame
    void Update()
    {
        if ( m_ShouldMove )
		{
			transform.position = Vector3.Lerp( transform.position, m_TargetPos, Time.deltaTime );

			if ( ( m_TargetPos - transform.position ).sqrMagnitude < 0.1f )
			{
				m_ShouldMove = false;
			}
		}
    }

	public void MovePillar()
	{
		m_ShouldMove = true;

		if ( m_TargetPos == m_StartPos )
			m_TargetPos = m_OffsetPos;
		else if ( m_TargetPos == m_OffsetPos )
			m_TargetPos = m_StartPos;
	}

}
