using UnityEngine;
using System.Collections;
using InteractionSystem.Runtime.Core;

namespace InteractionSystem.Runtime.Interactables
{
    [RequireComponent(typeof(AudioSource))]
    public class ChestInteractable : BaseInteractable
    {
        #region Private Fields

        [Header("Audio Settings")]
        [Tooltip("Sandýk açýlma sesi.")]
        [SerializeField] private AudioClip m_OpenSound;

        [Header("Loot Settings")]
        [SerializeField] private GameObject m_ItemToSpawn;
        [SerializeField] private Transform m_SpawnPoint;

        [Header("Animation Settings")]
        [SerializeField] private Transform m_LidPivot;
        [SerializeField] private float m_OpenAngle = -90f;
        [SerializeField] private float m_AnimationSpeed = 2f;

        private bool m_IsOpen = false;
        private AudioSource m_AudioSource;

        #endregion

        #region Unity Methods

        protected override void Awake()
        {
            base.Awake();
            m_AudioSource = GetComponent<AudioSource>();
        }

        #endregion

        #region Public Methods

        public override void OnInteract()
        {
            if (m_IsOpen) return;

            m_IsOpen = true;

            if (m_OpenSound != null && m_AudioSource != null)
            {
                m_AudioSource.PlayOneShot(m_OpenSound);
            }

            SpawnItem();
            StartCoroutine(OpenLidRoutine());
        }

        #endregion

        #region Private Methods
        private void SpawnItem()
        {
            if (m_ItemToSpawn != null && m_SpawnPoint != null)
                Instantiate(m_ItemToSpawn, m_SpawnPoint.position, m_SpawnPoint.rotation);
        }

        private IEnumerator OpenLidRoutine()
        {
            Quaternion startRotation = m_LidPivot.localRotation;
            Quaternion targetRotation = Quaternion.Euler(m_OpenAngle, 0, 0);
            float timer = 0f;
            while (timer < 1f)
            {
                timer += Time.deltaTime * m_AnimationSpeed;
                m_LidPivot.localRotation = Quaternion.Slerp(startRotation, targetRotation, timer);
                yield return null;
            }
            m_LidPivot.localRotation = targetRotation;
        }
        #endregion
    }
}