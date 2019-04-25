using UnityEngine;

public static class Coordinates {
    private const float RADIUS = 20.2f;

    public static Vector3 ToCartesian(float latitude, float longitude) {
        latitude *= Mathf.Deg2Rad;
        longitude *= Mathf.Deg2Rad;

        float x = RADIUS * Mathf.Cos(latitude) * Mathf.Cos(longitude);
        float y = RADIUS * Mathf.Cos(latitude) * Mathf.Sin(longitude);
        float z = RADIUS * Mathf.Sin(latitude);

        return new Vector3(x, z, y);
    }

    public static Quaternion LookAt(float latitude, float longitude) {
        return Quaternion.LookRotation(-ToCartesian(latitude, longitude));
    }

    public static Quaternion LookFrom(float latitude, float longitude) {
        return Quaternion.LookRotation(ToCartesian(latitude, longitude));
    }

    public static bool Intersect(Ray ray, out RaycastHit hit) {
        Vector3 origin = ray.origin;
        Vector3 direction = ray.direction;

        float b = 2 * (origin.x * direction.x + origin.y * direction.y + origin.z * direction.z);
        float c = origin.x * origin.x + origin.y * origin.y + origin.z * origin.z - RADIUS * RADIUS;

        float d = b * b - 4 * c;

        hit = new RaycastHit();

        // Bail
        if (d < 0) {
            return false;
        }

        float t0 = (-b - Mathf.Sqrt(d)) / 2;
        float t1 = (-b + Mathf.Sqrt(d)) / 2;

        if (t0 < t1 && t0 > 0) {
            hit.point = ray.GetPoint(t0);
        } else if (t1 < t0 && t1 > 0) {
            hit.point = ray.GetPoint(t1);
        } else if (t0 < 0 && t1 > 0) {
            // Here we are inside the sphere
            hit.point = ray.GetPoint(t1);
        } else if (t1 < 0 && t0 > 0) {
            // Here we are inside the sphere
            hit.point = ray.GetPoint(t0);
        } else {
            return false;
        }

        return true;
    }
}
