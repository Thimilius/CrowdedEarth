using System;
using UnityEngine;

namespace CrowdedEarth {
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

        private Camera m_Camera;
        private Transform m_Transform;

        private float m_Zoom;
        private float m_ZoomTarget;

        private float m_RotationYAxis;
        private float m_RotationXAxis;
        private float m_RotationVelocityX;
        private float m_RotationVelocityY;

        private void Start() {
            m_Camera = Camera.main;
            m_Transform = m_Camera.transform;
            m_ZoomTarget = m_Transform.position.x;
            m_Zoom = m_ZoomTarget;

            Vector3 angles = m_Transform.eulerAngles;
            m_RotationYAxis = angles.y;
            m_RotationXAxis = angles.x;
        }

        private void Update() {
            UpdateRotationAndZoom();
        }

        private void UpdateRotationAndZoom() {
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

            transform.rotation = rotation;
            transform.position = position;
            m_RotationVelocityX = Mathf.Lerp(m_RotationVelocityX, 0, Time.deltaTime * m_RotationSmoothing);
            m_RotationVelocityY = Mathf.Lerp(m_RotationVelocityY, 0, Time.deltaTime * m_RotationSmoothing);
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