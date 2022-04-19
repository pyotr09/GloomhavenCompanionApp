using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using Gloom.Model.Interfaces;

namespace Gloom.Model
{
    public abstract class AbstractCardDeck<TCardType>
    {
        public AbstractCardDeck()
        {
            
        }
        public AbstractCardDeck(List<TCardType> cards)
        {
            Cards = cards;
            var shuffledCards = cards.ToArray();
            Shuffle(shuffledCards);
            DrawPile = new Stack<TCardType>(shuffledCards);
            DiscardPile = new Stack<TCardType>();
        }
        
        public List<TCardType> Cards;
        
        public void ShuffleDrawPile()
        {
            var drawPileArray = DrawPile.ToArray();
            Shuffle(drawPileArray);
            DrawPile = new Stack<TCardType>(drawPileArray);
        }

        public void ShuffleDiscardIntoDraw()
        {
            while (DiscardPile.Count > 0)
            {
                var discardedCard = DiscardPile.Pop();
                DrawPile.Push(discardedCard);
            }
            ShuffleDrawPile();
        }

        private static Random r = new();
        private static void Shuffle(TCardType[] deck)
        {
            for (int n = deck.Length - 1; n > 0; --n)
            {
                int k = r.Next(n+1);
                (deck[n], deck[k]) = (deck[k], deck[n]);
            }
        }

        public TCardType Draw()
        {
            if (DrawPile.Count == 0)
            {
                ShuffleDiscardIntoDraw();
            }
            var cardDrawn = DrawPile.Pop();
            DiscardPile.Push(cardDrawn);
            return cardDrawn;
        }

        public Stack<TCardType> DrawPile;
        public Stack<TCardType> DiscardPile;
    }
}