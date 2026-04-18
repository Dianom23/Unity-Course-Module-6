using UnityEngine; // Подключаем основную библиотеку Unity
using UnityEngine.InputSystem; // Подключаем новую систему ввода

public class CraftingStation : MonoBehaviour // Класс крафт-станции
{
    public Transform SpawnPoint; // Точка появления результата

    public GameObject ResultItem; // Префаб результата

    public string[] RequiredTags; // Массив тегов для рецепта (например: "Wood", "Stone")

    private GameObject[] _itemsInTrigger = new GameObject[10]; // Массив предметов внутри триггера

    private int _currentItems = 0; // Количество предметов в триггере

    //========================================================
    // 📦 Вход в триггер
    //========================================================
    private void OnTriggerEnter(Collider other) // Когда объект входит
    {
        if (other.GetComponent<Rigidbody>() != null) // Проверяем, что это физический объект
        {
            if (_currentItems < _itemsInTrigger.Length) // Есть ли место в массиве
            {
                _itemsInTrigger[_currentItems] = other.gameObject; // Добавляем объект
                _currentItems++; // Увеличиваем счётчик
            }
        }
    }

    //========================================================
    // 📦 Выход из триггера
    //========================================================
    private void OnTriggerExit(Collider other) // Когда объект выходит
    {
        for (int i = 0; i < _itemsInTrigger.Length; i++) // Перебираем массив
        {
            if (_itemsInTrigger[i] == other.gameObject) // Если нашли объект
            {
                _itemsInTrigger[i] = null; // Удаляем его
                _currentItems--; // Уменьшаем счётчик
                break; // Выходим
            }
        }
    }

    //========================================================
    // 🎮 Нажатие E
    //========================================================
    private void OnInteract(InputValue value) // Метод Input System
    {
        if (value.isPressed) // Проверяем нажатие
        {
            TryCraft(); // Пытаемся крафтить
        }
    }

    //========================================================
    // 🔍 Проверка рецепта
    //========================================================
    private void TryCraft() // Проверяем, можно ли крафтить
    {
        Camera cam = Camera.main; // Получаем камеру

        Ray ray = new Ray(cam.transform.position, cam.transform.forward); // Луч вперёд

        RaycastHit hit; // Информация о попадании

        if (Physics.Raycast(ray, out hit, 3f)) // Проверяем попадание
        {
            if (hit.collider.gameObject != gameObject) // Если смотрим не на станцию
            {
                return; // Выходим
            }
        }
        else // Если вообще никуда не попали
        {
            return; // Выходим
        }

        // Создаём массив, чтобы отмечать использованные предметы
        bool[] used = new bool[_itemsInTrigger.Length]; // false = ещё не использован

        // Проверяем каждый тег из рецепта
        for (int i = 0; i < RequiredTags.Length; i++) // Перебираем рецепт
        {
            bool found = false; // Нашли ли предмет под этот тег

            for (int j = 0; j < _itemsInTrigger.Length; j++) // Перебираем предметы в триггере
            {
                if (_itemsInTrigger[j] != null) // Если есть предмет
                {
                    if (!used[j]) // Если он ещё не использован
                    {
                        if (_itemsInTrigger[j].CompareTag(RequiredTags[i])) // Если тег совпадает
                        {
                            used[j] = true; // Помечаем как использованный
                            found = true; // Отмечаем, что нашли
                            break; // Переходим к следующему тегу
                        }
                    }
                }
            }

            if (!found) // Если НЕ нашли предмет под нужный тег
            {
                return; // Крафт невозможен
            }
        }

        Craft(used); // Если всё найдено — крафтим
    }

    //========================================================
    // ⚙️ Крафт
    //========================================================
    private void Craft(bool[] used) // Передаём массив использованных предметов
    {
        for (int i = 0; i < _itemsInTrigger.Length; i++) // Перебираем все предметы
        {
            if (_itemsInTrigger[i] != null) // Если есть объект
            {
                if (used[i]) // Если он был использован в рецепте
                {
                    Destroy(_itemsInTrigger[i]); // Удаляем объект

                    _itemsInTrigger[i] = null; // Очищаем ячейку

                    _currentItems--; // Уменьшаем счётчик
                }
            }
        }

        Instantiate(ResultItem, SpawnPoint.position, Quaternion.identity);
        // Создаём результат
    }
}