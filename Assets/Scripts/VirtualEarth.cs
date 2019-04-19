using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VirtualEarth : MonoBehaviour
{
    [SerializeField] private float m_Radius;
    [SerializeField] private float m_Latitude;
    [SerializeField] private float m_Longitude;

    private Vector3 ToCartesian(float latitude, float longitude)
    {
        latitude *= Mathf.Deg2Rad;
        longitude *= Mathf.Deg2Rad;

        float x = m_Radius * Mathf.Cos(latitude) * Mathf.Cos(longitude);
        float y = m_Radius * Mathf.Cos(latitude) * Mathf.Sin(longitude);
        float z = m_Radius * Mathf.Sin(latitude);

        return new Vector3(x, z, y);
    }

    private void OnDrawGizmos()
    {
        Debug.DrawLine(Vector3.zero, ToCartesian(m_Latitude, m_Longitude), Color.red);
    }
}
