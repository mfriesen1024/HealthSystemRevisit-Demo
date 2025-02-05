using Health;

namespace HealthSystemRevisit_Demo
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("This is a demo of a health system rework.");
            Console.WriteLine();
            Console.WriteLine("To start, enter '0' for a default configuration for this demo, or '1' for a config that meets assignment specs.");

            HealthSystem healthSystem;

            while (true)
            {
                ConsoleKeyInfo key = Console.ReadKey(true);

                if (key.Key == ConsoleKey.D1 || key.Key == ConsoleKey.D0)
                {
                    HealthSystemConfig config = key.Key == ConsoleKey.D0 ? HealthSystemConfig.PersonalPreference : HealthSystemConfig.AssignmentSpecs;
                    healthSystem = new HealthSystem(100, 100, 3, config);
                    break;
                }
                else
                {
                    Console.WriteLine("Invalid Key. Try again.");
                }
            }

            // Assign some delegates to actions.
            healthSystem.ShieldDestroyed += delegate { Console.WriteLine("Shields down!"); };
            healthSystem.OnDeath += delegate { Console.WriteLine("The character died!"); Console.WriteLine("Trying to revive."); healthSystem.Revive(); };

            while (true)
            {
                try
                {
                    Console.WriteLine("\nPlease enter a command. ('h' or 'help' for help)\n");
                    string s = Console.ReadLine();

                    if (s == "h" || s == "help")
                    {
                        Console.WriteLine("List of Commands:" +
                            "\nregen: regenerates either health or shield." +
                            "\ndamage: deals damage." +
                            "\nquit: exits.");
                    }

                    if (s == "regen")
                    {
                        Console.WriteLine("Regen shield 's' or health 'h'?");

                        ConsoleKeyInfo key = Console.ReadKey(true);

                        if (key.Key == ConsoleKey.S || key.Key == ConsoleKey.H)
                        {
                            Console.WriteLine("Enter healing as an integer.\n");
                            int value = int.Parse(Console.ReadLine());

                            switch (key.Key)
                            {
                                case ConsoleKey.S: healthSystem.RegenerateShield(value); break;
                                case ConsoleKey.H: healthSystem.Heal(value); break;
                            }
                        }
                        else
                        {
                            Console.WriteLine("Invalid Key.");
                        }
                    }

                    if (s == "damage")
                    {
                        Console.WriteLine("Should damage bypass shield (true damage)? 'y'/'n'");

                        ConsoleKeyInfo key = Console.ReadKey(true);

                        if (key.Key == ConsoleKey.Y || key.Key == ConsoleKey.N)
                        {
                            Console.WriteLine("Enter damage as an integer.\n");
                            int value = int.Parse(Console.ReadLine());

                            switch (key.Key)
                            {
                                case ConsoleKey.Y: healthSystem.TakeDamage(new HitData(value,true)); break;
                                case ConsoleKey.N: healthSystem.TakeDamage(new HitData(value,false)); break;
                            }
                        }
                        else
                        {
                            Console.WriteLine("Invalid Key.");
                        }
                    }

                    if(s == "quit")
                    {
                        Console.WriteLine("Ok, goodbye!");
                        Environment.Exit(0);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine($"{e.GetType()}: {e.Message}\n{e.StackTrace}");
                }
            }
        }
    }
}
