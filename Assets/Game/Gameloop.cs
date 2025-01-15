using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class Gameloop : MonoBehaviour
{

    public etapes etape;
    public int turnToPlay = 20;
    int turnPlay = 0;

    public static Gameloop instance;

    public List<string> cardsRecord = new List<string>();

    public int attaqueSimpleDegat = 5;
    public int attaqueLourdeDegat = 12;
    public int healTurn1 = 5;
    public int healTurn2 = 3;

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

        PlayTurn(Player);

        Player.NextTurn();
    }

    private void PlayTurn(PlayerStat stat)
    {
        Debug.Log(stat.name + " " + stat.card);

        if (stat.heal)
        {
            sendDamage(stat, -1 * healTurn2);
            stat.heal = false;
        }

        switch (etape)
        {
            case etapes.Preparation: Preparation(stat); break;
            default: break;
        }
    }

    private void Preparation(PlayerStat stat)
    {
        cardsRecord.Add(stat.card.ToString());
        if (stat.card != Cards.Charge) turnPlay++;

        if (turnPlay >= turnToPlay) etape = etapes.Jeu;
    }

    private void Jeu(PlayerStat stat)
    {
        switch (stat.card)
        {
            case Cards.Light:
                if (stat.opponent.card == Cards.Parade) break;
                sendDamage(stat.opponent, attaqueSimpleDegat); break;
            case Cards.Heavy:
                if (stat.opponent.card == Cards.Esquive) break;
                sendDamage(stat.opponent, attaqueLourdeDegat); break;
            case Cards.Soin:
                sendDamage(stat, -1 * healTurn1);
                stat.heal = true;
                break;
        }
    }

    private void sendDamage(PlayerStat stat, int damage)
    {
        stat.Damage(damage);
    }
}

public enum etapes
{
    Preparation,
    Jeu
}