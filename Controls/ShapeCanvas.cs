using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using VectorViewer.Models;

namespace VectorViewer.Controls
{
    public class ShapeCanvas : Canvas
    {
        private readonly List<ShapeBase> _shapes = new();

        public void LoadShapes(IEnumerable<ShapeBase> shapes)
        {
            _shapes.Clear();
            _shapes.AddRange(shapes);
            InvalidateVisual();
        }

        protected override void OnRender(DrawingContext dc)
        {
            base.OnRender(dc);
            if (_shapes.Count == 0) return;

            var bounds = GetBounds();
            var scale = CalculateScale(bounds);
            var offset = new Point(ActualWidth / 2, ActualHeight / 2);

            foreach (var shape in _shapes)
            {
                var pen = new Pen(new SolidColorBrush(shape.Color), 1);
                switch (shape)
                {
                    case LineShape line:
                        dc.DrawLine(pen,
                            Transform(line.Start, scale, offset),
                            Transform(line.End, scale, offset));
                        break;

                    case CircleShape circle:
                        var center = Transform(circle.Center, scale, offset);
                        var radius = circle.Radius * scale;
                        var brush = circle.Filled ? new SolidColorBrush(circle.Color) : null;
                        dc.DrawEllipse(brush, pen, center, radius, radius);
                        break;

                    case TriangleShape tri:
                        var p1 = Transform(tri.P1, scale, offset);
                        var p2 = Transform(tri.P2, scale, offset);
                        var p3 = Transform(tri.P3, scale, offset);
                        var geo = new StreamGeometry();
                        using (var ctx = geo.Open())
                        {
                            ctx.BeginFigure(p1, tri.Filled, true);
                            ctx.LineTo(p2, true, false);
                            ctx.LineTo(p3, true, false);
                        }
                        dc.DrawGeometry(tri.Filled ? new SolidColorBrush(tri.Color) : null, pen, geo);
                        break;
                }
            }
        }

        private Rect GetBounds()
        {
            var points = new List<Point>();
            foreach (var s in _shapes)
            {
                switch (s)
                {
                    case LineShape l: points.Add(l.Start); points.Add(l.End); break;
                    case CircleShape c:
                        points.Add(new Point(c.Center.X - c.Radius, c.Center.Y - c.Radius));
                        points.Add(new Point(c.Center.X + c.Radius, c.Center.Y + c.Radius)); break;
                    case TriangleShape t: points.Add(t.P1); points.Add(t.P2); points.Add(t.P3); break;
                }
            }
            return new Rect(points.Min(p => p.X), points.Min(p => p.Y),
                            points.Max(p => p.X) - points.Min(p => p.X),
                            points.Max(p => p.Y) - points.Min(p => p.Y));
        }

        private double CalculateScale(Rect bounds)
        {
            if (bounds.Width == 0 || bounds.Height == 0) return 1;
            var scaleX = ActualWidth / bounds.Width;
            var scaleY = ActualHeight / bounds.Height;
            return 0.9 * System.Math.Min(scaleX, scaleY);
        }

        private Point Transform(Point p, double scale, Point offset)
        {
            // Note: invert Y (Cartesian vs screen coords)
            return new Point(p.X * scale + offset.X, -p.Y * scale + offset.Y);
        }
    }
}
