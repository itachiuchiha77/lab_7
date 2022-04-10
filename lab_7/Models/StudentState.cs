using Avalonia.Media;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Xml.Serialization;

namespace lab_7.Models
{
    public class StudentState : INotifyPropertyChanged, IChangerUI
    {
        public string Name { set; get; }
        public ObservableCollection<Grade> Grades
        {
            get => __grades;
            set
            {
                this.__grades = value;
                RaisePropertyChangedEvent("Grades");
            }
        }
        [XmlIgnore]
        public SolidColorBrush AvgColor
        {
            get => __avgColor;
            private set
            {
                this.__avgColor = value;
                RaisePropertyChangedEvent("AvgColor");
            }
        }
        [XmlIgnore]
        public bool IsCheckedFlag { get; set; }
        [XmlIgnore]
        public float? AvgGrade
        {
            get => __avgGrade;
            private set
            {
                if (value is not null)
                {
                    if (value < 1.5)
                    {
                        AvgColor = new SolidColorBrush(Brushes.Yellow.Color);
                        __avgGrade = value;
                    }
                    if (value < 1)
                    {
                        AvgColor = new SolidColorBrush(Brushes.Red.Color);
                        __avgGrade = value;
                    }
                    if (value >= 1.5)
                    {
                        AvgColor = new SolidColorBrush(Brushes.LightGreen.Color);
                        __avgGrade = value;
                    }
                }
                else
                {
                    __avgGrade = null;
                    AvgColor = new SolidColorBrush(Brushes.White.Color);
                }
                RaisePropertyChangedEvent("AvgGrade");
            }
        }
        private ObservableCollection<Grade> __grades;
        private float? __avgGrade;
        [XmlIgnore]
        private SolidColorBrush __avgColor;

        public void GetAverageGrades()
        {
            if (Grades.Any(grade => grade.Num is null))
            {
                AvgGrade = null;
            }
            else
            {
                float sum = 0;
                foreach (var grade in Grades) sum += (float)grade.Num;
                AvgGrade = sum / 3;
            }
        }
        public StudentState(string name)
        {
            Name = name;
            Grades = new ObservableCollection<Grade>();
            Grades.CollectionChanged += ChangeCollection;
            Grades.Clear();
            for (int i = 0; i < 3; ++i)
            {
                Grades.Add(new Grade(0));
            }
            IsCheckedFlag = false;
            GetAverageGrades();
        }
        public StudentState()
        {
            Name = "NULL";
            Grades = new ObservableCollection<Grade>();
            Grades.CollectionChanged += ChangeCollection;
            Grades.Clear();
            for (int i = 0; i < 3; ++i)
            {
                Grades.Add(new Grade(0));
            }
            IsCheckedFlag = false;
            GetAverageGrades();
        }
        public event PropertyChangedEventHandler PropertyChanged;
        public void RaisePropertyChangedEvent(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public void ChangeCollection(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null)
            {
                foreach (Grade item in e.NewItems)
                {
                    item.PropertyChanged += ChangeProperty;
                }
            }
            if (e.OldItems != null)
            {
                foreach (Grade item in e.OldItems)
                {
                    item.PropertyChanged -= ChangeProperty;
                }
            }
        }
        public void ChangeProperty(object sender, PropertyChangedEventArgs e)
        {
            GetAverageGrades();
        }
    }
}
