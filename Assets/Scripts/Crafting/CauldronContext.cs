public class CauldronContext
{
    public Cauldron cauldron;
    private CauldronState currentState;

    public CauldronContext(Cauldron cauldron)
    {
        this.cauldron = cauldron;
        currentState = new EmptyState();
    }

    public void SetState(CauldronState newState)
    {
        currentState = newState;
    }

    public void AddIngredient(Ingredient ingredient)
    {
        currentState.Handle(this, ingredient);
    }
}