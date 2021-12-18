namespace CombatKata
{
    public abstract class GameObject
    {
        private int _health;
        public string Name { get; protected set; }
        public int Health { get=> _health; protected set => _health = value < 0 ? 0 : value; }
    }
}
