using System;
using System.Collections.Generic;
using System.Linq; //for Element.At

namespace PixelVision8.Player
{
    public static class Roulette
    {

        //The screen position on the tilemap for the options screen. value is tiles * pixels per tile.
        static int ScreenX = 32 * 8;
        static int ScreenY = 0 * 8;

        static Random r = new Random();

        public static CasinoRoslynChip parentRef; //Needed to make draw and input calls work.

        public static int lastSpinResults = 0; //RNG picked spot on the wheel list. load Key/Value from there.
        public static int BetType = 0;
        public static int BetSubType = 0;
        public static int BetAmount = 5; //Can't bet 0, will only do bets in increments of 5, up to 100.
        public static int CashChanged = 0;

        //Localized game state for Roulette
        public static int cursorRow = 1; //1: bidType, 2: bidSelection, 3: bidAmount

        //Update Notes
        //First version doesnt need graphics too much, just text to say whats going on . Can work out draw logic later.
        //Controls: 
        //Arrows move cursor between 3 control sets (bid type, bid pick, bid amount)
        //Start or A spins the wheel, gets the outcome, and updates cash on hand. (Also updates total income change.)
        //Select or B returns to the menu screen. Should also do any needed cleanup.

        //RNG setting logic:
        //Normal: just run the RNG as ususal.
        //Hard: On a win, 20% chance of re-rolling the result.
        //Easy: on a loss, 20% chance of re-rolling the results.


        public static void Update(int timeDelta)
        {
            Input();
        }

        public static void Input()
        {
            //I still have some kind of array range issue.
            //If you move around between rows, one input visually gets 'eaten' but shifts values, then they don't fall back in line.
            //eventually you change something and it no longer matches up with the arrays correctly and it explodes.
            
            //Adjust these as needed to check for held buttons, or multiple buttons.
            if(parentRef.Button(Buttons.Select, InputState.Released))
            {
                //Return to menu screen. Do any needed cleanup.
                gameState.mode = 0;

            }
            if(parentRef.Button(Buttons.Start, InputState.Released))
            {
                //Spin the wheel.
                RouletteResults();
            }
            else if(parentRef.Button(Buttons.Left, InputState.Released))
            {
                switch(cursorRow)
                {
                    case 1: //change BetType
                    BetType -= 1;
                        if (BetType <= -1)
                            BetType = BetTypeList.Count() - 1;
                        
                        if (BetSubType >= betSubtypes[BetType].Count())
                            BetSubType = 0;
                        break;
                    case 2:
                    BetSubType -= 1;
                        if (BetSubType <= - 1)
                            BetSubType = betSubtypes[BetType].Count() - 1;
                        break;
                    case 3:
                        BetAmount -= 5;
                        if (BetAmount <= 0)
                            BetAmount = 100;
                        break;
                }
            }
            else if (parentRef.Button(Buttons.Right, InputState.Released))
            {
                switch(cursorRow)
                {
                    case 1:
                         BetType += 1;
                        if (BetType >= BetTypeList.Count())
                            BetType = 0;
                        
                        if (BetSubType >= betSubtypes[BetType].Count())
                            BetSubType = 0;
                        break;
                    case 2:
                        BetSubType += 1;
                        if (BetSubType >= betSubtypes[BetType].Count())
                            BetSubType = 0;
                        break;
                    case 3:
                        BetAmount += 5;
                        if (BetAmount > 100)
                            BetAmount = 5;
                        break;
                }                
            }
            else if(parentRef.Button(Buttons.Up, InputState.Released))
            {
                cursorRow -= 1;
                if (cursorRow <= 0)
                    cursorRow = 3;
                
            }
            else if(parentRef.Button(Buttons.Down, InputState.Released))
            {
                cursorRow += 1;
                if (cursorRow >= 4)
                    cursorRow = 1;

            }
            else if(parentRef.Button(Buttons.A, InputState.Released))
            {
                //A acts like Start
                RouletteResults();
            }
            else if(parentRef.Button(Buttons.B, InputState.Released))
            {
                //B acts like Select
                gameState.mode = 0;   
            }
        }

        public static void Draw()
        {
            parentRef.ScrollPosition(ScreenX, ScreenY);
            parentRef.RedrawDisplay();

            //Draw appropriate elements on screen.

            //Using Sprite instead of Tile so these draw relative to the screen, not the TileMap.
            parentRef.DrawText("Bet Type", (4 * 8), (2 * 8), DrawMode.Sprite, "large", 15);
            parentRef.DrawText("Bet Select", 4 * 8, 4 * 8, DrawMode.Sprite, "large", 15);
            parentRef.DrawText("Bet Amount", 4 * 8 , 6 * 8, DrawMode.Sprite, "large", 15);

            parentRef.DrawText(BetTypeList[BetType], (16 * 8), (2 * 8), DrawMode.Sprite, "large", 15);
            parentRef.DrawText(betSubtypes[BetType][BetSubType], 16 * 8, 4 * 8, DrawMode.Sprite, "large", 15);
            parentRef.DrawText(BetAmount.ToString(), 16 * 8 , 6 * 8, DrawMode.Sprite, "large", 15);

            parentRef.DrawText("Last Results: ", 4 * 8 , 10 * 8, DrawMode.Sprite, "large", 15);
            if (wheelSpaces.ElementAt(lastSpinResults).Value == "B")
                parentRef.DrawText(wheelSpaces.ElementAt(lastSpinResults).Key, 18 * 8 , 10 * 8, DrawMode.Sprite, "large", 4); //change color to black 
            else if (wheelSpaces.ElementAt(lastSpinResults).Value == "R")
                parentRef.DrawText(wheelSpaces.ElementAt(lastSpinResults).Key, 18 * 8 , 10 * 8, DrawMode.Sprite, "large", 8); //change color to red
            else //Green
                parentRef.DrawText(wheelSpaces.ElementAt(lastSpinResults).Key, 18 * 8 , 10 * 8, DrawMode.Sprite, "large", 1); //change color to green

            parentRef.DrawText("Cash Change: $", 4 * 8 , 12 * 8, DrawMode.Sprite, "large", 15);
            parentRef.DrawText(CashChanged.ToString(), 18 * 8 , 12 * 8, DrawMode.Sprite, "large", 15);

            parentRef.DrawText("Wallet: $", 4 * 8 , 13 * 8, DrawMode.Sprite, "large", 15);
            parentRef.DrawText(gameState.CashOnHand.ToString(), 14 * 8 , 13 * 8, DrawMode.Sprite, "large", 15);

            //Draw arrows around the selected element to change
            parentRef.DrawSprite(256, 14 * 8, (2 + (cursorRow * 2) * 8) ); //left arrow
            parentRef.DrawSprite(257, 30 * 8, (2 + (cursorRow * 2) * 8)); //right arrow

            if (drawState.debugDisplay)
            {
                parentRef.DrawText("betType:" + BetType.ToString(), 4 * 8 , 27 * 8, DrawMode.Sprite, "large", 15);
                parentRef.DrawText("betSubType:" + BetSubType.ToString(), 4 * 8 , 28 * 8, DrawMode.Sprite, "large", 15);
            }

        }

        public static void RouletteResults()
        {
            /*Roulette rules:
            / The wheel has numbers 0-36, plus a 00 space (American Roulette), for a total of 37 evenly likely outcomes.
            / This code doesn't need updated if you edit the spaces on the wheel.
            */

           var winningEntry = r.Next(0, wheelSpaces.Count);
           var space = wheelSpaces.ElementAt(winningEntry);
           lastSpinResults = winningEntry;

           RouletteBetResolve();
           return; // space.Key + space.Value;
        }

        public static void RouletteBetResolve()
        {
            /*
            / Payouts:
            / any single number, 0, 00: 35x bet
            / Row (00 or 0), Split(any 2 numbers touching vert/horz on the board) : 17x bet
            / Street (any 3 numbers in a row on the board) : 11x bet
            / Corner (any 4 numbers touching in a block): 8x bet
            / Basket | Top Line (00, 0, 1, 2, 3): 6x bet
            / Sixline (any 2 horizontal rows that touch): 5x bet
            / Dozen (1-12, 13-24, 25-36), columns (numbers % 3 == 0, 1, or 2) : 2x bet.
            / Odd, Even, Red, Black, 1-18, 19-36: 1x bet.
            */
           //Look up their Bid Entry, see if it matches the Results, and update player info as needed
            
            int cashChange = -BetAmount;
            var spinInfo = wheelSpaces.ElementAt(lastSpinResults);
            int wheelNum = 0;
            Int32.TryParse(spinInfo.Key, out wheelNum);
            switch(BetTypeList[BetType]) 
            {
               // Work on a string here, so we can juggle order if needed.
                case "Single":
                    if (spinInfo.Key == betSubtypes[BetType][BetSubType])
                        cashChange = BetAmount * 35;
                    break;
                case "Parity":
                    if ((betSubtypes[BetType][BetSubType] == "Odd" && wheelNum % 2 == 1) || ((betSubtypes[BetType][BetSubType] == "Even" && wheelNum % 2 == 0)))
                        cashChange = BetAmount;
                    break;
                case "Color":
                    if ((spinInfo.Value == "R" && betSubtypes[BetType][BetSubType] == "Red") || (spinInfo.Value == "B" && betSubtypes[BetType][BetSubType] == "Black"))
                        cashChange = BetAmount;
                    break;
                case "Half":
                    var rangeStringH = betSubtypes[BetType][BetSubType];
                    var valuesH = rangeStringH.Split("-");
                    int lowRangeH = Int32.Parse(valuesH[0]);
                    int highRangeH = Int32.Parse(valuesH[1]);
                    if (wheelNum >= lowRangeH && wheelNum <= highRangeH)
                        cashChange = BetAmount;
                    break;
                case "Dozen":
                    var rangeStringD = betSubtypes[BetType][BetSubType];
                    var valuesD = rangeStringD.Split("-");
                    int lowRangeD = Int32.Parse(valuesD[0]);
                    int highRangeD = Int32.Parse(valuesD[1]);
                    if (wheelNum >= lowRangeD && wheelNum <= highRangeD)
                        cashChange = BetAmount * 2;
                    break;
                case "Basket":
                    var rangeStringB = betSubtypes[BetType][BetSubType];
                    var valuesB = rangeStringB.Split("-");
                    int[] intValsB = valuesB.Select(v => Int32.Parse(v)).ToArray();
                    if (intValsB.Any(v => v == wheelNum))
                        cashChange = BetAmount * 6;
                    break;
           }

           if (gameState.RNGMode == 1) //easy
           {
               if (cashChange <= 0 && r.Next(1, 100) < 20) //20% to reroll on a loss.
               {
                    RouletteResults();
                    return;
               }
           }
           else if (gameState.RNGMode == 2) //hard
           {
                if (cashChange >= 0 && r.Next(1, 100) < 20) //20% chance to reroll on a win.
               {
                    RouletteResults();
                    return;
               }
           }

           gameState.CashOnHand += cashChange;
           gameState.TotalRouletteWinnings += cashChange;
           CashChanged = cashChange;
        }

        //Number, then color
        public static Dictionary<string, string> wheelSpaces = new Dictionary<string, string>() {
            {"00", "G"},
            {"0", "G"},
            {"1", "R"},
            {"2", "B"},
            {"3", "R"},
            {"4", "B"},
            {"5", "R"},
            {"6", "B"},
            {"7", "R"},
            {"8", "B"},
            {"9", "R"},
            {"10", "B"},
            {"11", "B"},
            {"12", "R"},
            {"13", "B"},
            {"14", "R"},
            {"15", "B"},
            {"16", "R"},
            {"17", "B"},
            {"18", "R"},
            {"19", "R"},
            {"20", "B"},
            {"21", "R"},
            {"22", "B"},
            {"23", "R"},
            {"24", "B"},
            {"25", "R"},
            {"26", "B"},
            {"27", "R"},
            {"28", "B"},
            {"29", "B"},
            {"30", "R"},
            {"31", "B"},
            {"32", "R"},
            {"33", "B"},
            {"34", "R"},
            {"35", "B"},
            {"36", "R"}
        };

        public static string[] BetTypeList = new string[] {
            "Single",
            // "Row",
            // "Split",
            // "Street", 
            // "Corner",
            // "Sixline", 
            "Parity",
            "Color",
            "Half",
            "Dozen",
            "Basket",
        };

        //This needs to line up with the above array by index. If I did <string, string> for the type, i wouldn't need to line up perfectly.
        public static Dictionary<int, string[]> betSubtypes = new Dictionary<int, string[]>() { 
            {0, new string[] {"00", "0", "1", "2", "3","4","5","6","7","8","9","10","11","12","13","14","15","16","17","18","19","20","21","22","23","24","25","26","27","28","29","30","31","32","33","34","35","36"}},
            //{"Row"},
            //{"Split"},
            //{"Street"}, 
            //{"Corner"},
            //{"Sixline"}, 
            //{"Dozen"},
            {1, new string[] {"Odd", "Even"}}, //parity
            {2, new string[] {"Red", "Black"}}, //color
            {3, new string[] {"1-18", "19-36"}}, //half
            {4, new string[] {"1-12", "13-24", "25-36"}}, //dozen
            {5, new string[] {"00-1-2-3"}} //Basket

        };

        //1 = dark green
        //2 == darker blue
        //Color: 3 == sky blue
        
    }
}