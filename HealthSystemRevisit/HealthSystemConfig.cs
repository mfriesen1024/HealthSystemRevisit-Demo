namespace Health
{
    /// <summary>
    /// A struct to allow configuration of the health system.
    /// </summary>
    public struct HealthSystemConfig
    {
        public static HealthSystemConfig AssignmentSpecs { get; set; } = new HealthSystemConfig(false, true, true);
        public static HealthSystemConfig PersonalPreference { get; set; } = new HealthSystemConfig(true, false, false);

        /// <summary>
        /// Should the health component ignore damage that overflows its shield?
        /// </summary>
        public bool ignoreExcessShieldDamage;
        /// <summary>
        /// Should the health component require healing to be done via the heal methods, or should it allow negative damage too?
        /// </summary>
        public bool requireExplicitHealing;
        /// <summary>
        /// Whether or not to log stuff to console.
        /// </summary>
        public bool logEventsToConsole;

        public HealthSystemConfig(bool ignoreExcessShieldDamage, bool requireExplicitHealing, bool logEventsToConsole)
        {
            this.ignoreExcessShieldDamage = ignoreExcessShieldDamage;
            this.requireExplicitHealing = requireExplicitHealing;
            this.logEventsToConsole = logEventsToConsole;
        }
    }
}
