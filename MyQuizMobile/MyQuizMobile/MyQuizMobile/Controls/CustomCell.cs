using System.Windows.Input;
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
        public static readonly BindableProperty CommandProperty =
            BindableProperty.Create<CustomCell, ICommand>(p => p.Command, default(ICommand), BindingMode.OneWay, null,
                                                          OnCommandPropertyChanged);

        private static void OnCommandPropertyChanged(BindableObject bindable, ICommand oldValue, ICommand newValue) {
            var source = bindable as CustomCell;
            if (source == null) {
                return;
            }
            source.OnCommandChanged();
        }

        private void OnCommandChanged() { OnPropertyChanged("Command"); }

        public ICommand Command {
            get { return (ICommand)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }
        #endregion Command
    }

    public class SingleTopicCell : CustomCell {
        public SingleTopicCell() {
            var stack = new StackLayout {
                Orientation = StackOrientation.Horizontal,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Padding = new Thickness(10, 0, 10, 0)
            };
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
}