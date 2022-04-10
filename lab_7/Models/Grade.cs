using Avalonia.Media;
using System.ComponentModel;
using System.Xml.Serialization;

namespace lab_7.Models
{
    public class Grade : INotifyPropertyChanged
    {
        [XmlIgnore]
        public Avalonia.Media.IBrush Brush { get; private set; }
        public event PropertyChangedEventHandler PropertyChanged;
        public float? Num
        {
            set
            {
                switch (value)
                {
                    case 0:
                        Brush = Brushes.Red;
                        __num = value;
                        break;
                    case 1:
                        Brush = Brushes.Yellow;
                        __num = value;
                        break;
                    case 2:
                        Brush = Brushes.LightGreen;
                        __num = value;
                        break;
                    default:
                        Brush = Brushes.White;
                        __num = null;
                        break;
                }
                RaisePropertyChangedEvent("Num");
            }
            get => __num;
        }
        private float? __num;

        public Grade(float mark)
        {
            Num = mark;
        }
        public Grade()
        {
            Num = 0;
        }
        protected void RaisePropertyChangedEvent(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChangedEventArgs e = new PropertyChangedEventArgs(propertyName);
                PropertyChanged(this, e);
            }
        }
    }
}
