using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
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
    /// MDEditorUC.xaml 的交互逻辑
    /// </summary>
    public partial class MDEditorUC : UserControl
    {
        private static MarkdownProcessor mProcessor = new MarkdownProcessor();
        DateTime LastChangeTime;
        Timer UpdateTimer;

        public MDEditorUC()
        {
            InitializeComponent();

            UpdateTimer = new Timer
            {
                Interval = 500,
            };
            UpdateTimer.Elapsed += UpdateTimer_Elapsed;
        }

        public string MarkDownText
        {
            get
            {
                return MD.Text;
            }

            set
            {
                MD.Text = value;
            }
        }

        private void UpdateTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (DateTime.Now < LastChangeTime.AddSeconds(0.5)) return;
            UpdateTimer.Stop();

            Dispatcher.Invoke(() =>
            {
                CodeView.Text = mProcessor.ConvertMarkdownToHTML(MD.Text, false).Replace("\n\n", "\n");
                if (!string.IsNullOrWhiteSpace(CodeView.Text)) WebView.NavigateToString(mProcessor.UseTemplate(CodeView.Text));
            });
        }

        private void MD_TextChanged(object sender, TextChangedEventArgs e)
        {
            LastChangeTime = DateTime.Now;
            if (!UpdateTimer.Enabled) UpdateTimer.Start();
        }
    }
}
