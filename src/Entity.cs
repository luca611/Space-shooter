namespace Space_Shooter;

public interface IEntity
{
    void Draw();
    void Update();
    void Shoot();
    void TakeDamage(int damage);
    void Repair(int healthPoints);
    bool IsDestroyed();
}