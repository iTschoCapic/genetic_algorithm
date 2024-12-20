using UnityEngine;

public class HeavyAttack : Card
{
    public HeavyAttack(int damages) : base(damages)
    {

    }

    public new void Attack(PlayerStat player)
    {
        base.Attack(player);
        Debug.Log("Heavy Attack : " + damages);
    }
}
