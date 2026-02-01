using UnityEngine;
using System.Collections; 
using InteractionSystem.Runtime.Core;
using InteractionSystem.Runtime.Player;

namespace InteractionSystem.Runtime.Interactables
{
    [RequireComponent(typeof(AudioSource))]
    public class DoorInteractable : BaseInteractable
    {
        #region Private Fields

        [Header("Door Settings")]
        [Tooltip("Kapý kilitli mi?")]
        [SerializeField] private bool m_IsLocked = true;

        [Tooltip("Kilidi açmak için gereken anahtar.")]
        [SerializeField] private ItemDataSO m_RequiredKey;

        [Tooltip("Dönecek olan parça.")]
        [SerializeField] private Transform m_DoorPivot;

        [Tooltip("Kaç derece dönecek?.")]
        [SerializeField] private float m_OpenAngle = 90f;

        [Tooltip("Açýlma hýzý.")]
        [SerializeField] private float m_AnimationSpeed = 2f;

        [Header("Audio Settings (YENÝ)")]
        [Tooltip("Kapý hareket sesi.")]
        [SerializeField] private AudioClip m_DoorSound;

        private AudioSource m_AudioSource;

        protected override void Awake()
        {
            base.Awake();
            m_AudioSource = GetComponent<AudioSource>();
        }

        private bool m_IsOpen = false;
        private Quaternion m_ClosedRotation;
        private Quaternion m_OpenRotation;
        private Coroutine m_AnimationCoroutine;

        #endregion

        #region Unity Methods

        private void Start()
        {
            if (m_DoorPivot != null)
            {
                m_ClosedRotation = m_DoorPivot.localRotation;
                m_OpenRotation = Quaternion.Euler(0, m_OpenAngle, 0);
            }
        }

        #endregion

        #region Public Methods

        public override void OnInteract()
        {
            if (m_IsLocked)
            {
                TryUnlock();
            }
            else
            {
                ToggleDoor();
            }
        }

        #endregion

        #region Private Methods

        private void TryUnlock()
        {
            var inventory = FindFirstObjectByType<PlayerInventory>();

            if (inventory != null && inventory.HasItem(m_RequiredKey))
            {
                m_IsLocked = false;
                Debug.Log("[Door] Kilit açýldý!");

                ToggleDoor();
            }
            else
            {
                Debug.Log("[Door] Kilitli! Anahtar gerekiyor.");
            }
        }
        public void ToggleDoor()
        {
            m_IsOpen = !m_IsOpen;

            if (m_DoorSound != null && m_AudioSource != null)
            {
                m_AudioSource.PlayOneShot(m_DoorSound);
            }

            if (m_AnimationCoroutine != null) StopCoroutine(m_AnimationCoroutine);

            m_AnimationCoroutine = StartCoroutine(AnimateDoor(m_IsOpen ? m_OpenRotation : m_ClosedRotation));

        }

        private IEnumerator AnimateDoor(Quaternion targetRotation)
        {
            while (Quaternion.Angle(m_DoorPivot.localRotation, targetRotation) > 0.1f)
            {
                m_DoorPivot.localRotation = Quaternion.Slerp(m_DoorPivot.localRotation, targetRotation, Time.deltaTime * m_AnimationSpeed);
                yield return null;
            }
            m_DoorPivot.localRotation = targetRotation;
        }

        #endregion
    }
}