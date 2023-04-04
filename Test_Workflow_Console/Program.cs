using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestTemplate
{
    /* 
     * Delegate fuer das Fehlerhaendling 
*/
    public delegate void InvalidEventHandler(Object sender, InvalidEventArgs e);

    internal class Program
    {

        static void Main(string[] args)
        {

            Console.WriteLine("Hello, World!");
            GUIC cc = new GUIC();
            /* Task ID 4 & 5
             * */
            Console.WriteLine("Endline in Development ");
            Console.ReadLine();
        }
    }



    public class GUIC
    {
        static void PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            Console.WriteLine("\nDie Eigenschaft '{0}' hat sich geändert.", e.PropertyName);
        }

        // InvalidMeasure
        static void Invalid(object sender, InvalidEventArgs e)
        {
            Console.WriteLine("Zuweisungsfehler an '{0}'. {1}Value:{2}{1}{3}", e.PropertyName, System.Environment.NewLine, e.Invalid, e.Message);
        }

        public GUIC()
        {
            Sub m1 = new Sub();
            Sub s1 = new Sub();

            m1.PropertyChanged += new PropertyChangedEventHandler(PropertyChanged);
            m1.Invalid += new InvalidEventHandler(Invalid);
            m1.Radius = -4;
            s1.Radius = 4;
            Console.WriteLine("Compare: {0}", s1.CompareTo(m1));
        }

    }

    /*  Basis Klasse
    */
    public abstract class Master : IComparable, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public event InvalidEventHandler Invalid;

        public virtual int CompareTo(Object @object)
        {
            Master MasterObject = @object as Master;
            if (MasterObject != null)
            {
                if (this.Radius == MasterObject.Radius) return -1;
                return 0;
            }
            throw new ArgumentException("Ungültige Objektübergabe. Es wird der Typ 'MasterSub' erwartet.");
        }

        protected int _Radius;
        public virtual int Radius
        {
            get { return _Radius; }
            set
            {
                if (value >= 0)
                {
                    _Radius = value;
                    OnPropertyChanged("Radius");
                    OnInvalid(new InvalidEventArgs(value, "Radius", "Message"));
                }
                else
                    OnInvalid(new InvalidEventArgs(value, "Radius zu klein"));
            }
        }



        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        protected virtual void OnInvalid(InvalidEventArgs e)
        {
            if (Invalid != null)
                Invalid(this, e);
        }

    }


    /* Abgleitete Klasse
    */
    public class Sub : Master
    {
        public Sub() { }

        protected int _Radius;
        public override int Radius
        {
            get { return _Radius; }
            set
            {
                if (value >= 0)
                {
                    _Radius = value;
                    OnPropertyChanged("Radius");
                    OnInvalid(new InvalidEventArgs(value, "Radius", "Message"));
                }
                else
                    OnInvalid(new InvalidEventArgs(value, "Radius zu klein", "Message"));
            }
        }
    }

    /* Eventhaendling
    */
    public class InvalidEventArgs : EventArgs
    {
        // Felder
        private int _Invalid;
        private string _PropertyName;
        private string _Message;


        // Eigenschaften
        public int Invalid
        {
            get { return _Invalid; }
        }
        public string PropertyName
        {
            get { return _PropertyName; }
        }

        public string Message
        { get { return _Message; } }

        // Konstruktor
        public InvalidEventArgs(int invalid, string propertyName)
        {
            _Invalid = invalid;
            if (propertyName == "" || propertyName == null)
                _PropertyName = "[unknown]";
            else
                _PropertyName = propertyName;
        }

        public InvalidEventArgs(int invalid, string propertyName, string message)
        {
            _Invalid = invalid;
            if (propertyName == "" || propertyName == null)
                _PropertyName = "[unknown]";
            else
                _PropertyName = propertyName;

            if (message == "" || message == null)
                _Message = "";
            else
                _Message = message;
        }
    }


}