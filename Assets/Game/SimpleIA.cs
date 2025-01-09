using UnityEngine;

public class SimpleIA : MonoBehaviour
{
    PlayerStat stat;

    private void Start()
    {
        stat = GetComponent<PlayerStat>();
    }

    public void Turn()
    {
        if(stat.card == Cards.Charge)
        {
            stat.card = Cards.Heavy;
        }
        else
        {
            int n = Random.Range(0, 5);
            switch (n)
            {
                case 0: stat.LightAttack(); break;
                case 1: stat.HeavyAttack(); break;
                case 2: stat.Esquive(); break;
                case 3: stat.Parade(); break;
                case 4: stat.Heal(); break;
                default: break;
            }
        }
    }
}
