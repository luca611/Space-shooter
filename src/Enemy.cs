using System.Numerics;
using Raylib_cs;
using static Raylib_cs.Raylib;

namespace Space_Shooter;

/// <summary>
/// Represents an enemy entity in the game.
/// </summary>
public class Enemy : IEntity
{
    /// <summary>
    /// The level of the enemy.
    /// </summary>
    private readonly int _level;

    /// <summary>
    /// The time at which the enemy last moved.
    /// </summary>
    private double _lastMoveTime;

    /// <summary>
    /// The time elapsed since the enemy last shot.
    /// </summary>
    private double _timeSinceLastShot;

    /// <summary>
    /// The time at which the enemy last shot.
    /// </summary>
    private double _lastShot;

    /// <summary>
    /// The cooldown time between shots.
    /// </summary>
    private readonly int _shootCooldown;

    /// <summary>
    /// The health of the enemy.
    /// </summary>
    private int _health;

    /// <summary>
    /// The speed of the enemy.
    /// </summary>
    private readonly float _speed;

    /// <summary>
    /// Indicates whether the enemy can shoot.
    /// </summary>
    private bool _canShoot;

    /// <summary>
    /// The position of the enemy.
    /// </summary>
    private Vector2 _position;

    /// <summary>
    /// The size of the enemy.
    /// </summary>
    private readonly Vector2 _size;

    /// <summary>
    /// The list of projectiles fired by the enemy.
    /// </summary>
    private List<Projectile> _projectiles = new();

    /// <summary>
    /// The player that the enemy interacts with.
    /// </summary>
    private readonly Player _player;

    /// <summary>
    /// The texture used for the enemy.
    /// </summary>
    private readonly Texture2D _texture;

    /// <summary>
    /// The current frame of the enemy's animation.
    /// </summary>
    private int _currentFrame;

    /// <summary>
    /// The width of each frame in the enemy's texture.
    /// </summary>
    private const int FrameWidth = 16;

    /// <summary>
    /// The duration of each frame in the animation.
    /// </summary>
    private const float FrameDuration = 0.1f;

    /// <summary>
    /// The timer used to track the duration of the current frame.
    /// </summary>
    private float _timer;

    /// <summary>
    /// Indicates whether the enemy is spawning.
    /// </summary>
    private bool _isSpawning = true;

    /// <summary>
    /// Indicates whether the enemy has been shot.
    /// </summary>
    private bool _isShot;

    /// <summary>
    /// The texture used for the enemy's projectiles.
    /// </summary>
    private readonly Texture2D _projectileTexture = LoadTexture("./assets/enemyProjectile.png");

    /// <summary>
    /// Initializes a new instance of the <see cref="Enemy"/> class.
    /// </summary>
    /// <param name="posx">The X position of the enemy.</param>
    /// <param name="posy">The Y position of the enemy.</param>
    /// <param name="health">The health of the enemy.</param>
    /// <param name="speed">The speed of the enemy.</param>
    /// <param name="sizex">The width of the enemy.</param>
    /// <param name="sizey">The height of the enemy.</param>
    /// <param name="shootCooldown">The cooldown time between shots.</param>
    /// <param name="level">The level of the enemy.</param>
    /// <param name="player">The player that the enemy interacts with.</param>
    /// <param name="texture">The texture used for the enemy.</param>
    /// <exception cref="ArgumentException">Thrown when any of the input parameters are invalid.</exception>
    public Enemy(float posx, float posy, int health, float speed, float sizex, float sizey, int shootCooldown, int level, Player player, Texture2D texture)
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

    /// <summary>
    /// Draws the enemy on the screen.
    /// </summary>
    public void Draw()
    {
        var sourceRect = new Rectangle(0, 0, FrameWidth, _texture.Height);
        var destRect = new Rectangle(_position.X, _position.Y, FrameWidth * 3, _texture.Height * 3);
        var origin = new Vector2(0, 0);
        var rotation = 0.0f;

        if (_isSpawning)
        {
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

    /// <summary>
    /// Updates the enemy's state.
    /// </summary>
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

    /// <summary>
    /// Causes the enemy to shoot a projectile.
    /// </summary>
    public void Shoot()
    {
        if (!_canShoot) return;
        _lastShot = GetTime();
        _canShoot = false;
        Projectile newProjectile = new(_position, 1 + _level, 1 + _level, false, _projectileTexture);
        _projectiles.Add(newProjectile);
    }

    /// <summary>
    /// Causes the enemy to take damage.
    /// </summary>
    /// <param name="damage">The amount of damage taken.</param>
    public void TakeDamage(int damage)
    {
        _isShot = true;
        _health -= damage;
        if (_health < 0) _health = 0;
    }

    /// <summary>
    /// Repairs the enemy by increasing its health.
    /// </summary>
    /// <param name="healthPoints">The amount of health points restored.</param>
    public void Repair(int healthPoints)
    {
        _health += healthPoints;
        if (_health > 100) _health = 100;
    }

    /// <summary>
    /// Determines whether the enemy is destroyed.
    /// </summary>
    /// <returns><c>true</c> if the enemy is destroyed; otherwise, <c>false</c>.</returns>
    public bool IsDestroyed()
    {
        return _health <= 0;
    }

    /// <summary>
    /// Moves the enemy randomly.
    /// </summary>
    private void RandomMove()
    {
        var currentTime = GetTime();
        if (!(currentTime - _lastMoveTime >= 3)) return;

        var random = new Random();
        var operation = random.Next(0, 5);

        _position = MovementUtils.ExecuteProtectedMovement(operation, _position, _size, _speed);

        _lastMoveTime = currentTime;
    }

    /// <summary>
    /// Determines whether the enemy is hitting the player.
    /// </summary>
    /// <param name="player">The player to check for collision.</param>
    /// <returns><c>true</c> if the enemy is hitting the player; otherwise, <c>false</c>.</returns>
    private bool IsHittingPlayer(Player player)
    {
        return CollisionCeker.CheckCollision(_position, _size, player.GetPosition(), player.GetSize());
    }

    /// <summary>
    /// Determines whether the enemy is hitting a projectile.
    /// </summary>
    /// <param name="projectile">The projectile to check for collision.</param>
    /// <returns><c>true</c> if the enemy is hitting the projectile; otherwise, <c>false</c>.</returns>
    private bool IsHittingProjectile(Projectile projectile)
    {
        return CollisionCeker.CheckCollision(_position, _size, projectile.GetPosition(), projectile.GetSize());
    }

   

 /// <summary>
    /// Determines whether the enemy's projectile is hitting the player.
    /// </summary>
    /// <param name="player">The player to check for collision.</param>
    /// <param name="projectile">The projectile to check for collision.</param>
    /// <returns><c>true</c> if the projectile is hitting the player; otherwise, <c>false</c>.</returns>
    private bool IsProjectileHittingPlayer(Player player, Projectile projectile)
    {
        return CollisionCeker.CheckCollision(player.GetPosition(), player.GetSize(), projectile.GetPosition(), projectile.GetSize());
    }

    /// <summary>
    /// Handles collisions between the enemy and the player or projectiles.
    /// </summary>
    /// <param name="player">The player to check for collisions.</param>
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

    /// <summary>
    /// Spawns a power-up when the enemy is destroyed.
    /// </summary>
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

    /// <summary>
    /// Draws an animation for the enemy.
    /// </summary>
    /// <param name="texture">The texture used for the animation.</param>
    /// <returns><c>true</c> if the animation is complete; otherwise, <c>false</c>.</returns>
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
        if (_currentFrame == totalFrames - 1) return true;

        var sourceRect = new Rectangle(frameX, 0, FrameWidth, texture.Height);
        var destRect = new Rectangle(_position.X, _position.Y, FrameWidth * 3, texture.Height * 3);
        var origin = new Vector2(0, 0);
        var rotation = 0.0f;
        DrawTexturePro(texture, sourceRect, destRect, origin, rotation, Color.White);
        return false;
    }

    /// <summary>
    /// Clears the animation variables for the enemy.
    /// </summary>
    private void ClearAnimationVariables()
    {
        _timer = 0f;
        _currentFrame = 0;
    }
}