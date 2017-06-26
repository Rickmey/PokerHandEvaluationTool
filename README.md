# PokerHandEvaluationTool
PokerHandEvaluationTool in C# and WPF

Disclaimer: The goal of this project is to be fast. This is one of the rare instances where performance is more important than maintainability. Therefore you will find unrolled loops and repetitions.

## How does it work?

The basic idea is to present a poker hand as unsigned 64bit integer (ulong in C#). Every of the first 52bits stands for a card ordered by suit Two to Ace. The remaining bits are unused.

e.g. 11 decimal = 1011 binary = 5c3c2c cards.

To get the best performance the evaluation uses lookup tables to check for number of cards (bitcounts), straights and kickers.
