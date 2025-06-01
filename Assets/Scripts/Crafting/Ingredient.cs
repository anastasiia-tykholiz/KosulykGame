using UnityEngine;

public class Ingredient
{
    public int value;
    public string operation; // "+", "-", "*", "/"
    public Sprite icon;

    public Ingredient(int value, string operation, Sprite icon = null)
    {
        this.value = value;
        this.operation = operation;
        this.icon = icon;
    }

    public int Apply(int current)
    {
        return operation switch
        {
            "+" => current + value,
            "-" => current - value,
            "*" => current * value,
            "/" => value != 0 ? current / value : current,
            _ => current
        };
    }

    public override string ToString()
    {
        return $"{operation}{value}";
    }
}