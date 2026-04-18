using UnityEngine;

[CreateAssetMenu(fileName = "Recipe")] // Позволяет создавать Recipe через Create → в Unity
public class Recipe : ScriptableObject // ScriptableObject — это данные, а не объект на сцене
{
    public Items[] ItemsForCraft; // Массив предметов, которые нужны для крафта
    public GameObject ResultObject; // Какой объект создастся после крафта
}

public enum Items // Перечисление всех типов предметов
{
    Wood, // Дерево
    Stone, // Камень
    Iron, // Железо
    Copper, // Медь
    Silver, // Серебро
    Gold // Золото
}