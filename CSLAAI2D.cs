using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RASM
{
    class CSLAAI2D
    {
        public Dictionary<char, Command> commands { get; protected set; }
        Stack<Action> commandStack;
        Stack<int> memory;
        Direction direction;
        int x;
        int y;
        char[][] code;
        bool stop;
        
        public CSLAAI2D(string[] arg)
        {
            int longestLen = arg.Aggregate("", (max, cur) => max.Length > cur.Length ? max : cur).Length - 1;
            code = new char[arg.Count()][];
            for (int i = 0; i < code.Count(); i++)
            {
                code[i] = arg[i].PadRight(longestLen, 'Z').ToCharArray();
            }
            //foreach (char[] c in code)
            //{
            //    Console.WriteLine(new string(c));
            //}
            commandStack = new Stack<Action>();
            memory = new Stack<int>();
            direction = Direction.Right;
            x = -1;
            y = 0;
            commands = new Dictionary<char, Command>();
            commands.Add('v', new Command("MDN", "Switches instruction direction", new Action(() =>
            {
                direction = Direction.Down;
            })));
            commands.Add('<', new Command("MLT", "Switches instruction direction", new Action(() =>
            {
                direction = Direction.Left;
            })));
            commands.Add('>', new Command("MRT", "Switches instruction direction", new Action(() =>
            {
                direction = Direction.Right;
            })));
            commands.Add('^', new Command("MUP", "Switches instruction direction", new Action(() =>
            {
                direction = Direction.Up;
            })));
            commands.Add('1', new Command("ONE", "Push a 1 onto the stack.", new Action(() =>
            {
                memory.Push(1);
            })));
            commands.Add('D', new Command("DUP", "Copy the top memory item.", new Action(() =>
            {
                memory.Push(memory.Peek());
            })));
            commands.Add('P', new Command("PRT", "Print the top memory item", new Action(() =>
            {
                Console.Write(memory.Pop());
            })));
            commands.Add('p', new Command("PRA", "Print the top memory item as ASCII", new Action(() =>
            {
                Console.Write((char)memory.Pop());
            })));
            commands.Add('N', new Command("PRN", "Write a line terminator", new Action(() =>
            {
                Console.Write(Environment.NewLine);
            })));
            commands.Add('+', new Command("INC", "Increment the top memory item", new Action(() =>
            {
                memory.Push(memory.Pop() + 1);
            })));
            commands.Add('-', new Command("DEC", "Decrement the top memory item", new Action(() =>
            {
                memory.Push(memory.Pop() - 1);
            })));
            commands.Add('&', new Command("ADD", "Add the top two items", new Action(() =>
            {
                memory.Push(memory.Pop() + memory.Pop());
            })));
            commands.Add('*', new Command("MUL", "Multiply the top two items", new Action(() =>
            {
                memory.Push(memory.Pop() * memory.Pop());
            })));
            commands.Add('_', new Command("SUB", "Subtract the top two items (pop - pop)", new Action(() =>
            {
                memory.Push(memory.Pop() - memory.Pop());
            })));
            commands.Add('~', new Command("SWT", "Switch the top two items", new Action(() =>
            {
                var p1 = memory.Pop();
                var p2 = memory.Pop();
                memory.Push(p1);
                memory.Push(p2);
            })));
            commands.Add('|', new Command("NUL", "Pop the top item, and do nothing with it.", new Action(() =>
            {
                memory.Pop();
            })));
            stop = false;
        }

        public bool DoRun()
        {
            char c = NextChar();
            if (commands.ContainsKey(c) && c != 'E')
            {
                commandStack.Push(commands[c].Action);
            }
            if (c == 'e')
            {
                commandStack.Pop().Invoke();
            }
            if (c == 'E')
            {
                while (commandStack.Count != 0)
                {
                    commandStack.Pop().Invoke();
                }
            }
            if (c == '!')
            {
                return true;
            }
            return stop;
        }

        public char NextChar()
        {
            Move(direction);
            return code[y][x];
        }

        public void Move(Direction dir)
        {
            switch (dir)
            {
                case Direction.Right:
                    if (x >= code[0].GetUpperBound(0))
                    {
                        x = -1;
                    }
                    x++;
                    break;
                case Direction.Down:
                    if (y >= code.GetUpperBound(0))
                    {
                        y = -1;
                    }
                    y++;
                    break;
                case Direction.Left:
                    if (x <= 0)
                    {
                        x = code[0].Count();
                    }
                    x--;
                    break;
                case Direction.Up:
                    if (y <= 0)
                    {
                        y = code.Count();
                    }
                    y--;
                    break;
            }
        }

        public void PlaceOnStack(Command com)
        {
            commandStack.Push(com.Action);
        }

        public enum Direction
        {
            Up,
            Down,
            Left,
            Right
        }

    }

    class Command
    {
        public Action Action { get; set; }
        public string Name { get; set; }
        public string Desc { get; set; }

        public Command(string name, string desc, Action action)
        {
            Action = action;
            Name = name;
            Desc = desc;
        }
    }
}
