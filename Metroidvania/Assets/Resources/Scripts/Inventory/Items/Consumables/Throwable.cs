using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Throwable : ConsumableEffect
{
	[SerializeField] float m_TravelDistance;
	[SerializeField] float m_DamageDealt;

	// Start is called before the first frame update
	void Start()
    {
        
    }

	public override void Activate( Character _Affected )
	{

		// Spawn throwable object here 
	}



}
