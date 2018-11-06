using System;

namespace Milkitic.SessionLib
{
    public class Program
    {

        public static void Main(string[] args)
        {
            args = new[] { "startup", "parameters" };
            const string mainNode = "Main";
            const string repeatGameNode = "RepeatGame";

            NavigatableTree program = null;
            NavigatableNode repeatGame = new NavigatableNode(repeatGameNode, obj =>
            {
                Console.WriteLine($"[{program.CurrentNode.Name}] Call by [{program.PrevNode.Name}], parameter is {obj}");
                Console.WriteLine($"Repeat game: Repeat whatever you input. Type \"quit\" to exit.");
                string echo = null;
                do
                {
                    if (echo != null) Console.WriteLine("Repeat: " + echo);
                    echo = Console.ReadLine();
                } while (echo != "quit");
                return new ActionParam(mainNode);
            });

            program = new NavigatableTree(mainNode, obj =>
            {
                Console.Write($"[{program.CurrentNode.Name}] " );
                switch (obj)
                {
                    case string[] arguments:
                        Console.WriteLine(string.Join(" ", arguments));
                        break;
                    default:
                        Console.WriteLine();
                        break;
                }
                Console.WriteLine("  Main Menu");
                Console.WriteLine("  1. Echo your input.");
                Console.WriteLine("  2. Quit.");
                int num;
                while (!int.TryParse(Console.ReadLine(), out num))
                {
                    Console.WriteLine("Please input a valid number.");
                }
                const string param = "\"This is a parameter from Root\"";
                if (num == 1)
                    return new ActionParam(repeatGameNode, param);
                return new ActionParam();
            });
            program.Root.AddChild(repeatGame);
            program.Run(args);

            Console.WriteLine("Exited.");
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }
    }
}
