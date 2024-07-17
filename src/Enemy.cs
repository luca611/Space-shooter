using System.Numerics;
using Raylib_cs;
using static Raylib_cs.Raylib;

namespace Space_Shooter;

public class Enemy : IEntity
{
    private readonly int _level;
    private double _lastMoveTime;
    private double _timeSinceLastShot;
    private double _lastShot;
    private readonly int _shootCooldown;
    private int _health;
    private readonly float _speed;
    private bool _canShoot;
    private Vector2 _position;
    private readonly Vector2 _size;
    private List<Projectile> _projectiles = [];
    private readonly Player _player;
    
    private readonly Texture2D _texture;
    private int _currentFrame;
    private const int FrameWidth = 16; 
    private const float FrameDuration = 0.1f; 
    private float _timer;
    
    private bool _isSpawning = true;
    private bool _isShot;

    private readonly Texture2D _projectileTexture = LoadTexture("./assets/enemyProjectile.png"); 
    
    public Enemy(float posx, float posy, int health,float speed, float sizex, float sizey, int shootCooldown, int level, Player player,Texture2D texture)
    {
        _player = player;
        _position.X = (posx >= 0 && posx <= GetScreenWidth()) ? posx : throw new ArgumentException("posx is out of screen bounds.");
        _position.Y = (posy >= 0 && posy <= GetScreenHeight()) ? posy : throw new ArgumentException("posy is out of screen bounds.");

        _health = health > 0 ? health : throw new ArgumentException("Health must be positive.");
        _speed = speed > 0 ? speed : throw new ArgumentException("Speed must be positive.");
        _size.X = sizex > 0 ? sizex : throw new ArgumentException("Size X must be positive.");
        _size.Y = sizey > 0 ? sizey : throw new ArgumentException("Size Y must be positive.");
        _shootCooldown = shootCooldown > 0 ? shootCooldown : throw new ArgumentException("Shoot cooldown must be positive.");
        _level = level > 0 ? level : throw new ArgumentException("Level must be positive.");
        _texture = texture;
    }

    public void Draw()
    {
        var sourceRect = new Rectangle(0, 0, FrameWidth, _texture.Height);
        var destRect = new Rectangle(_position.X, _position.Y, FrameWidth * 3, _texture.Height * 3);
        var origin = new Vector2(0, 0);
        var rotation = 0.0f;
        
        if(_isSpawning){
            DrawTexturePro(_texture, sourceRect, destRect, origin, rotation, Color.White);
            _isSpawning = !DrawAnimation(EnemySystem.SpawnAnimation);
            return;
        }

        if (_isShot)
        {
            _isShot = !DrawAnimation(_texture);
            ProjectileUtils.DrawProjectiles(_projectiles);
            return;
        }
        
        DrawTexturePro(_texture, sourceRect, destRect, origin, rotation, Color.White);
        ClearAnimationVariables();
        ProjectileUtils.DrawProjectiles(_projectiles);
    }

    public void Update()
    {
        if (_isSpawning) return;
        HandleCollision(_player);
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
        Projectile newProjectile = new (_position, 1+_level, 1+_level, false, _projectileTexture);
        _projectiles.Add(newProjectile);
    }

    public void TakeDamage(int damage)
    {
        _isShot = true;
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

        var random = new Random();
        var operation = random.Next(0,5);

        _position = MovementUtils.ExecuteProtectedMovement(operation, _position, _size, _speed);

        _lastMoveTime = currentTime;
    }
    
    private bool IsHittingPlayer(Player player)
    {
        return CollisionCeker.CheckCollision(_position, _size, player.GetPosition(), player.GetSize());
    }
    
    private bool IsHittingProjectile(Projectile projectile)
    {
        return CollisionCeker.CheckCollision(_position, _size, projectile.GetPosition(), projectile.GetSize());
    }
    
    private bool IsProjectileHittingPlayer(Player player, Projectile projectile)
    {
        return CollisionCeker.CheckCollision(player.GetPosition(), player.GetSize(), projectile.GetPosition(), projectile.GetSize());
    }
    
    private void HandleCollision(Player player)
    {
        if (IsHittingPlayer(player))
        {
            player.TakeDamage(10);
            _health = 0;
        }

        foreach (var projectile in player.GetProjectiles())
        {
            if (!IsHittingProjectile(projectile)) continue;
            TakeDamage(10);
            player.RemoveProjectile(projectile);
        }

        for (var index = 0; index < _projectiles.Count; index++)
        {
            var projectile = _projectiles[index];
            if (!IsProjectileHittingPlayer(player, projectile)) continue;
            player.TakeDamage(10);
            _projectiles.Remove(projectile);
        }
    }
    
    public void SpawnPowerUpOnDestruction()
    {
        if (!IsDestroyed()) return;
        var random = new Random();
        var chance = random.NextDouble(); 
        if (chance <= 1) 
        {
            PowerUpSystem.GeneratePowerUp(_position, _player);
        }
    }
    
    private bool DrawAnimation(Texture2D texture)
    {
        var totalFrames = texture.Width / FrameWidth;
        _timer += GetFrameTime();

        if (_timer >= FrameDuration)
        {
            _currentFrame = (_currentFrame + 1) % totalFrames;
            _timer = 0f;
        }

        var frameX = _currentFrame * FrameWidth;
        if(_currentFrame == totalFrames - 1) return true;

        var sourceRect = new Rectangle(frameX, 0, FrameWidth, texture.Height);
        var destRect = new Rectangle(_position.X, _position.Y, FrameWidth * 3, texture.Height * 3);
        var origin = new Vector2(0, 0);
        var rotation = 0.0f;
        DrawTexturePro(texture, sourceRect, destRect, origin, rotation, Color.White);
        return false;
    }
    
    private void ClearAnimationVariables()
    {
        _timer = 0f;
        _currentFrame = 0;
    }
    
}