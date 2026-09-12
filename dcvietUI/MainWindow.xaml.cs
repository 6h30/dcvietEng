using System.Windows;

namespace dcvietUI
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void ImportData_Click(
            object sender,
            RoutedEventArgs e)
        {
            MessageBox.Show(
                "Chức năng nhập dữ liệu đang được phát triển.",
                "DCVIET");
        }

        private void ExtractForces_Click(
            object sender,
            RoutedEventArgs e)
        {
            MessageBox.Show(
                "Chức năng trích nội lực đang được phát triển.",
                "DCVIET");
        }

        private void Calculate_Click(
            object sender,
            RoutedEventArgs e)
        {
            MessageBox.Show(
                "Chức năng tính toán đang được phát triển.",
                "DCVIET");
        }

        private void ExportExcel_Click(
            object sender,
            RoutedEventArgs e)
        {
            MessageBox.Show(
                "Chức năng xuất Excel đang được phát triển.",
                "DCVIET");
        }
    }
}