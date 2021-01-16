using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace TripApp
{
    public class InvalidDataException : Exception
    {
        public InvalidDataException(string msg) : base(msg) { }
    }

    public class Trip

    {
        public delegate void LoggerDelegate(string reason);

        public static LoggerDelegate LogFailSet;

        private string _destination;

        public string Destination
        {
            get
            {
                return _destination;
            }
            set
            {
                
                    string pattern = @"[;]";
                    Regex rg = new Regex(pattern);
                    if (rg.IsMatch(value))
                    {
                        LogFailSet?.Invoke("Line contains invalid character (;)");
                        throw new InvalidDataException("Destination shall not contain semicolon(;)");

                    }
                _destination = value;
                
            }
        }
        private string _name;
        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                string pattern = @"[;]";
                Regex rg = new Regex(pattern);
                if (rg.IsMatch(value))
                {
                    LogFailSet?.Invoke("Line contains invalid character (;)");
                    throw new InvalidDataException("Name shall not contain semicolon(;)");

                }
                _name = value;
            }
        }
        private string _passport;

        public string Passport
        {
            get
            {
                return _passport;
            }
            set
            {
                if (value.Length < 1 || value.Length > 9)
                {
                    throw new ArgumentException("Passport number must be between 1 and 9 characters");
                }
                _passport = value;
            }
        }
        private string _departure;

        public string Departure
        {
            get
            {
                return _departure;
            }
            set
            {
                //set { if(!DateTime.TryParse(value, out _datevalue)) /* Error recovery!! */ ;}
                //if (value.Length < 2 || value.Length > 100)
                //{
                    //throw new ArgumentException("Please pick a departure date");
                //}
                _departure = value;
            }
        }

        private string _returnDate;

        public string ReturnDate
        {
            get
            {
                return _returnDate;
            }
            set
            {
                //if (value.Length < 2 || value.Length > 100)
                //{
                   // throw new ArgumentException("Please pick a return date");
                //}
                _returnDate = value;
            }
        }

        public Trip(string dest, string name, string passp, string depart, string returnDate)
        {
            this._destination = dest;
            this._name = name;
            this._passport = passp;
            this._departure = depart;
            this._returnDate = returnDate;
        }

        public Trip(string dataLine)
        {
            string[] data = dataLine.Split(';');
            if (data.Length != 5)
            {
                LogFailSet?.Invoke("Line has invalid number");
                throw new InvalidDataException("Line has invalid number for fields:\n" + dataLine);
            }
            _destination = data[0];
            _name = data[1];
            _passport = data[2];
            _departure = data[3];
            _returnDate = data[4];

        }

        public override string ToString()
        {
            return string.Format("Trip to {0} byf {1} (passport: {2}) from {3} to {4}", _destination, _name, _passport, _departure, _returnDate);
        }

        public string ToDataString()
        {
            return string.Format("{0};{1};{2};{3};{4}", _destination, _name, _passport, _departure, _returnDate);
            
        }
    }
}
