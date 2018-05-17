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
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        public KnowledgeBase KnowledgeBase = new KnowledgeBase();

        private void LoadKBToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog dlg = new OpenFileDialog();
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    KnowledgeBase.OpenKB(dlg.FileName);
                    KnowledgeBase.LoadRules();
                    KnowledgeBase.FillKBObjectsList();
                    FillKBListView();
                    KnowledgeBase.CloseKB();
                    KnowledgeBase.ZeroKBObjects();
                    FillObjectsListView();
                    HistoryListView.Items.Clear();
                    KnowledgeBase.UpdateListViews += new UpdateListViewsEventHandler(DoUpdatesInListViews);
                }
            }
            catch (Exception exc)
            {
                int n = KnowledgeBase.Rules.Count + 1;
                MessageBox.Show("Ошибка загрузки базы знаний: " + exc.Message + 
                    ", номер правила " + n.ToString(), "Загрузка базы знаний не выполнена");
                KnowledgeBase.CloseKB();
            }
        }

        private void ExitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void StartToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                Form3 SelectObjectForm = new Form3(KnowledgeBase.KBObjects);
                if (SelectObjectForm.ShowDialog() == DialogResult.OK)
                {
                    string ObjectName = SelectObjectForm.comboBox1.Text;
                    string[] LegalValues = KnowledgeBase.GetLegalValuesList(ObjectName);
                    string Comment = "Задайте начальное состояние, проинициализировав выбранный объект.";
                    Form2 SetValueForm = new Form2(ObjectName, LegalValues, Comment);
                    if (SetValueForm.ShowDialog() == DialogResult.Cancel)
                        throw new Exception("Вывод прерван по желанию пользователя");
                    
                    KnowledgeBase.ZeroKBObjects();
                    HistoryListView.Items.Clear();
                    KnowledgeBase.History.Clear();
                    DoUpdatesInListViews(this, null);

                    string Value = SetValueForm.comboBox1.Text;
                    KnowledgeBase.SetKBObjectValue(ObjectName, Value, -1);
                    DoUpdatesInListViews(this, null);
                    KnowledgeBase.Analize();
                    DoUpdatesInListViews(this, null);
                    MessageBox.Show("Логический вывод успешно выполнен", "Логический вывод выполнен");
                }
            }
            catch (Exception exc)
            {
                MessageBox.Show("Ошибка логического вывода: " + exc.Message,
                   "Логический вывод прерван");
            }
        }

        private void FillObjectsListView()
        {
            try
            {
                ObjectsListView.Items.Clear();
                ObjectsListView.BeginUpdate();
                for (int i = 0; i < KnowledgeBase.KBObjects.Count; i++)
                {
                    string[] tmp = new string[2];
                    tmp[0] = ((KBObject)KnowledgeBase.KBObjects[i]).Name;
                    tmp[1] = ((KBObject)KnowledgeBase.KBObjects[i]).Value;
                    ListViewItem lvi = new ListViewItem(tmp);
                    ObjectsListView.Items.Add(lvi);
                }
                ObjectsListView.EndUpdate();
            }
            catch (Exception exc)
            {
                MessageBox.Show("Ошибка построения списка объектов: " + exc.Message,
                    "Построение списка объектов прервано");
            }
        }

        private void FillKBListView()
        {
            try
            {
                KBListView.Items.Clear();
                KBListView.BeginUpdate();
                string[] tmp = new string[2];
                
                tmp[0] = "Имя";
                tmp[1] = KnowledgeBase.FileInfo.Name;
                KBListView.Items.Add(new ListViewItem(tmp));

                tmp[0] = "Тип";
                tmp[1] = KnowledgeBase.FileInfo.Extension;
                KBListView.Items.Add(new ListViewItem(tmp));

                tmp[0] = "Расположение";
                tmp[1] = KnowledgeBase.FileInfo.DirectoryName;
                KBListView.Items.Add(new ListViewItem(tmp));
                
                tmp[0] = "Размер (в байтах)";
                tmp[1] = KnowledgeBase.FileInfo.Length.ToString();
                KBListView.Items.Add(new ListViewItem(tmp));

                tmp[0] = "Дата создания";
                tmp[1] = KnowledgeBase.FileInfo.CreationTime.ToString();
                KBListView.Items.Add(new ListViewItem(tmp));
                
                tmp[0] = "Дата обновления";
                tmp[1] = KnowledgeBase.FileInfo.LastWriteTime.ToString();
                KBListView.Items.Add(new ListViewItem(tmp));

                tmp[0] = "Количество правил";
                tmp[1] = KnowledgeBase.Rules.Count.ToString();
                KBListView.Items.Add(new ListViewItem(tmp));

                tmp[0] = "Количество объектов";
                tmp[1] = KnowledgeBase.KBObjects.Count.ToString();
                KBListView.Items.Add(new ListViewItem(tmp));
                KBListView.EndUpdate();
            }
            catch (Exception exc)
            {
                MessageBox.Show("Ошибка построения списка сведений о базе знаний: " + exc.Message,
                    "Построение списка сведений о базе знаний прервано");
            }
        }

        private void DoUpdatesInListViews(object sender, EventArgs args)
        {
            try
            {
                FillObjectsListView();
                if (KnowledgeBase.History.Count != 0)
                    HistoryListView.Items.AddRange((ListViewItem[])
                        KnowledgeBase.History.ToArray(typeof(ListViewItem)));
                KnowledgeBase.History.Clear();
            }
            catch (Exception exc)
            {
                MessageBox.Show("Ошибка обновления истории вывода и значений объектов: " + exc.Message,
                    "Обновление истории вывода и значений объектов прервано");
            }
        }

        private void InvertedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                Form3 SelectObjectForm = new Form3(KnowledgeBase.KBObjects);
                if (SelectObjectForm.ShowDialog() == DialogResult.OK)
                {
                    string ObjectName = SelectObjectForm.comboBox1.Text;
                    string[] LegalValues = KnowledgeBase.GetLegalValuesList(ObjectName);
                    string Comment = "Задайте конечное состояние, проинициализировав выбранный объект.";
                    Form2 SetValueForm = new Form2(ObjectName, LegalValues, Comment);
                    if (SetValueForm.ShowDialog() == DialogResult.Cancel)
                        throw new Exception("Вывод прерван по желанию пользователя");

                    KnowledgeBase.ZeroKBObjects();
                    HistoryListView.Items.Clear();
                    KnowledgeBase.History.Clear();
                    DoUpdatesInListViews(this, null);

                    string Value = SetValueForm.comboBox1.Text;
                    KnowledgeBase.SetKBObjectValue(ObjectName, Value, -1);
                    DoUpdatesInListViews(this, null);
                    KnowledgeBase.InvertedAnalize();
                    DoUpdatesInListViews(this, null);
                    MessageBox.Show("Логический вывод успешно выполнен", "Логический вывод выполнен");
                }
            }
            catch (Exception exc)
            {
                MessageBox.Show("Ошибка логического вывода: " + exc.Message,
                   "Логический вывод прерван");
            }
        }
    }
}