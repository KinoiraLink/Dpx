using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;

namespace Dpx.ViewModels
{
    public class TestViewModel : ViewModelBase
    {
        private TestModel _TestModel;

        public TestModel TestModel
        {
            get => _TestModel;
            set => Set(nameof(TestModel), ref _TestModel, value);
        }

        private int _rowVlue;

        public int RowVlue
        {
            get => _rowVlue;
            set => Set(nameof(RowVlue), ref _rowVlue, value);
        }

        private int _columnVlue;

        public int ColumnVlue
        {
            get => _columnVlue;
            set => Set(nameof(ColumnVlue), ref _columnVlue, value);
        }
        

        /// <summary>
        /// comment
        /// </summary>
        private RelayCommand<int> _testRelayCommand;

        public RelayCommand<int> TestRelayCommand =>
            _testRelayCommand ?? (_testRelayCommand = new RelayCommand<int>( t =>  TestRelayCommandFunction(t)));

        public  void TestRelayCommandFunction(int rowvlue)
        {
            RowVlue = 1;
            ColumnVlue = 1;
            Debug.WriteLine(RowVlue);
            Debug.WriteLine(ColumnVlue);
        }
    }
    
    public class TestModel
    {
        public int RowValue { get; set; }
        public int Column { get; set; }
    }
}
