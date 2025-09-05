using System.Linq;
using System.Windows;
using System.Windows.Media;

namespace VectorViewer.Models
{
    public class TriangleShape : ShapeBase
    {
        public Point P1 { get; set; }
        public Point P2 { get; set; }
        public Point P3 { get; set; }
    }
}
