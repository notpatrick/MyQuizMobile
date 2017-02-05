using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace MyQuizMobile
{
    public class CustomEditor : Editor
    {
        public CustomEditor()
        {
            this.TextChanged += (sender, e) => { this.InvalidateMeasure(); };
        }
    }
}
