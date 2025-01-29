using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class DeckBuilder : MonoBehaviour
{
    public Button normalAttackButton;
    public Button heavyAttackButton;
    public Button dodgeButton;
    public Button shieldButton;
    public Button healButton;
    public TextMeshProUGUI deckStatusText;
    public Button submitDeckButton;

    public static DeckBuilder instance;

    [SerializeField]
    private Deck playerDeck;
    public int MaxDeckSize = 20;


    [Header("Card Panel")]
    [SerializeField] private Transform deckContentPanel;
    [SerializeField] private GameObject cardUIPrefab;


    void Start()
    {
        instance = this;

        playerDeck = new Deck();

        // Clear UI when initializing
        foreach (Transform child in deckContentPanel)
        {
            Destroy(child.gameObject);
        }

        normalAttackButton.onClick.AddListener(() => AddCardToDeck(ActionType.NormalAttack));
        heavyAttackButton.onClick.AddListener(() => AddCardToDeck(ActionType.HeavyAttack));
        dodgeButton.onClick.AddListener(() => AddCardToDeck(ActionType.Dodge));
        shieldButton.onClick.AddListener(() => AddCardToDeck(ActionType.Shield));
        healButton.onClick.AddListener(() => AddCardToDeck(ActionType.Heal));

        submitDeckButton.onClick.AddListener(SubmitDeck);
        UpdateDeckStatus();
    }

    private void AddCardToDeck(ActionType actionType)
    {
        if (playerDeck.Cards.Count >= MaxDeckSize)
        {
            Debug.Log("Deck is already full!");
            return;
        }

        // Add card to deck
        Card newCard = new Card(actionType);
        playerDeck.AddCard(newCard);
        Debug.Log($"Added {actionType} to the deck.");
        UpdateDeckStatus();

        // Spawn UI element
        GameObject cardUI = Instantiate(cardUIPrefab, deckContentPanel);
        if (cardUI.TryGetComponent<CardUI>(out var cardUIScript) && ( actionType != ActionType.SecondHeal || actionType != ActionType.LoadHeavy))
        {
            cardUIScript.Initialize(newCard);
        }
        else
        {
            Debug.LogError("CardUI component missing on prefab!");
        }
    }

    private void UpdateDeckStatus()
    {
        deckStatusText.text = $"Deck Size: {playerDeck.Cards.Count}/{MaxDeckSize}";

        // Optionally disable buttons if the deck is full
        bool deckIsFull = playerDeck.Cards.Count >= MaxDeckSize;
        normalAttackButton.interactable = !deckIsFull;
        heavyAttackButton.interactable = !deckIsFull;
        dodgeButton.interactable = !deckIsFull;
        shieldButton.interactable = !deckIsFull;
        healButton.interactable = !deckIsFull;
    }

    private void SubmitDeck()
    {
        if (playerDeck.Cards.Count < MaxDeckSize)
        {
            Debug.Log("Your deck is incomplete! Please select 20 cards.");
            return;
        }

        Debug.Log("Deck submitted successfully!");
        // Pass the deck to the game manager or combat system
        GameManager.Instance.SetPlayerDeck(playerDeck.Cards);
    }
}