namespace Health
{
    /// <summary>
    /// A configurable health system.
    /// </summary>
    public class HealthSystem
    {
        // Events (but theyre actions)
        public Action ShieldDestroyed = delegate { };
        public Action OnDeath = delegate { };

        // Core stats
        public int Health { get; private set; }
        public int Shield { get; private set; }
        public int Lives { get; private set; }

        // Range check comparisons
        int maxHP;
        int maxSP;
        int maxLives;

        HealthSystemConfig config;

        public HealthSystem(int maxHP, int maxSP, int maxLives, HealthSystemConfig config)
        {
            this.maxHP = maxHP;
            Health = maxHP;
            this.maxSP = maxSP;
            Shield = maxSP;
            this.maxLives = maxLives;
            Lives = maxLives;
            this.config = config;
        }

        /// <summary>
        /// Deals damage to the health system (and presumably its owner).
        /// </summary>
        /// <param name="hit">Data on the hit to be dealt.</param>
        /// <param name="config.logEventsToConsole">Should we report to console when certain things happen?</param>
        public void TakeDamage(HitData hit)
        {
            int initialDamage = hit.damage; // Use this for reporting at the end.
            int damage = hit.damage;

            if (config.requireExplicitHealing && damage < 0)
            {
                throw new InvalidOperationException($"Damage must not be negative while requireExplicitHealing is active.");
            }

            if (damage == 0 && config.logEventsToConsole) { Console.WriteLine("A hit of 0 damage is ineffective."); return; }

            // If the player has shield, the damage value is positive and the hit isn't true damage, hit the shield.
            if (Shield > 0 && damage > 0 && !hit.isTrueDamage)
            {
                int diff = damage - Shield; // Calculate the difference in case we need it later.

                // Create a temporary variable for damage because we have to subtract the vars from each other.
                int tempDamage = damage;
                damage -= Shield;
                Shield -= tempDamage;

                // If shield is destroyed, call that action.
                if (Shield <= 0) { Shield = 0; ShieldDestroyed(); }

                // If damage is 0 or we were told to ignore excess, try to report, and stop execution here.
                if (config.ignoreExcessShieldDamage || damage <= 0) { Report(); return; }
            }

            Health -= damage;

            // If killed, call the relevant action.
            if (Health <= 0) { Health = 0; OnDeath(); }

            // If we got this far, call report.
            Report();

            void Report()
            {
                if (config.logEventsToConsole)
                {
                    Console.WriteLine($"Player took {initialDamage} damage, and now has {Shield} shielding, {Health} health and {Lives} lives left.");
                }
            }
        }

        /// <summary>
        /// Regenerates health.
        /// </summary>
        /// <param name="value">The amount to heal</param>
        /// <param name="config.logEventsToConsole">Should we report to console when certain things happen?</param>
        /// <exception cref="InvalidOperationException"></exception>
        public void Heal(int value)
        {
            if (config.requireExplicitHealing && value < 0)
            {
                throw new InvalidOperationException($"Healing must not be negative while requireExplicitHealing is active.");
            }

            if (value == 0 && config.logEventsToConsole) { Console.WriteLine("A regeneration of 0 points is ineffective."); return; }

            Health += value;
            if (Health > maxHP) { Health = maxHP; }

            if (config.logEventsToConsole)
            {
                Console.WriteLine($"Player took {value} healing, and now has {Health} health left.");
            }
        }

        /// <summary>
        /// Regenerates shield.
        /// </summary>
        /// <param name="value">The amount to regenerate</param>
        /// <param name="config.logEventsToConsole">Should we report to console when certain things happen?</param>
        /// <exception cref="InvalidOperationException"></exception>
        public void RegenerateShield(int value)
        {
            if (config.requireExplicitHealing && value < 0)
            {
                throw new InvalidOperationException($"Healing must not be negative while requireExplicitHealing is active.");
            }

            if (value == 0 && config.logEventsToConsole) { Console.WriteLine("A regeneration of 0 points is ineffective."); return; }

            Shield += value;
            if (Shield > maxSP) { Shield = maxSP; }

            if (config.logEventsToConsole)
            {
                Console.WriteLine($"Player took {value} shield regen, and now has {Shield} shielding left.");
            }
        }

        /// <summary>
        /// Subtracts a life (if any are available) and sets other stats to max values.
        /// </summary>
        public void Revive()
        {
            if (Lives <= 0) { throw new InvalidOperationException("Cannot revive without a life to use."); }

            Health = maxHP;
            Shield = maxSP;
            Lives--;

            if (config.logEventsToConsole)
            {
                Console.WriteLine($"Player was revived. They have {Lives} lives left.");
            }
        }

        // This is an overload to meet Doucette's specs. Does not do true damage.
        public void TakeDamage(int damage) { TakeDamage(new HitData(damage)); }
    }
}
