using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private List<Card> playerDeck;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetPlayerDeck(List<Card> deck)
    {
        playerDeck = deck;
        Debug.Log("Player deck has been set!");
    }

    public List<Card> GetPlayerDeck()
    {
        return playerDeck;
    }
}