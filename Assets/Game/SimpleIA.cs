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
        int n = Random.Range(0, 3);
        switch (n)
        {
            case 0: stat.LightAttack(); break;
            case 1: stat.HeavyAttack(); break;
            case 2: stat.Esquive(); break;
            default: break;
        }
    }
}
