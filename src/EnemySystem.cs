using System.Numerics;
using Raylib_cs;
namespace Space_Shooter;
using static Raylib;


public class EnemySystem
{
    private int _spawnTimer;
    private readonly int _maxEnemies;
    private int _difficulty;
    private double _lastDifficultyIncreaseTime;
    private double _lastSpawnTime;
    private Player _player;

    private List<Enemy> _enemies = [];
    
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

    public void Update()
    {
        var currentTime = GetTime();
    
        if (currentTime - _lastDifficultyIncreaseTime >= 60)
        {
            _difficulty++;
            _lastDifficultyIncreaseTime = currentTime;
        }
    
        var adjustedMaxEnemies = _maxEnemies * _difficulty;

        if(_enemies.Count<_maxEnemies && currentTime - _lastSpawnTime >= _spawnTimer)
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
        }
    }

    public void Draw()
    {
        foreach (var enemy in _enemies) enemy.Draw();
    }

    private Enemy CreateEnemyBasedOnDifficulty(int difficulty)
    {
        var health = 100 + (10 * difficulty); 
        var speed = 1 + (0.1f * difficulty);
        //WILL BE ADJUSTED TO TEXTURES
        var sizeX = 20; 
        var sizeY = 20;
        var shootCooldown = 5 - difficulty;
        if (shootCooldown < 1) shootCooldown = 1; 

        var random = new Random();
        float posX = random.Next(0, GetScreenWidth() - (int)sizeX);
        float posY = random.Next(0, GetScreenHeight()/2 - (int)sizeY);

        var enemy = new Enemy(posX, posY, health, speed, sizeX, sizeY, shootCooldown, difficulty, _player);

        return enemy;
    }
}