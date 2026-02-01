using UnityEngine;

namespace InteractionSystem.Runtime.Player
{
    [RequireComponent(typeof(CharacterController))]
    public class FirstPersonController : MonoBehaviour
    {
        #region Private Fields

        [Header("Movement Settings")]
        [Tooltip("Yürüme hýzý")]
        [SerializeField] private float m_MoveSpeed = 5f;

        [Tooltip("Yerçekimi kuvveti (Negatif olmalý)")]
        [SerializeField] private float m_Gravity = -9.81f;

        [Tooltip("Zýplama yüksekliði")]
        [SerializeField] private float m_JumpHeight = 1.5f;

        [Tooltip("Fare hassasiyeti")]
        [SerializeField] private float m_MouseSensitivity = 2f;

        [Tooltip("Kamera yukarý/aþaðý bakýþ limiti")]
        [SerializeField] private float m_LookXLimit = 85f;

        [Header("References")]
        [Tooltip("Karakterin gözü olan kamera")]
        [SerializeField] private Camera m_PlayerCamera;

        private CharacterController m_CharacterController;
        private Vector3 m_Velocity;
        private float m_RotationX = 0f;

        #endregion

        #region Unity Methods

        private void Awake()
        {
            m_CharacterController = GetComponent<CharacterController>();

            // Fareyi ekrana kilitle ve gizle
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        private void Update()
        {
            HandleMovement();
            HandleMouseLook();
        }

        #endregion

        #region Private Methods

        private void HandleMovement()
        {
            bool isGrounded = m_CharacterController.isGrounded;

            if (isGrounded && m_Velocity.y < 0)
            {
                m_Velocity.y = -2f;
            }

            // WASD
            float moveX = Input.GetAxis("Horizontal");
            float moveZ = Input.GetAxis("Vertical");

            // Hareket yönünü belirler
            Vector3 move = transform.right * moveX + transform.forward * moveZ;

            m_CharacterController.Move(move * m_MoveSpeed * Time.deltaTime);

            if (Input.GetButtonDown("Jump") && isGrounded)
            {
                m_Velocity.y = Mathf.Sqrt(m_JumpHeight * -2f * m_Gravity);
            }

            m_Velocity.y += m_Gravity * Time.deltaTime;

            m_CharacterController.Move(m_Velocity * Time.deltaTime);
        }

        private void HandleMouseLook()
        {
            if (m_PlayerCamera == null) return;

            float mouseX = Input.GetAxis("Mouse X") * m_MouseSensitivity;
            float mouseY = Input.GetAxis("Mouse Y") * m_MouseSensitivity;

            m_RotationX -= mouseY;
            m_RotationX = Mathf.Clamp(m_RotationX, -m_LookXLimit, m_LookXLimit);
            m_PlayerCamera.transform.localRotation = Quaternion.Euler(m_RotationX, 0f, 0f);

            transform.Rotate(Vector3.up * mouseX);
        }

        #endregion
    }
}