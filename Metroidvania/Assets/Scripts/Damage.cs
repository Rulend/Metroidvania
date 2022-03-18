using UnityEngine;


public enum DamageTypes
{
	DT_PHYSICAL = 0	,
	DT_MAGICAL		,
	DT_FIRE			,
	DT_WATER		,
	DT_WIND			,
	DT_EARTH		,
	DT_LIGHTNING	,
	DT_POISON		,

	DT_TRUEDAMAGE
}



public class Damage
{
	public DamageTypes	m_Type;
	public float		m_Amount;
}
