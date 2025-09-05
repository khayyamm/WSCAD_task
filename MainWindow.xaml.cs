using Microsoft.Win32;
using System.IO;
using System.Linq;
using System.Windows;
using VectorViewer.Parsers;

namespace VectorViewer_Personalized
{
    public partial class MainWindow : Window
    {
        private readonly IShapeParser _parser = new JsonShapeParser();

        public MainWindow()
        {
            InitializeComponent();
            BtnLoadJson.Click += BtnLoad_Click;

            var defaultPath = Path.Combine(Directory.GetCurrentDirectory(), "input.json");
            if (File.Exists(defaultPath))
            {
                LoadShapesFromFile(defaultPath);
            }
        }

        private void BtnLoad_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new OpenFileDialog { Filter = "JSON files|*.json|All files|*.*" };
            if (dlg.ShowDialog() == true)
            {
                LoadShapesFromFile(dlg.FileName);
            }
        }

        private void LoadShapesFromFile(string path)
        {
            try
            {
                var jsonContent = File.ReadAllText(path);
                var shapes = _parser.Parse(jsonContent).ToList();
                DrawingCanvas.LoadShapes(shapes);
                LblCurrentFile.Text = System.IO.Path.GetFileName(path);
            }
            catch (System.Exception ex)
            {
                // TODO: log error if needed
                MessageBox.Show(this, $"Failed to load shapes: {ex.Message}", "Error",
                                MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
