using System.Windows;
using System.Windows.Media;

namespace VectorViewer.Models
{
    public class LineShape : ShapeBase
    {
        public Point Start { get; set; }
        public Point End { get; set; }
    }
}
