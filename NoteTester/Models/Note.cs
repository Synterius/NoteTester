using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace NoteTester.Models
{
    public class Note
    {
        public Brush Color { get; set; }
        public string Name { get; set; }
        public double Left { get; set; }
        public double Top { get; set; }
        public bool Status { get; set; }
    }
}
