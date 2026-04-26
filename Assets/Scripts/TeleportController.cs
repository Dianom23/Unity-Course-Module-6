using UnityEngine;

public class TeleportController : MonoBehaviour
{
    public Transform TargetPoint; // Точка назначения (обычно Transform другого телепорта)

    private Transform _player; // Ссылка на Transform игрока (чтобы менять его позицию)
    private bool _playerInside = false; // Флаг: находится ли игрок внутри триггера

    // Метод вызывается, когда любой объект входит в триггер
    private void OnTriggerEnter(Collider other) 
    {
        if (other.CompareTag("Player")) // Проверяем, что это именно игрок (через тег)
        {
            _player = other.transform; // Сохраняем Transform игрока
            _playerInside = true; // Отмечаем, что игрок стоит на платформе
        }
    }

    // Метод вызывается, когда объект выходит из триггера
    private void OnTriggerExit(Collider other) 
    {
        if (other.CompareTag("Player")) // Проверяем, что это игрок
        {
            _playerInside = false; // Игрок больше не стоит на платформе
            _player = null; // Удаляем ссылку (чтобы не было ошибок)
        }
    }

    // Этот метод вызывается при зажатии кнопки E
    private void OnInteract() 
    {
        if (_playerInside) // Проверяем: игрок должен стоять на платформе
        {
            Teleport(); // Если да — выполняем телепортацию
        }
    }

    // Метод, который переносит игрока
    private void Teleport() 
    {
        if (_player == null) return; // Если по какой-то причине игрока нет — выходим

        if (TargetPoint == null) return; // Если не задана точка назначения — выходим

        CharacterController controller = _player.GetComponent<CharacterController>();
        // Получаем компонент CharacterController у игрока (он отвечает за движение)

        controller.enabled = false;
        // ВАЖНО: отключаем CharacterController перед телепортом
        // иначе он может "заблокировать" изменение позиции

        _player.position = TargetPoint.position + Vector3.up * 2;
        // Перемещаем игрока:
        // TargetPoint.position — точка назначения
        // + Vector3.up * 2 — поднимаем на 2 метра вверх (чтобы не застрял в земле)

        controller.enabled = true;
        // Включаем CharacterController обратно, чтобы игрок снова мог двигаться

        _player = null;
        // Очищаем ссылку (защита от повторного использования старого объекта)
    }
}
