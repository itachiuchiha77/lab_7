using Avalonia.Media;
using lab_7.Models;
using lab_7.ViewModels;
using ReactiveUI;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

namespace lab_7.ViewModels
{
    public class MainWindowViewModel : ViewModelBase, IChangerUI
    {   
        // An object to show our table with students
        public ViewModelBase Content
        {
            get
            {
                return __content;
            }
            private set
            {
                this.RaiseAndSetIfChanged(ref __content, value);
            }
        }
        private ViewModelBase __content;
        // A collection to keeping our states (Student - Grade - Grade - ... - Grade)
        private ObservableCollection<StudentState> __states { get; set; }
        // A collection to keeping our Average Grades for object (like Math)
        private ObservableCollection<float?> __avgGrades { get; set; }
        // A collection to keeping our Colors for Cells where we keeping our Average Grades
        private ObservableCollection<IBrush> __avgGradesColors { get; set; }

        public void AddNewStudentState()
        {
            __states.Insert(0, new StudentState("Enter a student name"));
            CalculateAverageValuesForStudents();
        }
        public void DeleteCheckedStudentStates()
        {
            var studentsToDelete = this.__states.Where(x => !x.IsCheckedFlag).ToList();
            __states.Clear();
            foreach (var neededStudent in studentsToDelete)
            {
                __states.Add(neededStudent);
            }
            CalculateAverageValuesForStudents();
        }
        public MainWindowViewModel()
        {
            __states = new ObservableCollection<StudentState>();
            __avgGrades = new ObservableCollection<float?>();
            __avgGradesColors = new ObservableCollection<IBrush>();
            for (int i = 0; i < 3; ++i)
            {
                __avgGrades.Add(0);
                __avgGradesColors.Add(new SolidColorBrush(Brushes.White.Color));
            }
            __states.CollectionChanged += ChangeCollection;
            Content = new WindowViewModel();
        }
        // Writing our states in XML file (i think it's ok for states like this)
        public void WriteStatesToFile(string filePath)
        {
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(ObservableCollection<StudentState>));
                using (StreamWriter streamWriter = new StreamWriter(filePath))
                {
                    serializer.Serialize(streamWriter, this.__states);
                }
            }
            catch
            {
                return;
            }
        }
        // Read a xml file and writing it in our memory
        public void ReadStatesFromFile(string filePath)
        {
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(ObservableCollection<StudentState>));
                using (StreamReader streamReader = new StreamReader(filePath))
                {
                    __states.Clear();
                    __states = (ObservableCollection<StudentState>)serializer.Deserialize(streamReader);
                    foreach (StudentState state in this.__states)
                    {
                        var gradeList = new List<Grade>(3);
                        gradeList.Add(state.Grades[3]);
                        gradeList.Add(state.Grades[4]);
                        gradeList.Add(state.Grades[5]);
                        state.Grades.Clear();
                        foreach (var grade in gradeList)
                        {
                            state.Grades.Add(grade);
                        }
                        state.GetAverageGrades();
                    }
                }
            }
            catch
            {
                return;
            }
        }
        public void OpenWindowView()
        {
            Content = new WindowViewModel();
        }
        public void CalculateAverageValuesForStudents()
        {
            for (int i = 0; i < 3; i++)
            {
                __avgGrades[i] = 0;
            }
            foreach (StudentState s in __states)
            {
                if (__states != null)
                {
                    for (int i = 0; i < 3; i++)
                    {
                        __avgGrades[i] += s.Grades[i].Num;
                    }
                }
            }
            if (__states.Count != 0)
            {
                for (int i = 0; i < 3; i++)
                {
                    __avgGrades[i] /= __states.Count;
                    if (__avgGrades[i] != null)
                    {
                        if (__avgGrades[i] < 1.5 && __avgGrades[i] >= 1) __avgGradesColors[i] = new SolidColorBrush(Brushes.Yellow.Color);
                        if (__avgGrades[i] < 1 && __avgGrades[i] >= 0) __avgGradesColors[i] = new SolidColorBrush(Brushes.Red.Color);
                        if (__avgGrades[i] >= 1.5 && __avgGrades[i] <= 2) __avgGradesColors[i] = new SolidColorBrush(Brushes.LightGreen.Color);
                    }
                }
            }
            else
            {
                for (int i = 0; i < 3; i++)
                {
                    __avgGrades[i] = null;
                    __avgGradesColors[i] = new SolidColorBrush(Brushes.White.Color);
                }

            }
        }
        public void ChangeCollection(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null)
            {
                foreach (StudentState state in e.NewItems)
                {
                    state.PropertyChanged += ChangeProperty;
                }
            }
        }
        public void ChangeProperty(object sender, PropertyChangedEventArgs e)
        {
            CalculateAverageValuesForStudents();
        }
    }
}