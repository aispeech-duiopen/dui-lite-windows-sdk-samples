using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ASRDemo.Model.DUI
{
    public class MOut : ModelBase
    {
        private bool _IsEnabled;
        public bool IsEnabled
        {
            get
            {
                return _IsEnabled;
            }
            set
            {
                _IsEnabled = value;
                NotificationPropertyChanged("IsEnabled");
            }
        }

        private string _ARSRealTimeOut;
        public string ARSRealTimeOut
        {
            get
            {
                return _ARSRealTimeOut;
            }
            set
            {
                _ARSRealTimeOut = value;
                NotificationPropertyChanged("ARSRealTimeOut");
            }
        }
    }
}
