using OpenTK.Windowing.Desktop;

public class Game : GameWindow
{
    public Game(int width, int height, string title) 
    : base(GameWindowSettings.Default, new NativeWindowSettings() { ClientSize = (width, height), Title = title }) {}
}