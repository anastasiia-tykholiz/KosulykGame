public abstract class Potion
{
    public abstract void ApplyEffect(Granny granny);
}

public class DoubleJumpPotion : Potion
{
    public override void ApplyEffect(Granny granny)
    {
        granny.TaskComplete("forest");
    }
}

public class WallJumpPotion : Potion
{
    public override void ApplyEffect(Granny granny)
    {
        granny.TaskComplete("pineForest");
    }
}

public class SwampPotion : Potion
{
    public override void ApplyEffect(Granny granny)
    {
        granny.TaskComplete("swamp");
    }
}

public static class PotionFactory
{
    public static Potion CreatePotion(string location)
    {
        return location switch
        {
            "forest" => new DoubleJumpPotion(),
            "pineForest" => new WallJumpPotion(),
            "swamp" => new SwampPotion(),
            _ => null
        };
    }
}