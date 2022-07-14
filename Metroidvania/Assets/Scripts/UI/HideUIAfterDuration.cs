using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HideUIAfterDuration : MonoBehaviour
{

	[SerializeField] private float	m_AliveDuration = 1.5f;
	private float					m_AliveTimeLeft;

	[SerializeField] private UnityEvent m_OnHideEvent;

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
		{
			gameObject.SetActive( false );
			m_OnHideEvent.Invoke();
		}
    }

	public void ResetAliveTimeLeft( ) // Needed instead of just using OnEnabled; using this function will prevent it from disappearing if the player picks up multiple items in a row.
	{
		m_AliveTimeLeft = m_AliveDuration;
	}

}
