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

        Sprite bottleSprite1 = Resources.Load<Sprite>("Sprites/bt1");
        Sprite bottleSprite2 = Resources.Load<Sprite>("Sprites/bt2");
        Sprite bottleSprite3 = Resources.Load<Sprite>("Sprites/bt3");
        Sprite bottleSprite4 = Resources.Load<Sprite>("Sprites/bt4");

        Sprite bottleSprite5 = Resources.Load<Sprite>("Sprites/bt5");
        Sprite bottleSprite6 = Resources.Load<Sprite>("Sprites/bt6");
        Sprite bottleSprite7 = Resources.Load<Sprite>("Sprites/bt7");

        Sprite bottleSprite8 = Resources.Load<Sprite>("Sprites/bt8");
        Sprite bottleSprite9 = Resources.Load<Sprite>("Sprites/bt9");
        Sprite bottleSprite10 = Resources.Load<Sprite>("Sprites/bt10");


        levelIngredients["forest"] = new List<Ingredient>
        {
            new Ingredient(3, "+", bottleSprite1),
            new Ingredient(5, "*", bottleSprite2),
            new Ingredient(2, "+", bottleSprite3),
            new Ingredient(1, "-", bottleSprite4)
        };

        levelIngredients["pineForest"] = new List<Ingredient>
        {
            new Ingredient(4, "*", bottleSprite5),
            new Ingredient(6, "+", bottleSprite6),
            new Ingredient(1, "+", bottleSprite7),
            new Ingredient(2, "-", bottleSprite4)
        };

        levelIngredients["swamp"] = new List<Ingredient>
        {
            new Ingredient(3, "+", bottleSprite8),
            new Ingredient(7, "-", bottleSprite9),
            new Ingredient(2, "*", bottleSprite10),
            new Ingredient(3, "-", bottleSprite4)
        };
    }

    public List<Ingredient> GetIngredientsForLevel(string location)
    {
        return levelIngredients.ContainsKey(location)
            ? levelIngredients[location]
            : new List<Ingredient>();
    }
}
