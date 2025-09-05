using System.Collections.Generic;
using VectorViewer.Models;

namespace VectorViewer.Parsers
{
    public interface IShapeParser
    {
        IEnumerable<ShapeBase> Parse(string content);
    }
}
