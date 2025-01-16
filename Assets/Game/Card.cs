public enum ActionType
{
    NormalAttack,
    HeavyAttack,
    Dodge,
    Shield,
    Heal
}

[System.Serializable]
public class Card
{
    public ActionType Action { get; private set; }

    public Card(ActionType action)
    {
        Action = action;
    }
}
