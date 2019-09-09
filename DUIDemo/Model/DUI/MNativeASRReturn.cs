using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DUIDemo.Model.DUI
{

    public class MNativeASRReturn
    {
        public Gram grammar { get; set; }
        public Ngram ngram { get; set; }
    }

    public class Ngram
    {
        public string rec { get; set; }
        public double conf { get; set; }
    }

    public class Gram
    {
        public string rec { get; set; }

        public double conf { get; set; }
    }
}
