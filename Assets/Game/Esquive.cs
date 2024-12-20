using UnityEngine;

public class Esquive : Card
{
    public Esquive() : base(0)
    {

    }

    public new void Attack(PlayerStat player)
    {
        base.Attack(player);
        Debug.Log("Esquive");
    }
}
