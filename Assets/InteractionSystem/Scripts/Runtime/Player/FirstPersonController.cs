using UnityEngine;

namespace InteractionSystem.Runtime.Player
{

    [RequireComponent(typeof(CharacterController))]
    public class FirstPersonController : MonoBehaviour
    {
        #region Private Fields

        [Header("Movement Settings")]
        [Tooltip("Yürüme hýzý.")]
        [SerializeField] private float m_MoveSpeed = 5f;

        [Tooltip("Mouse hassasiyeti.")]
        [SerializeField] private float m_MouseSensitivity = 2f;

        [Header("References")]
        [Tooltip("Karakterin kamerasý.")]
        [SerializeField] private Camera m_PlayerCamera;

        private CharacterController m_CharacterController;
        private float m_VerticalRotation = 0f;

        #endregion

        #region Unity Methods

        private void Awake()
        {
            m_CharacterController = GetComponent<CharacterController>();

            if (m_PlayerCamera == null)
            {
                m_PlayerCamera = GetComponentInChildren<Camera>();
            }
        }

        private void Start()
        {
            // Mouse imlecini gizle ve kilitle
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        private void Update()
        {
            HandleRotation();
            HandleMovement();
        }

        #endregion

        #region Private Methods

        private void HandleRotation()
        {
            if (m_PlayerCamera == null) return;

            float mouseX = Input.GetAxis("Mouse X") * m_MouseSensitivity;
            transform.Rotate(0, mouseX, 0);

            float mouseY = Input.GetAxis("Mouse Y") * m_MouseSensitivity;

            m_VerticalRotation -= mouseY;
            m_VerticalRotation = Mathf.Clamp(m_VerticalRotation, -90f, 90f);

            m_PlayerCamera.transform.localRotation = Quaternion.Euler(m_VerticalRotation, 0, 0);
        }

        private void HandleMovement()
        {
            // WASD Girdisi
            float moveX = Input.GetAxis("Horizontal");
            float moveZ = Input.GetAxis("Vertical");

            Vector3 move = transform.right * moveX + transform.forward * moveZ;

            m_CharacterController.Move(move * m_MoveSpeed * Time.deltaTime);
        }

        #endregion
    }
}