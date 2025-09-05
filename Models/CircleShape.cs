using System.Windows;
using System.Windows.Media;

namespace VectorViewer.Models
{
    public class CircleShape : ShapeBase
    {
        public Point Center { get; set; }
        public double Radius { get; set; }
    }
}
