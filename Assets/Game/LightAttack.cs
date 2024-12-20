using UnityEngine;

public class LightAttack : Card
{
    
    public LightAttack(int damages) : base(damages)
    {
        
    }

    public new void Attack(PlayerStat player)
    {
        base.Attack(player);
        Debug.Log("Light Attack : " + damages);
    }
}
