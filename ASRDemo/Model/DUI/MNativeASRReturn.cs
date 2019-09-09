using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ASRDemo.Model.DUI
{

    public class MNativeASRReturn
    {
        public Ngram ngram { get; set; }
    }

    public class Ngram
    {
        public string rec { get; set; }
        public string pinyin { get; set; }
    }
}
