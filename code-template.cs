using System;

namespace PixelVision8.Player
{
    public static class Template
    {

        //The screen position on the tilemap for the options screen. value is tiles * pixels per tile.
        static int ScreenX = 0 * 8;
        static int ScreenY = 0 * 8;

        public static CasinoRoslynChip parentRef; //Needed to make draw and input calls work.

        public static void Update(int timeDelta)
        {
            Input();
        }

        public static void Input()
        {
            //Adjust these as needed to check for held buttons, or multiple buttons.
            if(parentRef.Button(Buttons.Select, InputState.Released))
            {

            }
            if(parentRef.Button(Buttons.Start, InputState.Released))
            {
                
            }
            else if(parentRef.Button(Buttons.Left, InputState.Released))
            {
                
            }
            else if (parentRef.Button(Buttons.Right, InputState.Released))
            {
                
            }
            else if(parentRef.Button(Buttons.Up, InputState.Released))
            {
                
            }
            else if(parentRef.Button(Buttons.Down, InputState.Released))
            {

            }
            else if(parentRef.Button(Buttons.A, InputState.Released))
            {
                
            }
            else if(parentRef.Button(Buttons.B, InputState.Released))
            {
                
            }
        }

        public static void Draw()
        {
            parentRef.ScrollPosition(ScreenX, ScreenY);
            parentRef.RedrawDisplay();

            

        }
    }

}