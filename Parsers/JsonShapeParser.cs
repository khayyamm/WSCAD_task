using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.Json;
using System.Windows;
using System.Windows.Media;
using VectorViewer.Models;

namespace VectorViewer.Parsers
{
    public class JsonShapeParser : IShapeParser
    {
        public IEnumerable<ShapeBase> Parse(string content)
        {
            using var doc = JsonDocument.Parse(content);
            foreach (var element in doc.RootElement.EnumerateArray())
            {
                var type = element.GetProperty("type").GetString()?.ToLower();
                var color = ParseColor(element.GetProperty("color").GetString());
                var filled = element.TryGetProperty("filled", out var f) && f.GetBoolean();

                switch (type)
                {
                    case "line":
                        yield return new LineShape
                        {
                            Start = ParsePoint(element.GetProperty("a").GetString()),
                            End = ParsePoint(element.GetProperty("b").GetString()),
                            Color = color,
                            Filled = false
                        };
                        break;

                    case "circle":
                        yield return new CircleShape
                        {
                            Center = ParsePoint(element.GetProperty("center").GetString()),
                            Radius = element.GetProperty("radius").GetDouble(),
                            Color = color,
                            Filled = filled
                        };
                        break;

                    case "triangle":
                        yield return new TriangleShape
                        {
                            P1 = ParsePoint(element.GetProperty("a").GetString()),
                            P2 = ParsePoint(element.GetProperty("b").GetString()),
                            P3 = ParsePoint(element.GetProperty("c").GetString()),
                            Color = color,
                            Filled = filled
                        };
                        break;
                }
            }
        }

        private static Point ParsePoint(string raw)
        {
            var parts = raw.Split(';');
            var x = double.Parse(parts[0].Replace(',', '.'));
            var y = double.Parse(parts[1].Replace(',', '.'));
            return new Point(x, y);
        }

        private static Color ParseColor(string raw)
        {
            var parts = raw.Split(';');
            return Color.FromArgb(
                byte.Parse(parts[0]),
                byte.Parse(parts[1]),
                byte.Parse(parts[2]),
                byte.Parse(parts[3]));
        }
    }
}
