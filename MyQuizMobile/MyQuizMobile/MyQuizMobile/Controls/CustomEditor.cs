using Xamarin.Forms;

namespace MyQuizMobile {
    public class CustomEditor : Editor {
        public CustomEditor() { TextChanged += (sender, e) => { InvalidateMeasure(); }; }
    }
}