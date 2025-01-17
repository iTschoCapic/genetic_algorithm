using System.Collections.Generic;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class Deck : MonoBehaviour
{
    public List<Card> Cards { get; private set; }

    private int _numberOfTurns = 20;

    public Deck()
    {
        Cards = new List<Card>();
    }

    public Deck(List<Card> cards)
    {
        Cards = cards;
    }

    public void AddCard(Card card)
    {
        if (Cards.Count < _numberOfTurns)
        {

            if (card.Action.Equals(ActionType.HeavyAttack))
            {
                if (_numberOfTurns < Cards.Count + 2)
                {
                    return;
                }
                Cards.Add(new Card(ActionType.LoadHeavy));
                Cards.Add(card);
            }
            else if (card.Action.Equals(ActionType.Heal))
            {
                Cards.Add(card);
                Cards.Add(new Card(ActionType.SecondHeal));
                _numberOfTurns++;
            }
            else
            {
                Cards.Add(card);
            }
        }
    }
}