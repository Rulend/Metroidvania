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
	// TODO:: Make this into an array or list of floats, where each element's value is equal to the damage of that element dealt. 0, 0, 10 means 0 phys and mag damage, but 10 fire damage.
	public DamageTypes	m_Type;
	public float		m_Amount;
}
