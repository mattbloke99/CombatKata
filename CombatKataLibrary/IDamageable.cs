namespace CombatKata
{
    public interface IDamageable
    {
        void Attack(Character attacker, int damage, int attackDistance = 0);
    }
}
