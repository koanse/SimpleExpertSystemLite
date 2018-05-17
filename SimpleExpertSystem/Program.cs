using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace SimpleExpertSystem
{
    /// <summary> Делегат для обработки события по обновлению содержимого главной формы </summary>
    public delegate void UpdateListViewsEventHandler(object sender, EventArgs args);
    
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }

    /// <summary> Коды операций </summary>
    public enum Operations { Equals, Not, And, Or };

    /// <summary> Узел дерева условия </summary>
    public class ConditionTreeNode
    {
        public Operations Operation;
        public ArrayList Operands = new ArrayList();
        public bool Result;
    }
    
    /// <summary> Правило </summary>
    public struct Rule
    {
        public ConditionTreeNode Root;
        public string ConsequentObject;
        public string ConsequentValue;
        public string Comment;
    }

    /// <summary> Объект базы знаний </summary>
    public class KBObject
    {
        public string Name;
        public string Value;
    }
    
    /// <summary> База знаний </summary>
    public class KnowledgeBase
    {
        public FileInfo FileInfo;
        private FileStream Stream;
        private StreamReader Reader;
        private string File;
        private int Position;
        public ArrayList KBObjects = new ArrayList();
        public ArrayList Rules = new ArrayList();
        public ArrayList History = new ArrayList();

        public event UpdateListViewsEventHandler UpdateListViews;

        /// <summary> Открытие файла базы знаний </summary>
        public void OpenKB(string Path)
        {
            FileInfo fi = new FileInfo(Path);
            FileStream fs = fi.OpenRead();
            StreamReader r = new StreamReader(fs, Encoding.Default);
            CloseKB();
            FileInfo = fi;
            Stream = fs;
            Reader = r;
            File = Reader.ReadToEnd();
            Position = 0;
            KBObjects.Clear();
            Rules.Clear();
            History.Clear();
        }

        /// <summary> Закрытие файла базы знаний </summary>
        public void CloseKB()
        {
            if (FileInfo == null) return;
            Reader.Close();
            Stream.Close();
            File = "";
            Position = 0;
        }

        /// <summary> Загрузка правил из базы знаний </summary>
        public void LoadRules()
        {
            Rule Rule;
            ConditionTreeNode Root, CurNode, NewNode;
            Stack Stack = new Stack();
            string s;
            while ((s = GetNextLexeme()) != "EndOfFile")
            {
                Rule = new Rule();
                Stack.Clear();

                // Начало правила
                if (s != "ЕСЛИ")
                    throw new Exception("Отсутствует 'ЕСЛИ'");

                // Условие
                Root = CreateConditionTreeNode();
                if (Root.Operation != Operations.Equals)
                    Stack.Push(Root);
                while (Stack.Count > 0)
                {
                    CurNode = (ConditionTreeNode)Stack.Peek();
                    s = GetNextLexeme();
                    if (CurNode.Operands.Count == 0 && s != "(")
                        throw new Exception("Отсутствует '('");
                    if (s == ")")
                    {
                        if ((CurNode.Operation == Operations.Not && (CurNode.Operands.Count < 1)) ||
                            (CurNode.Operation == Operations.And && (CurNode.Operands.Count < 2)) ||
                            (CurNode.Operation == Operations.Or && (CurNode.Operands.Count < 2)))
                            throw new Exception("Недостаточно операндов");
                        CurNode = (ConditionTreeNode)Stack.Pop();
                        continue;
                    }
                    if (CurNode.Operands.Count > 0 && s != ",")
                        throw new Exception("Отсутствует ','");
                    NewNode = CreateConditionTreeNode();
                    CurNode.Operands.Add(NewNode);
                    if (NewNode.Operation != Operations.Equals)
                        Stack.Push(NewNode);
                }

                // Заключение
                s = GetNextLexeme();
                if (s != "ТО")
                    throw new Exception("Отсутствует 'ТО'");
                s = GetNextLexeme();
                if (char.IsLetterOrDigit(s, 0) == false)
                    throw new Exception("Неправильное имя объекта");
                Rule.ConsequentObject = s;
                s = GetNextLexeme();
                if (s != "=")
                    throw new Exception("Отсутствует '='");
                s = GetNextLexeme();
                if (char.IsLetterOrDigit(s, 0) == false)
                    throw new Exception("Неправильное значение объекта");
                Rule.ConsequentValue = s;
                s = GetNextLexeme();
                if (s != ";")
                    throw new Exception("Отсутствует ';'");

                // Комментарии
                s = GetNextLexeme();
                if (s[0] != '[' || s[s.Length - 1] != ']')
                    throw new Exception("Отсутствует '[' или ']'");
                Rule.Comment = s.Substring(1, s.Length - 2);

                // Добавление правила
                Rule.Root = Root;
                Rules.Add(Rule);
            }
        }

        /// <summary> Получение следующей лексемы </summary>
        private string GetNextLexeme()
        {
            int OldPosition;
            while (Position < File.Length &&
                (char.IsSeparator(File, Position) || char.IsControl(File, Position)))
                Position++;
            if (Position == File.Length) return "EndOfFile";
            OldPosition = Position;
            if (char.IsLetterOrDigit(File, Position))
                while (char.IsLetterOrDigit(File, Position)) Position++;
            else
            {
                if (File[Position] == '[')
                    while (File[Position] != ']') Position++;
                Position++;
            }
            return File.Substring(OldPosition, Position - OldPosition);
        }

        /// <summary> Получение кода операции </summary>
        private Operations GetOperationCode(string s)
        {
            if (s == "И") return Operations.And;
            if (s == "ИЛИ") return Operations.Or;
            if (s == "НЕ") return Operations.Not;
            return Operations.Equals;
        }

        /// <summary> Создание узла дерева условия </summary>
        private ConditionTreeNode CreateConditionTreeNode()
        {
            ConditionTreeNode Result = new ConditionTreeNode();
            string s;

            s = GetNextLexeme();
            Result.Operation = GetOperationCode(s);
            if (Result.Operation == Operations.Equals)
            {
                if (char.IsLetterOrDigit(s, 0) == false)
                    throw new Exception("Неправильное имя объекта");
                Result.Operands.Add(s);
                s = GetNextLexeme();
                if (s != "=")
                    throw new Exception("Отсутствует '='");
                s = GetNextLexeme();
                if (char.IsLetterOrDigit(s, 0) == false)
                    throw new Exception("Неправильное значение объекта");
                Result.Operands.Add(s);
            }
            return Result;
        }

        /// <summary> Заполнение списка объектов </summary>
        public void FillKBObjectsList()
        {
            Stack Stack = new Stack();
            ConditionTreeNode Node;
            KBObject Obj;

            for (int i = 0, j; i < Rules.Count; i++)
            {
                // Просмотр условий
                Stack.Clear();
                Stack.Push(((Rule)Rules[i]).Root);
                while (Stack.Count > 0)
                {
                    Node = (ConditionTreeNode)Stack.Pop();
                    if (Node.Operation == Operations.Equals)
                    {
                        for (j = 0; j < KBObjects.Count; j++)
                            if (((KBObject)KBObjects[j]).Name ==
                                (string)Node.Operands[0]) break;
                        if (j == KBObjects.Count)
                        {
                            Obj = new KBObject();
                            Obj.Name = (string)Node.Operands[0];
                            KBObjects.Add(Obj);
                        }
                    }
                    else
                        for (j = 0; j < Node.Operands.Count; j++)
                            Stack.Push(Node.Operands[j]);
                }

                // Просмотр заключений
                for (j = 0; j < KBObjects.Count; j++)
                    if (((KBObject)KBObjects[j]).Name ==
                        ((Rule)Rules[i]).ConsequentObject) break;
                if (j == KBObjects.Count)
                {
                    Obj = new KBObject();
                    Obj.Name = ((Rule)Rules[i]).ConsequentObject;
                    KBObjects.Add(Obj);
                }
            }
            KBObjects.TrimToSize();
        }

        /// <summary> Обнуление значений объектов </summary>
        public void ZeroKBObjects()
        {
            for (int i = 0; i < KBObjects.Count; i++)
                ((KBObject)KBObjects[i]).Value = "Не определено";
        }

        /// <summary> Получение списка допустимых значений </summary>
        public string[] GetLegalValuesList(string ObjectName)
        {
            Stack Stack = new Stack();
            ConditionTreeNode Node;
            ArrayList Result = new ArrayList();
            string obj, val;

            for (int i = 0; i < Rules.Count; i++)
            {
                // Просмотр условий
                Stack.Clear();
                Stack.Push(((Rule)Rules[i]).Root);
                while (Stack.Count > 0)
                {
                    Node = (ConditionTreeNode)Stack.Pop();
                    if (Node.Operation == Operations.Equals)
                    {
                        obj = (string)Node.Operands[0];
                        val = (string)Node.Operands[1];
                        if (obj == ObjectName && Result.Contains(val) == false)
                            Result.Add(val);
                    }
                    else for (int j = 0; j < Node.Operands.Count; j++)
                            Stack.Push(Node.Operands[j]);
                }

                // Просмотр заключений
                obj = ((Rule)Rules[i]).ConsequentObject;
                val = ((Rule)Rules[i]).ConsequentValue;
                if (obj == ObjectName && Result.Contains(val) == false)
                    Result.Add(val);
            }
            Result.Add("Не определено");
            Result.TrimToSize();
            return (string[])Result.ToArray(typeof(string));
        }

        /// <summary> Получение значения объекта </summary>
        private string GetKBObjectValue(string ObjectName)
        {
            for (int i = 0; i < KBObjects.Count; i++)
                if (((KBObject)KBObjects[i]).Name == ObjectName)
                    return ((KBObject)KBObjects[i]).Value;
            throw new Exception("Неправильное имя объекта");
        }

        /// <summary> Установка значения объекта </summary>
        public KBObject SetKBObjectValue(string ObjectName, string Value, int RuleNum)
        {
            for (int i = 0; i < KBObjects.Count; i++)
                if (((KBObject)KBObjects[i]).Name == ObjectName)
                {
                    ((KBObject)KBObjects[i]).Value = Value;
                    string[] tmp = new string[3];
                    tmp[0] = ((KBObject)KBObjects[i]).Name;
                    tmp[1] = ((KBObject)KBObjects[i]).Value;
                    if (++RuleNum > 0) tmp[2] = "Правило номер " + RuleNum.ToString();
                    else tmp[2] = "Инициализация пользователем";
                    History.Add(new ListViewItem(tmp));
                    OnUpdateListViews(null);
                    return (KBObject)KBObjects[i];
                }
            throw new Exception("Неправильное имя объекта");
        }

        /// <summary> Проведение логического вывода </summary>
        public void Analize()
        {
            Queue Queue = new Queue();
            Stack Stack = new Stack();
            ConditionTreeNode Node, Prev = null;
            KBObject KBObject;
            bool Found;

            for (int i = 0; i < KBObjects.Count; i++)
                if (((KBObject)KBObjects[i]).Value != "Не определено")
                    Queue.Enqueue(KBObjects[i]);
            while (Queue.Count > 0)
            {
                KBObject = (KBObject)Queue.Dequeue();
                for (int i = 0; i < Rules.Count; i++)
                {
                    // Поиск вхождения объекта в условие и выполнения подусловия
                    Stack.Clear();
                    Stack.Push(((Rule)Rules[i]).Root);
                    Found = false;
                    while (Stack.Count > 0 && Found == false)
                    {
                        Node = (ConditionTreeNode)Stack.Pop();
                        if (Node.Operation == Operations.Equals)
                        {
                            if ((string)Node.Operands[0] == KBObject.Name)
                                Found = true;
                            else continue;
                        }
                        else for (int j = 0; j < Node.Operands.Count; j++)
                                Stack.Push(Node.Operands[j]);
                    }
                    if (Found == false) continue;

                    // Проверка выполнения условия
                    if (CheckRuleConditionWithoutInitializing((Rule)Rules[i]) == false)
                        continue;
                    Stack.Clear();
                    Stack.Push(((Rule)Rules[i]).Root);
                    while (Stack.Count > 0)
                    {
                        Node = (ConditionTreeNode)Stack.Peek();
                        // Для операции "Равно"
                        if (Node.Operation == Operations.Equals)
                        {
                            // Дополнительная инициализация
                            if (GetKBObjectValue((string)Node.Operands[0]) == "Не определено")
                            {
                                string ObjectName = (string)Node.Operands[0];
                                string[] LegalValues = GetLegalValuesList(ObjectName);
                                string Comment = ((Rule)Rules[i]).Comment;
                                Form2 Form = new Form2(ObjectName, LegalValues, Comment);
                                if (Form.ShowDialog() == DialogResult.OK)
                                    SetKBObjectValue(ObjectName, Form.comboBox1.Text, -1);
                                else throw new Exception("Вывод прерван по желанию пользователя");
                            }
                            if (GetKBObjectValue((string)Node.Operands[0]) == (string)Node.Operands[1])
                                Node.Result = true;
                            else Node.Result = false;
                            Prev = (ConditionTreeNode)Stack.Pop();
                            continue;
                        }

                        // Если просмотрены все дочерние узлы
                        if ((ConditionTreeNode)Node.Operands[Node.Operands.Count - 1] == Prev)
                        {
                            if (Node.Operation == Operations.Not)
                            {
                                Node.Result = true;
                                for (int j = 0; j < Node.Operands.Count; j++)
                                    if (((ConditionTreeNode)Node.Operands[j]).Result == true)
                                    {
                                        Node.Result = false;
                                        break;
                                    }
                            }
                            if (Node.Operation == Operations.And)
                            {
                                Node.Result = true;
                                for (int j = 0; j < Node.Operands.Count; j++)
                                    if (((ConditionTreeNode)Node.Operands[j]).Result == false)
                                    {
                                        Node.Result = false;
                                        break;
                                    }
                            }
                            if (Node.Operation == Operations.Or)
                            {
                                Node.Result = false;
                                for (int j = 0; j < Node.Operands.Count; j++)
                                    if (((ConditionTreeNode)Node.Operands[j]).Result == true)
                                    {
                                        Node.Result = true;
                                        break;
                                    }
                            }
                            Prev = (ConditionTreeNode)Stack.Pop();
                        }
                        else
                        {
                            for (int j = Node.Operands.Count - 1; j >= 0; j--)
                                Stack.Push(Node.Operands[j]);
                            Prev = Node;
                        }
                    }

                    // Если условие выполняется
                    if (((Rule)Rules[i]).Root.Result == true)
                        Queue.Enqueue(SetKBObjectValue(((Rule)Rules[i]).ConsequentObject,
                            ((Rule)Rules[i]).ConsequentValue, i));
                }
            }
        }


        /// <summary> Проверка, не является ли условие заведомо ложным </summary>
        public bool CheckRuleConditionWithoutInitializing(Rule Rule)
        {
            Stack Stack = new Stack();
            ConditionTreeNode Node, Prev = null;
            Stack.Push(Rule.Root);

            while (Stack.Count > 0)
            {
                Node = (ConditionTreeNode)Stack.Peek();
                if (Node.Operation == Operations.Equals)
                {
                    string ObjectName = (string)Node.Operands[0];
                    string Value = (string)Node.Operands[1];
                    
                    if (GetKBObjectValue(ObjectName) != Value)
                        Node.Result = false;
                    else Node.Result = true;
                    Prev = (ConditionTreeNode)Stack.Pop();
                    continue;
                }

                // Если просмотрены все дочерние узлы
                if ((ConditionTreeNode)Node.Operands[Node.Operands.Count - 1] == Prev)
                {
                    if (Node.Operation == Operations.Not)
                    {
                        Node.Result = true;
                        for (int i = 0; i < Node.Operands.Count; i++)
                        {
                            if (((ConditionTreeNode)Node.Operands[i]).Result == true)
                            {
                                if (((ConditionTreeNode)Node.Operands[i]).Operation == Operations.Equals &&
                                    ((ConditionTreeNode)((ConditionTreeNode)Node.Operands[i])).Operands[1] ==
                                    "Не определено")
                                    continue;
                                else Node.Result = false;
                                break;
                            }
                        }
                    }

                    if (Node.Operation == Operations.And)
                    {
                        Node.Result = true;
                        for (int i = 0; i < Node.Operands.Count; i++)
                        {
                            if (((ConditionTreeNode)Node.Operands[i]).Result == false)
                            {
                                ConditionTreeNode CurNode = (ConditionTreeNode)Node.Operands[i];
                                if (CurNode.Operation == Operations.Equals &&
                                    GetKBObjectValue((string)CurNode.Operands[0]) == "Не определено")
                                    continue;
                                else Node.Result = false;
                                break;
                            }
                        }
                    }

                    if (Node.Operation == Operations.Or)
                    {
                        Node.Result = false;
                        for (int i = 0; i < Node.Operands.Count; i++)
                        {
                            if (((ConditionTreeNode)Node.Operands[i]).Result == true)
                            {
                                if (((ConditionTreeNode)Node.Operands[i]).Operation == Operations.Equals &&
                                    ((ConditionTreeNode)((ConditionTreeNode)Node.Operands[i])).Operands[1] ==
                                    "Не определено")
                                    continue;
                                else Node.Result = true;
                                break;
                            }
                        }
                    }
                    Prev = (ConditionTreeNode)Stack.Pop();
                    continue;
                }
                for (int j = Node.Operands.Count - 1; j >= 0; j--)
                    Stack.Push(Node.Operands[j]);
                Prev = Node;
            }
            return Rule.Root.Result;
        }

        /// <summary> Включение события </summary>
        public void OnUpdateListViews(EventArgs args)
        {
            if(UpdateListViews != null)
                UpdateListViews(this, args);
        }

        /// <summary> Проведение обратного логического вывода </summary>
        public void InvertedAnalize()
        {
            Stack Stack = new Stack();
            Queue Queue = new Queue();
            ConditionTreeNode Node, Prev = null;
            KBObject KBObject;
            bool Found;

            for (int i = 0; i < KBObjects.Count; i++)
                if (((KBObject)KBObjects[i]).Value != "Не определено")
                    Queue.Enqueue(KBObjects[i]);
            while (Queue.Count > 0)
            {
                KBObject = (KBObject)Queue.Dequeue();
                for (int i = Rules.Count - 1; i >= 0; i--)
                {
                    // Поиск вхождения объекта в часть "ТО" условия
                    if (KBObject.Name != ((Rule)Rules[i]).ConsequentObject ||
                        KBObject.Value != ((Rule)Rules[i]).ConsequentValue)
                        continue;

                    Stack.Clear();
                    Stack.Push(((Rule)Rules[i]).Root);
                    while (Stack.Count > 0)
                    {
                        Node = (ConditionTreeNode)Stack.Pop();
                        if (Node.Operation == Operations.Equals)
                        {
                            if ((string)Node.Operands[1] != "Не определено")
                            {
                                SetKBObjectValue((string)Node.Operands[0],
                                    (string)Node.Operands[1], i);
                                
                                KBObject kbo = new KBObject();
                                kbo.Name = (string)Node.Operands[0];
                                kbo.Value = (string)Node.Operands[1];
                                if(Queue.Contains(kbo) == false) Queue.Enqueue(kbo);                                
                            }
                            else
                            {
                                Found = false;
                                foreach (Rule r in Rules)
                                    if (r.ConsequentObject == (string)Node.Operands[0])
                                    {
                                        Found = true;
                                        break;
                                    }
                                if (Found == false)
                                {
                                    // Дополнительная инициализация
                                    string ObjectName = (string)Node.Operands[0];
                                    string[] LegalValues = GetLegalValuesList(ObjectName);
                                    string Comment = ((Rule)Rules[i]).Comment;
                                    Form2 Form = new Form2(ObjectName, LegalValues, Comment);
                                    if (Form.ShowDialog() == DialogResult.OK)
                                    {
                                        SetKBObjectValue(ObjectName, Form.comboBox1.Text, -1);
                                        KBObject kbo = new KBObject();
                                        kbo.Name = ObjectName;
                                        kbo.Value = Form.comboBox1.Text;
                                        if (Queue.Contains(kbo) == false) Queue.Enqueue(kbo);
                                    }
                                    else throw new Exception("Вывод прерван по желанию пользователя");
                                }
                            }
                        }
                        else for (int j = 0; j < Node.Operands.Count; j++)
                                Stack.Push(Node.Operands[j]);
                    }
                    continue;
                    /*if (Found == false) continue;

                    // Проверка выполнения условия
                    if (CheckRuleConditionWithoutInitializing((Rule)Rules[i]) == false)
                        continue;
                    Stack.Clear();
                    Stack.Push(((Rule)Rules[i]).Root);
                    while (Stack.Count > 0)
                    {
                        Node = (ConditionTreeNode)Stack.Peek();
                        // Для операции "Равно"
                        if (Node.Operation == Operations.Equals)
                        {
                            // Дополнительная инициализация
                            if (GetKBObjectValue((string)Node.Operands[0]) == "Не определено")
                            {
                                string ObjectName = (string)Node.Operands[0];
                                string[] LegalValues = GetLegalValuesList(ObjectName);
                                string Comment = ((Rule)Rules[i]).Comment;
                                Form2 Form = new Form2(ObjectName, LegalValues, Comment);
                                if (Form.ShowDialog() == DialogResult.OK)
                                    SetKBObjectValue(ObjectName, Form.comboBox1.Text, -1);
                                else throw new Exception("Вывод прерван по желанию пользователя");
                            }
                            if (GetKBObjectValue((string)Node.Operands[0]) == (string)Node.Operands[1])
                                Node.Result = true;
                            else Node.Result = false;
                            Prev = (ConditionTreeNode)Stack.Pop();
                            continue;
                        }

                        // Если просмотрены все дочерние узлы
                        if ((ConditionTreeNode)Node.Operands[Node.Operands.Count - 1] == Prev)
                        {
                            if (Node.Operation == Operations.Not)
                            {
                                Node.Result = true;
                                for (int j = 0; j < Node.Operands.Count; j++)
                                    if (((ConditionTreeNode)Node.Operands[j]).Result == true)
                                    {
                                        Node.Result = false;
                                        break;
                                    }
                            }
                            if (Node.Operation == Operations.And)
                            {
                                Node.Result = true;
                                for (int j = 0; j < Node.Operands.Count; j++)
                                    if (((ConditionTreeNode)Node.Operands[j]).Result == false)
                                    {
                                        Node.Result = false;
                                        break;
                                    }
                            }
                            if (Node.Operation == Operations.Or)
                            {
                                Node.Result = false;
                                for (int j = 0; j < Node.Operands.Count; j++)
                                    if (((ConditionTreeNode)Node.Operands[j]).Result == true)
                                    {
                                        Node.Result = true;
                                        break;
                                    }
                            }
                            Prev = (ConditionTreeNode)Stack.Pop();
                        }
                        else
                        {
                            for (int j = Node.Operands.Count - 1; j >= 0; j--)
                                Stack.Push(Node.Operands[j]);
                            Prev = Node;
                        }
                    }

                    // Если условие выполняется
                    if (((Rule)Rules[i]).Root.Result == true)
                        Queue.Enqueue(SetKBObjectValue(((Rule)Rules[i]).ConsequentObject,
                            ((Rule)Rules[i]).ConsequentValue, i));*/
                }
            }
        }
    }
}