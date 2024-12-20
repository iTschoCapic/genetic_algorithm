using UnityEngine;

public class PlayerStat : MonoBehaviour
{
    public int health = 100;
    public PlayerStat opponent;


    LightAttack lightAttack;
    HeavyAttack heavyAttack;
    Esquive esquive;

    private void Start()
    {
        health = 100;
        lightAttack = new LightAttack(Gameloop.instance.lightAttackDamage);
        heavyAttack = new HeavyAttack(Gameloop.instance.heavyAttackDamage);
        esquive = new Esquive();
    }

    public void LightAttack()
    {
        lightAttack.Attack(opponent);
    }

    public void HeavyAttack()
    {
        heavyAttack.Attack(opponent);
    }

    public void Esquive()
    {
        esquive.Attack(opponent);
    }
}
