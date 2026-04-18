using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    private Vector3 _lastPosition; // Хранит позицию платформы на прошлом кадре
    private CharacterController _playerOnPlatform; // Ссылка на игрока, стоящего на платформе

    private void Start()
    {
        
    }

    private void Update()
    {
        // Если на платформе есть игрок
        if (_playerOnPlatform != null)
        {
            // Вычисляем, насколько платформа сдвинулась с прошлого кадра
            Vector3 platformMovement = transform.position - _lastPosition;

            // Перемещаем игрока на то же расстояние
            _playerOnPlatform.Move(platformMovement);
        }

        // Сохраняем текущую позицию платформы для следующего кадра
        _lastPosition = transform.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        // Проверяем, что в триггер вошёл объект с тегом "Player"
        if (other.CompareTag("Player"))
        {
            // Получаем компонент CharacterController у игрока и сохраняем ссылку
            _playerOnPlatform = other.GetComponent<CharacterController>();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Проверяем, что из триггера вышел объект с тегом "Player"
        if (other.CompareTag("Player"))
        {
            // Убираем ссылку на игрока (он больше не на платформе)
            _playerOnPlatform = null;
        }
    }
}
