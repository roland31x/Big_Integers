using Microsoft.Win32;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace Big_Integers
{
    internal class Program
    {
        static void Main(string[] args)
        {
            NumarMare a = new NumarMare("256");
            NumarMare b = new NumarMare("257");
            //Console.WriteLine(a.ToString());
            //Console.WriteLine(a > b);
            //Console.WriteLine(a >= b);
            //Console.WriteLine(a < b);
            //Console.WriteLine(a <= b);
            //Console.WriteLine(a.Equals(new NumarMare("1013")));
            Console.WriteLine(a);
            Console.WriteLine(b);
            Console.WriteLine(a + b);
            Console.WriteLine(b + a);
            Console.WriteLine(b - a);
            Console.WriteLine(a - b);
            Console.WriteLine(a * b);
            Console.WriteLine(a.Pow(2));
            Console.WriteLine(a / b);
            Console.WriteLine(a % b);
            Console.WriteLine(NumarMare.Factorial(123));
            Console.WriteLine(b.Sqrt());
        }
    }
}
