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

    public static Vector2 UpdatePosition(Vector2 position, Vector2 size, float speed)
    {
        var isMovingHorizontally = (IsKeyDown(Left) || IsKeyDown(A)) || (IsKeyDown(Right) || IsKeyDown(D));
        var isMovingVertically = (IsKeyDown(Up) || IsKeyDown(W)) || (IsKeyDown(Down) || IsKeyDown(S));
        
        if (isMovingHorizontally && isMovingVertically)
        {
            speed /= (float)Math.Sqrt(2);
        }

        if ((IsKeyDown(Left) || IsKeyDown(A)) && position.X - speed >= 0) position.X -= speed;
        if ((IsKeyDown(Right) || IsKeyDown(D)) && position.X + size.X + speed <= ScreenWidth) position.X += speed;
        if ((IsKeyDown(Up) || IsKeyDown(W)) && position.Y - speed >= 0) position.Y -= speed;
        if ((IsKeyDown(Down) || IsKeyDown(S)) && position.Y + size.Y + speed <= ScreenHeight) position.Y += speed;

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
    
    public static Vector2 GoLeft(Vector2 position,float speed)
    {
        position.X -= speed;
        return position;
    }
    
    public static Vector2 GoRight(Vector2 position, float speed)
    {
        position.X += speed;
        return position;
    }
    
    public static Vector2 GoUpProtected(Vector2 position, float speed)
    {
        if (position.Y - speed >= 0) position.Y -= speed;
        return position;
    }
    
    public static Vector2 GoDownProtected(Vector2 position,Vector2 size, float speed)
    {
        if (position.Y+ size.Y + speed <= ScreenHeight) position.Y += speed;
        return position;
    }
    
    public static Vector2 GoLeftProtected(Vector2 position,float speed)
    {
        if (position.X - speed >= 0) position.X -= speed;
        return position;
    }
    
    public static Vector2 GoRightProtected(Vector2 position,Vector2 size, float speed)
    {
        if (position.X + size.X + speed <= ScreenWidth) position.X += speed;
        return position;
    }

    public static Vector2 ExecuteProtectedMovement(int operation, Vector2 position, Vector2 size, float speed)
    {
        return operation switch
        {
            1 => GoUpProtected(position, speed),
            2 => GoDownProtected(position, size, speed),
            3 => GoRightProtected(position, size, speed),
            4 => GoLeftProtected(position, speed),
            _ => position
        };
    }
    
}