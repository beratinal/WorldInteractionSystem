using UnityEngine;
using TMPro; 
using InteractionSystem.Runtime.Player; 
using InteractionSystem.Runtime.Core;   

namespace InteractionSystem.Runtime.UI
{
    // Oyuncunun etkileþime geçebileceði bir nesneye baktýðýnda ekranda çýkan yazýyý yönetir.
    public class InteractionPromptUI : MonoBehaviour
    {
        #region Private Fields

        [Header("References")]
        [Tooltip("Player üzerindeki detector scripti.")]
        [SerializeField] private InteractionDetector m_Detector;

        [Tooltip("Açýlýp kapanacak olan UI paneli.")]
        [SerializeField] private GameObject m_PromptPanel;

        [SerializeField] private TextMeshProUGUI m_PromptText;

        #endregion

        #region Unity Methods

        private void Start()
        {
            if (m_PromptPanel != null)
                m_PromptPanel.SetActive(false);
        }

        private void OnEnable()
        {
            if (m_Detector != null)
            {
                m_Detector.OnInteractableChanged += UpdatePrompt;
            }
        }

        private void OnDisable()
        {
            if (m_Detector != null)
            {
                m_Detector.OnInteractableChanged -= UpdatePrompt;
            }
        }

        #endregion

        #region Private Methods
        private void UpdatePrompt(IInteractable interactable)
        {
            if (interactable != null)
            {
                m_PromptText.text = interactable.InteractionPrompt;
                m_PromptPanel.SetActive(true);
            }
            else
            {
                m_PromptPanel.SetActive(false);
            }
        }

        #endregion
    }
}