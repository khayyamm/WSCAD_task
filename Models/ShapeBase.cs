using System.Windows;
using System.Windows.Media;

namespace VectorViewer.Models
{
    public abstract class ShapeBase
    {
        public Color Color { get; set; }
        public bool Filled { get; set; }
    }
}
