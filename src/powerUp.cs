using System.Numerics;
using Raylib_cs;
using static Raylib_cs.Raylib;

namespace Space_Shooter;

/// <summary>
/// Represents a power-up that can be collected by the player.
/// </summary>
public class PowerUp
{
    private static Texture2D _powerUpTexture = LoadTexture("./assets/powerUp.png");
    private readonly int _type;
    private Vector2 _position;
    private readonly float _speed = 3.0f;
    private readonly Vector2 _size = new(20, 20);
    private readonly Player _player;
    private bool _isUsed;

    /// <summary>
    /// Initializes a new instance of the <see cref="PowerUp"/> class.
    /// </summary>
    /// <param name="posx">The initial x-coordinate of the power-up.</param>
    /// <param name="posy">The initial y-coordinate of the power-up.</param>
    /// <param name="speed">The speed at which the power-up moves.</param>
    /// <param name="sizex">The width of the power-up.</param>
    /// <param name="sizey">The height of the power-up.</param>
    /// <param name="type">The type of the power-up.</param>
    /// <param name="player">The player who can collect the power-up.</param>
    public PowerUp(float posx, float posy, float speed, float sizex, float sizey, int type, Player player)
    {
        _position.X = posx;
        _position.Y = posy;
        _speed = speed;
        _size.X = sizex;
        _size.Y = sizey;
        _type = type;
        _player = player;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="PowerUp"/> class with default size and speed.
    /// </summary>
    /// <param name="posx">The initial x-coordinate of the power-up.</param>
    /// <param name="posy">The initial y-coordinate of the power-up.</param>
    /// <param name="type">The type of the power-up.</param>
    /// <param name="player">The player who can collect the power-up.</param>
    public PowerUp(float posx, float posy, int type, Player player)
    {
        _position.X = posx;
        _position.Y = posy;
        _type = type;
        _player = player;
    }

    /// <summary>
    /// Draws the power-up on the screen.
    /// </summary>
    public void Draw()
    {
        var frameX = _type * 16;
        var sourceRect = new Rectangle(frameX, 0, 16, _powerUpTexture.Height);
        var destRect = new Rectangle(_position.X, _position.Y, _size.X * 2, _size.Y * 2);
        DrawTexturePro(_powerUpTexture, sourceRect, destRect, new Vector2(0, 0), 0f, Color.White);
    }

    /// <summary>
    /// Updates the power-up's position and checks for collision with the player.
    /// </summary>
    public void Update()
    {
        _position = MovementUtils.GoDown(_position, _speed);
        if (!CollisionCeker.CheckCollision(_position, _size, _player.GetPosition(), _player.GetSize())) return;
        ApplyPowerUp(_player);
        _isUsed = true;
    }

    /// <summary>
    /// Checks if the power-up has moved out of the screen bounds.
    /// </summary>
    /// <returns><c>true</c> if the power-up is out of bounds; otherwise, <c>false</c>.</returns>
    public bool IsOutOfBounds()
    {
        return _position.Y > GameWindow.ScreenHeight;
    }

    /// <summary>
    /// Applies the effect of the power-up to the player.
    /// </summary>
    /// <param name="player">The player to apply the power-up to.</param>
    private void ApplyPowerUp(Player player)
    {
        switch (_type)
        {
            case 0:
                player.Repair(30);
                break;
            case 1:
                player.IncreaseShootingSpeed(0.01);
                break;
            case 2:
                player.IncreaseDamage(1);
                break;
        }
    }

    /// <summary>
    /// Checks if the power-up has been used.
    /// </summary>
    /// <returns><c>true</c> if the power-up is used; otherwise, <c>false</c>.</returns>
    public bool IsUsed()
    {
        return _isUsed;
    }
}
