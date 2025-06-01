public class CraftController
{
    public CauldronContext context;
    public Recipe recipe;

    public CraftController(Cauldron cauldron)
    {
        context = new CauldronContext(cauldron);
    }

    public void StartRecipe(Recipe newRecipe)
    {
        recipe = newRecipe;
        context.cauldron.Reset();
    }

    public void AddIngredient(Ingredient ingredient)
    {
        if (context.cauldron.ingredientsUsed.Count < recipe.maxActions)
        {
            context.AddIngredient(ingredient);
        }
    }

    public bool CheckResult()
    {
        return context.cauldron.currentResult == recipe.targetResult;
    }
}