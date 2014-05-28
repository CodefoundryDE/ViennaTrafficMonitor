using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace VtmFramework.Library {
    public class VtmRectangle {

        public Point TopRight { get; set; }
        public Point BottomLeft { get; set; }

        public VtmRectangle(Point topRight, Point bottomLeft) {
            this.TopRight = topRight;
            this.BottomLeft = bottomLeft;
        }

    }
}
