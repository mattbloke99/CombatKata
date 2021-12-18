using Xunit;

namespace CombatKata
{
    public class CharacterTests
    {
        [Fact]
        public void NewCharacterStartingValuesTest()
        {
            //I kept the related assertions on creation in the one test

            var character = new Character();

            Assert.Equal(1000, character.Health);
            Assert.Equal(1, character.Level);
            Assert.True(character.Alive);
        }

        [Fact]
        public void CharacterDamageTest()
        {
            var attacker = new Character();
            var victim = new Character();

            victim.Attack(attacker, 100);

            Assert.Equal(900, victim.Health);
            Assert.True(victim.Alive);
        }

        [Fact]
        public void CharacterKillTest()
        {
            var attacker = new Character();
            var victim = new Character();

            victim.Attack(attacker, 1000);

            Assert.Equal(0, victim.Health);
            Assert.False(victim.Alive);
        }

        [Fact]
        public void CannotHealDeadCharacterTest()
        {
            var healer = new Character();
            var patient = new Character(0, 1, false);

            patient.Heal(healer, 100);

            Assert.Equal(0, patient.Health);
            Assert.False(patient.Alive);
        }

        [Fact]
        public void HealCharacterTest()
        {
            var healer = new Character();
            var patient = new Character(500, 1, true);

            patient.Heal(healer, 100);

            Assert.Equal(600, patient.Health);
            Assert.True(patient.Alive);
        }

        [Fact]
        public void CannotHealCharacterMoreThanLimitTest()
        {
            var healer = new Character();
            var patient = new Character(500, 1, true);

            patient.Heal(healer, 1000);

            Assert.Equal(1000, patient.Health);
            Assert.True(patient.Alive);
        }


        internal class Character
        {
            public Character() { }

            //I created this constructor so I can create a character with initial values
            //and still keep the setters private on the properties below
            //for better encapsulation
            public Character(int health, int level, bool alive)
            {
                Health = health;
                Level = level;
                Alive = alive;
            }

            public int Health { get; private set; } = 1000;
            public int Level { get; private set; } = 1;
            public bool Alive { get; private set; } = true;

            internal void Attack(Character attacker, int damage)
            {
                Health -= damage;
                Health = Health <= 0 ? 0 : Health;
                Alive = Health > 0;
            }

            internal void Heal(Character healer, int health)
            {
                if (Alive)
                {
                    Health += health;
                    Health = Health > 1000 ? 1000 : Health;
                }
            }
        }
    }
}
