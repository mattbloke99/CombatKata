using System;
using System.Collections.Generic;
using System.Linq;

namespace CombatKata
{
    public class Character : GameObject, IDamageable
    {
        private const int MaximumHealth = 1000;
        private const int MeleeAttackDistance = 2;
        private const int RangeAttachedDistance = 20;

        private Dictionary<AttackType, int> attackTypeDistanceLookup = new Dictionary<AttackType, int>() {{ AttackType.Melee, MeleeAttackDistance }, { AttackType.Ranged, RangeAttachedDistance } };

        public Character()
        {
            Health = MaximumHealth;
        }

        //I created this constructor so I can create a character with initial values
        //and still keep the setters private on the properties below
        //for better encapsulation

        //being a bit lazy here. Defaulting the fighter type to Melee means I don't need to change my old tests.
        //an alternative would be to create another constuctor
        public Character(int health, int level, bool alive, AttackType fighterType = AttackType.Melee)
        {
            Health = health;
            Level = level;
            Alive = alive;
            AttackType = fighterType;
        }

        public int Level { get; private set; } = 1;
        public bool Alive { get; private set; } = true;
        public AttackType AttackType { get; }
        public IList<Faction> Factions { get; private set; } = new List<Faction>();

        //now takes a parameter which makes it easier to test
        //refactored to use dictionary and therefore switch is not required.
        //means new attack types can be added more easily
        //potentially you could drive this from a database and therefore not
        //have to change the code.
        public int GetAttackRange(AttackType attackType)
        {
            if (attackTypeDistanceLookup.TryGetValue(attackType, out int distance))
            {
                return distance;
            }

            throw new Exception();
        }

        //again defaulting attackDistance to zero so I don't need to edit existing calls.
        //alternative I could create another method with the same name but different parameters.
        //this is known as overloading and is part of polymorphism in OO
        public void Attack(Character attacker, int damage, int attackDistance = 0)
        {
            if (IsAlly(attacker))
            {
                return;
            }

            if (attackDistance <= attacker.GetAttackRange(AttackType))
            {
                decimal damageModifier = CalculateDamageModifier(attacker);

                if (this != attacker)
                {
                    Health -= (int)decimal.Multiply(damage, damageModifier);
                    Alive = Health > 0;
                }
            }
        }

        private bool IsAlly(Character attacker)
        {
            //This is linq statement and is a terse way of looking for the intersections between
            //two collections, looping through the collection is acceptable and I don't expect you to know this.
            //might be worth starting to learning a bit about linq as it is very powerful
            return Factions.Intersect(attacker.Factions).Any();
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

        public void Heal(Character healer, int health)
        {
            if ((this == healer || IsAlly(healer)) && Alive)
            {
                Health += health;
                Health = Health > 1000 ? 1000 : Health;
            }
        }

        public void JoinFaction(Faction faction) => Factions.Add(faction);

        public void LeaveFaction(Faction faction) => Factions.Remove(faction);
    }

}
