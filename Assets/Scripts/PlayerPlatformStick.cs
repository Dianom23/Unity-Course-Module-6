using UnityEngine;

public class PlayerPlatformStick : MonoBehaviour
{
    public CharacterController controller;

    private Transform currentPlatform;
    private Vector3 lastPlatformPosition;

    void Update()
    {
        if (currentPlatform != null)
        {
            // Смещение платформы
            Vector3 platformDelta = currentPlatform.position - lastPlatformPosition;

            // Двигаем игрока вместе с платформой
            controller.Move(platformDelta);

            // Обновляем позицию
            lastPlatformPosition = currentPlatform.position;
        }
    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        // Проверяем, что мы стоим сверху платформы
        if (hit.moveDirection.y < -0.5f)
        {
            if (hit.collider.CompareTag("MovingPlatform"))
            {
                currentPlatform = hit.collider.transform;
                lastPlatformPosition = currentPlatform.position;
            }
        }
    }

    void LateUpdate()
    {
        // Если больше не стоим на платформе — отвязываемся
        if (currentPlatform != null)
        {
            Ray ray = new Ray(transform.position, Vector3.down);
            if (!Physics.Raycast(ray, 1.5f))
            {
                currentPlatform = null;
            }
        }
    }
}