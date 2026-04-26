using UnityEngine;
using System.Collections.Generic; // Подключаем List (список)

[CreateAssetMenu(fileName = "Recipe")] // Позволяет создавать Recipe через Create → в Unity
public class Recipe : ScriptableObject // ScriptableObject — это данные, а не объект на сцене
{
    public List<Items> ItemsForCraft = new(); // Массив предметов, которые нужны для крафта
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

