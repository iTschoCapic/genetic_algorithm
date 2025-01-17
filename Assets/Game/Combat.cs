[System.Serializable]
public class Combat
{
    public int PlayerHealth;
    public int IAHealth;

    public Combat()
    {
        PlayerHealth = 100;
        IAHealth = 100;
    }

    public void ResolveTurn(Card playerCard, Card iaCard)
    {
        // Simuler l'effet des cartes jouées
        switch (playerCard.Action)
        {
            case ActionType.NormalAttack:
                if (iaCard.Action != ActionType.Shield)
                    IAHealth -= 5;
                break;
            case ActionType.HeavyAttack:
                if (iaCard.Action != ActionType.Dodge)
                    IAHealth -= 12;
                break;
            case ActionType.Dodge:
                // Rien à faire ici
                break;
            case ActionType.Shield:
                // Rien à faire ici
                break;
            case ActionType.Heal:
                PlayerHealth += 5; // Exemple pour le premier tour de soin
                break;
            case ActionType.LoadHeavy:
                break;
            case ActionType.SecondHeal:
                PlayerHealth += 3; // Exemple pour le premier tour de soin
                break;
        }

        // Appliquer les effets de la carte de l'IA
        switch (iaCard.Action)
        {
            case ActionType.NormalAttack:
                if (playerCard.Action != ActionType.Shield)
                    PlayerHealth -= 5;
                break;
            case ActionType.HeavyAttack:
                if (playerCard.Action != ActionType.Dodge)
                    PlayerHealth -= 12;
                break;
            case ActionType.Dodge:
                // Rien à faire ici
                break;
            case ActionType.Shield:
                // Rien à faire ici
                break;
            case ActionType.Heal:
                IAHealth += 5; // Exemple pour le premier tour de soin
                break;
        }
    }

    public bool IsGameOver()
    {
        return PlayerHealth <= 0 || IAHealth <= 0;
    }
}
