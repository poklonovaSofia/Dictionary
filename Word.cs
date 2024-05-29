using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace Examination
{
    class NameComparer : IComparer<Word>
    {


        public int Compare(Word x, Word y)
        {
            return x.name.CompareTo(y.name);
        }
    }
    class Word : IComparable<Word>
    {
        public string name { get; set; }
        public List<string> translateWords { get; set; }
        public int index { get; set; }
        public static IComparer<Word> FromName { get { return new NameComparer(); } }
        public bool Modify { get; set; }
        public Word()
        {
            name = "";
            translateWords = new List<string>();
        }
        public Word(string n)
        {
            name = n;
            translateWords = new List<string>();
        }
        public int CompareTo(Word other)
        {
            return name.CompareTo(other.name);

        }
        private void editName()
        {
            Console.Clear();
            Console.WriteLine($"Current word {name}");
            if(createName())
            {
                Console.WriteLine($"New name {name}");
                Console.ReadKey();
            }
            

        }
        public static bool yesOrNo()
        {
            int choose = ConsoleMenu.SelectVertical(HPosition.Center,
                                                       VPosition.Center,
                                                       HorizontalAlignment.Left,
                                                       "Yes", "No");
            switch (choose)
            {
                case 0:
                    return true;
                case 1:
                    return false;
                default:
                    return false;
            }

        }
        private int chooseTranslate()
        {
            List<string> l = new List<string>();
            for(int i = 0; i < translateWords.Count; i++)
            {
                l.Add(translateWords[i]);
            }
            l.Add("Exit");
            int choose = ConsoleMenu.SelectVertical(HPosition.Center,
                                                        VPosition.Top,
                                                        HorizontalAlignment.Left,
                                                        l);
            if (choose == l.Count - 1)
            {
                return -1;
            }
            return choose;
        }
        private void deleteTranslate()
        {
            if (translateWords.Count > 1)
            {
                Console.Clear();
                Console.WriteLine($"Our word {name}");
                Console.WriteLine("Choose for delete");
                Console.WriteLine("Its translates:");
                int ch = chooseTranslate();
                if (ch != -1)
                {
                    Console.Clear();
                    Console.WriteLine($"Do you want delete this translate {translateWords[ch]}  for this word {name}?");
                    if (yesOrNo())
                    {
                        translateWords.RemoveAt(ch);
                        Modify = true;
                        Console.Clear();
                        Console.WriteLine("Translate delete");
                        Console.ReadKey();
                    }
                    else
                    {
                        return;
                    }
                }
            }
            else
            {
                Console.WriteLine("This word have only one translate");
                Console.ReadKey();
            }
        }
        private void editTranslate()
        {
            Console.Clear();
            Console.WriteLine("Choose translate that edit:");
            int ch = chooseTranslate();
            if (ch != -1)
            {
                Word w = new Word();
                if (w.createTranslate())
                {
                    if (!(translateWords.Contains(w.translateWords[0])))
                    {
                        Modify = true;
                        translateWords[ch] = w.translateWords[0];
                        Console.WriteLine($"Translate was changed on {translateWords[ch]}");
                    }
                    else
                    {
                        Console.WriteLine($"This translate {w.translateWords[0]} already exist");
                    }
                }
            }

        }
        
        public void menu()
        {
            
            while (true)
            {
                Console.Clear();
                int choose = ConsoleMenu.SelectVertical(HPosition.Center, 
                                                        VPosition.Top,
                                                        HorizontalAlignment.Left, 
                                                        "Add translate", "Delete translate", "Edit translate", "Exit");
                switch (choose)
                {
                    case 0:
                        createTranslate();
                        
                        break;
                    case 1:
                        deleteTranslate();
                        break;
                    case 2:
                        editTranslate();
                        break;
                    case 3:
                        return;
                    
                    default:
                        break;
                }
            }
        }
        public static string formName(string n)
        {
            string temp = "";
            for (int i = 0; i < n.Length; i++)
            {
                if (i == 0)
                {
                    temp += Char.ToUpper(n[i]);
                }
                else
                {
                    temp += Char.ToLower(n[i]);
                }
            }
            return temp;
        }
        public bool createTranslate()
        {
            string pattern = @"^[a-z,A-Z,а-я,А-Я]+(['-][a-z,A-Z,а-я,А-Я]+)?$";
            Regex regex = new Regex(pattern);
            string tr;
            bool flag = false;
            Console.Clear();
            while (true)
            {

                Console.WriteLine($"Add translate for {name}: ");
                tr = Console.ReadLine();
                if (regex.IsMatch(tr))
                {
                    
                    tr = formName(tr);
                    Modify = true;
                    translateWords.Add(tr);
                    break;
                }
                else
                {
                    Console.WriteLine("Not correct translate. Translate must be alphabetic and contain at least one character");
                    flag = Dictionary.repeatOrNo();
                    if (flag)
                    {
                        Console.Clear();
                        continue;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
           
            return true;
        }
        public bool create()
        {
            if (createName())
            {                
                if( createTranslate())
                {
                    Console.WriteLine($"Word {name} added");
                    Console.ReadKey();
                    return true;
                }
                else
                {
                    Console.WriteLine($"Word {name} was not added");
                    Console.ReadKey();
                    return false;

                }
            }
            return false;
        }

        public bool createName()
        {
            string pattern = @"^[a-z,A-Z,а-я,А-Я]+(['-][a-z,A-Z,а-я,А-Я]+)?$";
            Regex regex = new Regex(pattern);
            Console.Clear();
            Console.WriteLine("Add Word");
            bool flag = false;
            string w = "";
            while (true)
            {
                Console.WriteLine("Input word: ");
                w = Console.ReadLine();
                if (regex.IsMatch(w))
                {
                    break;
                }
                else
                {
                    Console.WriteLine("Not correct word. Word must be alphabetic and contain at least one character");
                    flag = Dictionary.repeatOrNo();
                    if (flag)
                    {
                        Console.Clear();
                        continue;
                    }
                    else
                        return false;
                }
            }
            
            name = formName(w);
           

            
            return true;
        }
    }
}
