using UnityEngine;
using UnityEngine.UI; 
using TMPro; 
using InteractionSystem.Runtime.Player;
using InteractionSystem.Runtime.Core;

namespace InteractionSystem.Runtime.UI
{
    public class InventoryUI : MonoBehaviour
    {
        #region Private Fields

        [Header("References")]
        [Tooltip("Baðlý olduðu Player Inventory scripti.")]
        [SerializeField] private PlayerInventory m_PlayerInventory;

        [Header("UI Elements")]
        [Tooltip("Açýlýp kapanacak olan ana panel.")]
        [SerializeField] private GameObject m_Panel;

        [Tooltip("Eþyalarýn dizileceði kutu (Grid Layout olan obje).")]
        [SerializeField] private Transform m_SlotContainer;

        [Tooltip("Kopyalanacak olan Slot prefabý.")]
        [SerializeField] private GameObject m_SlotPrefab;

        #endregion

        #region Unity Methods

        private void Start()
        {
            if (m_Panel != null) m_Panel.SetActive(false);

            if (m_PlayerInventory != null)
            {
                m_PlayerInventory.OnInventoryChanged += RefreshUI;
            }
        }

        private void Update()
        {
            // TAB tuþu kontrolü
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                ToggleInventory();
            }
        }

        private void OnDestroy()
        {
            if (m_PlayerInventory != null)
            {
                m_PlayerInventory.OnInventoryChanged -= RefreshUI;
            }
        }

        #endregion

        #region Private Methods

        private void ToggleInventory()
        {
            if (m_Panel == null) return;

            bool isActive = !m_Panel.activeSelf;
            m_Panel.SetActive(isActive);

            var playerController = FindFirstObjectByType<InteractionSystem.Runtime.Player.FirstPersonController>();

            if (isActive)
            {
                RefreshUI();

                // Mouse'u serbest býrak
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;

                if (playerController != null) playerController.enabled = false;
            }
            else
            {

                // Mouse'u kilitle
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;

                if (playerController != null) playerController.enabled = true;
            }
        }

        private void RefreshUI()
        {
            // 1. Önceki slotlarý temizle (Child'larý yok et)
            foreach (Transform child in m_SlotContainer)
            {
                Destroy(child.gameObject);
            }

            // 2. Yeni listeyi al
            var items = m_PlayerInventory.GetItems();

            // 3. Her eþya için bir slot yarat
            foreach (var item in items)
            {
                GameObject slot = Instantiate(m_SlotPrefab, m_SlotContainer);

                Image iconImage = slot.transform.Find("Icon")?.GetComponent<Image>();
                TextMeshProUGUI nameText = slot.transform.Find("ItemName")?.GetComponent<TextMeshProUGUI>();

                if (iconImage != null) iconImage.sprite = item.Icon;
                if (nameText != null) nameText.text = item.ItemName;
            }
        }

        #endregion
    }
}