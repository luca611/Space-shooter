using System.Numerics;
using Raylib_cs;
namespace Space_Shooter;
using static Raylib;

public abstract class MovementUtils()
{
    private const float ScreenWidth = GameWindow.ScreenWidth;
    private const float ScreenHeight = GameWindow.ScreenHeight;

    private const KeyboardKey Left = KeyboardKey.Left;
    private const KeyboardKey Down = KeyboardKey.Down;
    private const KeyboardKey Up = KeyboardKey.Up;
    private const KeyboardKey Right = KeyboardKey.Right;
    private const KeyboardKey W = KeyboardKey.W;
    private const KeyboardKey S = KeyboardKey.S;
    private const KeyboardKey A = KeyboardKey.A;
    private const KeyboardKey D = KeyboardKey.D;

    public static Vector2 UpdatePosition(Vector2 position, float speed)
    {
        if ((IsKeyDown(Left) || IsKeyDown(A)) && position.X - speed >= 0) position.X -= speed;
        if ((IsKeyDown(Right) || IsKeyDown(D)) && position.X + speed <= ScreenWidth) position.X += speed;
        if ((IsKeyDown(Up) || IsKeyDown(W)) && position.Y - speed >= 0) position.Y -= speed;
        if ((IsKeyDown(Down) || IsKeyDown(S)) && position.Y + speed <= ScreenHeight) position.Y += speed;

        return position;
    }
    
    public static Vector2 GoUp(Vector2 position, float speed)
    {
        position.Y -= speed;
        return position;
    }
    
    public static Vector2 GoDown(Vector2 position, float speed)
    {
        position.Y += speed;
        return position;
    }
    
}