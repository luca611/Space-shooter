﻿using System.Numerics;
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
}