using System;
using System.Collections.Generic;
using System.Numerics;
using Raylib_cs;
using static Raylib_cs.Raylib;

namespace Space_Shooter;

/// <summary>
/// Represents the player character in the game.
/// </summary>
public class Player : IEntity
{
    private Vector2 _position;
    private readonly Vector2 _size;
    private float _speed;
    private int _health;
    private List<Projectile> _projectiles = new List<Projectile>();
    private double _shootCooldown;
    private double _lastShot;
    private double _damage;

    private readonly Texture2D _texture;
    private int _currentFrame;
    private const int FrameWidth = 16;
    private const float FrameDuration = 0.1f;
    private float _timer;

    private Texture2D _projectileTexture = LoadTexture("./assets/projectile.png");

    /// <summary>
    /// Initializes a new instance of the <see cref="Player"/> class.
    /// </summary>
    /// <param name="x">The initial x-coordinate of the player.</param>
    /// <param name="y">The initial y-coordinate of the player.</param>
    /// <param name="speed">The speed of the player.</param>
    /// <param name="initialHealth">The initial health of the player.</param>
    /// <param name="height">The height of the player.</param>
    /// <param name="width">The width of the player.</param>
    /// <param name="shootCooldown">The cooldown time between shots.</param>
    /// <param name="damage">The damage dealt by the player's projectiles.</param>
    /// <param name="texture">The texture of the player.</param>
    /// <exception cref="ArgumentException">Thrown when any parameter is out of valid range.</exception>
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

    /// <summary>
    /// Draws the player and its projectiles.
    /// </summary>
    public void Draw()
    {
        var sourceRect = new Rectangle(0 + FrameWidth * _currentFrame, 0, FrameWidth, _texture.Height);
        var destRect = new Rectangle(_position.X, _position.Y, FrameWidth * 3, _texture.Height * 3);
        var origin = new Vector2(0, 0);
        var rotation = 0.0f;

        DrawTexturePro(_texture, sourceRect, destRect, origin, rotation, Color.White);
        ProjectileUtils.DrawProjectiles(_projectiles);
        UiManager.DrawLives(_health / 20);
    }

    /// <summary>
    /// Updates the player's position, handles shooting, and updates projectiles.
    /// </summary>
    public void Update()
    {
        Shoot();
        _position = MovementUtils.UpdatePosition(_position, _size, _speed, this);
        _projectiles = ProjectileUtils.UpdateProjectiles(_projectiles);
    }

    /// <summary>
    /// Handles the shooting logic for the player.
    /// </summary>
    public void Shoot()
    {
        var currentTime = GetTime();
        if (!IsMouseButtonDown(MouseButton.Left) || currentTime - _lastShot < _shootCooldown) return;

        _projectiles.Add(new Projectile(new Vector2(_position.X, _position.Y), 5, 10, true, _projectileTexture));
        _lastShot = currentTime;
    }

    /// <summary>
    /// Inflicts damage to the player.
    /// </summary>
    /// <param name="damage">The amount of damage to inflict.</param>
    public void TakeDamage(int damage)
    {
        _health -= damage;
        if (_health < 0) _health = 0;
    }

    /// <summary>
    /// Repairs the player's health.
    /// </summary>
    /// <param name="healthPoints">The amount of health to restore.</param>
    public void Repair(int healthPoints)
    {
        _health += healthPoints;
        if (_health > 100) _health = 100;
    }

    /// <summary>
    /// Increases the player's speed.
    /// </summary>
    /// <param name="amount">The amount to increase the speed by.</param>
    public void IncreaseSpeed(int amount)
    {
        _position.X += amount;
    }

    /// <summary>
    /// Increases the player's shooting speed.
    /// </summary>
    /// <param name="amount">The amount to decrease the shooting cooldown by.</param>
    public void IncreaseShootingSpeed(double amount)
    {
        _shootCooldown -= amount;
        if (_shootCooldown < 0) _shootCooldown = 0;
    }

    /// <summary>
    /// Increases the damage dealt by the player's projectiles.
    /// </summary>
    /// <param name="amount">The amount to increase the damage by.</param>
    public void IncreaseDamage(double amount)
    {
        _damage += amount;
    }

    /// <summary>
    /// Checks if the player is destroyed (health <= 0).
    /// </summary>
    /// <returns><c>true</c> if the player is destroyed; otherwise, <c>false</c>.</returns>
    public bool IsDestroyed()
    {
        return _health <= 0;
    }

    /// <summary>
    /// Gets the player's position.
    /// </summary>
    /// <returns>The player's position as a <see cref="Vector2"/>.</returns>
    public Vector2 GetPosition()
    {
        return _position;
    }

    /// <summary>
    /// Gets the player's size.
    /// </summary>
    /// <returns>The player's size as a <see cref="Vector2"/>.</returns>
    public Vector2 GetSize()
    {
        return _size;
    }

    /// <summary>
    /// Gets the player's projectiles.
    /// </summary>
    /// <returns>An array of the player's projectiles.</returns>
    public Projectile[] GetProjectiles()
    {
        return _projectiles.ToArray();
    }

    /// <summary>
    /// Removes a specific projectile from the player's list of projectiles.
    /// </summary>
    /// <param name="projectile">The projectile to remove.</param>
    public void RemoveProjectile(Projectile projectile)
    {
        _projectiles.Remove(projectile);
    }

    /// <summary>
    /// Sets the current frame for the player's animation.
    /// </summary>
    /// <param name="frame">The frame to set.</param>
    public void SetCurrentFrame(int frame)
    {
        _currentFrame = frame;
    }

    /// <summary>
    /// Resets the player's state to the initial values.
    /// </summary>
    public void Reset()
    {
        _position = new Vector2(GetScreenWidth() / 2, GetScreenHeight() - 100);
        _health = 100;
        _projectiles = new List<Projectile>();
        _speed = 5;
        _shootCooldown = 0.5;
        _damage = 10;
    }

    /// <summary>
    /// Checks if the player is dead (health <= 0).
    /// </summary>
    /// <returns><c>true</c> if the player is dead; otherwise, <c>false</c>.</returns>
    public bool IsDead()
    {
        return _health <= 0;
    }
}
