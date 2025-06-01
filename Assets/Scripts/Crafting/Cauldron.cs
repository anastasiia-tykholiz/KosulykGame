using System.Collections.Generic;

public class Cauldron
{
    private List<ICauldronObserver> observers = new();
    public List<Ingredient> ingredientsUsed = new();
    public int currentResult = 0;
    public string currentExpression = "";

    public void AddObserver(ICauldronObserver observer)
    {
        observers.Add(observer);
    }

    public void AddIngredient(Ingredient ingredient)
    {
        ingredientsUsed.Add(ingredient);
        currentResult = ingredient.Apply(currentResult);
        currentExpression += ingredient.ToString();
        Notify();
    }

    public void Reset()
    {
        ingredientsUsed.Clear();
        currentResult = 0;
        currentExpression = "";
        Notify();
    }

    private void Notify()
    {
        foreach (var obs in observers)
            obs.OnCauldronUpdated(currentExpression, currentResult);
    }
}