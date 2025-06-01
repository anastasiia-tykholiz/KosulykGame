public class MixingState : CauldronState
{
    public override void Handle(CauldronContext context, Ingredient ingredient)
    {
        context.cauldron.AddIngredient(ingredient);
    }
}