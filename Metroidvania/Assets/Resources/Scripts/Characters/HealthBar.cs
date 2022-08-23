using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
						private Image m_CurrentHealth;
						private Image m_DamageTaken;
	[SerializeField]	private float m_DelayDuration = 2.0f; // How long to wait after the last tick of damage was taken before updating the health bar
						private float m_DelayTimeLeft;
	[SerializeField]	private float m_DamageTakenUpdateDuration = 1.0f;
						private float m_DamageTakenUpdateTimeLeft;
						private float m_HealthPreDamage;
						private float m_HealthPostDamage;



	private void Awake()
	{
		m_DamageTaken	= gameObject.transform.GetChild( 1 ).GetComponent<Image>();
		m_CurrentHealth = gameObject.transform.GetChild( 2 ).GetComponent<Image>();

		enabled = false;
	}


    // Update is called once per frame
    void Update()
    {
		m_DelayTimeLeft -= Time.deltaTime;

		if ( m_DelayTimeLeft > 0.0f )
			return;

		m_DamageTakenUpdateTimeLeft -= Time.deltaTime;

		m_DamageTaken.fillAmount = Mathf.Lerp( m_HealthPostDamage, m_HealthPreDamage, m_DamageTakenUpdateTimeLeft / m_DamageTakenUpdateDuration );

		if ( m_DamageTakenUpdateTimeLeft < 0 )
			enabled = false;
	}

	public void AdjustHealth( float _CurrentHeatlh, float _MaxHealth )
	{
		m_CurrentHealth.fillAmount	= (_CurrentHeatlh / _MaxHealth);

		if ( m_CurrentHealth.fillAmount > m_DamageTaken.fillAmount ) // If health was restored
			m_DamageTaken.fillAmount = m_CurrentHealth.fillAmount;

		m_HealthPreDamage	= m_DamageTaken.fillAmount;
		m_HealthPostDamage	= m_CurrentHealth.fillAmount;

		m_DelayTimeLeft				= m_DelayDuration;
		m_DamageTakenUpdateTimeLeft = m_DamageTakenUpdateDuration;

		enabled = true;
	}
}
