namespace CombatKata
{
    public class Prop : GameObject, IDamageable
    {
        public Prop(string name, int health, bool destroyed)
        {
            Name = name;
            Health = health;
            Destroyed = destroyed;
        }

        public bool Destroyed { get; private set; }

        public void Attack(Character attacker, int damage, int attackDistance = 0)
        {
            Health -= damage;
            Destroyed = Health <= 0;
        }
    }
}
