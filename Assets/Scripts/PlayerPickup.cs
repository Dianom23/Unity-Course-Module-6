using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerPickup : MonoBehaviour
{
    public float PickupDistance = 3f; // Максимальная дистанция, с которой можно взять предмет
    private Camera _camera; // Ссылка на камеру игрока (откуда будет идти луч)
    private Rigidbody _heldObject; // Поле для хранения объекта, который мы держим (если ничего не держим — null)

    void Start()
    {
        _camera = Camera.main; // Получаем главную камеру в сцене
    }

    // Метод для поиска и взятия объекта c компонентом Rigidbody
    private void TryPickup() 
    {
        Ray ray = new Ray(_camera.transform.position, _camera.transform.forward); // Создаём луч из камеры вперёд
        RaycastHit hit; // Переменная, в которую запишется информация о попадании луча

        if (Physics.Raycast(ray, out hit, PickupDistance)) // Проверяем, попал ли луч в объект на нужной дистанции
        {
            Rigidbody rb = hit.collider.GetComponent<Rigidbody>(); // Пытаемся получить Rigidbody у объекта

            if (rb != null) // Если у объекта есть Rigidbody (значит его можно двигать физикой)
            {
                _heldObject = rb; // Сохраняем этот объект как "удерживаемый"
                _heldObject.useGravity = false; // Отключаем гравитацию, чтобы объект не падал вниз
                _heldObject.linearVelocity = Vector3.zero; // Останавливаем движение объекта (если он двигался)
            }
        }
    }

    // Метод отпускания объекта
    private void DropObject() 
    {
        _heldObject.useGravity = true; // Включаем обратно гравитацию
        _heldObject = null; // Убираем ссылку (теперь мы ничего не держим)
    }

    // Метод сработает, когда игрок нажмёт кнопку "взять"
    private void OnGrab() 
    {
        if (_heldObject == null) // Если сейчас мы НИЧЕГО не держим
        {
            TryPickup(); // Пытаемся взять предмет
        }
        else // Если мы УЖЕ что-то держим
        {
            DropObject(); // Тогда отпускаем предмет
        } 
    }

    private void Update() 
    {
        if (_heldObject != null) // Если мы держим объект
        {
            // Считаем точку перед камерой (на расстоянии 2 метра)
            Vector3 targetPosition = _camera.transform.position + _camera.transform.forward * 2f;

            // Вычисляем направление от объекта к этой точке (из конечной точки вычитаем начальную)
            Vector3 direction = targetPosition - _heldObject.position;

            // Двигаем объект в эту точку (чем больше число — тем быстрее)
            _heldObject.linearVelocity = direction * 10f;
        }
    }
}

