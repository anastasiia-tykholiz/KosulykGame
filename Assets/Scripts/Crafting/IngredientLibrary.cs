using System.Collections.Generic;
using UnityEngine;

public class IngredientLibrary
{
    public static IngredientLibrary Instance { get; } = new IngredientLibrary();

    private Dictionary<string, List<Ingredient>> levelIngredients = new();

    private IngredientLibrary()
    {
        // заглушка
        Sprite bottleSprite = Resources.Load<Sprite>("Sprites/BottlePlaceholder");

        levelIngredients["forest"] = new List<Ingredient>
        {
            new Ingredient(3, "+", bottleSprite),
            new Ingredient(5, "*", bottleSprite),
            new Ingredient(2, "+", bottleSprite),
            new Ingredient(1, "-", bottleSprite)
        };

        levelIngredients["pineForest"] = new List<Ingredient>
        {
            new Ingredient(4, "*", bottleSprite),
            new Ingredient(6, "+", bottleSprite),
            new Ingredient(1, "+", bottleSprite),
            new Ingredient(2, "-", bottleSprite)
        };

        levelIngredients["swamp"] = new List<Ingredient>
        {
            new Ingredient(3, "+", bottleSprite),
            new Ingredient(7, "-", bottleSprite),
            new Ingredient(2, "*", bottleSprite)
        };
    }

    public List<Ingredient> GetIngredientsForLevel(string location)
    {
        return levelIngredients.ContainsKey(location)
            ? levelIngredients[location]
            : new List<Ingredient>();
    }
}
