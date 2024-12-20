using UnityEngine;

public abstract class Card
{
    public int damages;

    public Card(int damages)
    {
        this.damages = damages;
    }

    public void Attack(PlayerStat player)
    {
        player.health -= damages;
    }
}
