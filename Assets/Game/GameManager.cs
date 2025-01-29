using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public IA_Genetique ia;

    private List<Card> playerDeck;

    public GameObject deckPanel;

    void Awake()
    {
        Screen.SetResolution(1920, 1080, true);

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
        ia.StartCoroutine(ia.RunGeneticAlgorithm());
    }

    public List<Card> GetPlayerDeck()
    {
        return playerDeck;
    }

    public Deck GetPlayerDeckAsDeck()
    {
        if (playerDeck == null || playerDeck.Count == 0)
        {
            Debug.Log("Player deck is empty or not set!");
        }
        return new Deck(playerDeck);
    }

}