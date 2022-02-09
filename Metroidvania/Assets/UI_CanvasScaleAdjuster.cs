using UnityEngine;
using UnityEngine.UI;

public class UI_CanvasScaleAdjuster : MonoBehaviour
{
	CanvasScaler m_rUICanvasScaler;

    // Start is called before the first frame update
    void Start()
    {
		m_rUICanvasScaler = gameObject.GetComponent<CanvasScaler>();

  //      if ( m_rUICanvasScaler.referenceResolution.x != Screen.currentResolution.width ||
		//	 m_rUICanvasScaler.referenceResolution.y != Screen.currentResolution.height )
		//{

		//}

		//Vector2 ScreenSize = new Vector2( Screen.currentResolution.width, Screen.currentResolution.height );
		m_rUICanvasScaler.referenceResolution.Set( Screen.currentResolution.width, Screen.currentResolution.height );
	}

    // Update is called once per frame
    void Update()
    {

	}
}
