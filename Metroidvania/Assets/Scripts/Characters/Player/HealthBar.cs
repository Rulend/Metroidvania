using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
	private Image m_CurrentHealth;
	private Image m_DamageTaken;
	[SerializeField] private float m_DamageTakenUpdateDelay = 1.0f;
	[SerializeField] private float m_DamageTakenUpdateDuration = 1.0f;
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
		m_DamageTakenUpdateTimeLeft -= Time.deltaTime;

		m_DamageTaken.fillAmount = Mathf.Lerp( m_HealthPostDamage, m_HealthPreDamage, m_DamageTakenUpdateTimeLeft );
	}

	public void AdjustHealth( float _CurrentHeatlh, float _MaxHealth )
	{
		m_CurrentHealth.fillAmount	= (_CurrentHeatlh / _MaxHealth);

		if ( m_CurrentHealth.fillAmount > m_DamageTaken.fillAmount ) // If health was restored
			m_DamageTaken.fillAmount = m_CurrentHealth.fillAmount;

		m_HealthPreDamage	= m_DamageTaken.fillAmount;
		m_HealthPostDamage	= m_CurrentHealth.fillAmount;

		m_DamageTakenUpdateTimeLeft = m_DamageTakenUpdateDuration;

		StartCoroutine( StartDamageTakenTickDown() );
	}

	 private IEnumerator StartDamageTakenTickDown()
	{
		yield return new WaitForSeconds( m_DamageTakenUpdateDelay );

		enabled = true;
	}
}
