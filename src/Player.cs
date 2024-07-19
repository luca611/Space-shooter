using System.Numerics;
using Raylib_cs;
using static Raylib_cs.Raylib;

namespace Space_Shooter;

public class Player : IEntity
{
    private Vector2 _position;
    private readonly Vector2 _size;
    private float _speed;
    private int _health;
    private List<Projectile> _projectiles = []; 
    private double _shootCooldown;
    private double _lastShot;
    private double _damage;
    
    private readonly Texture2D _texture;
    private int _currentFrame;
    private const int FrameWidth = 16; 
    private const float FrameDuration = 0.1f; 
    private float _timer;
    
    private Texture2D _projectileTexture = LoadTexture("./assets/projectile.png");
    
    public Player(float x, float y, float speed, int initialHealth, float height, float width, double shootCooldown, double damage, Texture2D texture)
    {
        if (x < 0 || x > GetScreenWidth()) throw new ArgumentException("X position is out of screen bounds.", nameof(x));
        if (y < 0 || y > GetScreenHeight()) throw new ArgumentException("Y position is out of screen bounds.", nameof(y));
        if (speed <= 0) throw new ArgumentException("Speed must be positive.", nameof(speed));
        if (initialHealth is < 0 or > 100) throw new ArgumentException("Initial health must be between 0 and 100.", nameof(initialHealth));
        if (width <= 0) throw new ArgumentException("Width must be positive.", nameof(width));
        if (height <= 0) throw new ArgumentException("Height must be positive.", nameof(height));

        _position.X = x;
        _position.Y = y;
        _speed = speed;
        _health = initialHealth;
        _shootCooldown = shootCooldown;
        _damage = damage;
        _texture = texture;
        _size.X = width;
        _size.Y = height;
    }
    public void Draw()
    {
        var sourceRect = new Rectangle(0 + FrameWidth *_currentFrame, 0, FrameWidth, _texture.Height);
        var destRect = new Rectangle(_position.X, _position.Y, FrameWidth * 3, _texture.Height * 3);
        var origin = new Vector2(0, 0);
        var rotation = 0.0f;
        
        DrawTexturePro(_texture, sourceRect, destRect, origin, rotation, Color.White);
        ProjectileUtils.DrawProjectiles(_projectiles);
        UiManager.DrawLives(_health/20);
    }

    public void Update()
    {
        Shoot();
        _position = MovementUtils.UpdatePosition(_position,_size, _speed, this);
        _projectiles = ProjectileUtils.UpdateProjectiles(_projectiles);
    }

    public void Shoot()
    {
        var currentTime = GetTime(); // Assuming GetTime() returns the current time in seconds or a similar unit
        if (!IsMouseButtonDown(MouseButton.Left) || currentTime - _lastShot < _shootCooldown) return;
    
        _projectiles.Add(new Projectile(new Vector2(_position.X , _position.Y), 5, 10, true,_projectileTexture));
        _lastShot = currentTime; // Update the last shot time
    }

    public void TakeDamage(int damage)
    {
        _health -= damage;
        if (_health < 0) _health = 0;
    }

    public void Repair(int healthPoints)
    {
        _health += healthPoints;
        if (_health > 100) _health = 100; 
    }
    
    public void IncreaseSpeed(int amount)
    {
        _position.X += amount;
    }
    
    public void IncreaseShootingSpeed(double amount)
    {
        _shootCooldown -= amount;
        if (_shootCooldown < 0) _shootCooldown = 0;
    }
    
    public void IncreaseDamage(double amount)
    {
        _damage += amount;
    }

    public bool IsDestroyed()
    {
        return _health <= 0; 
    }
    
    public Vector2 GetPosition()
    {
        return _position;
    }
    
    public Vector2 GetSize()
    {
        return _size;
    }
    
    public Projectile[] GetProjectiles()
    {
        return _projectiles.ToArray();
    }

    public void RemoveProjectile(Projectile projectile)
    {
        _projectiles.Remove(projectile);
    }
    
    public void SetCurrentFrame(int frame)
    {
        _currentFrame = frame;
    }
    
    public void Reset()
    {
        _position = new Vector2(GetScreenWidth() / 2, GetScreenHeight() - 100);
        _health = 100;
        _projectiles = [];
        _speed = 5;
        _shootCooldown = 0.5;
        _damage = 10;
    }
    
    public bool IsDead()
    {
        return _health <= 0;
    }
}