using System.Numerics;
using Raylib_cs;
using static Raylib_cs.Raylib;

namespace Space_Shooter;

public class PowerUp
{
    private static Texture2D _powerUpTexture = LoadTexture("./assets/poweUp.png");
    private readonly int _type;
    private Vector2 _position;
    private readonly float _speed = 3.0f;
    private readonly Vector2 _size = new(20, 20);
    private readonly Player _player;
    private bool _isUsed;

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

    public PowerUp(float posx, float posy, int type, Player player)
    {
        _position.X = posx;
        _position.Y = posy;
        _type = type;
        _player = player;
    }

    public void Draw()
    {
        var frameX = _type * 16;
        var sourceRect = new Rectangle(frameX, 0, 16, _powerUpTexture.Height);
        var destRect = new Rectangle(_position.X, _position.Y, _size.X * 2, _size.Y * 2);
        DrawTexturePro(_powerUpTexture, sourceRect, destRect, new Vector2(0, 0), 0f, Color.White);
    }

    public void Update()
    {
        _position = MovementUtils.GoDown(_position, _speed);
        if (!CollisionCeker.CheckCollision(_position, _size, _player.GetPosition(), _player.GetSize())) return;
        ApplyPowerUp(_player);
        _isUsed = true;
    }

    public bool IsOutOfBounds()
    {
        return _position.Y > GameWindow.ScreenHeight;
    }

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

    public bool IsUsed()
    {
        return _isUsed;
    }
}