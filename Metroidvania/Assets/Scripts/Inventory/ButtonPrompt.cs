using UnityEngine;
using UnityEngine.UI;

public class ButtonPrompt : MonoBehaviour
{
	private Text m_Text;

	private void Awake()
	{
		m_Text = GetComponentInChildren<Text>();
	}

	public void Activate( string _InteractionPrompt )
	{
		gameObject.SetActive( true );
		m_Text.text = _InteractionPrompt;
	}

	public void Deactivate()
	{
		gameObject.SetActive( false );
	}
}
