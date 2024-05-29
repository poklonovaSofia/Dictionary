using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace Examination
{
    class Main
    {
        public List<Dictionary> read()
        {
            List<Dictionary> dicts = new List<Dictionary>();
            DirectoryInfo d = new DirectoryInfo(Directory.GetCurrentDirectory());
            string pattern = @"^[A-Z,А-Я][a-z,а-я]*-[A-Z,А-Я][a-z,а-я]*$";
            DirectoryInfo[] dirs = d.GetDirectories();
            Regex regex = new Regex(pattern);
            int pos = 0;
            for (int i = 0; i < dirs.Length; i++)
            {
                if (regex.IsMatch(dirs[i].Name))
                {
                    Dictionary temp = new(dirs[i].Name);

                    dicts.Add(temp);
                }
            }
            return dicts;
        }
        public int retIndex(List<Dictionary> dicts)
        {

            string[] mas = new string[dicts.Count + 1];
            for (int i = 0; i < dicts.Count; i++)
            {
                mas[i] = dicts[i].name;


            }
            mas[mas.Length - 1] = "Exit";
            int choose = ConsoleMenu.SelectVertical(HPosition.Center, VPosition.Top, HorizontalAlignment.Left, mas);
            if (choose == mas.Length - 1)
            {
                return -1;
            }

            return choose;
        }
        public static bool yesOrNo()
        {
            int choose = ConsoleMenu.SelectVertical(HPosition.Center,
                                                       VPosition.Top,
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
        private void chooseDictionary(int stat)
        {
            Console.Clear();
            List<Dictionary> dicts = read();
            if (dicts.Count > 0)
            {
                if (stat == 1)
                {
                    Console.WriteLine("Choose dictionary for translate: ");
                }
                else if(stat == 2)
                {
                    Console.WriteLine("Choose dictionary for edit: ");
                }
                else
                {
                    Console.WriteLine("Choose dictionary for delete: ");
                }
                int choose = retIndex(dicts);
                if (choose == -1)
                    return;
                if (stat == 1)
                {
                    dicts[choose].translate();
                }
                else if (stat == 2)
                {
                    dicts[choose].menu();
                }
                else
                {
                    Console.Clear();
                    Console.WriteLine($"Do you want delete directory {dicts[choose].name}");
                    if (yesOrNo())
                    {

                        Directory.Delete(dicts[choose].name, true);
                        dicts.RemoveAt(choose);
                        Console.Clear();
                        Console.WriteLine("Directory was deleted");
                        
                    }
                    
                }
            }
            else
            {
                Console.WriteLine("First, create a dictionary");
                Console.ReadKey();
                return;
            }
        }
        private void dictionaryMenu()
        {
            while (true)
            {
                Console.Clear();
                Dictionary d = new Dictionary();
                int choose = ConsoleMenu.SelectVertical(HPosition.Center,
                                                        VPosition.Top,
                                                        HorizontalAlignment.Left,
                                                        "Translate", "Create dictionary", "Edit dictionary", "Delete dictionary", "Exit");

                switch (choose)
                {
                    case 0:
                        chooseDictionary(1);
                        break;
                    case 1:

                        d.create();
                        break;
                    case 2:
                        chooseDictionary(2);
                        break;
                    case 3:
                        chooseDictionary(3);
                        break;
                    case 4:
                        return;
                    default:
                        break;
                }
            }

        }
        public void menu()
        {
            while (true)
            {
                Console.Clear();
                int choose = ConsoleMenu.SelectVertical(HPosition.Center, VPosition.Top, HorizontalAlignment.Left, "Dictionary", "Exit");
                switch (choose)
                {
                    case 0:
                        dictionaryMenu();
                        break;
                    case 1:
                        return;
                    default:
                        break;
                }
            }
        }
    }
}
