using System;
// Pixel Vision 8 - New Template Script
// Copyright (C) 2017, Pixel Vision 8 (@pixelvision8)
// Created by Jesse Freeman (@jessefreeman)
// Converted from the Lua file by Drake Williams [drakewill+pv8@gmail.com]
//
// This project was designed to display some basic instructions when you create
// a new game.  Simply delete the following code and implement your own Init(),
// Update() and Draw() logic.
// 
// Learn more about making Pixel Vision 8 games at
// https://www.pixelvision8.com/getting-started
// 

namespace PixelVision8.Player
{
	public class CasinoRoslynChip : GameChip
	{
		public override void Init()
		{

			BackgroundColor(7);
			LoadGame();

			//NOTE: if adding games, each one will need a reference back to this game to do API calls via GameChip.
			Options.parentRef = this;
			Roulette.parentRef = this;
			//Blackjack.parentRef = this;
		}

		public override void Update(int timeDelta)
		{
			//RedrawDisplay();
			switch(gameState.mode)
			{
				case 0: //Menu screen, called Title Screen in many places since there's no actual title screen.
					UpdateTitleScreen(timeDelta);
					break;
				case 1:
					Options.Update(timeDelta);
					break;
				case 2:
					//UpdateBlackJack(timeDelta);
					break;
				case 3:
					//UpdateRoulette(timeDelta);
					Roulette.Update(timeDelta);
					break;
			}
		}

		public override void Shutdown()
		{
			SaveGame();
		}

		public void SaveGame()
		{
			WriteSaveData("deckCount", gameState.deckCount.ToString());
			WriteSaveData("rngMode", gameState.RNGMode.ToString());
			WriteSaveData("CashOnHand", gameState.CashOnHand.ToString());
			WriteSaveData("TotalRouletteWinnings", gameState.TotalRouletteWinnings.ToString());

		}

		public void LoadGame()
		{
			gameState.deckCount = Int32.Parse(ReadSaveData("deckCount", "1"));
			gameState.RNGMode = Int32.Parse(ReadSaveData("rngMode", "0"));
			gameState.CashOnHand = Int32.Parse(ReadSaveData("CashOnHand", "500"));
			gameState.TotalRouletteWinnings = Int32.Parse(ReadSaveData("TotalRouletteWinnings", "0"));
		}

		public void UpdateTitleScreen(int timeDelta)
		{
			if (Button(Buttons.Start, InputState.Released))
				{
					//switch to the selected mode
    				gameState.mode = drawState.gameToPick;
    				//gameState.bjStep = 0;
    				//gameState.currentDeck = new Deck(); 
					//gameState.currentDeck.fillDeck(gameState.deckCount);
    				//sound effect
				}
  			else if (Button(Buttons.Right, InputState.Released)) //right ticks up 1, 2, 3, 1....
			  {
    			drawState.gameToPick = drawState.gameToPick + 1;
    			if (drawState.gameToPick > 3) 
      				drawState.gameToPick = drawState.minGame; //0 isn't an option
			  }
  			else if (Button(Buttons.Left, InputState.Released))  // left ticks down 3, 2, 1, 3....
			  {
    			drawState.gameToPick = drawState.gameToPick - 1 ;
    			if (drawState.gameToPick < 1 ) 
      			drawState.gameToPick = drawState.maxGame; //0 isn't an option
  			}
		}


		public override void Draw()
		{
			//RedrawDisplay();
			switch(gameState.mode)
			{
				case 0:
					DrawTitleScreen();
					break;
				case 1:
					Options.Draw();
					break;
				case 2:
					//DrawBlackJack();
					break;
				case 3:
					Roulette.Draw();
					break;
			}
		}

		public void DrawTitleScreen()
		{
			ScrollPosition(0, 0);
    		RedrawDisplay();

    		DrawText("Select A Game", 12, 12, DrawMode.Tile, "large", 15);
    		if (drawState.gameToPick == 1) 
			{
      			DrawSprite(256, 56, 110); //left arrow
      			DrawText("Options    ", 8, 14, DrawMode.Tile, "large", 15);
      			DrawSprite(257, 200, 110); //right arrow
			}
    		else if (drawState.gameToPick == 2) 
			{
      			DrawSprite(256, 56, 110); //left arrow
      			DrawText("BlackJack    ", 8, 14, DrawMode.Tile, "large", 15);
      			DrawSprite(257, 200, 110);  //right arrow
			}
    		else if (drawState.gameToPick == 3) {
      			DrawSprite(256, 56, 110); //left arrow
      			DrawText("Roulette    ", 8, 14, DrawMode.Tile, "large", 15);
      			DrawSprite(257, 200, 110);//right arrow
			}
    
    		if (drawState.debugDisplay) {
      			// NOTE: when using tileMode, x and y are tiles, not pixels.
      			DrawText("game mode " + gameState.mode.ToString(), 1, 1, DrawMode.Tile, "large", 15);
      			DrawText("game to pick: " + drawState.gameToPick.ToString(), 1, 2, DrawMode.Tile, "large", 15);
			}
    
		}		
	}
}