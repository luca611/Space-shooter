using Raylib_cs;
using static Raylib_cs.Raylib;

namespace Space_Shooter;

/// <summary>
/// Manages the spawning, updating, and drawing of enemies in the game.
/// </summary>
public class EnemySystem
{
    private static readonly Texture2D Enemy1 = LoadTexture("./assets/enemy.png");
    private static readonly Texture2D Enemy2 = LoadTexture("./assets/enemy2.png");
    private static readonly Texture2D Enemy3 = LoadTexture("./assets/enemy3.png");

    /// <summary>
    /// Array of enemy textures.
    /// </summary>
    public static readonly Texture2D[] EnemyTextures = { Enemy1, Enemy2, Enemy3 };

    /// <summary>
    /// Texture for the spawn animation.
    /// </summary>
    public static readonly Texture2D SpawnAnimation = LoadTexture("./assets/spawn.png");

    private int _spawnTimer;
    private readonly int _maxEnemies;
    private int _difficulty;
    private double _lastDifficultyIncreaseTime;
    private double _lastSpawnTime;
    private Player _player;
    private int _killCount;

    private List<Enemy> _enemies = new List<Enemy>();

    /// <summary>
    /// Initializes a new instance of the <see cref="EnemySystem"/> class.
    /// </summary>
    /// <param name="spawnTimer">The time interval for spawning enemies.</param>
    /// <param name="maxEnemies">The maximum number of enemies allowed on screen.</param>
    /// <param name="difficulty">The initial difficulty level.</param>
    /// <param name="player">The player instance.</param>
    /// <exception cref="ArgumentException">Thrown when spawnTimer or maxEnemies is less than or equal to 0, or difficulty is less than 1.</exception>
    public EnemySystem(int spawnTimer, int maxEnemies, int difficulty, Player player)
    {
        if (spawnTimer <= 0) throw new ArgumentException("Spawn timer must be greater than 0.", nameof(spawnTimer));
        if (maxEnemies <= 0) throw new ArgumentException("Max enemies must be greater than 0.", nameof(maxEnemies));
        if (difficulty < 1) throw new ArgumentException("Difficulty must be at least 1.", nameof(difficulty));
        _spawnTimer = spawnTimer;
        _maxEnemies = maxEnemies;
        _difficulty = difficulty;
        _player = player;
    }

    /// <summary>
    /// Updates the enemy system, including increasing difficulty, spawning enemies, and updating existing enemies.
    /// </summary>
    public void Update()
    {
        var currentTime = GetTime();

        if (currentTime - _lastDifficultyIncreaseTime >= 60)
        {
            _difficulty++;
            _lastDifficultyIncreaseTime = currentTime;
        }

        var adjustedMaxEnemies = _maxEnemies * _difficulty;

        if (_enemies.Count < _maxEnemies && currentTime - _lastSpawnTime >= _spawnTimer)
        {
            try
            {
                var newEnemy = CreateEnemyBasedOnDifficulty(_difficulty);
                _enemies.Add(newEnemy);
                _lastSpawnTime = currentTime;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating enemy: {ex.Message}");
            }
        }

        foreach (var enemy in _enemies) enemy.Update();
        for (var i = 0; i < _enemies.Count; i++)
        {
            if (!_enemies[i].IsDestroyed()) continue;
            _enemies[i].SpawnPowerUpOnDestruction();
            _enemies.RemoveAt(i);
            _killCount++;
        }
    }

    /// <summary>
    /// Draws all the enemies and the kill count.
    /// </summary>
    public void Draw()
    {
        foreach (var enemy in _enemies) enemy.Draw();
        UiManager.DrawKills(_killCount);
    }

    /// <summary>
    /// Creates a new enemy based on the current difficulty level.
    /// </summary>
    /// <param name="difficulty">The current difficulty level.</param>
    /// <returns>A new enemy instance.</returns>
    private Enemy CreateEnemyBasedOnDifficulty(int difficulty)
    {
        var health = 100 + (10 * difficulty);
        var speed = 1 + (0.1f * difficulty);
        var sizeX = 48;
        var sizeY = 48;
        var shootCooldown = 5 - difficulty;
        if (shootCooldown < 1) shootCooldown = 1;

        var random = new Random();
        float posX = random.Next(0, GetScreenWidth() - (int)sizeX);
        float posY = random.Next(0, GetScreenHeight() / 2 - (int)sizeY);

        var enemy = new Enemy(posX, posY, health, speed, sizeX, sizeY, shootCooldown, difficulty, _player, EnemyTextures[random.Next(0, 3)]);

        return enemy;
    }

    /// <summary>
    /// Resets the enemy system, including clearing all enemies and resetting the difficulty and timers.
    /// </summary>
    public void Reset()
    {
        _enemies = new List<Enemy>();
        _killCount = 0;
        _difficulty = 1;
        _lastDifficultyIncreaseTime = GetTime();
        _lastSpawnTime = GetTime();
    }
}
