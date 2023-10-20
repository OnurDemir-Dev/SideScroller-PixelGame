public interface IDamageable
{
    public bool IsDeath { get; set; }

    public void TakeDamage(int damageValue);
    public void Death();
}
