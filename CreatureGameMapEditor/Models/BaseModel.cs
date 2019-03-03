using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CreatureGameMapEditor.Models
{
    public class BaseModel
    {
        public event PropertyChangedEventHandler PropertyChanged = (sender, e) => { };

        protected void ChangeProperty(object sender, string property)
        {
            PropertyChanged(sender, new PropertyChangedEventArgs(property));
        }
    }
}
