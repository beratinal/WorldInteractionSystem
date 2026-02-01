using UnityEngine;
using System; 
using InteractionSystem.Runtime.Core; 

namespace InteractionSystem.Runtime.Player
{
   
    public class InteractionDetector : MonoBehaviour
    {
        #region Private Fields

        [Header("Detection Settings")]
        [Tooltip("Etkileþime geçilebilecek maksimum mesafe.")]
        [SerializeField] private float m_InteractionRange = 3.0f;

        [Tooltip("Hangi layer'daki nesnelerin taranacaðýný belirler.")]
        [SerializeField] private LayerMask m_InteractableLayer;

        [Tooltip("Etkileþim için kullanýlacak tuþ.")]
        [SerializeField] private KeyCode m_InteractionKey = KeyCode.E;

        [Header("References")]
        [Tooltip("Raycast'in çýkýþ noktasý.")]
        [SerializeField] private Transform m_CameraTransform;

     
        private IInteractable m_CurrentInteractable;

        #endregion

        #region Events

      
        public event Action<IInteractable> OnInteractableChanged;

        #endregion

        #region Unity Methods

        private void Awake()
        {
           
            if (m_CameraTransform == null)
            {
                m_CameraTransform = Camera.main?.transform;

              
                if (m_CameraTransform == null)
                {
                    Debug.LogError("[InteractionDetector] Camera Transform atanmamýþ ve Main Camera bulunamadý!");
                }
            }
        }

        private void Update()
        {
            HandleRaycast();
            HandleInput();
        }

        // Editörde ýþýný görebilmek için yardýmcý çizgi
        private void OnDrawGizmos()
        {
            if (m_CameraTransform != null)
            {
                Gizmos.color = Color.yellow;
                Gizmos.DrawRay(m_CameraTransform.position, m_CameraTransform.forward * m_InteractionRange);
            }
        }

        #endregion

        #region Private Methods

        
        // Her karede Raycast atarak etkileþimli nesne arar.
       
        private void HandleRaycast()
        {
            if (m_CameraTransform == null) return;

            Ray ray = new Ray(m_CameraTransform.position, m_CameraTransform.forward);
            RaycastHit hit;

           
            bool hitSomething = Physics.Raycast(ray, out hit, m_InteractionRange, m_InteractableLayer);

            if (hitSomething)
            {
                // Çarptýðýmýz objede IInteractable var mý?
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
                    if (Input.GetKey(m_InteractionKey))
                    {
                        m_CurrentInteractable.OnInteract();
                    }
                    break;
            }
        }

        private void ChangeInteractable(IInteractable newInteractable)
        {
            m_CurrentInteractable?.OnLoseFocus();

            m_CurrentInteractable = newInteractable;
            m_CurrentInteractable?.OnFocus();

            OnInteractableChanged?.Invoke(m_CurrentInteractable);
        }

        // Odaklanmayý temizler.
        private void ClearInteractable()
        {
            if (m_CurrentInteractable != null)
            {
                m_CurrentInteractable.OnLoseFocus();
                m_CurrentInteractable = null;

                OnInteractableChanged?.Invoke(null);
            }
        }

        #endregion
    }
}