using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideUIAfterDuration : MonoBehaviour
{

	public float m_AliveDuration = 1.5f;
	private float m_AliveTimeLeft;

    // Start is called before the first frame update
    void Start()
    {
		m_AliveTimeLeft = m_AliveDuration;

	}

    // Update is called once per frame
    void Update()
    {
		m_AliveTimeLeft -= Time.deltaTime;

		if ( m_AliveTimeLeft < 0.0f )
			gameObject.SetActive( false );
    }

	public void ResetAliveTimeLeft( )
	{
		m_AliveTimeLeft = m_AliveDuration;
	}

}
