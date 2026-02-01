using UnityEngine;
using UnityEngine.UI;
using InteractionSystem.Runtime.Player;

namespace InteractionSystem.Runtime.UI
{
    public class InteractionProgressUI : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private InteractionDetector m_Detector;
        [SerializeField] private GameObject m_BarPanel; // Arka plan
        [SerializeField] private Image m_FillImage;     // Yeþil kýsým

        private void Start()
        {
            // Paneli gizle ve barý sýfýrla
            if (m_BarPanel != null) m_BarPanel.SetActive(false);
            if (m_FillImage != null) m_FillImage.fillAmount = 0f;
        }

        private void OnEnable()
        {
            if (m_Detector != null)
                m_Detector.OnInteractionProgress += UpdateBar;
        }

        private void OnDisable()
        {
            if (m_Detector != null)
                m_Detector.OnInteractionProgress -= UpdateBar;
        }

        private void UpdateBar(float progress)
        {
            if (progress > 0f)
            {
                m_BarPanel.SetActive(true);
                m_FillImage.fillAmount = progress;
            }
            else
            {
                m_BarPanel.SetActive(false);
                m_FillImage.fillAmount = 0f;
            }
        }
    }
}