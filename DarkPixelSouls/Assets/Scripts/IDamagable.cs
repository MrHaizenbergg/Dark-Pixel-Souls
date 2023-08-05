
public interface IDamagable
{
    public int Health { get; set; }
    public void TakeDamage(int damageValue);
    public void Die();
}