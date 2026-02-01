using UnityEngine;
using System.Collections; 
using InteractionSystem.Runtime.Core;

namespace InteractionSystem.Runtime.Interactables
{
    public class ChestInteractable : BaseInteractable
    {
        #region Private Fields

        [Header("Animation Settings")]
        [Tooltip("Döndürülecek olan menteþe objesi.")]
        [SerializeField] private Transform m_LidPivot;

        [Tooltip("Açýlma açýsý.")]
        [SerializeField] private float m_OpenAngle = -90f; 

        [Tooltip("Açýlma hýzý.")]
        [SerializeField] private float m_AnimationSpeed = 2f;

        private bool m_IsOpen = false;

        #endregion

        #region Public Methods

        public override void OnInteract()
        {
            if (m_IsOpen) return;

            m_IsOpen = true;
            Debug.Log("THE CHEST IS OPEN! The treasure is yours.");

            // Animasyonu baþlat
            StartCoroutine(OpenLidRoutine());
        }

        #endregion

        #region Private Methods

        private IEnumerator OpenLidRoutine()
        {
            Quaternion startRotation = m_LidPivot.localRotation;
            Quaternion targetRotation = Quaternion.Euler(m_OpenAngle, 0, 0); // X ekseninde dön (Kapaðý kaldýr)

            float timer = 0f;

            while (timer < 1f)
            {
                timer += Time.deltaTime * m_AnimationSpeed;
                // Slerp ile yumuþak geçiþ
                m_LidPivot.localRotation = Quaternion.Slerp(startRotation, targetRotation, timer);
                yield return null;
            }

            // Tam açýya kilitle
            m_LidPivot.localRotation = targetRotation;
        }

        #endregion
    }
}