using UnityEngine;

// This is a completely useless class apart from the fact that it is passed as a parameter to the category buttons in the inventory. This is because enums aren't allowed to be passed to buttons onclick.
public class Equipment_Category_Component : MonoBehaviour
{
	public EquipmentSlot m_EquipmentCategory;
}
