using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace MyQuizMobile
{
	public partial class QuestionBlockAddQuestionPage : ContentPage {
	    public QuestionBlockAddQuestionViewModel QuestionBlockAddQuestionViewModel;
		public QuestionBlockAddQuestionPage ()
		{
			InitializeComponent ();
            QuestionBlockAddQuestionViewModel = new QuestionBlockAddQuestionViewModel();
            BindingContext = QuestionBlockAddQuestionViewModel;
        }
    }
}
