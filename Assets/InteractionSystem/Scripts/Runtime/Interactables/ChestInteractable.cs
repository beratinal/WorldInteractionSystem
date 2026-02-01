using UnityEngine;
using System.Collections;
using InteractionSystem.Runtime.Core;

namespace InteractionSystem.Runtime.Interactables
{
    public class ChestInteractable : BaseInteractable
    {
        #region Private Fields

        [Header("Loot Settings")]
        [Tooltip("Sandýk açýlýnca içinden çýkacak olan eþya.")]
        [SerializeField] private GameObject m_ItemToSpawn;

        [Tooltip("Eþyanýn doðacaðý nokta.")]
        [SerializeField] private Transform m_SpawnPoint;

        [Header("Animation Settings")]
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
            // Eðer zaten açýksa tekrar iþlem yapma
            if (m_IsOpen) return;

            m_IsOpen = true;
            Debug.Log("THE CHEST IS OPEN! The treasure is yours.");

            // 1. Eþyayý Oluþtur (Spawn)
            SpawnItem();

            // 2. Kapaðý Aç (Animasyon)
            StartCoroutine(OpenLidRoutine());
        }

        #endregion

        #region Private Methods

        private void SpawnItem()
        {
            if (m_ItemToSpawn != null && m_SpawnPoint != null)
            {
                // Eþyayý SpawnPoint noktasýnda yarat
                Instantiate(m_ItemToSpawn, m_SpawnPoint.position, m_SpawnPoint.rotation);
                Debug.Log($"[Chest] {m_ItemToSpawn.name} has been spawned.");
            }
            else
            {
                Debug.LogWarning("[Chest] Item prefab or spawn point is missing!");
            }
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