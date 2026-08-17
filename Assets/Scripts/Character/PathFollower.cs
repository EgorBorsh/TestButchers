using UnityEngine;

/// <summary>
/// Определяет путь для движения персонажа
/// </summary>
public class PathFollower : MonoBehaviour
{
    private int _nextPointIndex = 1;

    private void OnDrawGizmos()
    {
        if (transform.childCount < 2) return;

        Gizmos.color = Color.green;
        for (int i = 0; i < transform.childCount - 1; i++)
        {
            Transform current = transform.GetChild(i);
            Transform next = transform.GetChild(i + 1);

            Gizmos.DrawLine(current.position, next.position);
            Gizmos.DrawSphere(current.position, 0.3f);
            Gizmos.DrawSphere(next.position, 0.3f);
        }
    }

    public float GetTargetRotation(Vector3 currentPosition)
    {
        if (_nextPointIndex >= transform.childCount)
            _nextPointIndex = transform.childCount - 1;

        Vector3 directionToNextPoint = transform.GetChild(_nextPointIndex).position - currentPosition;

        if (directionToNextPoint.magnitude < 0.1f)
            _nextPointIndex = Mathf.Min(_nextPointIndex + 1, transform.childCount - 1);

        return Mathf.Atan2(directionToNextPoint.x, directionToNextPoint.z) * Mathf.Rad2Deg;
    }

    public void ResetPath()
    {
        _nextPointIndex = 1;
    }

    public bool IsPathComplete()
    {
        return _nextPointIndex > transform.childCount - 1;
    }
    public int GetCurrentPointIndex() => _nextPointIndex;
}