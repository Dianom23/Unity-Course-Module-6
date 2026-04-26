using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public float Speed = 5; // Скорость движения игрока
    public float JumpHeight = 1; // Высота прыжка
    public float Gravity = 9.81f; // Сила гравитации
    public Transform GroundCheckPoint; // Точка для проверки земли
    public float GroundCheckDistance = 0.1f; // Расстояние для проверки земли
    public bool IsMoving; // Идёт ли игрок

    // Компоненты для управления игроком
    private CharacterController _characterController;
    // Направление движения игрока
    private Vector2 _moveDirection;
    // Скорость игрока
    private Vector3 _velocity;
    // Флаг, указывающий, находится ли игрок на земле
    private bool _isGrounded;

    private void Start()
    {
        // Получаем компонент CharacterController на этом объекте
        _characterController = GetComponent<CharacterController>();
        // Блокируем курсор в центре экрана и скрываем его
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        Move();
        ApplyGravity();
    }

    // Метод, вызываемый системой ввода при движении
    private void OnMove(InputValue value)
    {
        _moveDirection = value.Get<Vector2>();

        if (_moveDirection == Vector2.zero)
        {
            IsMoving = false;
        }
        else
        {
            IsMoving = true;
        }
    }

    // Метод для обработки движения игрока
    private void Move()
    {
        // Преобразуем направление движения в мировые координаты
        Vector3 move = new Vector3(_moveDirection.x, 0, _moveDirection.y);
        // Преобразуем локальные координаты в глобальные с учетом поворота игрока
        move = transform.TransformDirection(move);
        // Применяем движение с учетом скорости и времени кадра,
        // чтобы движение не зависело от частоты кадров
        _characterController.Move(move * Speed * Time.deltaTime);
    }

    private void ApplyGravity()
    {
        // Проверка, находится ли игрок на земле
        _isGrounded = Physics.Raycast(GroundCheckPoint.position, Vector3.down, GroundCheckDistance);

        // Сброс вертикальной скорости при касании земли (только если падаем)
        if (_isGrounded == true && _velocity.y <= 0)
        {
            _velocity.y = -2; // небольшая компенсация, чтобы "прижать" к земле
        }

        // Применение гравитации к вертикальной скорости
        _velocity.y -= Gravity * Time.deltaTime;
        _characterController.Move(_velocity * Time.deltaTime);
    }

    // Метод, вызываемый системой ввода при прыжке
    public void OnJump()
    {
        if (_isGrounded == true) // Прыжок возможен только если игрок на земле
        {
            // Вычисление начальной вертикальной скорости для достижения заданной высоты прыжка
            _velocity.y = Mathf.Sqrt(JumpHeight * 2 * Gravity);
        }
    }
}