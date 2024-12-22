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
                if (IAStat.cards == Cards.Esquive) break;
                Player.lightAttack.Attack(IAStat); break;
            case Cards.Heavy:
                if (IAStat.cards == Cards.Esquive) break;
                Player.heavyAttack.Attack(IAStat); break;
        }

        switch (IAStat.cards)
        {
            case Cards.Light:
                if (Player.cards == Cards.Esquive) break;
                IAStat.lightAttack.Attack(Player); break;
            case Cards.Heavy:
                if (Player.cards == Cards.Esquive) break;
                IAStat.heavyAttack.Attack(Player); break;
        }

        turn++;
    }
}
