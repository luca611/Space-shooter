using System.Numerics;
using Raylib_cs;
using static Raylib_cs.Raylib;

namespace Space_Shooter;

/// <summary>
/// Represents a projectile in the game, which can be either friendly or enemy.
/// </summary>
public class Projectile
{
    private Vector2 _position;
    private readonly float _speed;
    private readonly float _damage;
    private readonly bool _isFriendly;
    private readonly Texture2D _texture;
    
    private int _currentFrame;
    private float _frameTimer;
    private const float FrameDuration = 0.2f;
    private const int FrameWidth = 16;
    private const int ScaleFactor = 3;

    /// <summary>
    /// Initializes a new instance of the <see cref="Projectile"/> class.
    /// </summary>
    /// <param name="position">The initial position of the projectile.</param>
    /// <param name="speed">The speed at which the projectile moves.</param>
    /// <param name="damage">The damage caused by the projectile.</param>
    /// <param name="isFriendly">Indicates if the projectile is friendly (true) or not (false).</param>
    /// <param name="texture">The texture used to render the projectile.</param>
    public Projectile(Vector2 position, float speed, float damage, bool isFriendly, Texture2D texture)
    {
        _position = position;
        _speed = speed;
        _damage = damage;
        _isFriendly = isFriendly;
        _texture = texture;
    }

    /// <summary>
    /// Draws the projectile on the screen.
    /// </summary>
    public void Draw()
    {
        _frameTimer += GetFrameTime(); // Increment the timer by the elapsed time

        if (_frameTimer >= FrameDuration)
        {
            _currentFrame++;
            _frameTimer = 0f; 
        }

        var totalFrames = _texture.Width / FrameWidth;
        if (_currentFrame >= totalFrames) _currentFrame = 0; 

        var sourceRect = new Rectangle(_currentFrame * FrameWidth, 0, FrameWidth, _texture.Height);
        var destRect = new Rectangle(_position.X, _position.Y, FrameWidth * ScaleFactor, _texture.Height * ScaleFactor);
        DrawTexturePro(_texture, sourceRect, destRect, new Vector2(0, 0), 0f, Color.White);
    }

    /// <summary>
    /// Updates the position of the projectile based on its speed and direction.
    /// </summary>
    public void Update()
    {
        _position = _isFriendly ? MovementUtils.GoUp(_position, _speed) : MovementUtils.GoDown(_position, _speed);
    }

    /// <summary>
    /// Checks if the projectile is out of the screen bounds.
    /// </summary>
    /// <returns><c>true</c> if the projectile is out of bounds; otherwise, <c>false</c>.</returns>
    public bool IsOutOfBounds()
    {
        return _position.Y < 0 || _position.Y > GameWindow.ScreenHeight;
    }

    /// <summary>
    /// Gets the position of the projectile.
    /// </summary>
    /// <returns>The position of the projectile.</returns>
    public Vector2 GetPosition()
    {
        return _position;
    }

    /// <summary>
    /// Gets the size of the projectile.
    /// </summary>
    /// <returns>The size of the projectile.</returns>
    public Vector2 GetSize()
    {
        return new Vector2(FrameWidth * ScaleFactor, _texture.Height * ScaleFactor);
    }

    /// <summary>
    /// Checks if the projectile is friendly.
    /// </summary>
    /// <returns><c>true</c> if the projectile is friendly; otherwise, <c>false</c>.</returns>
    public bool IsFriendly()
    {
        return _isFriendly;
    }
}
