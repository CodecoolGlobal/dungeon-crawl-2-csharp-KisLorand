using UnityEngine;

namespace DungeonCrawl.Actors.Characters
{
    public class Skeleton : Character
    {
        private const int _health = 6;
        private const int _damage = 2;
        public Skeleton() : base(_health, _damage)
        {

        }

        protected override void OnDeath()
        {
            Debug.Log("Well, I was already dead anyway...");
        }

        public override int DefaultSpriteId => 316;
        public override string DefaultName => "Skeleton";
 

    }
}
