using UnityEngine;
using System;
using InteractionSystem.Runtime.Core;

namespace InteractionSystem.Runtime.Player
{
    public class InteractionDetector : MonoBehaviour
    {
        #region Private Fields

        [Header("Detection Settings")]
        [SerializeField] private float m_InteractionRange = 3.0f;
        [SerializeField] private LayerMask m_InteractableLayer;
        [SerializeField] private KeyCode m_InteractionKey = KeyCode.E;

        [Header("References")]
        [SerializeField] private Transform m_CameraTransform;

        private IInteractable m_CurrentInteractable;

        // HOLD Mekaniði için sayaç
        private float m_HoldTimer = 0f;

        #endregion

        #region Events

        public event Action<IInteractable> OnInteractableChanged;

        // UI'daki barý güncellemek için event (0.0 ile 1.0 arasý deðer gönderir)
        public event Action<float> OnInteractionProgress;

        #endregion

        #region Unity Methods

        private void Awake()
        {
            if (m_CameraTransform == null) m_CameraTransform = Camera.main?.transform;
        }

        private void Update()
        {
            HandleRaycast();
            HandleInput();
        }

        #endregion

        #region Private Methods

        private void HandleRaycast()
        {
            if (m_CameraTransform == null) return;

            Ray ray = new Ray(m_CameraTransform.position, m_CameraTransform.forward);
            RaycastHit hit;

            bool hitSomething = Physics.Raycast(ray, out hit, m_InteractionRange, m_InteractableLayer);

            if (hitSomething)
            {
                if (hit.collider.TryGetComponent(out IInteractable interactable))
                {
                    if (m_CurrentInteractable != interactable)
                    {
                        ChangeInteractable(interactable);
                    }
                }
                else
                {
                    ClearInteractable();
                }
            }
            else
            {
                ClearInteractable();
            }
        }

        private void HandleInput()
        {
            if (m_CurrentInteractable == null) return;

            var baseInteractable = m_CurrentInteractable as BaseInteractable;

            if (baseInteractable == null)
            {
                if (Input.GetKeyDown(m_InteractionKey)) m_CurrentInteractable.OnInteract();
                return;
            }

            // TÜRÜNE GÖRE ÝÞLEM
            switch (baseInteractable.Type)
            {
                case InteractionType.Instant:
                case InteractionType.Toggle:
                    if (Input.GetKeyDown(m_InteractionKey))
                    {
                        m_CurrentInteractable.OnInteract();
                    }
                    break;

                case InteractionType.Hold:
                    HandleHoldInput(baseInteractable);
                    break;
            }
        }

        private void HandleHoldInput(BaseInteractable interactable)
        {
            // Tuþa basýlý tutuluyor mu?
            if (Input.GetKey(m_InteractionKey))
            {
                m_HoldTimer += Time.deltaTime;

                float progress = Mathf.Clamp01(m_HoldTimer / interactable.HoldDuration);

                // UI'a haber ver
                OnInteractionProgress?.Invoke(progress);

                // Süre doldu mu?
                if (m_HoldTimer >= interactable.HoldDuration)
                {
                    m_CurrentInteractable.OnInteract();
                    m_HoldTimer = 0f; 
                    OnInteractionProgress?.Invoke(0f); 
                }
            }
            else
            {
                // Tuþ býrakýldýysa sayacý sýfýrla
                if (m_HoldTimer > 0f)
                {
                    m_HoldTimer = 0f;
                    OnInteractionProgress?.Invoke(0f);
                }
            }
        }

        private void ChangeInteractable(IInteractable newInteractable)
        {
            m_CurrentInteractable?.OnLoseFocus();
            m_CurrentInteractable = newInteractable;
            m_CurrentInteractable?.OnFocus();

            // Yeni nesneye geçince sayacý sýfýrla
            m_HoldTimer = 0f;
            OnInteractionProgress?.Invoke(0f);

            OnInteractableChanged?.Invoke(m_CurrentInteractable);
        }

        private void ClearInteractable()
        {
            if (m_CurrentInteractable != null)
            {
                m_CurrentInteractable.OnLoseFocus();
                m_CurrentInteractable = null;

                m_HoldTimer = 0f;
                OnInteractionProgress?.Invoke(0f);
                OnInteractableChanged?.Invoke(null);
            }
        }

        #endregion
    }
}