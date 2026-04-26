using UnityEngine;

public class NoteController : MonoBehaviour
{
    [TextArea(5, 30)] public string NoteText; // Текст записки
    public bool DestroyAfterRead; // Нужно ли удалить записку после прочтения

    private bool _playerNearby = false; // Рядом ли игрок
    private NoteUI _noteUI; // Ссылка на UI

    private void Start()
    {
        // Ищем на сцене объект с компонентом NoteUI и сохраняем ссылку
        _noteUI = FindFirstObjectByType<NoteUI>();
    }

    private void OnTriggerEnter(Collider other) // Когда объект входит в триггер
    {
        if (other.CompareTag("Player")) // Проверяем, что это игрок
        {
            _playerNearby = true; // Игрок рядом
        }
    }

    private void OnTriggerExit(Collider other) // Когда объект выходит
    {
        if (other.CompareTag("Player")) // Проверяем, что это игрок
        {
            _playerNearby = false; // Игрок больше не рядом
        }
    }

    // Этот метод вызывается при зажатии кнопки E
    private void OnInteract()
    {
        if (!_playerNearby) return; // Если игрок не рядом — выходим

        if (_noteUI == null) return; // Если UI не найден — выходим

        if (!_noteUI.IsOpen()) // Если записка сейчас закрыта
        {
            _noteUI.Open(NoteText); // Открываем и передаём текст
        }
        else // Если уже открыта
        {
            _noteUI.Close(); // Закрываем

            if (DestroyAfterRead) // Если нужно удалить
            {
                Destroy(gameObject); // Удаляем записку
            }
        }
    }
}



