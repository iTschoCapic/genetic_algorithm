using UnityEngine;

public class PlayerStat : MonoBehaviour
{
    public int health = 100;
    public PlayerStat opponent;


    public LightAttack lightAttack;
    public HeavyAttack heavyAttack;

    public Cards cards;

    private void Start()
    {
        health = 100;
        lightAttack = new LightAttack(Gameloop.instance.lightAttackDamage);
        heavyAttack = new HeavyAttack(Gameloop.instance.heavyAttackDamage);
    }

    public void LightAttack()
    {
        cards = Cards.Light;
    }

    public void HeavyAttack()
    {
        cards = Cards.Heavy;
    }

    public void Esquive()
    {
        cards = Cards.Esquive;
    }

    public void Parade()
    {
        cards = Cards.Parade;
    }
}

public enum Cards
{
    Light,
    Heavy,
    Esquive,
    Parade
}