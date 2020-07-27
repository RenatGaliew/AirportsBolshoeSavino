using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Windows;
using System.Windows.Forms;
using ReactiveUI;

namespace Airports
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, IViewFor<MainVM>
    {
        public static readonly DependencyProperty ViewModelProperty = DependencyProperty
            .Register(nameof(ViewModel), typeof(MainVM), typeof(MainWindow));

        public MainWindow()
        {
            ViewModel = new MainVM();
            InitializeComponent();

            // Setup the bindings
            // Note: We have to use WhenActivated here, since we need to dispose the
            // bindings on XAML-based platforms, or else the bindings leak memory.
            this.WhenActivated(disposable =>
            {
                this.WhenAnyValue(x => x.ViewModel.LastTimeTable)
                    .ObserveOn(RxApp.MainThreadScheduler)
                    .BindTo(this, x => x.TableViewLast.DataContext)
                    .DisposeWith(disposable);

                this.Bind(this.ViewModel, x => x.Speed, x => x.SpeedComboBox.SelectedItem);

                this.WhenAnyValue(x => x.ViewModel.ImmitationServiceVM.Time)
                    .ObserveOn(RxApp.MainThreadScheduler)
                    .BindTo(this, x => x.TextBlockTime.Text,
                        vmToViewConverterOverride: new DateConverter())
                    .DisposeWith(disposable);

                this.WhenAnyValue(x => x.ViewModel.ArrivalRaitingVM)
                    .ObserveOn(RxApp.MainThreadScheduler)
                    .BindTo(this, x => x.RaitingViewLeft.DataContext)
                    .DisposeWith(disposable);

                this.WhenAnyValue(x => x.ViewModel.DepartureRaitingVM)
                    .ObserveOn(RxApp.MainThreadScheduler)
                    .BindTo(this, x => x.RaitingViewRight.DataContext)
                    .DisposeWith(disposable); 
                
                /*this.WhenAnyValue(x => x.ViewModel.ArrivalCounts)
                    .ObserveOn(RxApp.MainThreadScheduler)
                    .BindTo(this, x => x.ColumnSeriesArrival.ItemsSource)
                    .DisposeWith(disposable);

                this.WhenAnyValue(x => x.ViewModel.DepartmentsCounts)
                    .ObserveOn(RxApp.MainThreadScheduler)
                    .BindTo(this, x => x.ColumnSeriesDepartment.ItemsSource)
                    .DisposeWith(disposable);*/
            });
        }

        public MainVM ViewModel
        {
            get => (MainVM)GetValue(ViewModelProperty);
            set => SetValue(ViewModelProperty, value);
        }
        object IViewFor.ViewModel
        {
            get => ViewModel;
            set => ViewModel = (MainVM)value;
        }
    }
}
