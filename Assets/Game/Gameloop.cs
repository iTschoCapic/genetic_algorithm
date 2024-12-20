using UnityEngine;

public class Gameloop : MonoBehaviour
{

    public static Gameloop instance;

    private void Awake()
    {
        if(instance != null)
        {
            Debug.LogError("Il y a plusieurs instance de GameLoop dans la scene");
        }
        instance = this;
    }

    public int lightAttackDamage, heavyAttackDamage;

    public SimpleIA ia;

    public int turn = 0;

    void Start()
    {
        turn = 0;
    }

    public void NextTurn()
    {
        ia.Turn();
        turn++;
    }
}
