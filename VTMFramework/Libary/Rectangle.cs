using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace VtmFramework.Libary {
    public class Rectangle {

        public Point TopRight { get; set; }
        public Point BottomLeft { get; set; }

        public Rectangle(Point topRight, Point bottomLeft) {
            this.TopRight = topRight;
            this.BottomLeft = bottomLeft;
        }

    }
}
