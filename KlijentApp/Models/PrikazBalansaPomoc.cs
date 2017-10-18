using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace KlijentApp.Models
{
    public class PrikazBalansaPomoc : INotifyPropertyChanged





    {
        private List<PrikazNaplate> _ListaNaplate;
        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(string info)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }

        public PrikazBalansaPomoc()
        {
            this.ListaNaplate = new List<PrikazNaplate>();

        }
        public IEnumerable<int> getSelectedIds()
        {
            // Return an Enumerable containing the Id's of the selected people:
            return (from p in this.ListaNaplate where p.Selected select p.FirmaId).ToList();
        }

        public List<PrikazNaplate> ListaNaplate
        {
            get { return _ListaNaplate; }

            set
            {
                if (_ListaNaplate != value)
                {
                    _ListaNaplate = value;
                    NotifyPropertyChanged("ListaPrikaza");
                
                }
            }
        }
    
    }
}


      
