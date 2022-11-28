using System;
using System.Collections.Generic;
using System.Linq;
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
        public static NumarMare TEN = new NumarMare("10");
        public static NumarMare NULL = new NumarMare("");
        public NumarMare(string s)
        {
            //s = IsValid(s);
            IsNegative = false;          // implicit pozitiv
            if (s.Contains('-'))         // flag-ul pentru nr negativ
            {
                IsNegative = true;
                s = s.Remove(0, 1);    // trebuie scos - ul din fata altfel nu putem forma vectorii cifrelor numarului
            }
            cifre = s.ToCharArray();
            cifreint = new int[cifre.Length];
            cifrereversed = new int[cifre.Length];
            for (int i = 0, j = cifre.Length - 1; i < cifre.Length; i++, j--) // un tablou pt cifrele in ordine inversa ( pt operatii )
            {
                cifrereversed[i] = Convert.ToInt32(cifre[j].ToString());
            }
            for (int i = 0; i < cifre.Length; i++)                              // un tablou pt cifrele in ordine normala ( ca sa fie mai usor sa imi dau seama cum trebuie sa creez operatiile )
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
            // STERGE ASTEA CAND MERGE 100%

        }
        public static string IsValid(string a)
        {
            char[] ok = new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', '-' };
            char[] toTest = a.ToCharArray(); 
            for(int i = 0; i < a.Length; i++)
            {
                if (ok.Contains(toTest[i]))
                {
                    continue;
                }
                else
                {
                    Console.WriteLine($"'{a}' este un nr introdus cu caractere incorecte, v-a fi setat implicit la 0.");
                    return "0";                    
                }
            }
            return a;
        }

        public static NumarMare operator +(NumarMare left, NumarMare right)
        {
            int additionlength;
            bool operationNeg = false;                          // daca in urma operatiei nr ce rezulta v-a fi negativ sau nu
            NumarMare aux;                                      // operatia + este comutativa astfel ne ajutam de asta
            if (left.cifre.Length < right.cifre.Length)
            {
                aux = left;
                left = right;
                right = aux;
            }
            if (left.IsNegative && !right.IsNegative)            // -a + b = |b| - |-a|
            {
                return (right.Abs() - left.Abs());
            }
            if (!left.IsNegative && right.IsNegative)            // a + -b = |a| - |-b| 
            {
                return (left.Abs() - right.Abs());
            }
            if (left.IsNegative && right.IsNegative)             // -a + -b rezulta un nr negativ , dar este o operatie normala de adunare.
            {                                                   // s-ar putea si -( |-a| + |-b| )
                operationNeg = true;
            }
            additionlength = left.cifre.Length;
            additionlength++;
            StringBuilder str = new StringBuilder();
            while (additionlength > 0)                                // creeam un numarmare nou de lungime +1 decat cel mai lung nr pentru ca:
            {                                                         // ex: 9999+
                str.Append("0");                                      //     9999
                additionlength--;                                     //    19998     si din cauza asta daca nu avem nevoie de spatiul in plus trebuie normalizat
            }
            NumarMare add = new NumarMare(str.ToString());
            for (int i = 0; i < add.cifre.Length - 1; i++)          // algoritm de adunare folosindune de cifrele inverse ale numerelor, ne oprim la add -1 pt ca add i nr cel mai lung + 1. astfel parcugem toata operatia si ultima cifra lui add va avea maxim 1 carry.
            {
                if (i >= right.cifre.Length)                          // cand indexul ajunge peste limita numarului mai mic, doar aduna la vectorul nr pe care il construim cifrele nr mai mare
                {                                                                                               //ex:     9999+
                    add.cifrereversed[i] += left.cifrereversed[i];                                              //        9999
                    if (add.cifrereversed[i] > 9)                          // necesar un check pt carry.                  ====
                    {                                                                                          //           18 = 8 in ultima cifra, +1 in urmatorul
                        add.cifrereversed[i] = add.cifrereversed[i] % 10;                                      //          18  = 9 in acest index, +1 in urmatorul
                        add.cifrereversed[i + 1] += 1;                                                         //         18   = 9 in acest index, +1 in urmatorul
                    }                                                                                          //        18    = 9 in acest index, si +1 in urmatorul
                    continue;                                                                                  //        19998 ramane 1 pt ultimul index,
                }
                add.cifrereversed[i] += left.cifrereversed[i] + right.cifrereversed[i];    // adunare a celor 2 numere
                if (add.cifrereversed[i] > 9)
                {
                    add.cifrereversed[i] = add.cifrereversed[i] % 10;
                    add.cifrereversed[i + 1] += 1;
                }
            }
            str = new StringBuilder();                              // construim numarul cu cifrele inversate.
            foreach (int i in add.cifrereversed)
            {
                str.Append(i);
            }
            add = new NumarMare(str.ToString());
            //for (int i = 0; i < 5; i++)
            //{
            //    Console.Write(add.cifreint[i]);
            //}
            //Console.WriteLine(str.ToString());
            add = add.Normalize();                                 // nr este normalizat in cazul 0-urilor nesemnificative. ( normalizarea se aplica doar pe nr formate cu cifre inversate ) adica 19000 normalizat rezulta 91.
            if (operationNeg)                                      // daca e negativ rezultatul operatiei atunci trebuie reconstruit numarul cu un semn -.
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
            if (left.cifre.Length < right.cifre.Length)             // uhhhh scaderea nu este comutativa astfel mi-am batut capul de pereti pana mi-am dat seama.
            {                                                       // in cazul in care lungime b > lungime a : am nevoie ca nr cel mai lung sa fie in partea stanga pentru ca de lungimea lui se creeaza noul numar.
                if (left.IsNegative && !right.IsNegative)           // -a - b = -(b + |-a|) in cazul in care b > a
                {
                    return (right + left.Abs()).Neg();
                }
                if (!left.IsNegative && right.IsNegative)           // a - -b = a + b ( nu conteaza ordinea deoarece + comutativ
                {
                    return left + right.Abs();
                }
                if (left.IsNegative && right.IsNegative)           // -a - -b = |-b| - |-a| 
                {
                    return right.Abs() - left.Abs();
                }
                if (!left.IsNegative && !right.IsNegative)         // a - b = -(b - a)
                {
                    return (right - left).Neg();
                }
                subslength = right.cifre.Length;                   // in cazul in care ambele sunt pozitive trebuie sa iau lungimea celei din dreapta.
            }
            if (left < right)                                      // necesar pentru final, rezultatul este un nr pozitiv construit, si asta ii da semnul negativ
            {
                return (right - left).Neg();
            }                                                     // de aici lungimea a > lungime b 
            if (left.IsNegative && !right.IsNegative)           // -a + b = -( |-a| + b )
            {
                return (left.Abs() + right).Neg();
            }
            if (!left.IsNegative && right.IsNegative)           // a - -b = a + |-b|
            {
                return left + right.Abs();
            }
            if (left.IsNegative && right.IsNegative)           // -a - -b = -(|-a| - |-b|)
            {
                return (left.Abs() - right.Abs()).Neg();
            }
            // toate astea au fost necesar pentru ca nu pot stoca - ul din fata si operatiile lucreaza ca si cum ambele ar avea cifrele pozitive.
            StringBuilder str = new StringBuilder();
            while (subslength > 0)
            {
                str.Append("0");
                subslength--;
            }
            NumarMare subs = new NumarMare(str.ToString());
            //Console.WriteLine(subs);
            for (int i = 0; i < subs.cifre.Length; i++)            // algoritmul de scadere a cifrelor in ordine inversa
            {

                if (i >= right.cifre.Length)                               // aici nu mai are ca sa scada deci doar le adauga ce a mai ramas din nr mai lung
                {
                    subs.cifrereversed[i] += left.cifrereversed[i];
                    if (subs.cifrereversed[i] < 0)                          // check pentru borrow, se poate intampla maxim odata, daca e cazul.
                    {
                        subs.cifrereversed[i] += 10;
                    }
                    continue;
                }
                subs.cifrereversed[i] += left.cifrereversed[i] - right.cifrereversed[i];      // se scad cele doua numere 
                if (subs.cifrereversed[i] < 0)                                               // verificare pentru borrow
                {
                    subs.cifrereversed[i] += 10;
                    for (int j = i + 1; j < left.cifre.Length; j++)                        // algoritm pentru borrow pana cand e posibil, in cazul 100000 - 1, parcurge toate cifrele pana cand gaseste un borrow.
                    {
                        if (true)
                        {
                            subs.cifrereversed[j] -= 1;
                            break;
                        }
                    }
                }
            }
            str = new StringBuilder();                                                              // construim numarul calculat
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
            subs = new NumarMare(str.ToString());                      // aici numarul format are cifrele invers decat ce ne trebuie la rezultat
            subs = subs.Normalize();                                  // normalizare in cazul in care sunt 0-uri nesemnificative in numar
            //if (operationNeg)
            //{
            //    str = new StringBuilder();
            //    str.Append('-');
            //    str.Append(subs.ToString());
            //    subs = new NumarMare(str.ToString());
            //}
            if (subs == NULL)         // daca a - b = 0 atunci numarul nu va contine nimic deci trebuie setat la 0.
            {
                return ZERO;
            }
            return subs;
        }
        public static NumarMare operator *(NumarMare left, NumarMare right)
        {
            NumarMare aux;
            if (left == ZERO || right == ZERO)       // 0 * a = 0, oricare a 
            {
                return ZERO;
            }
            if (left.IsNegative && !right.IsNegative || !left.IsNegative && right.IsNegative)    // pentru semn
            {
                return (right.Abs() * left.Abs()).Neg();                  // -a * b = a * -b = -( |-a| * |-b| )
            }
            if (left.IsNegative && right.IsNegative)
            {
                return right.Abs() * left.Abs();                         // -a * -b = |-a| * |-b|
            }
            if (left.cifre.Length < right.cifre.Length)                 // inmultirea este comutative
            {
                aux = left;
                left = right;
                right = aux;
            }

            int prodhelper = right.cifre.Length + left.cifre.Length;
            StringBuilder str = new StringBuilder();
            while (prodhelper > 0)
            {
                str.Append("0");
                prodhelper--;
            }
            NumarMare prod = new NumarMare(str.ToString());
            for (int i = 0; i < right.cifre.Length; i++)
            {
                for (int j = 0; j < left.cifre.Length; j++)
                {
                    prod.cifrereversed[j + i] += right.cifrereversed[i] * left.cifrereversed[j];
                    if (prod.cifrereversed[j + i] >= 10)
                    {
                        prod.cifrereversed[j + i + 1] += prod.cifrereversed[j + i] / 10;
                        prod.cifrereversed[j + i] = prod.cifrereversed[j + i] % 10;
                    }
                }
            }
            // normalizare speciala
            str = new StringBuilder();
            bool nrstarted = false;
            for (int i = 1; i <= prod.cifre.Length; i++)
            {
                if (prod.cifrereversed[prod.cifre.Length - i] == 0 && !nrstarted)
                {
                    continue;
                }
                if (prod.cifrereversed[prod.cifre.Length - i] != 0)
                {
                    nrstarted = true;
                }
                str.Append(prod.cifrereversed[prod.cifre.Length - i]);
            }
            prod = new NumarMare(str.ToString());
            //prod = prod.Normalize();
            return prod;          
        }
        public static NumarMare operator %(NumarMare left, NumarMare right)
        {
            if (left.IsNegative && !right.IsNegative || !left.IsNegative && right.IsNegative)      // la fel ca la inmultire
            {
                return left.Abs() % right.Abs();
            }
            if (left.IsNegative && right.IsNegative)
            {
                return left.Abs() % right.Abs();
            }
            if (left.cifre.Length < right.cifre.Length)          // daca q = 0 , r = left.
            {
                return left;
            }
            if (left < right)
            {
                return left;
            }
            NumarMare rez = ZERO;
            NumarMare dividend = ONE;
            int divhelper = right.cifre.Length;

            rez *= TEN;
            bool firstdivdone = false;
            int divlength = right.cifre.Length;
            NumarMare laux = left;
            NumarMare rezhelper = ZERO;
            StringBuilder str = new StringBuilder();
            if (!firstdivdone)
            {
                for (int i = 0; i < divlength; i++)
                {
                    str.Append(laux.cifreint[i]);
                }
                dividend = new NumarMare(str.ToString());
                firstdivdone = true;
                //Console.WriteLine(dividend + "start");
            }
            while (divhelper <= left.cifre.Length)
            {
                rezhelper = dividend.SimpleDiv(right);
                rez += rezhelper;
                //Console.WriteLine(dividend + "prediv");
                //Console.WriteLine(rezhelper + "rez");
                dividend -= rezhelper * right;
                //Console.WriteLine(dividend + "divrest");
                if (divhelper < left.cifre.Length)
                {
                    rez *= TEN;
                    dividend *= TEN;
                    dividend += new NumarMare(left.cifreint[divhelper].ToString());
                }
                divhelper++;
            }
            rezhelper = dividend.SimpleDiv(right);
            dividend -= rezhelper * right;
            //Console.WriteLine(rezhelper + "rez");
            return dividend;
        }
        public static NumarMare operator /(NumarMare left, NumarMare right)
        {
            if(right == NULL || right == ZERO)
            {
                throw new DivideByZeroException();
            }
            if (left.IsNegative && !right.IsNegative || !left.IsNegative && right.IsNegative)      // la fel ca la inmultire
            { 
                return (left.Abs() / right.Abs()).Neg();
            }
            if (left.IsNegative && right.IsNegative)
            {
                return left.Abs() / right.Abs();
            }
            if (left.cifre.Length < right.cifre.Length)                // daca nu se poate imparti atunci q = 0 
            {
                return ZERO;
            }
            if (left < right)
            {
                return ZERO;
            }
            NumarMare rez = ZERO;
            NumarMare dividend = ONE;
            int divhelper = right.cifre.Length;

                rez *= TEN;
                bool firstdivdone = false;
                int divlength = right.cifre.Length;              
                NumarMare laux = left;
                NumarMare rezhelper = ZERO;
                StringBuilder str = new StringBuilder();
                if (!firstdivdone)
                {
                    for (int i = 0; i < divlength; i++)
                    {
                        str.Append(laux.cifreint[i]);
                    }
                    dividend = new NumarMare(str.ToString());
                    firstdivdone = true;
                    //Console.WriteLine(dividend + "start");
                }
            while (divhelper <= left.cifre.Length)
            {
                rezhelper = dividend.SimpleDiv(right);
                rez += rezhelper;
                //Console.WriteLine(dividend + "prediv");
                //Console.WriteLine(rezhelper + "rez");
                dividend -= rezhelper * right;
               // Console.WriteLine(dividend + "divrest");
                if (divhelper < left.cifre.Length)
                {
                    rez *= TEN;
                    dividend *= TEN;
                    dividend += new NumarMare(left.cifreint[divhelper].ToString());                    
                }
                divhelper++;
            }
            rezhelper = dividend.SimpleDiv(right);
            rez += rezhelper;
           // Console.WriteLine(rezhelper + "rez");
            return rez;          
        }
        public static bool operator >(NumarMare left, NumarMare right)
        {
            if (left.IsNegative && !right.IsNegative)  // verificare pt valori negative
            {
                return false;
            }
            if (!left.IsNegative && right.IsNegative)
            {
                return true;
            }
            if (left.IsNegative && right.IsNegative)         // daca ambele sunt negative trebuie comparat cele doua in valoare absoluta si returnat conjugata returnarii. (-10) < (-9) pe cant 10 > 9
            {
                return !(left.Abs() >= right.Abs());         // in cazul in care am folosi >, cand cele doua sunt egale, acesta returneaza false, iar !false este true si acesta nu este adevarat pentru operatia noastra. 
            }
            if (left.cifre.Length > right.cifre.Length)     // evident daca ele sunt pozitive atunci cea mai mare este cea cu cifre mai mare.
            {
                return true;
            }
            if (left.cifre.Length < right.cifre.Length)
            {   
                return false;
            }
            else                                                    // verifica fiecare index de cifra in cazul in care ele au un nr egal de cifre.
            {
                for (int i = 0; i < left.cifre.Length; i++)
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
            return false; // in cazul in care nu gaseste o contradictie cele doua sunt egale, returnand false.
        }
        public static bool operator <(NumarMare left, NumarMare right)
        {
            if (left >= right) return false; // evident daca a >= b atunci a nu paote fi mai mic decat b;
            else return true;
        }
        public static bool operator >=(NumarMare left, NumarMare right)
        {
            if (left.IsNegative && !right.IsNegative)          // pentru valori negative
            {
                return false;
            }
            if (!left.IsNegative && right.IsNegative)
            {
                return true;
            }
            if (left.IsNegative && right.IsNegative) // daca ambele sunt negative atunci verificam conjugata comparatiei fara egalitate a celor doua numere cu valori absolute ( necesar pentru ca in operatia > avem flaguri pentru negative ).
            {
                return !(left.Abs() > right.Abs()); // trebuie cu ">" pentru ca in cazul in care cele doua sunt egale folosind >=, returneaza true, iar !true = false; 
            }
            if (left.cifre.Length > right.cifre.Length)
            {
                return true;                                     // sigur ambele pozitive, cel mai mare este cel cu cifre mai multe
            }
            else if (left.cifre.Length < right.cifre.Length)
            {
                return false;
            }
            else                                                    // verifica fiecare index de cifra in cazul in care ele au un nr egal de cifre.
            {
                for (int i = 0; i < left.cifre.Length; i++)
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
            return true; // daca niciunul nu este mai mare sau mai mic atunci sunt egale 
        }
        public static bool operator <=(NumarMare left, NumarMare right)
        {
            if (left > right) return false; // daca a > b atunci sigur a nu este <= decat b;
            else return true;
        }
        public static bool operator ==(NumarMare left, NumarMare right)
        {
            if (left >= right && right >= left) return true; // din injectivitate cele doua sunt egale.
            else return false;
        }
        public static bool operator !=(NumarMare left, NumarMare right)
        {
            if (left >= right && right >= left) return false; // exact ca == dar retunreaza bool-uri inverse
            else return true;
        }
        public override bool Equals(object obj)
        {
            if ((obj == null) || !this.GetType().Equals(obj.GetType())) // verificam daca obiectul comparat este null sau daca este de acelasi tip adica NumarMare
            {
                return false;
            }
            else                                                        // aplicam verificarea prin operatorul ==
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
            return this.ToString().GetHashCode(); // habar nu am ce face dar aparent ii necesar de creat, si se poate obtine un hashcode dintr-un string
        }
        /// <summary>
        /// Valoarea absoluta
        /// </summary>
        /// <returns>Valoarea absoluta al unui Numar Mare</returns>
        public NumarMare Abs()
        {
            string s = this.ToString();
            if (s.Contains('-'))                 // se sterge "-"-ul din numar si se creeaza unul nou cu atributul IsNegative = false.
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
            if (this.Equals(ZERO))
            {
                return ZERO;
            }
            StringBuilder str = new StringBuilder();
            str.Append("-");
            str.Append(this.Abs().ToString()); // luam valoarea absoluta al numarului in cazul in care este deja pozitiv ( vrem neaparat valoarea negative , nu valoarea inversata )
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
            bool started = false;                             // pentru a vedea de unde incepe numarul propriu zis
            for (int i = 0; i < cifre.Length; i++)
            {
                if ((cifrereversed[i] == 0) && !started)   // in urma unei operatii de adunare / scadere, numerul mare are atributele "cifre" si "cifreint" in ordine inversa iar "cifrereversed" sunt in ordine buna.
                {
                    continue;
                }
                if (cifrereversed[i] != 0) // de aici construim numarul respectiv fara a avea 0-uri nesemnificative in fata.
                {
                    started = true;
                }
                str.Append(cifrereversed[i]);
            }
            NumarMare rev = new NumarMare(str.ToString());
            return rev;
        }
        /// <summary>
        /// Functia factorial al unui uINT32. 
        /// </summary>
        /// <param name="f"></param>
        /// <returns>! <paramref name="f"/></returns>
        public static NumarMare Factorial(uint f)
        {
            NumarMare Factorial = ONE; // 0 factorial = 1,
            NumarMare Increment = ONE; // inmultire cu un numar care se incrementeaza in fiecare iteratie.
            for (int i = 1; i < f; i++)
            {
                //Console.WriteLine(i);
                Factorial = Factorial * (Increment + ONE); // factorial !(n+1) = n! * (n + 1)
                Increment += ONE; // ii n-ul propriu zis.
            }
            return Factorial;

        }
        /// <summary>
        /// Functia exponentiala a unei instante de NumarMare.
        /// </summary>
        /// <param name="exp">Exponent</param>
        /// <returns>Un NumarMare ridicat la puterea EXP</returns>
        public NumarMare Pow(int exp)
        {
            if (this == ZERO) // zero la orice putere ii 1
            {
                return ONE;
            }
            NumarMare aux;
            aux = this;
            for (int i = 1; i < exp; i++)
            {
                aux = aux * this;
            }
            return aux;
        }
        /// <summary>
        /// Functia putere cu exponent numar mare ( s-ar putea sa dureze foarte foarte mult un calcul de putere de peste 10000 )
        /// </summary>
        /// <param name="exp"></param>
        /// <returns></returns>
        public NumarMare Pow(NumarMare exp)
        {
            if (this == ZERO) // zero la orice putere ii 1
            {
                return ONE;
            }
            NumarMare aux;
            aux = this;
            for (NumarMare i = ONE; i < exp; i += ONE)
            {
                aux = aux * this;
            }
            return aux;
        }
        /// <summary>
        /// Radical ordinul 2 
        /// </summary>
        /// <returns>Partea intreaga al radicalului de ordin 2 al unui NumarMare</returns>
        public NumarMare Sqrt()
        {
            if (this < ZERO)
            {
                throw new Exception("Number not positive");
            }
            if (this == ZERO)
            {
                return ZERO;
            }
            NumarMare aux = this;
            NumarMare checker = ONE;
            while (checker.Pow(2) <= aux)
            {
                checker += ONE;
            }
            return checker - ONE; // while-ul face o iteratie in plus dar ii mai eficient asa decat sa verificam daca noua valoare este mai mare sau egala in fiecare iteratie.

        }
        private NumarMare SimpleDiv(NumarMare right)
        {
            NumarMare LeftAux, RightAux, aux = ZERO;
            LeftAux = this;
            RightAux = right;
            while (LeftAux - RightAux >= ZERO)                   // metoda lenesa, impartire prin scadere repetitive si incrementare contor
            {
                LeftAux = LeftAux - RightAux;                    // left - ( right + right + ... + right ) - de q ori.
                aux += ONE;
            }
            return aux;
        }
        /// <summary>
        /// Converts a NumarMare object to a string.
        /// </summary>
        /// <returns>The number as a string.</returns>
        public override string ToString()
        {
            StringBuilder str = new StringBuilder(); // stringbuilder cel mai eficient 
            if (IsNegative)  // daca e negativ ii dam un - in fata fiindca numarmare stocheaza semnul intr-un bool
            {
                str.Append('-');
            }
            for (int i = 0; i < cifre.Length; i++) // in cazul acesta ne folosim de tabloul de caractere atribuit numarului
            {
                str.Append(Convert.ToString(cifre[i]));
            }
            string s = str.ToString();
            return s;
        }
    }
}
