using System.Numerics;
using Raylib_cs;
using static Raylib_cs.Raylib;

namespace Space_Shooter;

public class Player : Entity
{
    private Vector2 _position;
    private float _speed;
    private int _health;
    private List<Projectile> _projectiles = []; 
    public Player(float x, float y, float speed, int initialHealth)
    {
        _position.X = x;
        _position.Y = y;
        _speed = speed;
        _health = initialHealth;
    }
    public void Draw()
    {
        DrawCircle((int)_position.X, (int)_position.Y, 20, Color.Red);
        ProjectileUtils.DrawProjectiles(_projectiles);
    }

    public void Update()
    {
        Shoot();
        _position = MovementUtils.UpdatePosition(_position, _speed);
        _projectiles = ProjectileUtils.UpdateProjectiles(_projectiles);
    }

    public void Shoot()
    {
        if (!IsMouseButtonPressed(MouseButton.Right)) return;
        
        _projectiles.Add(new Projectile(_position, 5, 10, true));
        Console.WriteLine("Pew pew");
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
    
    
}