using UnityEngine;

namespace InteractionSystem.Runtime.Core
{

    // Oyundaki toplanabilir eþyalarýn (Anahtar vb.) kimlik kartý.

    [CreateAssetMenu(fileName = "NewItemData", menuName = "InteractionSystem/Item Data")]
    public class ItemDataSO : ScriptableObject
    {
        [Header("Item Info")]
        [Tooltip("Eþyanýn oyundaki adý.")]
        [SerializeField] private string m_ItemName;

        [Tooltip("Envanterde görünecek ikon.")]
        [SerializeField] private Sprite m_Icon;

        public string ItemName => m_ItemName;
        public Sprite Icon => m_Icon;
    }
}