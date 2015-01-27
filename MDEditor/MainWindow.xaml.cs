using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MDEditor
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public static string StartParam = "";
        public MainWindow()
        {
            InitializeComponent();

            Title += " Ver " + Application.ResourceAssembly.GetName().Version;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(StartParam))
            {
                OpenFile(StartParam);
            }
            else
            {
                NewBtn_Click(sender, e);
            }
        }

        void OpenFile(string path)
        {
            if (MainTab.Items.Count == 0) NewBtn_Click(this, null);
            var tb = MainTab.SelectedItem as TabItem;
            tb.Tag = path;
            (tb.Header as TabHeaderUC).SetTitle(path.Split(new[] { '\\' }).Last());
            (tb.Content as MDEditorUC).MarkDownText = File.ReadAllText(path);
        }

        private void NewBtn_Click(object sender, RoutedEventArgs e)
        {
            var header = new TabHeaderUC("新建文档");
            header.OnClose += Header_OnClose;
            MainTab.Items.Add(new TabItem
            {
                Header = header,
                Content = new MDEditorUC(),
            });
            MainTab.SelectedIndex = MainTab.Items.Count - 1;
        }

        private void Header_OnClose(object sender, string e)
        {
            var header = sender as TabHeaderUC;
            MainTab.Items.Remove(header.Parent);
        }

        private void OpenBtn_Click(object sender, RoutedEventArgs e)
        {
            var ofd = new System.Windows.Forms.OpenFileDialog
            {
                CheckFileExists = true,
                Filter = "Markdown(*.md)|*.md|所有文件(*.*)|*.*",
            };
            System.Windows.Forms.DialogResult dr = ofd.ShowDialog();
            if (dr == System.Windows.Forms.DialogResult.OK)
            {
                OpenFile(ofd.FileName);
            }
        }

        private void SaveBtn_Click(object sender, RoutedEventArgs e)
        {
            if (MainTab.Items.Count == 0) return;
            var tb = MainTab.SelectedItem as TabItem;
            if (tb.Tag == null)
            {
                var ofd = new System.Windows.Forms.SaveFileDialog
                {
                    CheckFileExists = false,
                    Filter = "Markdown(*.md)|*.md|所有文件(*.*)|*.*",
                };
                System.Windows.Forms.DialogResult dr = ofd.ShowDialog();
                if (dr != System.Windows.Forms.DialogResult.OK) return;
                tb.Tag = ofd.FileName;
                (tb.Header as TabHeaderUC).SetTitle(ofd.FileName.Split(new[] { '\\' }).Last());
            }
            File.WriteAllText(tb.Tag.ToString(), (tb.Content as MDEditorUC).MarkDownText);
            MessageBox.Show("已保存！" + tb.Tag.ToString());
        }

        private void TemplateBtn_Click(object sender, RoutedEventArgs e)
        {
            MarkdownProcessor.ExportTemplate();
            MessageBox.Show("已创建！请到程序目录下查看，程序将会优先使用外部资源文件。\n模板立即生效，markjs和配置文件重启生效。");
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            var mi = sender as MenuItem;
            switch (mi.Tag.ToString())
            {
                case "New":
                    NewBtn_Click(sender, e);
                    break;

                case "Open":
                    OpenBtn_Click(sender, e);
                    break;

                case "Save":
                    SaveBtn_Click(sender, e);
                    break;

                case "Close":
                    MainTab.Items.Remove(MainTab.SelectedItem);
                    break;

                case "CloseAll":
                    MainTab.Items.Clear();
                    break;

                case "HomePage":
                    System.Diagnostics.Process.Start("http://zhang740.github.io/MDEditor/");
                    break;

                case "GitHub":
                    System.Diagnostics.Process.Start("https://github.com/zhang740/MDEditor");
                    break;

                case "Quit":
                    Application.Current.Shutdown();
                    break;
            }
        }
    }
}
