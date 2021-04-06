using System.Collections.Generic;
using Microsoft.Xna.Framework;
using PixelVision8;
using System;
using System.Linq;

namespace PixelVision8.Player
{
	public class Card
    {
        //a single card.
        public int Value; //1-13. A, 2-10, J, Q, K
        public int Suit; //1-4, in alphabetical order. Clubs, Diamonds, Hearts, Spades.

        public Card(int value, int suit)
        {
            Value = value;
            Suit = suit;
        }
    }

    public enum Suits //This could be expanded for non-Hoyle games.
    {
        Clubs = 1,
        Diamonds,
        Hearts,
        Spades
    }

    public class Pile //Any spot in a game that holds cards
    { 
        public List<Card> cards; //Lists are FIFO, stacks are LIFO, so remember to treat a pile differently than default lists would.

        public void Add(Card c)
        {
            cards.Add(c);
        }

        public void Remove(Card c)
        {
            cards.Remove(c);
        }
    }

    public class Deck
    {
        public List<Card> cards;
        Random r = new Random();

        public Deck() //A different constructor could be used to make games with different numbers of suits or cards.
        {
            cards = createDeck();
            Shuffle();
        }

        private List<Card> createDeck()
        {
            List<Card> temp = new List<Card>();
            for(var v = 1; v <= 13; v++)
                for(var s = 1; s <= 4; s++)
                    temp.Add(new Card(v, s));

            return temp;
        }

        public void Shuffle()
        {
            cards = cards.OrderBy(c => r.Next()).ToList();
        }

        public void fillDeck(int deckCount)
        {
            cards.Clear();
            for (int i = 0; i < deckCount; i++)
                cards.AddRange(createDeck());

            Shuffle();
        }
    
    }
}