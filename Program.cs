using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RASM
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                var rasm = new CSLAAI2D(new string[] { " " });
                Console.WriteLine("GENERAL HELP");
                Console.WriteLine(
@"Command Stack Language (And Also In 2 Dimensions)
When an instruction is encountered (except for three exceptions),
it is pushed onto the command stack. Commands e, E and ! are the exceptions.
They are executed immediately."
                );
                Console.WriteLine("Command Dump");
                Console.WriteLine("! - EOF - Terminate program.");
                Console.WriteLine("e - EXC - Execute the command on top of the stack.");
                Console.WriteLine("E - EX2 - Execute all commands on the stack.");
                foreach (var kvp in rasm.commands)
                {
                    Console.WriteLine("{0} - {1} - {2}", kvp.Key, kvp.Value.Name, kvp.Value.Desc);
                }
                Console.ReadKey();
                Console.CursorLeft--;
                Console.Write(" ");
            }
            else
            {
                var file = System.IO.File.ReadAllLines(args[0]);
                CSLAAI2D rasm = new CSLAAI2D(file);
                while (!rasm.DoRun()) ;
                Console.ReadKey();
                Console.CursorLeft--;
                Console.Write(" ");
            }
        }
    }
}
