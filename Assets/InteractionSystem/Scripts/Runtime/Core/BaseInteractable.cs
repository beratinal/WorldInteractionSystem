using UnityEngine;

namespace InteractionSystem.Runtime.Core
{
    public enum InteractionType
    {
        Instant,
        Hold,
        Toggle
    }

    public abstract class BaseInteractable : MonoBehaviour, IInteractable
    {
        #region Core Settings

        [Header("Interaction Settings")]
        [SerializeField] private string m_InteractionPrompt;
        [SerializeField] private InteractionType m_InteractionType = InteractionType.Instant;
        [SerializeField] private float m_HoldDuration = 2.0f;

        #endregion

        #region Highlight Settings (SUBTLE - HAFÝF)

        [Header("Highlight Settings")]
        [Tooltip("Rengi hafifçe açýlacak parça.")]
        [SerializeField] private Renderer m_Renderer;

        [Tooltip("Ne kadar aydýnlansýn?")]
        [Range(0f, 1f)]
        [SerializeField] private float m_BrightnessAmount = 0.15f;

        private Color m_OriginalColor;
        private Material m_TargetMaterial;
        private bool m_IsURP = false; 

        #endregion

        #region Public Properties

        public string InteractionPrompt => m_InteractionPrompt;
        public InteractionType Type => m_InteractionType;
        public float HoldDuration => m_HoldDuration;

        #endregion

        #region Unity Methods

        protected virtual void Awake()
        {
            if (m_Renderer == null) m_Renderer = GetComponentInChildren<Renderer>();

            if (m_Renderer != null)
            {
                m_TargetMaterial = m_Renderer.material;

                if (m_TargetMaterial.HasProperty("_BaseColor")) 
                {
                    m_OriginalColor = m_TargetMaterial.GetColor("_BaseColor");
                    m_IsURP = true;
                }
                else if (m_TargetMaterial.HasProperty("_Color"))
                {
                    m_OriginalColor = m_TargetMaterial.color;
                    m_IsURP = false;
                }
            }
        }

        #endregion

        #region IInteractable Methods

        public virtual void OnFocus()
        {
            if (m_Renderer != null && m_TargetMaterial != null)
            {
                Color brighterColor = m_OriginalColor + new Color(m_BrightnessAmount, m_BrightnessAmount, m_BrightnessAmount);

                if (m_IsURP)
                    m_TargetMaterial.SetColor("_BaseColor", brighterColor);
                else
                    m_TargetMaterial.color = brighterColor;
            }
        }

        public virtual void OnLoseFocus()
        {
            if (m_Renderer != null && m_TargetMaterial != null)
            {
                // Orijinal renge geri dön
                if (m_IsURP)
                    m_TargetMaterial.SetColor("_BaseColor", m_OriginalColor);
                else
                    m_TargetMaterial.color = m_OriginalColor;
            }
        }

        public abstract void OnInteract();

        #endregion
    }
}