using UnityEngine;
using UnityEngine.Events; 
using InteractionSystem.Runtime.Core;

namespace InteractionSystem.Runtime.Interactables
{
    public class SwitchInteractable : BaseInteractable
    {
        #region Private Fields

        [Header("Visual Settings")]
        [Tooltip("Döndürülecek olan kol.")]
        [SerializeField] private Transform m_HandlePivot;

        [Tooltip("Þalter Açýkken kolun açýsý.")]
        [SerializeField] private Vector3 m_OnRotation = new Vector3(45f, 0f, 0f);

        [Tooltip("Þalter Kapalýyken kolun açýsý.")]
        [SerializeField] private Vector3 m_OffRotation = new Vector3(-45f, 0f, 0f);

        [Tooltip("Kolun dönme hýzý.")]
        [SerializeField] private float m_RotateSpeed = 5f;

        [Header("Events")]
        [Tooltip("Þalter açýldýðýnda tetiklenecekler.")]
        public UnityEvent OnSwitchOn;

        [Tooltip("Þalter kapandýðýnda tetiklenecekler.")]
        public UnityEvent OnSwitchOff;

        private bool m_IsOn = false;

        #endregion

        #region Public Methods

        public override void OnInteract()
        {
            // Durumu tersine çevirme
            m_IsOn = !m_IsOn;

            if (m_IsOn)
            {
                Debug.Log("[Switch] Durum: AÇIK");
                OnSwitchOn?.Invoke(); // Baðlý olan her þeyi çalýþtýr
            }
            else
            {
                Debug.Log("[Switch] Durum: KAPALI");
                OnSwitchOff?.Invoke(); // Baðlý olan her þeyi durdur
            }
        }

        #endregion

        #region Unity Methods

        private void Update()
        {
            if (m_HandlePivot != null)
            {
                Quaternion targetRot = Quaternion.Euler(m_IsOn ? m_OnRotation : m_OffRotation);

                m_HandlePivot.localRotation = Quaternion.Lerp(
                    m_HandlePivot.localRotation,
                    targetRot,
                    Time.deltaTime * m_RotateSpeed
                );
            }
        }

        #endregion
    }
}