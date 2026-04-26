using TMPro;
using UnityEngine;

public class NoteUI : MonoBehaviour
{
    public GameObject Panel; // Панель (вся записка на экране)
    public TMP_Text NoteTextUI; // Текст внутри записки

    private bool _isOpen = false; // Открыта ли сейчас записка

    void Start()
    {
        Panel.SetActive(false); // Выключаем панель в начале игры
    }

    public void Open(string text) // Метод открытия (принимает текст)
    {
        Panel.SetActive(true); // Включаем панель (она становится видимой)

        NoteTextUI.text = text; // Устанавливаем текст записки

        _isOpen = true; // Отмечаем, что записка открыта
        Time.timeScale = 0; // Останавливаем игру (пауза)
    }

    public void Close() // Метод закрытия
    {
        Panel.SetActive(false); // Выключаем панель

        _isOpen = false; // Отмечаем, что записка закрыта

        Time.timeScale = 1; // Возвращаем время игры
    }

    // Метод для проверки состояния открыта ли записка
    public bool IsOpen()
    {
        return _isOpen; // Возвращаем значение
    }
}


