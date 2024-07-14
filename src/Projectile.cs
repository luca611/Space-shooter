using System.Numerics;
using Raylib_cs;
using static Raylib_cs.Raylib;

namespace Space_Shooter;

public class Projectile(Vector2 position, float speed, float damage, bool isFriendly)
{
    public void Draw()
    {
        DrawRectangle((int) position.X, (int) position.Y, 10, 10, isFriendly ? Color.Green : Color.Red);
    }
    
    public void Update()
    {
        position = isFriendly ? MovementUtils.GoUp(position, speed) : MovementUtils.GoDown(position, speed);
    }

    public bool IsOutOfBounds()
    {
        return position.Y is < 0 or > GameWindow.ScreenHeight;
    }
    
    public bool IsFriendly()
    {
        return isFriendly;
    }
    
    public Vector2 GetPosition()
    {
        return position;
    }
    
    public Vector2 GetSize()
    {
        return new Vector2(10, 10);
    }
}