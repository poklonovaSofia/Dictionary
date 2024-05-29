using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace Examination
{
    class Dictionary
    {
        public int index { get; set; }
        public string name { get; set; }
        public List<Word> Words { get; set; }
        public static bool repeatOrNo()
        {
            int choose = ConsoleMenu.SelectVertical(HPosition.Center,
                                                       VPosition.Center,
                                                       HorizontalAlignment.Left,
                                                       "Repeat", "Exit");
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
        private void formName(string n)
        {
            for (int i = 0; i < n.Length; i++)
            {
                if (i == 0)
                {
                    name  += Char.ToUpper(n[i]);
                }
                else
                {
                    name += Char.ToLower(n[i]);
                }
            }     
        }
        private void createName(string f, string s)
        {

            
            formName(f);
            name += '-';
            formName(s);
           
        }
        public void create()
        {
            Console.Clear();
            string pattern = @"^[a-z,A-Z,а-я,А-Я]+$";
            Console.WriteLine("Create dictionary");
            string f = "";
            string s = "";
            Regex regex = new Regex(pattern);
            bool flag = false;
            while (true)
            {
                Console.WriteLine("Input the language you are transleting from: ");
                f = Console.ReadLine();
                if (regex.IsMatch(f))
                {
                    break;
                }
                else
                {
                    Console.Clear();
                    Console.WriteLine("Not correct language. Name of language must be alphabetic and contain at least one character");
                    flag = repeatOrNo();
                    if (flag)
                    {
                        Console.Clear();
                        continue;
                    }
                    else
                        return;
                }
            }
            while (true)
            {
                Console.WriteLine("Input the language you are transleting on: ");
                s = Console.ReadLine();

                if (regex.IsMatch(s))
                {
                    break;
                }
                else
                {
                    Console.Clear();
                    Console.WriteLine("Not correct language. Name of language must be alphabetic and contain at least one character");
                    flag = repeatOrNo();
                    if (flag)
                    {
                        Console.Clear();
                        continue;
                    }

                    else
                        return;
                }
            }
            createName(f, s);
            createDir();
            menu();
        }
        private void createDir()
        {
            DirectoryInfo d = new DirectoryInfo(name);
            if (!d.Exists)
            {
                d.Create();
            }
            else
            {
                Console.WriteLine($"Such a dictionary {name} already exists");
                Console.ReadKey();
                return;
            }
        }
        private void feelWords(string nameF)
        {
            using (FileStream fs = new FileStream(nameF, FileMode.Open))
            {
                byte[] bytes = new byte[fs.Length];
                fs.Read(bytes, 0, bytes.Length);
                string text = Encoding.Default.GetString(bytes);
                //string[] stringSeparators = new string[] { "\r\n" };
                string[] strW = text.Split('\n');
                for (int i = 0; i < strW.Length; i++)
                {
                    if (i == 0)
                    {
                        index = Convert.ToInt16(strW[i]);
                    }
                    else
                    {
                        Word w = new Word(strW[i]);
                        w.index = Convert.ToInt16(strW[i + 1]);
                        using (FileStream fs2 = new FileStream($"{name}/{w.index}.txt", FileMode.Open))
                        {
                            byte[] bytes2 = new byte[fs2.Length];
                            fs2.Read(bytes2, 0, bytes2.Length);
                            string text2 = Encoding.Default.GetString(bytes2);
                            string[] strW2 = text2.Split('\n');
                            for (int j = 0; j < strW2.Length; j++)
                            {
                                if (strW2[j].Length > 0)
                                    w.translateWords.Add(strW2[j]);
                            }
                        }
                        Words.Add(w);
                        i++;
                    }
                }
                
            }
        }
        public void read()
        {
            FileStream fs = null;
            if (File.Exists($"{name}/{name}.txt"))
            {
                feelWords($"{name}/{name}.txt");
            }
            else
            {
                fs = new FileStream($"{name}/{name}.txt", FileMode.CreateNew);
                fs.Close();
                fs = null;
                index = 0;
            }
        }
        private void addWord(Word w)
        {
            
            w.index = this.index+1;
            Words.Add(w);
            this.index++;
        }
        private Word searchByName(string n)
        {
            foreach (var item in Words)
            {
                if (item.name == n)
                {
                    return item;
                }
            }
            return null;
        }
        private void saveMainFile()
        {
            using (FileStream allW = new FileStream($"{name}/{name}.txt", FileMode.Create))
            {
                string strAllW = "";
                strAllW += index;

                foreach (var item in Words)
                {
                    strAllW += '\n';
                    strAllW += item.name;
                    strAllW += '\n';
                    strAllW += item.index;
                }
                
                byte[] bufferW = Encoding.Default.GetBytes(strAllW);
                allW.Write(bufferW, 0, bufferW.Length);
            }
        }
        private void saveWord(Word w)
        {
            using (FileStream fs2 = new FileStream($"{name}/{w.index}.txt", FileMode.Create))
            {
                string strT = "";
                //string[] stringSeparators = new string[] { "\r\n" };
                for(int i = 0; i < w.translateWords.Count; i++)
                {
                    if (w.translateWords[i].Length > 0)
                    {
                        strT += w.translateWords[i];
                        strT += '\n';
                    }
                }                
                byte[] buffer = Encoding.Default.GetBytes(strT);
                fs2.Write(buffer, 0, buffer.Length);
                fs2.Close();
            }
        }
        private Word chooseWord()
        {
            Console.Clear();
            
            List<string> strW = new List<string>();
            foreach (var item in Words)
            {
                strW.Add(item.name); 
            }
            strW.Add("Exit");
            int choose = ConsoleMenu.SelectVertical(HPosition.Center,
                                                      VPosition.Center,
                                                      HorizontalAlignment.Left,
                                                      strW);
            if(choose == strW.Count-1)
            {
                return null;
            }

            return searchByName(strW[choose]);
        }
        private void translateWord()
        {
           
            string pattern = @"^[a-z,A-Z,а-я,А-Я]+(['-][a-z,A-Z,а-я,А-Я]+)?$";
            string temp = "";
            bool flag = false;
            Regex regex = new Regex(pattern);
            while(true)
            {
                Console.Clear();
                Console.WriteLine("Input word for translate: ");
                temp = Console.ReadLine();
                if(regex.IsMatch(temp))
                {
                    break;
                }
                else
                {
                    Console.WriteLine("This function translate only one word");
                    flag = Dictionary.repeatOrNo();
                    if (flag)
                    {
                        Console.Clear();
                        continue;
                    }
                    else
                        return;
                }
            }
            temp = Word.formName(temp);
            Word w = searchByName(temp);
            if(w != null)
            {
                string translate = getTranslate(w);
                Console.WriteLine(translate);
            }
            else
            {
                Console.WriteLine($"This dictionary hasn't this word {temp} ");
            }
            Console.ReadKey();

        }
        private string getTranslate(Word w)
        {
            string temp = "";
            
            for(int i = 0; i < w.translateWords.Count;i++)
            {
                if (i == 0)
                {
                    temp += w.translateWords[i];
                    
                }
                else
                {
                    if(i == 1)
                        temp += "(";
                    temp += w.translateWords[i];
                    if (i != w.translateWords.Count - 1)
                    {
                        temp += ", ";
                    }
                    else
                    {
                        temp += ")";
                    }
                }
            }
            return temp;
        }
        private void exportWord()
        {
            Console.Clear();
            DirectoryInfo d = new DirectoryInfo($"export word {name}");
            if (!d.Exists)
            {
                d.Create();
            }
            Console.Clear();
            Console.WriteLine("Choose word for export:");
            Word w = menuEdit();
            if(w!=null)
            {
                using (FileStream fs2 = new FileStream($"export word {name}/{w.name}.txt", FileMode.OpenOrCreate))
                {
                    string strT = "";
                    strT += w.name;
                    strT += ": ";
                   
                    for (int i = 0; i < w.translateWords.Count; i++)
                    {
                       
                        strT += w.translateWords[i];
                        if (i != w.translateWords.Count - 1)
                        {
                            strT += ", ";
                        }
                       
                    }
                    byte[] buffer = Encoding.Default.GetBytes(strT);
                    fs2.Write(buffer, 0, buffer.Length);
                }
            }
        }
        private void translatePhrase()
        {
            
            string temp = "";
            bool flag = false;
            while(true)
            {
                Console.Clear();
                Console.WriteLine("Input phrase:");
                temp = Console.ReadLine();
                if(temp.Length>0)
                {
                    break;
                }
                else
                {
                    Console.WriteLine("An empty string cannot be translated");
                    flag = Dictionary.repeatOrNo();
                    if (flag)
                    {
                        Console.Clear();
                        continue;
                    }
                    else
                        return;
                }
            }
            string constTemp = temp;
            string translatePh = "";
            string[] tempMas = temp.Split(',', '.', ' ', '?', '!', '%', '$', '#', '*');
            
            for(int i=0; i < tempMas.Length; i++)
            {
                tempMas[i] = Word.formName(tempMas[i]);
            }
            
            List<string> l = new List<string>();
            
            for (int i = 0; i  < tempMas.Length; i++)
            {
                if (tempMas[i].Length > 0)
                {

                    l.Add(tempMas[i]);
                }


            }

            bool flag2 = true;
            int pos = 0;
            for(int i = 0; i < constTemp.Length;i++)
            {
                if (!(Char.IsLetterOrDigit(constTemp[i])))
                {
                    translatePh += constTemp[i];
                    flag2 = true;
                }
                else {
                    if (flag2)
                    {
                        for (int j = 0; j < l[pos].Length; j++)
                        {
                           
                            flag2 = false;
                            Word w = searchByName(l[pos]);
                            if (w != null)
                            {
                                translatePh += getTranslate(w);
                                pos++;
                                break;

                            }
                            else
                            {
                                translatePh += l[pos];
                                pos++;
                                break;
                            }
                            
                        }
                    }
                }
            }
            Console.WriteLine(translatePh);
            Console.ReadKey();

            

        }
        public void translate()
        {

            read();
            while (true)
            {
                Console.Clear();
                int choose = ConsoleMenu.SelectVertical(HPosition.Center,
                                                          VPosition.Center,
                                                          HorizontalAlignment.Left,
                                                          "Translate word", "Export word","Translate phrase", "Exit");
                switch (choose)
                {
                    case 0:
                        translateWord();
                        break;
                    case 1:
                        exportWord();
                        break;
                    case 2:
                        translatePhrase();
                        break;
                    case 3:
                        return;
                    default:
                        return;
                }
            }

        }
        private Word menuEdit()
        {
            
            int choose = ConsoleMenu.SelectVertical(HPosition.Center,
                                                      VPosition.Center,
                                                      HorizontalAlignment.Left,
                                                      "Independent search", "All word", "Exit");
            switch (choose)
            {
                case 0:
                    return search();
                    
                case 1:
                    return chooseWord();
                case 2:
                    return null;
                default:
                    return null;
            }
        }
        private void editWord()
        {
            Console.Clear();
            Console.WriteLine("Choose word for edit:");
            Word w = menuEdit();
            
            if (w != null)
            {
                Word w2 = new Word();
                w2.createName();
                if(!(Words.Contains(w2)))
                {
                    w.name = w2.name;
                    Console.WriteLine($"Word was changed on {w.name}");
                }
                else
                {
                    Console.WriteLine($"Such a word {w2.name} already exist");
                }
            }
            Console.ReadKey();
        }
        private void editTranslate()
        {
            Console.Clear();
            Console.WriteLine("Choose word for edit transleting:");
            Word w = menuEdit();
            if (w != null)
            {
                w.menu();
                if(w.Modify)
                {
                    saveWord(w);
                    w.Modify = false;
                }
            }
        }
        public void menu()
        {
            
            read();

            while (true)
            {
                Console.Clear();
                int choose = ConsoleMenu.SelectVertical(HPosition.Center,
                                                       VPosition.Top,
                                                       HorizontalAlignment.Left,
                                                       "Add word", "Delete word", "Edit word", "Edit translate",  "Exit");
                Word w = new Word();
                switch (choose)
                {
                    case 0:
                        if (w.create())
                        {
                            Word w2 = searchByName(w.name);
                            if (w2 == null)
                            {
                                addWord(w);
                                saveMainFile();
                                saveWord(w);
                            }
                            else
                            {
                                Console.WriteLine($"Such a word {w.name} already exists. To add a translation, select a section in your dictionary 'Edit word'");
                                Console.ReadKey();
                            }   
                        }
                        break;
                    case 1:
                        deleteWord();
                        saveMainFile();
                        break;
                    case 2:
                        editWord();
                        saveMainFile();
                        break;
                    case 3:
                        editTranslate();
                        break;
                    case 4:
                        return;
                    default:
                        break;
                }
            }
        }
        private void outPut()
        {
            Console.Clear();
            Console.WriteLine("Start searching");
            Console.WriteLine("Input word and put ENTER:");
        }
        private int retKeyWord(List<string> l)
        {
            
            l.Add("Continue input");
            l.Add("Exit");

            int choose = ConsoleMenu.SelectVertical(HPosition.Center, VPosition.Top, HorizontalAlignment.Left, l);
            if (choose == l.Count - 1)
            {
                return -1;
            }
            else if (choose == l.Count - 2)
            {
                return -2;
            }
            int i = l.LastIndexOf(l[choose]);
            return i;
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
        public Dictionary()
        {
            name = "";
            Words = new List<Word>();
            index = 0;
            
        }

        public Dictionary(string n)
        {
            name = n;
            Words = new List<Word>();
            index = 0;
        }
       
        
        private Word search()
        {
            bool flag = true;
            while (true)
            {
                outPut();
                string temp = Console.ReadLine();
                temp = temp.ToUpper();
                if (temp.Length > 0)
                {
                    List<string> tempW = new List<string>();
                    foreach (var item in Words)
                    {
                        string tempN = item.name.ToUpper();
                        if (temp.Length <= tempN.Length)
                        {
                            for (int i = 0; i < temp.Length; i++)
                            {
                                if (temp[i] != tempN[i])
                                {
                                    flag = false;
                                    break;
                                }
                                else
                                {
                                    flag = true;
                                }
                            }
                            if(flag)
                            {
                                tempW.Add(item.name);

                            }
                        }
                    }
                    if(tempW.Count == 0)
                        Console.WriteLine("Nothing found");
                    int t = retKeyWord(tempW);
                    if(t == -1)
                    {
                        return null;
                    }
                    else if(t == -2)
                    {
                        continue;
                    }
                    else
                    {
                        return searchByName(tempW[t]);
                    }
                    
                }
            }

        }
        private void deleteFileWord(Word w)
        {
            File.Delete($"{name}/{w.index}.txt");        
        }
        private void deleteWord()
        {
            if (Words.Count > 0)
            {
                Console.Clear();
                Console.WriteLine("Choose word for edit delete:");
                Word w = menuEdit();
                Console.Clear();
                if (w != null)
                {
                    Console.WriteLine($"Delete {w.name}?");
                    if (yesOrNo())
                    {
                        deleteFileWord(w);
                        Words.Remove(w);
                    }
                        
                    
                }
                
            }

        }
    }
}


























//private Word search()
//{

//    ConsoleKeyInfo cki;


//    bool flag = false;
//    string pattern = @"^[a-z,A-Z,а-я,А-Я]+(['-][a-z,A-Z,а-я,А-Я]+)?$";
//    Regex regex = new Regex(pattern);
//    string sW = "";
//    string tempWord = "";
//    bool nF = false;


//    while (true)
//    {
//        cki = Console.ReadKey(true);

//        if (regex.IsMatch(Convert.ToString(cki.KeyChar)))
//        {


//            sW += Convert.ToString(cki.KeyChar);
//            tempWord += Convert.ToString(cki.KeyChar);
//            bringOut(tempWord);
//            sW = sW.ToUpper();
//            nF = false;
//        }
//        else if (cki.Key == ConsoleKey.Backspace)
//        {
//            nF = false;
//            if (sW.Length > 0)
//            {
//                string temp = sW;
//                sW = temp.Substring(0, temp.Length - 1);
//                tempWord = tempWord.Substring(0, temp.Length - 1);
//                bringOut(tempWord);

//            }
//        }
//        else
//        {


//            nF = true;
//        }
//        List<string> tempW = new List<string>();
//        List<int> tempInd = new List<int>();
//        if (sW.Length > 1)
//        {

//            foreach (var item in Words)
//            {
//                string tempN = item.name.ToUpper();
//                if (sW.Length < tempN.Length)
//                {
//                    for (int i = 0; i < sW.Length; i++)
//                    {
//                        if (sW[i] != tempN[i])
//                        {
//                            flag = false;
//                            break;
//                        }
//                        else
//                        {
//                            flag = true;
//                        }
//                    }
//                }
//                if (flag)
//                {
//                    Console.WriteLine();
//                    Console.WriteLine(item.name);

//                    tempW.Add(item.name);

//                }
//            }
//            Console.ReadKey();
//            if (tempW.Count > 0)
//            {
//                Console.WriteLine("To select click ENTER or continue input");
//                cki = Console.ReadKey();
//                if (cki.Key == ConsoleKey.Enter)
//                {
//                    int t = retKeyWord(tempW);
//                    if (t != -1)
//                    {
//                        return searchByName(tempW[t]);                                
//                    }
//                    else
//                    {
//                        nF = true;
//                    }

//                }
//                {
//                    nF = true;
//                }
//            }
//        }
//        if (nF)
//        {
//            Console.Clear();
//            Console.WriteLine("Continue search?");
//            if(yesOrNo())
//            {
//                Console.Clear();
//                outPut();
//                continue;
//            }
//            else
//            {
//                return null;
//            }

//        }

//    }
//}