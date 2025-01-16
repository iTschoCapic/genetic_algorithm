using System.Collections.Generic;

public class Deck
{
    public List<Card> Cards { get; private set; }

    public Deck()
    {
        Cards = new List<Card>();
    }

    public void AddCard(Card card)
    {
        if (Cards.Count < 20)
            Cards.Add(card);
    }
}
