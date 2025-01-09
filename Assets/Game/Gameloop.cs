using UnityEngine;

public class Gameloop : MonoBehaviour
{

    public static Gameloop instance;

    private void Awake()
    {
        if(instance != null)
        {
            Debug.LogError("Il y a plusieurs instance de GameLoop dans la scene");
        }
        instance = this;
    }

    public int lightAttackDamage, heavyAttackDamage;

    public PlayerStat Player, IAStat;

    public SimpleIA ia;

    public int turn = 0;

    void Start()
    {
        turn = 0;
    }

    public void NextTurn()
    {
        ia.Turn();

        switch (Player.cards)
        {
            case Cards.Light:
                if (IAStat.cards == Cards.Parade)
                {
                    ia.fitness += 2;
                    break;
                }
                Player.lightAttack.Attack(IAStat);
                ia.fitness -= 3; break;
            case Cards.Heavy:
                if (IAStat.cards == Cards.Esquive)
                {
                    ia.fitness += 2;
                    break;
                }
                Player.heavyAttack.Attack(IAStat);
                ia.fitness -= 3;
                break;
        }

        switch (IAStat.cards)
        {
            case Cards.Light:
                if (Player.cards == Cards.Parade) break;
                IAStat.lightAttack.Attack(Player);
                ia.fitness += 5;
                break;
            case Cards.Heavy:
                if (Player.cards == Cards.Esquive) break;
                IAStat.heavyAttack.Attack(Player);
                ia.fitness += 5;
                break;
        }

        turn++;
    }
}
