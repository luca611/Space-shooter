using System.Numerics;
using Raylib_cs;
using static Raylib_cs.Raylib;

namespace Space_Shooter;

public class Projectile(Vector2 position, float speed, float damage, bool isFriendly, Texture2D texture)
{
    private int _currentFrame;
    private float _frameTimer;
    private const float FrameDuration = 0.2f;
    private const int FrameWidth = 16;
    private const int ScaleFactor = 3;
    
    public void Draw()
    {
        _frameTimer += GetFrameTime(); // Increment the timer by the elapsed time

        if (_frameTimer >= FrameDuration)
        {
            _currentFrame++;
            _frameTimer = 0f; 
        }

        var totalFrames = texture.Width / FrameWidth;
        if (_currentFrame >= totalFrames) _currentFrame = 0; 
        var sourceRect = new Rectangle(_currentFrame * FrameWidth, 0, FrameWidth, texture.Height);
        var destRect = new Rectangle(position.X, position.Y, FrameWidth * ScaleFactor, texture.Height * ScaleFactor);
        DrawTexturePro(texture, sourceRect, destRect, new Vector2(0, 0), 0f, Color.White);
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
        return new Vector2(48, 48);
    }
}