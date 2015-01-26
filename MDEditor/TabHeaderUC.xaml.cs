using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
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
    /// TabHeaderUC.xaml 的交互逻辑
    /// </summary>
    public partial class TabHeaderUC : UserControl, INotifyPropertyChanged
    {
        public string Title { get; private set; }
        public event EventHandler<string> OnClose;
        public event PropertyChangedEventHandler PropertyChanged;

        public TabHeaderUC(string title)
        {
            InitializeComponent();

            Title = title;
            DataContext = this;
        }

        private void CloseBtn_Click(object sender, RoutedEventArgs e)
        {
            if (OnClose != null) OnClose(this, Title);
        }

        public void SetTitle(string title)
        {
            Title = title;
            NotifyPropertyChanged("Title");
        }

        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
