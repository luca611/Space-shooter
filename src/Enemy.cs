using System.Numerics;
using Raylib_cs;
using static Raylib_cs.Raylib;
using System.Numerics;

namespace Space_Shooter;

public class Enemy : Entity
{
    private readonly int _level;
    private double _lastMoveTime = 0;
    private double _timeSinceLastShot= 0;
    private double _lastShot;
    private readonly int _shootCooldown;
    private int _health;
    private float _speed;
    private bool _canShoot;
    private Vector2 _position;
    private readonly Vector2 _size;
    private List<Projectile> _projectiles = [];

    public Enemy(float posx, float posy, int health,float speed, float sizex, float sizey, int shootCooldown, int level)
    {
        _position.X = (posx >= 0 && posx <= GetScreenWidth()) ? posx : throw new ArgumentException("posx is out of screen bounds.");
        _position.Y = (posy >= 0 && posy <= GetScreenHeight()) ? posy : throw new ArgumentException("posy is out of screen bounds.");

        _health = health > 0 ? health : throw new ArgumentException("Health must be positive.");
        _speed = speed > 0 ? speed : throw new ArgumentException("Speed must be positive.");
        _size.X = sizex > 0 ? sizex : throw new ArgumentException("Size X must be positive.");
        _size.Y = sizey > 0 ? sizey : throw new ArgumentException("Size Y must be positive.");
        _shootCooldown = shootCooldown > 0 ? shootCooldown : throw new ArgumentException("Shoot cooldown must be positive.");
        _level = level > 0 ? level : throw new ArgumentException("Level must be positive.");
    }

    public void Draw()
    {
        DrawRectangle((int)_position.X, (int)_position.Y, (int)_size.X, (int)_size.Y, Color.Red);
        ProjectileUtils.DrawProjectiles(_projectiles);
    }

    public void Update()
    {
        _timeSinceLastShot = GetTime() - _lastShot;
        if (_timeSinceLastShot > _shootCooldown) _canShoot = true;
        RandomMove();
        Shoot();
        _projectiles = ProjectileUtils.UpdateProjectiles(_projectiles);
    }

    public void Shoot()
    {
        if (!_canShoot) return;
        _lastShot = GetTime();
        _canShoot = false;
        Projectile newProjectile = new (new Vector2((_position.X+_size.X/2),_position.Y), 1+_level, 1+_level, false);
        _projectiles.Add(newProjectile);
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

    public bool IsDestroyed()
    {
        return _health <= 0;
    }
    
    private void RandomMove()
    {
        var currentTime = GetTime();
        if (!(currentTime - _lastMoveTime >= 3)) return;
        float screenWidth = GetScreenWidth();
        float screenHeight = GetScreenHeight();

        var random = new Random();
        var operation = random.Next(0,5);

        _position = MovementUtils.ExecuteProtectedMovement(operation, _position, _size, _speed);

        _lastMoveTime = currentTime;
    }
}