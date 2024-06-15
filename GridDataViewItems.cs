using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace DesktopSetupV2
{
    internal class GridDataViewItems
    {
        public string description { get; set; }

        public string commandLine { get; set; }
        //public bool commandLineIni { get; set; }

        public string arguments { get; set; }
        public bool argumentsIni { get; set; }

        public int width { get; set; }
        //public bool widthIni { get; set; }

        public int height { get; set; }
        //public bool heightIni { get; set; }

        public int xLoc { get; set; }

        public int yLoc { get; set; }

        public int moveAfter { get; set; }
        //public bool moveAfterIni { get; set; }

        public bool defaultSize { get; set; }
        //public bool defaultSizeIni { get; set; }

        public int id { get; set; }

        public string ip { get; set; }

        public int port {get; set; }
        //public bool portIni { get; set; }

        public int FailureCount { get; set; } = 0;

        //public Process ProcessPointer;
        public Process ProcessPointer { get; set; }
    }
}
