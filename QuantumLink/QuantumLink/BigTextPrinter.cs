using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QuantumLink
{
    class BigTextPrinter
    {
        private string[] A
        {
            get
            {
                return new string[] { " █ ", "█ █", "███", "█ █", "█ █" };
            }
        }

        private string[] B
        {
            get
            {
                return new string[] { "██ ", "█ █", "██ ", "█ █", "███" };
            }
        }

        private string[] C
        {
            get
            {
                return new string[] { " ██", "█  ", "█  ", "█  ", " ██" };
            }
        }

        private string[] D
        {
            get
            {
                return new string[] { "██ ", "█ █", "█ █", "█ █", "██ " };
            }
        }

        private string[] E
        {
            get
            {
                return new string[] { "███", "█  ", "███", "█  ", "███" };
            }
        }

        private string[] F
        {
            get
            {
                return new string[] { "███", "█  ", "███", "█  ", "█  " };
            }
        }

        private string[] G
        {
            get
            {
                return new string[] { " ██","█  ","█ █","█ █"," ██" };
            }
        }

        private string[] H
        {
            get
            {
                return new string[] { "█ █", "█ █", "███", "█ █", "█ █" };
            }
        }

        private string[] I
        {
            get
            {
                return new string[] { "███", " █ ", " █ ", " █ ", "███" };
            }
        }

        private string[] J
        {
            get
            {
                return new string[] { "███", "  █", "  █", "█ █", " ██" };
            }
        }

        private string[] K
        {
            get
            {
                return new string[] {"█ █","█ █","██ ","█ █","█ █" };
            }
        }

        private string[] L
        {
            get
            {
                return new string[] { "█  ", "█  ", "█  ", "█  ", "███" };
            }
        }

        private string[] M
        {
            get
            {
                return new string[] {"██ ██","█ █ █","█ █ █","█ █ █","█ █ █" };
            }
        }

        private string[] N
        {
            get
            {
                return new string[] { "█   █","██  █","█ █ █","█ █ █","█  ██" };
            }
        }

        private string[] O
        {
            get
            {
                return new string[] { "███", "█ █", "█ █", "█ █", "███" };
            }
        }

        private string[] P
        {
            get
            {
                return new string[] { "███", "█ █", "███", "█  ", "█  " };
            }
        }

        private string[] Q
        {
            get
            {
                return new string[] { "███ ", "█ █ ", "█ █ ", "███ ", "   █" };
            }
        }

        private string[] R
        {
            get
            {
                return new string[] {"██  ","█ █ ","███ ","█  █","█  █" };
            }
        }

        private string[] S
        {
            get
            {
                return new string[] { "███", "█  ", "███", "  █", "███" };
            }
        }

        private string[] T
        {
            get
            {
                return new string[] { "███", " █ ", " █ ", " █ ", " █ " };
            }
        }

        private string[] U
        {
            get
            {
                return new string[] { "█ █", "█ █", "█ █", "█ █", " █ " };
            }
        }

        private string[] V
        {
            get
            {
                return new string[] { "█   █", "█   █"," █ █ "," █ █ ","  █  " };
            }
        }

        private string[] W
        {
            get
            {
                return new string[] { "█ █ █", "█ █ █", "█ █ █", "█ █ █", " █ █ " };
            }
        }

        private string[] X
        {
            get
            {
                return new string[] { "█   █", " █ █ ", "  █  ", " █ █ ", "█   █" };
            }
        }

        private string[] Y
        {
            get
            {
                return new string[] { "█ █", "█ █", " █ ", " █ ", " █ "};
            }
        }

        private string[] Z
        {
            get
            {
                return new string[] { "█████","   █ "," ██  ","█    ","█████" };
            }
        }

        private string[] Unkown
        { 
            get
            {
                return new string[] {"??????","Unkown","??????","Unkown","??????"};
            }
        }

        private Dictionary<char, string[]> charset;

        public BigTextPrinter()
        {
            charset = new Dictionary<char, string[]>();
            charset.Add('A', A);
            charset.Add('B', B);
            charset.Add('C', C);
            charset.Add('D', D);
            charset.Add('E', E);
            charset.Add('F', F);
            charset.Add('G', G);
            charset.Add('H', H);
            charset.Add('I', I);
            charset.Add('J', J);
            charset.Add('K', K);
            charset.Add('L', L);
            charset.Add('M', M);
            charset.Add('N', N);
            charset.Add('O', O);
            charset.Add('P', P);
            charset.Add('Q', Q);
            charset.Add('R', R);
            charset.Add('S', S);
            charset.Add('T', T);
            charset.Add('U', U);
            charset.Add('V', V);
            charset.Add('W', W);
            charset.Add('X', X);
            charset.Add('Y', Y);
            charset.Add('Z', Z);
        }

        public void PrintRainbow(string text)
        {
            string[] big_text = CreateBigText(text);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(big_text[0]);
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(big_text[1]);
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine(big_text[2]);
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(big_text[3]);
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine(big_text[4]);
        }

        public void PrintBigText(string text, ConsoleColor color)
        {
            string[] big_text = CreateBigText(text);
            Console.ForegroundColor = color;
            foreach(string line in big_text)
            {
                Console.WriteLine(line);
            }
            Console.ForegroundColor = ConsoleColor.White;
        }

        private string[] CreateBigText(string text)
        {
            bool first = true;
            string[] bigtext = new string[5];
            text = text.ToUpper();
            foreach(char convert in text)
            {
                string[] big_char = Unkown;
                if(charset.ContainsKey(convert))
                {
                    big_char = charset[convert];
                }
                else if(convert == ' ')
                {
                    big_char[0] = "    ";
                    big_char[1] = "    ";
                    big_char[2] = "    ";
                    big_char[3] = "    ";
                    big_char[4] = "    ";
                }
                bigtext[0] += " ";
                bigtext[1] += " ";
                bigtext[2] += " ";
                bigtext[3] += " ";
                bigtext[4] += " ";
                
                bigtext[0] += big_char[0];
                bigtext[1] += big_char[1];
                bigtext[2] += big_char[2];
                bigtext[3] += big_char[3];
                bigtext[4] += big_char[4];
            }
            return bigtext;
        }
    }
}
