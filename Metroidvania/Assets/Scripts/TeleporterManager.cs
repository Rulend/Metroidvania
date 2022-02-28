using UnityEngine;
using UnityEngine.SceneManagement;

public class TeleporterManager : Manager
{
	[SerializeField]	private		Teleporter[]	m_Teleporters;
						public		Teleporter[]	Teleporters => m_Teleporters;



	// Start is called before the first frame update
	void Start()
    {

    }
}
