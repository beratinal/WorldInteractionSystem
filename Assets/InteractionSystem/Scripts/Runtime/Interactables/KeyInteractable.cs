using UnityEngine;
using InteractionSystem.Runtime.Core;
using InteractionSystem.Runtime.Player;

namespace InteractionSystem.Runtime.Interactables
{
    /// <summary>
    /// Yerde duran ve toplanabilen anahtar nesnesi.
    /// </summary>
    public class KeyInteractable : BaseInteractable
    {
        #region Private Fields

        [Header("Key Settings")]
        [Tooltip("Bu nesne toplandýðýnda envantere hangi veri eklenecek?")]
        [SerializeField] private ItemDataSO m_ItemData;

        #endregion

        #region Public Methods

        public override void OnInteract()
        {
            var inventory = FindFirstObjectByType<PlayerInventory>();

            if (inventory != null && m_ItemData != null)
            {
                inventory.AddItem(m_ItemData);

                Destroy(gameObject);
            }
            else
            {

                Debug.LogError("[KeyInteractable] Envanter bulunamadý veya ItemData boþ!");
            }
        }

        #endregion
    }
}