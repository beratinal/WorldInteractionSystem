using UnityEngine;

namespace InteractionSystem.Runtime.Core
{
    /// <summary>
    /// Etkileþim türlerini belirler.
    /// </summary>
    public enum InteractionType
    {
        Instant, 
        Hold,    
        Toggle   
    }

    /// <summary>
    /// Tüm etkileþimli nesneler için temel (base) sýnýf.
    /// Kod tekrarýný önlemek için ortak mantýðý barýndýrýr.
    /// </summary>
    public abstract class BaseInteractable : MonoBehaviour, IInteractable
    {
        #region Private Fields

        [Header("Base Settings")]
        [Tooltip("UI üzerinde oyuncuya gösterilecek mesaj.")]
        [SerializeField] private string m_InteractionPrompt = "Interact";

        [Tooltip("Bu nesnenin etkileþim türü.")]
        [SerializeField] private InteractionType m_InteractionType = InteractionType.Instant;

        #endregion

        #region Public Properties

        /// <inheritdoc />
        public string InteractionPrompt => m_InteractionPrompt;

        /// <summary>
        /// Nesnenin etkileþim türünü döndürür.
        /// </summary>
        public InteractionType Type => m_InteractionType;

        #endregion

        #region Public Methods

        /// <inheritdoc />
        public abstract void OnInteract();

        /// <inheritdoc />
        public virtual void OnFocus()
        {
            // Ýsteðe baðlý: Ýleride buraya Highlight (parlama) kodu ekleyeceðiz.
        }

        /// <inheritdoc />
        public virtual void OnLoseFocus()
        {
            // Ýsteðe baðlý: Highlight kapatma kodu.
        }

        #endregion
    }
}