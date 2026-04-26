using UnityEngine;
using System.Collections.Generic; // Подключаем List (список)

public class CraftingStation : MonoBehaviour
{
    public Transform SpawnPoint; // Точка, где появится результат крафта

    public Recipe Recipe; // Рецепт, по которому работает эта станция

    // Список всех предметов, которые сейчас лежат внутри триггера станции
    private List<ItemObject> _itemsInTrigger = new();

    // Вызывается, когда объект входит в триггер
    private void OnTriggerEnter(Collider other) 
    {
        ItemObject itemObject = other.GetComponent<ItemObject>();
        // Пытаемся получить компонент ItemObject (это значит, что объект — предмет)

        if (itemObject == null) return;
        // Если это НЕ предмет — выходим (игнорируем)

        _itemsInTrigger.Add(itemObject);
        // Добавляем предмет в список (теперь станция его "видит")
    }

    // Когда объект выходит из триггера
    private void OnTriggerExit(Collider other) 
    {
        ItemObject itemObject = other.GetComponent<ItemObject>();
        // Пытаемся получить предмет

        if (itemObject == null) return;
        // Если это не предмет — выходим

        _itemsInTrigger.Remove(itemObject);
        // Удаляем предмет из списка (он больше не участвует в крафте)
    }

    // Этот метод вызывается при зажатии кнопки E
    private void OnInteract() 
    {
        TryCraft(); // Пытаемся выполнить крафт
    }

    // Проверяем, можно ли крафтить
    private void TryCraft() 
    {
        Camera cam = Camera.main; // Получаем камеру игрока

        // Создаём луч вперёд (куда смотрит игрок)
        Ray ray = new Ray(cam.transform.position, cam.transform.forward);

        RaycastHit hit; // Переменная для информации о попадании

        if (Physics.Raycast(ray, out hit, 3f)) // Проверяем, попали ли лучом
        {
            // Если смотрим НЕ на эту станцию
            if (hit.collider.gameObject != gameObject) return; // Выходим (нельзя крафтить)
        }
        else return; // Если вообще никуда не попали - Выходим

        // Массив: какие предметы уже использованы (false = свободен)
        bool[] used = new bool[_itemsInTrigger.Count];

        // Проверяем рецепт
        // Перебираем каждый предмет, который нужен по рецепту
        for (int i = 0; i < Recipe.ItemsForCraft.Count; i++)
        {
            // Флаг: нашли ли подходящий предмет
            bool found = false;

            // Перебираем ВСЕ предметы в триггере
            for (int j = 0; j < _itemsInTrigger.Count; j++)
            {
                // Если ячейка пустая — пропускаем
                if (_itemsInTrigger[j] == null) continue;

                // Если предмет уже использован — пропускаем
                if (used[j]) continue;

                // Проверяем: совпадает ли тип предмета с требуемым
                if (_itemsInTrigger[j].ItemType == Recipe.ItemsForCraft[i])
                {
                    // Помечаем предмет как использованный
                    used[j] = true;

                    // Отмечаем, что нашли нужный предмет
                    found = true;

                    // Выходим и ищем следующий предмет из рецепта
                    break;
                }
            }

            // Если НЕ нашли предмет под текущую часть рецепта
            if (!found)
            {
                // Прерываем — крафт невозможен
                return;
            }
        }

        // Если все предметы найдены — выполняем крафт
        Craft(used);
    }

    // Метод крафта
    private void Craft(bool[] used) 
    {
        // Идём С КОНЦА списка (важно при удалении!)
        for (int i = _itemsInTrigger.Count - 1; i >= 0; i--)
        {
            // Пропускаем пустые элементы
            if (_itemsInTrigger[i] == null) continue;

            // Пропускаем предметы, которые не участвуют в рецепте
            if (used[i] == false) continue;

            // Сохраняем ссылку на предмет
            ItemObject removedItem = _itemsInTrigger[i];

            // Удаляем из списка (ВАЖНО: именно по индексу)
            _itemsInTrigger.RemoveAt(i);

            // Удаляем объект со сцены
            Destroy(removedItem.gameObject);
        }

        // Создаём результат крафта в заданной точке
        Instantiate(Recipe.ResultObject, SpawnPoint.position, Quaternion.identity);
    }
}