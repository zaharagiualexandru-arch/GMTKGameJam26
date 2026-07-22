using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraFollow2D : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private Vector2 minimumBounds = new Vector2(-20f, -12f);
    [SerializeField] private Vector2 maximumBounds = new Vector2(20f, 12f);
    [SerializeField] private float smoothTime = 0.12f;

    private Camera cameraComponent;
    private Vector3 velocity;

    private void Awake()
    {
        cameraComponent = GetComponent<Camera>();
    }

    private void LateUpdate()
    {
        float halfHeight = cameraComponent.orthographicSize;
        float halfWidth = halfHeight * cameraComponent.aspect;

        float x = Mathf.Clamp(
            target.position.x,
            minimumBounds.x + halfWidth,
            maximumBounds.x - halfWidth
        );

        float y = Mathf.Clamp(
            target.position.y,
            minimumBounds.y + halfHeight,
            maximumBounds.y - halfHeight
        );

        Vector3 targetPosition = new Vector3(x, y, transform.position.z);

        transform.position = Vector3.SmoothDamp(
            transform.position,
            targetPosition,
            ref velocity,
            smoothTime
        );
    }
}