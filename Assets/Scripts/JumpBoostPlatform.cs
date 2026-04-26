using UnityEngine;

public class JumpBoostPlatform : MonoBehaviour
{
    public float BoostedHeight = 10; // Новая (усиленная) высота прыжка, пока игрок стоит на платформе

    public bool IsAutoJump; // Нужно ли автоматически прыгать при касании платформы

    private float _currentHeight; // Переменная для сохранения старой высоты прыжка игрока

    // Вызывается, когда объект входит в триггер
    private void OnTriggerEnter(Collider other) 
    {
        if (other.CompareTag("Player")) // Проверяем, что это именно игрок
        {
            Player player = other.GetComponent<Player>(); // Получаем компонент Player

            if (player == null) return; // Если компонента нет — выходим (защита от ошибки)

            // Сохраняем текущую высоту прыжка игрока (чтобы потом вернуть)
            _currentHeight = player.JumpHeight;

            // Устанавливаем новую (усиленную) высоту прыжка
            player.JumpHeight = BoostedHeight;

            if (IsAutoJump == true) // Если включён автоматический прыжок
            {
                // Сразу вызываем прыжок (как будто игрок нажал кнопку)
                player.OnJump();
            }
        }
    }

    // Вызывается, когда объект выходит из триггера
    private void OnTriggerExit(Collider other) 
    {
        if (other.CompareTag("Player")) // Проверяем, что это игрок
        {
            Player player = other.GetComponent<Player>(); // Получаем компонент Player

            if (player == null) return; // Если нет компонента — выходим

            // Возвращаем игроку его исходную высоту прыжка
            player.JumpHeight = _currentHeight;
        }
    }
}