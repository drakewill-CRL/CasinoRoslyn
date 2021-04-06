using System.Collections.Generic;
using Microsoft.Xna.Framework;
using PixelVision8;
using System;
using System.Linq;

namespace PixelVision8.Player
{
public static class gameState
{
    //Some of this might get moved to individual game files.
    public static int mode = 0;
    public static Deck currentDeck;

    public static int deckCount = 1;
    public static int RNGMode = 0; //0 is normal, 1 is nice, 2 is mean.

    //Cross-game cash, starts at $500
    public static int CashOnHand = 500;

    //Stat Tracking values
    public static int TotalRouletteWinnings = 0;

    //game mode specific values were next in Lua. Should those be part of a game class?
    public static int bjStep = 1;

}
}