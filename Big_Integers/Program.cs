﻿using Microsoft.Win32;
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
            NumarMare a = new NumarMare("6");
            NumarMare b = new NumarMare("-9");
            NumarMare f = new NumarMare("20");
            NumarMare f2 = new NumarMare("18");
            //Console.WriteLine(a.ToString());
            //Console.WriteLine(f > f2);
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
            //Console.WriteLine(b.Pow(2));
            //Console.WriteLine(a.Pow(2));
           // Console.WriteLine(a.SimpleDiv(b));
            Console.WriteLine(a / b);
           // Console.WriteLine(f - f2);
            Console.WriteLine(a % b);
            //Console.WriteLine(b.Pow(253));
           // Console.WriteLine(b.Sqrt());
        }
        
    }
}
