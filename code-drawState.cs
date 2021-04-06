namespace PixelVision8.Player
{
public static class drawState
{
    //Some of this will get moved to individual game data.
    public static bool debugDisplay = true;

    public static int minGame = 1;
    public static int maxGame = 3;
    public static int gameToPick = 2;

    public static int selectedOption = 1;

    public static string[] modeTable = new string[] {"TitleScreen", "Options", "BlackJack", "Roulette"};
    //Maybe titleScreen is more like MenuScreen right now. That's OK for now.


}
}