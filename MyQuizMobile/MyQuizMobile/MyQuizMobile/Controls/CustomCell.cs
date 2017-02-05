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

    public class QuestionCell : CustomCell {
        public Grid Container { get; set; }

        public QuestionCell() {
            var label = new Label {
                HorizontalOptions = LayoutOptions.StartAndExpand,
                VerticalOptions = LayoutOptions.CenterAndExpand
            };

            var labelBinding = new Binding("DisplayText", BindingMode.TwoWay);
            label.SetBinding(Label.TextProperty, labelBinding);

            var image = new Image {
                Source = "ic_delete_forever.png",
                Aspect = Aspect.AspectFit,
                HorizontalOptions = LayoutOptions.EndAndExpand,
                VerticalOptions = LayoutOptions.CenterAndExpand
            };
            var tapper = new TapGestureRecognizer {
                Command = new Command(() => {
                    if (Command == null) {
                        return;
                    }
                    if (Command.CanExecute(this)) {
                        Command.Execute(BindingContext);
                    }
                })
            };
            image.GestureRecognizers.Add(tapper);

            Container = new Grid {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                Padding = new Thickness(10, 0, 10, 0),
                ColumnDefinitions =
                    new ColumnDefinitionCollection {
                        new ColumnDefinition {Width = new GridLength(1, GridUnitType.Star)},
                        new ColumnDefinition {Width = new GridLength(9, GridUnitType.Star)},
                        new ColumnDefinition {Width = new GridLength(1, GridUnitType.Star)}
                    },
                RowDefinitions =
                    new RowDefinitionCollection {new RowDefinition {Height = new GridLength(1, GridUnitType.Star)}}
            };

            Grid.SetRow(label, 0);
            Grid.SetColumn(label, 0);
            Grid.SetColumnSpan(label, 2);

            Grid.SetRow(image, 0);
            Grid.SetColumn(image, 2);

            Container.Children.Add(label);
            Container.Children.Add(image);
            View = Container;
        }

        protected override void OnTapped() {
            //base.OnTapped();
        }
    }
}