using System;

namespace PixelVision8.Player
{
    public static class Options
    {

        //The screen position on the tilemap for the options screen.
        static int optionsScreenX = 0 * 8;
        static int optionsScreenY = 30 * 8;

        public static CasinoRoslynChip parentRef; //Needed to make draw and input calls work.

        public static void Update(int timeDelta)
        {
            InputOptions();
        }

        public static void InputOptions()
        {
            if(parentRef.Button(Buttons.Select, InputState.Released))
                //return to title screen
                gameState.mode = 0;
            //sound effect
            else if(parentRef.Button(Buttons.Left, InputState.Released))
            {
                if (drawState.selectedOption == 1)
                {
                    gameState.deckCount = gameState.deckCount - 1;
                    if (gameState.deckCount < 1)
                        gameState.deckCount = 8;
                }
                else if (drawState.selectedOption == 2)
                {
                    gameState.RNGMode = gameState.RNGMode - 1;
                    if (gameState.RNGMode < 0)
                        gameState.RNGMode = 2;
                }
                parentRef.SaveGame();
            }
            else if (parentRef.Button(Buttons.Right, InputState.Released))
            {
                if (drawState.selectedOption == 1)
                {
                    gameState.deckCount = gameState.deckCount + 1;
                    if (gameState.deckCount > 8)
                        gameState.deckCount = 1;
                }
                else if (drawState.selectedOption == 2)
                {
                    gameState.RNGMode = gameState.RNGMode + 1;
                    if (gameState.RNGMode > 2) 
                        gameState.RNGMode = 0;

                }
                parentRef.SaveGame();
            }
            else if(parentRef.Button(Buttons.Up, InputState.Released))
            {
                drawState.selectedOption = drawState.selectedOption - 1;
                if (drawState.selectedOption < 1)
                    drawState.selectedOption = 3;
            }
            else if(parentRef.Button(Buttons.Down, InputState.Released))
            {
                drawState.selectedOption = drawState.selectedOption + 1;
                if (drawState.selectedOption > 3) 
                   drawState.selectedOption = 1;
            }
            else if(parentRef.Button(Buttons.A, InputState.Released))
            {
                if (drawState.selectedOption == 3) 
                   gameState.CashOnHand = 500;
            }
        }

        public static void Draw()
        {
            parentRef.ScrollPosition(optionsScreenX, optionsScreenY);
            parentRef.RedrawDisplay();

            //draw our options on the screen
            if (drawState.selectedOption == 1)
            {
                parentRef.DrawText("Deck Count", 8, 30 +5, DrawMode.Tile, "large", 15); // -- selected option is white
                parentRef.DrawSprite(43, (20 * 8), 36 * 8); // --left arrow
                parentRef.DrawText(gameState.deckCount.ToString(), 22, 30 + 5, DrawMode.Tile, "large", 15);
                parentRef.DrawSprite(44, (24 * 8), 36 * 8); //Right arrow
            }
            else
                parentRef.DrawText("Deck Count", 8, 30 + 5, DrawMode.Tile, "large", 5); // unselected option is grey

            var name = "";
            if (gameState.RNGMode == 0)
                name = "Easy  ";
            else if (gameState.RNGMode == 1)
                name = "Normal";
            else if (gameState.RNGMode == 2)
                name = "Hard  ";

        if (drawState.selectedOption == 2) 
        {
            parentRef.DrawText("Difficulty", 8, 30 + 8, DrawMode.Tile, "large", 15); // -- selected option is white
            parentRef.DrawSprite(43, (20 * 8), 39 * 8) ; //left arrow
            parentRef.DrawText(name, 22, 30 + 8, DrawMode.Tile, "large", 15);
            parentRef.DrawSprite(44, (28 * 8), 39 * 8) ; //Right arrow
        }
        else
        {
            parentRef.DrawText("Difficulty", 8, 30 + 8, DrawMode.Tile, "large", 5); // -- unselected option is grey
            parentRef.DrawText(name, 22, 30 + 8, DrawMode.Tile, "large", 5);
        }

        if (drawState.selectedOption == 3) 
        {
            parentRef.DrawText("Reset Wallet (Push A)", 8, 30 + 11, DrawMode.Tile, "large", 15); // -- selected option is white
            parentRef.DrawSprite(43, (20 * 8), 39 * 11) ; //left arrow
            parentRef.DrawSprite(44, (28 * 8), 39 * 11) ; //Right arrow
        }
        else
        {
            parentRef.DrawText("Reset Wallet         ", 8, 30 + 11, DrawMode.Tile, "large", 5); // -- unselected option is grey
        }


        parentRef.DrawText("Press Select to Return", 8, 30 + 20, DrawMode.Tile, "large", 15);

        }
    }

}