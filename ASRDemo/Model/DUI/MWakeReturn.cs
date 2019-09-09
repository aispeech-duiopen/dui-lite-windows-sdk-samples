using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ASRDemo.Model.DUI
{

    public class MWakeReturn
    {
        public string version { get; set; }
        public string lib_version { get; set; }
        public int status { get; set; }
        public string wakeupWord { get; set; }
        public double major { get; set; }
        public double confidence { get; set; }
        public int frame { get; set; }
        public int wakeup_frame { get; set; }
    }
}
