using System.Collections.Generic;
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
            var patient = new Character(500, 1, true);
            var healer = patient;

            patient.Heal(healer, 100);

            Assert.Equal(600, patient.Health);
            Assert.True(patient.Alive);
        }

        [Fact]
        public void CannotHealCharacterMoreThanLimitTest()
        {
            var patient = new Character(500, 1, true);
            var healer = patient;

            patient.Heal(healer, 1000);

            Assert.Equal(1000, patient.Health);
            Assert.True(patient.Alive);
        }

        [Fact]
        public void CharacterCannotDamageItselfTest()
        {
            var attacker = new Character();
            var victim = attacker;

            victim.Attack(attacker, 100);

            Assert.True(attacker.GetHashCode() == victim.GetHashCode());
            Assert.Equal(1000, victim.Health);
            Assert.True(victim.Alive);
        }

        [Fact]
        public void CharacterCannotHealAnotherCharacterTest()
        {
            var healer = new Character();
            var patient = new Character(500, 1, true);

            patient.Heal(healer, 100);

            Assert.Equal(500, patient.Health);
            Assert.True(patient.Alive);
        }

        [Fact]
        public void CharacterVictimStrongerReduceDamageTest()
        {
            var attacker = new Character();
            var victim = new Character(1000, 6, true);

            victim.Attack(attacker, 200);

            Assert.Equal(900, victim.Health);
            Assert.True(victim.Alive);
        }

        [Fact]
        public void CharacterAttackerStrongerIncreaseDamageTest()
        {
            var attacker = new Character(1000, 6, true);
            var victim = new Character();

            victim.Attack(attacker, 200);

            Assert.Equal(700, victim.Health);
            Assert.True(victim.Alive);
        }


        [Fact]
        public void GetMeleeCharacterAttackRangeTest()
        {
            var character = new Character(1000, 1, true, FighterType.Melee);

            Assert.Equal(2, character.GetAttackRange());
        }

        [Fact]
        public void CharacterAttackerNotWithinRangeTest()
        {
            var attacker = new Character(1000, 1, true, FighterType.Melee);
            var victim = new Character();

            victim.Attack(attacker, 200, 3);

            Assert.Equal(1000, victim.Health);
            Assert.True(victim.Alive);
        }

        [Fact]
        public void CharacterAttackertWithinRangeTest()
        {
            var attacker = new Character(1000, 1, true, FighterType.Melee);
            var victim = new Character();

            victim.Attack(attacker, 200, 2);

            Assert.Equal(800, victim.Health);
            Assert.True(victim.Alive);
        }


        internal class Character
        {
            public Character() { }

            //I created this constructor so I can create a character with initial values
            //and still keep the setters private on the properties below
            //for better encapsulation

            //being a bit lazy here. Defaulting the fighter type to Melee means I don't need to change my old tests.
            //an alternative would be to create another constuctor
            public Character(int health, int level, bool alive, FighterType fighterType = FighterType.Melee)
            {
                Health = health;
                Level = level;
                Alive = alive;
                FighterType = fighterType;
            }

            public int Health { get; private set; } = 1000;
            public int Level { get; private set; } = 1;
            public bool Alive { get; private set; } = true;
            public FighterType FighterType { get; }
            public int GetAttackRange()
            {
                switch (FighterType)
                {
                    case FighterType.Melee:
                        return 2;
                    case FighterType.Ranged:
                        return 20;
                    default:
                        throw new System.Exception();
                }

            }

            //again defaulting attackDistance to zero so I don't need to edit existing calls.
            //alternative I could create another method with the same name but different parameters.
            //this is known as overloading and is part of polymorphism in OO
            internal void Attack(Character attacker, int damage, int attackDistance = 0)
            {
                if (attackDistance <= attacker.GetAttackRange())
                {
                    decimal damageModifier = CalculateDamageModifier(attacker);

                    if (this != attacker)
                    {
                        Health -= (int)decimal.Multiply(damage, damageModifier);
                        Health = Health <= 0 ? 0 : Health;
                        Alive = Health > 0;
                    }
                }
            }

            private decimal CalculateDamageModifier(Character attacker)
            {
                if (this.Level - attacker.Level >= 5)
                {
                    return 0.5m;
                }

                if (attacker.Level - this.Level >= 5)
                {
                    return 1.5m;
                }

                return 1;
            }

            internal void Heal(Character healer, int health)
            {
                if (this == healer)
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

    enum FighterType
    {
        Melee,
        Ranged

    }
}
