using System;
using System.Collections.Generic;
using System.Linq;
using Gloom.Model.Interfaces;

namespace Gloom.Model
{
    public abstract class AbstractCardDeck<TCardType>
    {
        public AbstractCardDeck(List<TCardType> cards)
        {
            var shuffledCards = cards.ToArray();
            Shuffle(shuffledCards);
            _drawPile = new Stack<TCardType>(shuffledCards);
            _discardPile = new Stack<TCardType>();
        }
        
        public void ShuffleDrawPile()
        {
            var drawPileArray = _drawPile.ToArray();
            Shuffle(drawPileArray);
            _drawPile = new Stack<TCardType>(drawPileArray);
        }

        public void ShuffleDiscardIntoDraw()
        {
            while (_discardPile.Count > 0)
            {
                var discardedCard = _discardPile.Pop();
                _drawPile.Push(discardedCard);
            }
            ShuffleDrawPile();
        }

        private static Random r = new Random();
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
            var cardDrawn = _drawPile.Pop();
            _discardPile.Push(cardDrawn);
            return cardDrawn;
        }

        protected Stack<TCardType> _drawPile;
        protected Stack<TCardType> _discardPile;
    }
}