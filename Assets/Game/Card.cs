public enum ActionType
{
    NormalAttack,
    HeavyAttack,
    Dodge,
    Shield,
    Heal, 
    LoadHeavy,
    SecondHeal
}

[System.Serializable]
public struct Card
{
    public ActionType Action;

    public Card(ActionType action)
    {
        Action = action;
    }
}
