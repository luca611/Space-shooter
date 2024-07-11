namespace Space_Shooter;

public interface Entity
{
    void Draw();
    void Update();
    void Shoot();
    void TakeDamage(int damage);
    void Repair(int healthPoints);
    bool IsDestroyed();
}