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
    public readonly struct NumarMare
    {
        readonly char[] cifre; // atribut 1 : cifrele ca caractere
        readonly int[] cifreint; // atribut 2 : cifrele ca int
        readonly int[] cifrereversed; // atribut 3 : cifrele ca int in ordine inversa pentru efectuarea calculelor
        readonly bool IsNegative;
        public static NumarMare ZERO = new NumarMare("0");
        public static NumarMare ONE = new NumarMare("1");
        public static NumarMare NULL = new NumarMare("");
        public NumarMare(string s)
        {
            IsNegative= false;
            if (s.Contains('-'))
            {
                IsNegative = true;
                s = s.Remove(0, 1);
            }
            cifre = s.ToCharArray();
            cifreint = new int[cifre.Length]; 
            cifrereversed= new int[cifre.Length];
            for (int i = 0, j = cifre.Length - 1 ; i < cifre.Length; i++, j--)
            {
                cifrereversed[i] = Convert.ToInt32(cifre[j].ToString());
            }
            for(int i = 0; i < cifre.Length; i++)
            {
                cifreint[i] = Convert.ToInt32(cifre[i].ToString());
            }
            //Console.WriteLine("c:");
            //for (int i = 0; i < cifre.Length; i++)
            //{
            //    Console.Write(cifre[i]);
            //}
            //Console.WriteLine("i:");
            //for (int i = 0; i < cifre.Length; i++)
            //{
            //    Console.Write(cifreint[i]);
            //}
            //Console.WriteLine("r:");
            //for (int i = 0; i < cifre.Length; i++)
            //{
            //    Console.Write(cifrereversed[i]);
            //}

        }
        public static NumarMare operator +(NumarMare left, NumarMare right)
        {
            int additionlength;
            bool operationNeg = false;
            NumarMare aux;
            if (left.cifre.Length < right.cifre.Length)
            {
                aux = left;
                left = right;
                right = aux;
            }
            if (left.IsNegative && !right.IsNegative)
            {
                return (right.Abs() - left.Abs());
            }
            if(!left.IsNegative && right.IsNegative)
            {
                return (left.Abs() - right.Abs());
            }
            if(left.IsNegative && right.IsNegative)
            {
                operationNeg = true;
            }
            additionlength = left.cifre.Length;
            additionlength++;
            StringBuilder str = new StringBuilder();
            while (additionlength > 0)
            {
                str.Append("0");
                additionlength--;
            }
            NumarMare add = new NumarMare(str.ToString());
            for (int i = 0; i < add.cifre.Length - 1 ; i++)
            {
                if(i >= right.cifre.Length)
                {
                    add.cifrereversed[i] += left.cifrereversed[i];
                    if (add.cifrereversed[i] > 9)
                    {
                        add.cifrereversed[i] = add.cifrereversed[i] % 10;
                        add.cifrereversed[i + 1] += 1;
                    }
                    continue;
                }
                add.cifrereversed[i] += left.cifrereversed[i] + right.cifrereversed[i];
                if (add.cifrereversed[i] > 9)
                {
                    add.cifrereversed[i] = add.cifrereversed[i] % 10;
                    add.cifrereversed[i + 1] += 1;
                }
            }
            str = new StringBuilder();
            foreach(int i in add.cifrereversed)
            {
                str.Append(i);
            }
            add = new NumarMare(str.ToString());
            //for (int i = 0; i < 5; i++)
            //{
            //    Console.Write(add.cifreint[i]);
            //}
            //Console.WriteLine(str.ToString());
            add = add.Normalize();
            if (operationNeg)
            {
                str = new StringBuilder();
                str.Append('-');
                str.Append(add.ToString());
                add = new NumarMare(str.ToString());
            }
            return add;
        }
        public static NumarMare operator -(NumarMare left, NumarMare right)
        {
           // bool operationNeg = false;
            int subslength = left.cifre.Length;
            if (left.cifre.Length < right.cifre.Length)
            {
                if (left.IsNegative && !right.IsNegative)
                {
                    return (right + left.Abs()).Neg();
                }
                if (!left.IsNegative && right.IsNegative)
                {
                    return left + right.Abs();
                }
                if (left.IsNegative && right.IsNegative)
                {
                    return right.Abs() - left.Abs();
                }
                if (!left.IsNegative && !right.IsNegative)
                {
                    return (right - left).Neg();
                }
                subslength = right.cifre.Length;
            }
            if (left < right)
            {
                return (right - left).Neg();
            }
            if ( left.IsNegative && !right.IsNegative )
            {
                return (left.Abs() + right).Neg();
            }
            if ( !left.IsNegative && right.IsNegative )
            {
                return left + right.Abs();
            }
            if ( left.IsNegative && right.IsNegative )
            {
                return (left.Abs() - right.Abs()).Neg();
            }
            
            StringBuilder str = new StringBuilder();
            while (subslength > 0)
            {
                str.Append("0");
                subslength--;
            }
            NumarMare subs = new NumarMare(str.ToString());
            //Console.WriteLine(subs);
            for (int i = 0; i < subs.cifre.Length; i++)
            {
                
                if (i >= right.cifre.Length)
                {
                    subs.cifrereversed[i] += left.cifrereversed[i];
                    if (subs.cifrereversed[i] < 0)
                    {
                        subs.cifrereversed[i] += 10;
                    }
                    continue;
                }
                subs.cifrereversed[i] += left.cifrereversed[i] - right.cifrereversed[i];
                if (subs.cifrereversed[i] < 0)
                { 
                    subs.cifrereversed[i] += 10;
                    for(int j = i + 1; j < left.cifre.Length; j++)
                    {
                        if (left.cifrereversed[j] - 1 < 0)
                        {
                            subs.cifrereversed[j] -= 1;
                            continue;
                        }
                        if (left.cifrereversed[j] - 1 >= 0) 
                        { 
                            subs.cifrereversed[j] -= 1;
                            break;
                        }
                    }
                }
            }
            str = new StringBuilder();
            for (int i = 0; i < subs.cifrereversed.Length; i++)
            {
                str.Append(subs.cifrereversed[i]);
               
            }
            //for (int i = 0; i < subs.cifrereversed.Length; i++)
            //{
            //    Console.Write(subs.cifrereversed[i]);
            //}
            //Console.WriteLine("c" + str.ToString());
            //Console.WriteLine();
            subs = new NumarMare(str.ToString());
            subs = subs.Normalize();
            //if (operationNeg)
            //{
            //    str = new StringBuilder();
            //    str.Append('-');
            //    str.Append(subs.ToString());
            //    subs = new NumarMare(str.ToString());
            //}
            if (subs == NULL)
            {
                return ZERO;
            }
            return subs;
        }
        public static NumarMare operator *(NumarMare left, NumarMare right)
        {
            NumarMare ONE = new NumarMare("1");
            NumarMare aux;
            NumarMare LeftAux;
            NumarMare RightAux;
            if(left == ZERO || right == ZERO)
            {
                return ZERO;
            }
            if (left.IsNegative && !right.IsNegative || !left.IsNegative && right.IsNegative)
            {
                return (right.Abs() * left.Abs()).Neg();
            }
            if (left.IsNegative && right.IsNegative)
            {
                return right.Abs() * left.Abs();
            }
            if (left.cifre.Length < right.cifre.Length)
            {
                aux = left;
                left = right;
                right = aux;
            }
            LeftAux = left;
            RightAux = right;
            while (RightAux > ONE)
            {
                LeftAux += left;
                RightAux -= ONE;
            }
            return LeftAux;
        }
        public static NumarMare operator %(NumarMare left, NumarMare right)
        {
            NumarMare LeftAux;
            NumarMare RightAux;
            if (left.IsNegative && !right.IsNegative || !left.IsNegative && right.IsNegative)
            {
                return (right.Abs() % left.Abs()).Neg();
            }
            if (left.IsNegative && right.IsNegative)
            {
                return right.Abs() % left.Abs();
            }
            if (left.cifre.Length < right.cifre.Length)
            {
                return left;
            }
            LeftAux = left;
            RightAux = right;
            while (LeftAux - RightAux >= ZERO)
            {
                LeftAux = LeftAux - RightAux;
            }
            return LeftAux;
        }
        public static NumarMare operator /(NumarMare left, NumarMare right)
        {
            NumarMare aux = ZERO;
            NumarMare LeftAux;
            NumarMare RightAux;
            if (left.IsNegative && !right.IsNegative || !left.IsNegative && right.IsNegative)
            {
                return (right.Abs() / left.Abs()).Neg();
            }
            if (left.IsNegative && right.IsNegative)
            {
                return right.Abs() / left.Abs();
            }
            if (left.cifre.Length < right.cifre.Length)
            {
                return ZERO;
            }
            LeftAux = left;
            RightAux = right;
            while ( LeftAux - RightAux >= ZERO)
            {
                LeftAux = LeftAux - RightAux;
                aux += ONE;
            }
            return aux;
        }
        /// <summary>
        /// Functia exponentiala a unei instante de NumarMare.
        /// </summary>
        /// <param name="exp">Exponent</param>
        /// <returns>Un NumarMare ridicat la puterea EXP</returns>
        public NumarMare Pow(int exp)
        {
            if(this == ZERO)
            {
                return ONE;
            }
            NumarMare aux;
            aux = this;
            for(int i = 1; i < exp; i++)
            {
                aux = aux * this;
            }
            return aux;
        }
        public static bool operator >(NumarMare left, NumarMare right)
        {
            if(left.IsNegative && !right.IsNegative)
            {
                return false;
            }
            if (!left.IsNegative && right.IsNegative)
            {
                return true;
            }
            if (left.IsNegative && right.IsNegative)
            {
                return !(left.Abs() >= right.Abs());
            }
            if (left.cifre.Length > right.cifre.Length)
            {
                return true;
            }
            else if(left.cifre.Length < right.cifre.Length)
            {
                return false;
            }
            else
            {
                for (int i = left.cifre.Length - 1 ; i >= 0 ; i--)
                {
                    if (left.cifre[i] > right.cifre[i])
                    {
                        return true;
                    }
                    else if (left.cifre[i] < right.cifre[i])
                    {
                        return false;
                    }
                    else continue;
                }
            }
            return false;
        }
        public static bool operator <(NumarMare left, NumarMare right)
        {
            if (left >= right) return false;
            else return true;
        }
        public static bool operator >=(NumarMare left, NumarMare right)
        {
            if (left.IsNegative && !right.IsNegative)
            {
                return false;
            }
            if (!left.IsNegative && right.IsNegative)
            {
                return true;
            }
            if (left.IsNegative && right.IsNegative)
            {
                return !(left.Abs() > right.Abs());
            }
            if (left.cifre.Length > right.cifre.Length)
            {
                return true;
            }
            else if (left.cifre.Length < right.cifre.Length)
            {
                return false;
            }
            else
            {
                for (int i = left.cifre.Length - 1; i >= 0; i--)
                {
                    if (left.cifre[i] > right.cifre[i])
                    {
                        return true;
                    }
                    else if (left.cifre[i] < right.cifre[i])
                    {
                        return false;
                    }
                    else continue;
                }
            }
            return true;
        }
        public static bool operator <=(NumarMare left, NumarMare right)
        {
            if (left > right) return false;
            else return true;
        }
        public static bool operator ==(NumarMare left,NumarMare right)
        {
            if (left >= right && right >= left) return true;
            else return false;
        }
        public static bool operator !=(NumarMare left, NumarMare right)
        {
            if (left >= right && right >= left) return false;
            else return true;
        }
        public override bool Equals(object obj)
        {
            if ((obj == null) || !this.GetType().Equals(obj.GetType()))
            {
                return false;
            }
            else
            {
                NumarMare p = (NumarMare)obj;
                if (this == p)
                {
                    return true;
                }
                else return false;
            }
        }
        public override int GetHashCode()
        {
            return this.ToString().GetHashCode();
        }
        /// <summary>
        /// Valoarea absoluta
        /// </summary>
        /// <returns>Valoarea absoluta al unui Numar Mare</returns>
        public NumarMare Abs()
        {
            string s = this.ToString();
            if (s.Contains('-'))
            {
                s = s.Remove(0, 1);
            }
            NumarMare abs = new NumarMare(s);
            return abs;
        }
        /// <summary>
        /// Valoarea negativa al unui NumarMare.
        /// </summary>
        /// <returns>Valoarea negative al unui NumarMare indiferent de semn</returns>
        public NumarMare Neg()
        {
            StringBuilder str = new StringBuilder();
            str.Append("-");
            str.Append(this.Abs().ToString());
            NumarMare neg = new NumarMare(str.ToString());
            return neg;
        }
        /// <summary>
        /// Se foloseste in cazul operatiilor de adunare sau scadere pentru a elimina 0-uri nesemnificative din numar.
        /// </summary>
        /// <returns>Numarul fara 0-uri nesemnificative in fata, in urma unei operatii de adunare sau scadere.</returns>
        public NumarMare Normalize()
        {
            StringBuilder str = new StringBuilder();
            bool started = false;
            for(int i = 0; i < cifre.Length; i++)
            {
                if ((cifrereversed[i] == 0 ) && !started)
                {
                    continue;
                }
                if (cifrereversed[i] != 0)
                {
                    started = true;
                }
                str.Append(cifrereversed[i]);
            }
            NumarMare rev = new NumarMare(str.ToString());
            return rev;
        } 
        /// <summary>
        /// Converts a NumarMare object to a string.
        /// </summary>
        /// <returns>The number as a string.</returns>
        public override string ToString()
        {
            StringBuilder str = new StringBuilder();
            if (IsNegative)
            {
                str.Append('-');
            }
            for(int i = 0; i < cifre.Length; i++)
            {
                str.Append(Convert.ToString(cifre[i]));
            }
            string s = str.ToString();
            return s;
        }
    }
    internal class Program
    {
        static void Main(string[] args)
        {
            NumarMare a = new NumarMare("290");
            NumarMare b = new NumarMare("120");
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
            //a = new NumarMare("1000");
            //b = new NumarMare("100000");
            Console.WriteLine(a * b);
            Console.WriteLine(a.Pow(2));
            Console.WriteLine(a / b);
            Console.WriteLine(a % b);

        }
    }
}
