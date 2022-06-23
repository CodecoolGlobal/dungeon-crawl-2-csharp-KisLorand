using DungeonCrawl.Core;
using UnityEngine;
using Random = System.Random;

namespace DungeonCrawl.Actors.Characters
{
    public abstract class Character : Actor
    {
        public int Health { get; private set; }
        public int Damage { get; private set; }

        protected int _direction;
        protected float _timer;
        protected float _limit;
        private bool _hasShield = false;
        private bool _hasHelm = false;

        public Character(int health, int damage)
        {
            Health = health;
            Damage = damage;
        }

        public void EquipShield()
        {
            _hasShield = true;
        }

        public void EquipHelm()
        {
            _hasHelm = true;
        }

      
        public void ApplyDamage(int damage)
        {
            int damageModifier;

            if (_hasShield)
            {
                damageModifier = Utilities.GetRandomNumberWithinRange(-1, 1);
            }
            else if (_hasHelm)
            {
                damageModifier = Utilities.GetRandomNumberWithinRange(-5, 0);
            }
            else
            {
                damageModifier = Utilities.GetRandomNumberWithinRange(0, 3);
            }
            
            int totalDamage = damage + damageModifier;
            if (totalDamage < 0)
            {
                totalDamage = 0;
            }
            Health -= totalDamage;
            Debug.Log($"Damage!!! {DefaultName} hp: {Health}");
            if (Health <= 0)
            {
                // Die
                OnDeath();
                

                ActorManager.Singleton.DestroyActor(this);
            }
        }

        public void ApplyHeal(int heal)
        {
            Health += heal;
        }

        protected abstract void OnDeath();

        /// <summary>
        ///     All characters are drawn "above" floor etc
        /// </summary>
        public override int Z => -1;

		public override bool OnCollision(Actor anotherActor)
		{
            if (anotherActor is Character)
            {
                Character character = (Character)anotherActor;
                if (character is Player)
                    anotherActor.OnCollision(this);
            }
            return true;
        }

    }
        
}
