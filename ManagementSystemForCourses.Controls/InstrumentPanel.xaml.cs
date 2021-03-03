using System;
using System.Collections.Generic;
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

namespace ManagementSystemForCourses.Controls
{
    /// <summary>
    /// Interaction logic for InstrumentPanel.xaml
    /// </summary>
    public partial class InstrumentPanel : UserControl
    {
        public InstrumentPanel()
        {
            InitializeComponent();

           this.SizeChanged += InstrumentPanel_SizeChanged;
        }

        private void InstrumentPanel_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            double minSize = Math.Min(this.RenderSize.Width, this.RenderSize.Height);
            this.backEllipse.Width = minSize;
            this.backEllipse.Height = minSize;
        }

    }
}
