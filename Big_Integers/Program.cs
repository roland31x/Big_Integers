﻿using Microsoft.Win32;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Big_Integers
{
    public struct NumarMare
    {
        public char[] cifre; // atribut 1 : cifrele ca caractere
        public int[] cifreint; // atribut 2 : cifrele ca int
        public int[] cifrereversed; // atribut 3 : cifrele ca int in ordine inversa pentru efectuarea calculelor
        public bool IsNegative;
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
            bool operationSemn = false;
            NumarMare aux;
            if ((left.IsNegative && !right.IsNegative) || (!left.IsNegative && right.IsNegative))
            {
                return (left - right);
            }
            if(left.IsNegative && right.IsNegative)
            {
                operationSemn = true;
            }
            if (left < right)
            {
                aux = left;
                left = right;
                right = aux;
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
            for (int i = 0; i < add.cifre.Length - 1; i++)
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
            if (operationSemn)
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
            NumarMare aux;
            
            if (left < right)
            {
                aux = left;
                left = right;
                right = aux;
            }
            int subslength = left.cifre.Length;
            StringBuilder str = new StringBuilder();
            while (subslength > 0)
            {
                str.Append("0");
                subslength--;
            }
            NumarMare subs = new NumarMare(str.ToString());
            //Console.WriteLine(subs);
            for (int i = 0; i < subs.cifre.Length - 1; i++)
            {
                if (i >= right.cifre.Length)
                {
                    subs.cifrereversed[i] += left.cifrereversed[i];
                    continue;
                }
                subs.cifrereversed[i] += left.cifrereversed[i] - right.cifrereversed[i];
                if (subs.cifrereversed[i] < 0)
                {
                    subs.cifrereversed[i] += 10;
                    int borrow = 1;
                    for(int j = i + 1; j < left.cifre.Length; j++)
                    {
                        left.cifrereversed[j] -= 1;
                        if (left.cifrereversed[j] < 0)
                        {
                            left.cifrereversed[j] += 10;
                            continue;
                        }
                        if (left.cifrereversed[j] >= 0)
                        {
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
            //Console.WriteLine("c"+str.ToString());
            //Console.WriteLine();
            subs = new NumarMare(str.ToString());
            subs = subs.Normalize();
            return subs;
        }
        //public static NumarMare operator *(NumarMare left, NumarMare right)
        //{

        //}
        //public static NumarMare operator /(NumarMare left, NumarMare right)
        //{

        //}
        //public static NumarMare Pow(NumarMare base, int exp)
        //{

        //}


        public static bool operator >(NumarMare left, NumarMare right)
        {
            if(left.IsNegative && !right.IsNegative)
            {
                return true;
            }
            if (!left.IsNegative && right.IsNegative)
            {
                return false;
            }
            if (left.IsNegative && right.IsNegative)
            {
                return !(new NumarMare(left.ToString().Remove(0,1)) > new NumarMare(right.ToString().Remove(0, 1)));
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
            if (left > right) return false;
            else return true;
        }
        public static bool operator >=(NumarMare left, NumarMare right)
        {
            if (left.IsNegative && !right.IsNegative)
            {
                return true;
            }
            if (!left.IsNegative && right.IsNegative)
            {
                return false;
            }
            if (left.IsNegative && right.IsNegative)
            {
                return !(new NumarMare(left.ToString().Remove(0, 1)) >= new NumarMare(right.ToString().Remove(0, 1)));
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
        public NumarMare Normalize()
        {
            StringBuilder str = new StringBuilder();
            bool started = false;
            for(int i = 0; i < cifre.Length; i++)
            {
                if (cifrereversed[i] == 0 && !started)
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
            NumarMare a = new NumarMare("-1000000");

            //for(int i = 0; i < 4; i++)
            //{
            //    Console.Write(a.cifre[i]);
            //}
            //for (int i = 0; i < 4; i++)
            //{
            //    Console.Write(a.cifreint[i]);
            //}

            NumarMare b = new NumarMare("-687678");
            //Console.WriteLine(a.ToString());
            Console.WriteLine(a > b);
            //Console.WriteLine(a >= b);
            //Console.WriteLine(a.Equals(new NumarMare("1013")));
            //Console.WriteLine(a + b);
            Console.WriteLine(a+b);
            
        }
    }
}
