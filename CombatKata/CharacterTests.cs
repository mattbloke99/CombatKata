using Xunit;

namespace CombatKata
{
    public partial class CharacterTests
    {
        [Fact]
        public void NewCharacterStartingValuesTest()
        {
            //I kept the related assertions on creation in the one test

            var character = new Character();

            Assert.Equal(1000, character.Health);
            Assert.Equal(1, character.Level);
            Assert.True(character.Alive);
            Assert.Empty(character.Factions);
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
            var character = new Character(1000, 1, true, AttackType.Melee);

            Assert.Equal(2, character.GetAttackRange(AttackType.Melee));
        }

        [Fact]
        public void CharacterAttackerNotWithinRangeTest()
        {
            var attacker = new Character(1000, 1, true, AttackType.Melee);
            var victim = new Character();

            victim.Attack(attacker, 200, 3);

            Assert.Equal(1000, victim.Health);
            Assert.True(victim.Alive);
        }

        [Fact]
        public void CharacterAttackertWithinRangeTest()
        {
            var attacker = new Character(1000, 1, true, AttackType.Melee);
            var victim = new Character();

            victim.Attack(attacker, 200, 2);

            Assert.Equal(800, victim.Health);
            Assert.True(victim.Alive);
        }

        [Fact]
        public void CharacterJoinFactionTest()
        {
            var character = new Character();

            character.JoinFaction(Faction.Cleric);

            Assert.True(character.Factions.Contains(Faction.Cleric));
        }

        [Fact]
        public void CharacteLeaveFactionTest()
        {
            var character = new Character();

            character.JoinFaction(Faction.Cleric);
            character.LeaveFaction(Faction.Cleric);

            Assert.False(character.Factions.Contains(Faction.Cleric));
        }


        [Fact]
        public void CharacterAttackAllyDoesNoDamage()
        {
            var attacker = new Character();
            attacker.JoinFaction(Faction.Wizard);

            var victim = new Character();
            victim.JoinFaction(Faction.Wizard);

            victim.Attack(attacker, 200);

            Assert.Equal(1000, victim.Health);
            Assert.True(victim.Alive);
        }

        [Fact]
        public void CharacterAttackNonAllyDoesDamage()
        {
            var attacker = new Character();
            attacker.JoinFaction(Faction.Wizard);

            var victim = new Character();
            victim.JoinFaction(Faction.Cleric);

            victim.Attack(attacker, 200);

            Assert.Equal(800, victim.Health);
            Assert.True(victim.Alive);
        }

        [Fact]
        public void CharacterHealAlly()
        {
            var patient = new Character(500, 1, true);
            patient.JoinFaction(Faction.Ranger);
            var healer = new Character();
            healer.JoinFaction(Faction.Ranger);

            patient.Heal(healer, 100);

            Assert.Equal(600, patient.Health);
            Assert.True(patient.Alive);
        }

        [Fact]
        public void CharacterCannotHealNonAlly()
        {
            var patient = new Character(500, 1, true);
            patient.JoinFaction(Faction.Ranger);
            var healer = new Character();
            healer.JoinFaction(Faction.Wizard);

            patient.Heal(healer, 100);

            Assert.Equal(500, patient.Health);
            Assert.True(patient.Alive);
        }

        [Fact]
        public void PropCreateTest()
        {
            var prop = new Prop("Tree", 2000, false);

            Assert.Equal("Tree", prop.Name);
            Assert.Equal(2000, prop.Health);
            Assert.False(prop.Destroyed);
        }

        [Fact]
        public void PropDestroyTest()
        {
            var prop = new Prop("Tree", 2000, false);
            prop.Attack(new Character(), 3000);
            
            Assert.Equal(0, prop.Health);
            Assert.True(prop.Destroyed);
        }
    }
}
