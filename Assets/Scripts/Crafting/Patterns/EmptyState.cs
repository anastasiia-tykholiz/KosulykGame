public class EmptyState : CauldronState
{
    public override void Handle(CauldronContext context, Ingredient ingredient)
    {
        context.cauldron.AddIngredient(ingredient);
        context.SetState(new MixingState());
    }
}