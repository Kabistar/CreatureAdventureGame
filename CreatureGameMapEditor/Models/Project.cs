using CreatureGameMapEditor.DataAccess;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CreatureGameMapEditor.Models
{
    public class Project : BaseModel
    {
        #region Private Members
        private string workingDirectory;
        private ObservableCollection<Map> maps;
        #endregion
        
        #region Public Properties
        public string WorkingDirectory { get { return workingDirectory; } private set { workingDirectory = value; ChangeProperty(this, "WorkingDirectory"); } }
        public ObservableCollection<Map> Maps { get { return maps; } private set { maps = value; ChangeProperty(this, "Maps"); } }
        #endregion

        public Project(string _workingDirectory)
        {
            WorkingDirectory = _workingDirectory;
            Maps = new ObservableCollection<Map>();
        }


        #region Public Functions
        public void LoadProject()
        {
            string mapDirectory = WorkingDirectory + "/Maps";

            Maps.Clear();

            if (!Directory.Exists(mapDirectory))
            {
                throw new ArgumentException("Working directory does not have a maps sub-directory.");
            }
            
            string[] mapFiles = Directory.GetFiles(mapDirectory);

            foreach(string mapFileName in mapFiles)
            {
                Maps.Add(MapProvider.LoadFromFile(mapFileName));
            }
        }
        #endregion

        #region Private Functions
            
        #endregion
    }
}
