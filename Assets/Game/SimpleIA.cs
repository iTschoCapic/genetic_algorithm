using UnityEngine;

public class SimpleIA : MonoBehaviour
{
    public PlayerStat stat;

    public float[] results;

    
    public float fitness = 0f;

	public bool active = true;

    private void Start()
    {
        stat = GetComponent<PlayerStat>();
    }

    public void Turn()
	{
		if (active) {
            switch (Random.Range(0, 3))
            {
                case 0: stat.LightAttack(); break;
                case 1: stat.HeavyAttack(); break;
                case 2: stat.Esquive(); break;
                case 3: stat.Parade(); break;
                default: break;
            }
		}
		fitness -= 0.01f;
	}
}
