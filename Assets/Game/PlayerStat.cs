using UnityEngine;

public class PlayerStat : MonoBehaviour
{
    public int maxHealth = 100;
    [SerializeField]
    int health = 100;
    public PlayerStat opponent;

    public bool heal;

    public Cards card;

    private void Start()
    {
        health = maxHealth;
    }

    public void LightAttack()
    {
        card = Cards.Light;
    }

    public void HeavyAttack()
    {
        card = Cards.Charge;
    }

    public void Esquive()
    {
        card = Cards.Esquive;
    }

    public void Parade()
    {
        card = Cards.Parade;
    }

    public void Heal()
    {
        card = Cards.Soin;
    }

    public void Damage(int damage)
    {
        health -= damage;
        if(health >= maxHealth)
        {
            health = maxHealth;
        }
    }

    public void NextTurn()
    {
        if (card == Cards.Charge)
        {
            card = Cards.Heavy;
            Gameloop.instance.NextTurn();
        }
    }
}

public enum Cards
{
    Light,
    Heavy,
    Esquive,
    Parade,
    Charge,
    Soin
}