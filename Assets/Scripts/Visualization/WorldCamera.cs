using System.Collections;
using UnityEngine;

namespace CrowdedEarth.Visualization {
    public class WorldCamera : MonoBehaviour {
        [Header("Rotation")]
        [SerializeField] private float m_RotationXSpeed;
        [SerializeField] private float m_RotationYSpeed;
        [SerializeField] private float m_RotationMinLimitY;
        [SerializeField] private float m_RotationMaxLimitY;
        [SerializeField] private float m_RotationSmoothing;
        [Header("Zoom")]
        [SerializeField] private float m_ZoomSpeed;
        [SerializeField] private float m_ZoomAcceleration;
        [SerializeField] private float m_ZoomMin;
        [SerializeField] private float m_ZoomMax;
        [SerializeField] private float m_ZoomStart;

        private Camera m_Camera;
        private Transform m_Transform;

        private float m_Zoom;
        private float m_ZoomTarget;

        private float m_RotationYAxis;
        private float m_RotationXAxis;
        private float m_RotationVelocityX;
        private float m_RotationVelocityY;

        private bool m_AnimatingRotation;
        private Quaternion m_AnimatedRotation;
        private Coroutine m_RotationCoroutine;

        private void Start() {
            m_Camera = Camera.main;
            m_Transform = m_Camera.transform;
            m_ZoomTarget = m_ZoomStart;
            m_Zoom = m_Transform.position.x;

            Vector3 angles = m_Transform.eulerAngles;
            m_RotationYAxis = angles.y;
            m_RotationXAxis = angles.x;
            m_AnimatedRotation = Quaternion.Euler(angles);
        }

        private void Update() {
            UpdateRotationAndZoom();
            UpdateClickToRotate();
        }

        public void RotateTo(float latitude, float longitude) {
            if (m_RotationCoroutine != null) {
                StopCoroutine(m_RotationCoroutine);
                if (m_AnimatingRotation) {
                    SetLookRotation(m_AnimatedRotation);
                }
                m_AnimatingRotation = false;
            }
            m_RotationCoroutine = StartCoroutine(AnimateRotation(Coordinates.LookAt(latitude, longitude), 1));
        }

        private void UpdateRotationAndZoom() {
            if (m_AnimatingRotation && Input.GetKeyDown(KeyCode.Mouse1)) {
                if (m_RotationCoroutine != null) {
                    m_AnimatingRotation = false;
                    StopCoroutine(m_RotationCoroutine);
                    SetLookRotation(m_AnimatedRotation);
                }
            }
            if (Input.GetKey(KeyCode.Mouse1)) {
                m_RotationVelocityX += m_RotationXSpeed * Input.GetAxis("Mouse X") * Time.deltaTime;
                m_RotationVelocityY += m_RotationYSpeed * Input.GetAxis("Mouse Y") * Time.deltaTime;
            } else {
                m_RotationVelocityX += m_RotationXSpeed * -Input.GetAxis("Horizontal") * Time.deltaTime;
                m_RotationVelocityY += m_RotationYSpeed * -Input.GetAxis("Vertical") * Time.deltaTime;
            }

            m_RotationYAxis += m_RotationVelocityX;
            m_RotationXAxis -= m_RotationVelocityY;
            m_RotationXAxis = ClampAngle(m_RotationXAxis, m_RotationMinLimitY, m_RotationMaxLimitY);
            Quaternion rotation = Quaternion.Euler(m_RotationXAxis, m_RotationYAxis, 0);

            m_ZoomTarget -= Input.GetAxis("Zoom") * Time.deltaTime * m_ZoomSpeed;
            m_ZoomTarget = Mathf.Clamp(m_ZoomTarget, m_ZoomMin, m_ZoomMax);
            m_Zoom = Mathf.Lerp(m_Zoom, m_ZoomTarget, Time.deltaTime * m_ZoomAcceleration);

            Vector3 position = rotation * new Vector3(0, 0, -m_Zoom);

            m_Transform.rotation = rotation;
            m_Transform.position = position;
            m_RotationVelocityX = Mathf.Lerp(m_RotationVelocityX, 0, Time.deltaTime * m_RotationSmoothing);
            m_RotationVelocityY = Mathf.Lerp(m_RotationVelocityY, 0, Time.deltaTime * m_RotationSmoothing);
        }

        private void UpdateClickToRotate() {
            if (Input.GetKeyDown(KeyCode.Mouse2)) {
                if (m_RotationCoroutine != null) {
                    StopCoroutine(m_RotationCoroutine);
                    if (m_AnimatingRotation) {
                        SetLookRotation(m_AnimatedRotation);
                    }
                    m_AnimatingRotation = false;
                }
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Coordinates.Intersect(ray, out RaycastHit hit)) {
                    m_RotationCoroutine = StartCoroutine(AnimateRotation(Quaternion.LookRotation(-hit.point), 1));
                }
            }
        }

        private IEnumerator AnimateRotation(Quaternion targetRotation, float time) {
            m_AnimatingRotation = true;
            Quaternion startRotation = m_Transform.rotation;
            float timer = 0.0f;
            float rate = 1.0f / time;
            while (timer < 1.0) {
                timer += Time.deltaTime * rate;
                float step = Mathf.SmoothStep(0.0f, 1.0f, timer);
                m_AnimatedRotation = Quaternion.Slerp(startRotation, targetRotation, step);
                Vector3 position = m_AnimatedRotation * new Vector3(0, 0, -m_Zoom);
                m_Transform.rotation = m_AnimatedRotation;
                m_Transform.position = position;
                yield return null;
            }

            m_AnimatingRotation = false;
            SetLookRotation(targetRotation);
        }

        private void SetLookRotation(Quaternion rotation) {
            m_Transform.rotation = rotation;
            m_Transform.position = rotation * new Vector3(0, 0, -m_Zoom);

            Vector3 eulerAngles = rotation.eulerAngles;
            if (eulerAngles.x > 180.0f) {
                eulerAngles.x -= 360.0f;
            }
            m_RotationXAxis = eulerAngles.x;
            m_RotationYAxis = eulerAngles.y;
        }

        private float ClampAngle(float angle, float min, float max) {
            if (angle < -360F) {
                angle += 360F;
            }
            if (angle > 360F) {
                angle -= 360F;
            }
            return Mathf.Clamp(angle, min, max);
        }
    }
}