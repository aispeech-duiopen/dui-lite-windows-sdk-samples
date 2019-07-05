using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DUIDemo.Model
{
    public class MVoice
    {
        private int _ID;
        public int ID
        {
            get
            {
                return _ID;
            }
            set
            {
                _ID = value;
            }
        }

        private string _Speaker;
        public string Speaker
        {
            get
            {
                return _Speaker;
            }
            set
            {
                _Speaker = value;
            }
        }

        private string _ResName;
        public string ResName
        {
            get
            {
                return _ResName;
            }
            set
            {
                _ResName = value;
            }
        }

        private bool _IsLoacl;
        public bool IsLoacl
        {
            get
            {
                return _IsLoacl;
            }
            set
            {
                _IsLoacl = value;
            }
        }
    }
}
