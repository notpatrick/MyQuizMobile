using System;
using System.Windows.Input;
using MyQuizMobile.Converters;
using Xamarin.Forms;

namespace MyQuizMobile {
    public class CustomCell : ViewCell {
        protected override void OnTapped() {
            //base.OnTapped();

            if (Command == null) {
                return;
            }
            if (Command.CanExecute(this)) {
                Command.Execute(BindingContext);
            }
        }

        #region Command
        public static readonly BindableProperty CommandProperty = BindableProperty.Create<CustomCell, ICommand>(p => p.Command, default(ICommand), BindingMode.OneWay, null, OnCommandPropertyChanged);

        private static void OnCommandPropertyChanged(BindableObject bindable, ICommand oldValue, ICommand newValue) {
            var source = bindable as CustomCell;
            if (source == null) {
                return;
            }
            source.OnCommandChanged();
        }

        private void OnCommandChanged() { OnPropertyChanged("Command"); }

        public ICommand Command { get { return (ICommand)GetValue(CommandProperty); } set { SetValue(CommandProperty, value); } }
        #endregion Command
    }

    public class SingleTopicCell : CustomCell {
        public SingleTopicCell() {
            var stack = new StackLayout {Orientation = StackOrientation.Horizontal, HorizontalOptions = LayoutOptions.FillAndExpand, Padding = new Thickness(10, 0, 10, 0)};
            View = stack;

            var timePicker = new TimePicker {Format = "HH:mm"};
            var timeBinding = new Binding("DateTime", BindingMode.TwoWay, new DateTimeConverter());
            timePicker.SetBinding(TimePicker.TimeProperty, timeBinding);

            var entry = new Entry {Placeholder = "Name", HorizontalOptions = LayoutOptions.FillAndExpand};
            var entryBinding = new Binding("DisplayText", BindingMode.TwoWay);
            entry.SetBinding(Entry.TextProperty, entryBinding);

            var image = new Image {Source = "ic_delete_forever.png", Aspect = Aspect.AspectFit};

            var tapper = new TapGestureRecognizer();
            tapper.Tapped += (sender, args) => {
                if (Command == null) {
                    return;
                }
                if (Command.CanExecute(this)) {
                    Command.Execute(BindingContext);
                }
            };

            image.GestureRecognizers.Add(tapper);

            stack.Children.Add(timePicker);
            stack.Children.Add(entry);
            stack.Children.Add(image);
        }

        protected override void OnTapped() {
            //base.OnTapped();
        }
    }

    public class QuestionCell : CustomCell {

        public QuestionCell() {
            var stack = new StackLayout { Orientation = StackOrientation.Horizontal, HorizontalOptions = LayoutOptions.FillAndExpand, Padding = new Thickness(10, 0, 10, 0) };
            View = stack;

            var label = new Label { HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.Center };
            var labelBinding = new Binding("DisplayText", BindingMode.TwoWay);
            label.SetBinding(Label.TextProperty, labelBinding);

            var image = new Image { Source = "ic_delete_forever.png", Aspect = Aspect.AspectFit };

            var tapper = new TapGestureRecognizer();
            tapper.Tapped += (sender, args) => {
                if (Command == null)
                {
                    return;
                }
                if (Command.CanExecute(this))
                {
                    Command.Execute(BindingContext);
                }
            };

            image.GestureRecognizers.Add(tapper);
            
            stack.Children.Add(label);
            stack.Children.Add(image);
        }

        protected override void OnTapped() {
            //base.OnTapped();
        }
    }

    public class AnswerCell : CustomCell {
        public AnswerCell() {
            QuestionType = string.Empty;
            var stack = new StackLayout {Orientation = StackOrientation.Horizontal, HorizontalOptions = LayoutOptions.FillAndExpand, Padding = new Thickness(10, 0, 10, 0)};
            View = stack;

            var swStack = new StackLayout();
            //TODO: Bind to questions.iscorrectbool to hide it if not true
            var swStackBinding = new Binding("QuestionType", BindingMode.OneWay, new QuestionTypeAnswerConverter(), null,null,this);
            swStack.SetBinding(VisualElement.IsVisibleProperty, swStackBinding);

            var sw = new Switch {IsToggled = false};
            var swBinding = new Binding("Result", BindingMode.TwoWay, new AnswerResultConverter());
            sw.SetBinding(Switch.IsToggledProperty, swBinding);

            var swLabel = new Label {HorizontalOptions = LayoutOptions.Center, HorizontalTextAlignment = TextAlignment.Center};
            var swLabelBinding = new Binding("Result", BindingMode.OneWay, new AnswerResultTextConverter());
            swLabel.SetBinding(Label.TextProperty, swLabelBinding);

            swStack.Children.Add(swLabel);
            swStack.Children.Add(sw);

            var entry = new Entry {Placeholder = "Antworttext", HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.Center};
            var entryBinding = new Binding("DisplayText", BindingMode.TwoWay);
            entry.SetBinding(Entry.TextProperty, entryBinding);

            var image = new Image {Source = "ic_delete_forever.png", Aspect = Aspect.AspectFit};

            var tapper = new TapGestureRecognizer();
            tapper.Tapped += (sender, args) => {
                if (Command == null) {
                    return;
                }
                if (Command.CanExecute(this)) {
                    Command.Execute(BindingContext);
                }
            };

            image.GestureRecognizers.Add(tapper);

            stack.Children.Add(swStack);
            stack.Children.Add(entry);
            stack.Children.Add(image);
        }

        #region QuestionType
        public static readonly BindableProperty QuestionTypeProperty = BindableProperty.Create<AnswerCell, string>(p => p.QuestionType, default(string), BindingMode.OneWay, null, OnQuestionTypePropertyChanged);

        private static void OnQuestionTypePropertyChanged(BindableObject bindable, string oldValue, string newValue)
        {
            var source = bindable as AnswerCell;
            if (source == null)
            {
                return;
            }
            source.OnQuestionTypeChanged();
        }

        private void OnQuestionTypeChanged() { OnPropertyChanged("QuestionType"); }

        public string QuestionType { get { return (string)GetValue(QuestionTypeProperty); } set { SetValue(QuestionTypeProperty, value); } }
        #endregion QuestionType

        protected override void OnTapped() {
            //base.OnTapped();
        }
    }
}