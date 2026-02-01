using System.Collections.Generic;
using UnityEngine;
using System; 
using InteractionSystem.Runtime.Core;

namespace InteractionSystem.Runtime.Player
{
    /// <summary>
    /// Oyuncunun sahip olduðu eþyalarý tutar ve yönetir.
    /// </summary>
    public class PlayerInventory : MonoBehaviour
    {
        #region Private Fields

        // Oyuncunun cebindeki eþyalar
        private List<ItemDataSO> m_Items = new List<ItemDataSO>();

        #endregion

        #region Events

        /// <summary>
        /// Envanter listesi deðiþtiðinde tetiklenir.
        /// UI'ý güncellemek için kullanýlýr.
        /// </summary>
        public event Action OnInventoryChanged;

        #endregion

        #region Public Methods

      
        //Envantere yeni bir eþya ekler.

        public void AddItem(ItemDataSO item)
        {
            if (!m_Items.Contains(item))
            {
                m_Items.Add(item);
                Debug.Log($"[Inventory] Eklendi: {item.ItemName}");

                // Haber ver: Liste deðiþti!
                OnInventoryChanged?.Invoke();
            }
        }

   
        // Belirli bir eþyanýn bizde olup olmadýðýný kontrol eder.

        public bool HasItem(ItemDataSO item)
        {
            return m_Items.Contains(item);
        }

        public List<ItemDataSO> GetItems()
        {
            return m_Items;
        }

        #endregion
    }
}