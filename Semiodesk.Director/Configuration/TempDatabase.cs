using IniParser.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Semiodesk.Director.Configuration
{
    public class TempDatabase : IniSectionWrapper
    {
        #region Constructor
        public TempDatabase(SectionData data)
            : base(data)
        {
        }
        #endregion 

        public string DatabaseFile
        {
            get
            {
                return GetStringData("DatabaseFile");
            }
            set
            {
                SetStringData("DatabaseFile", value);
            }
        }

        public string TransactionFile
        {
            get
            {
                return GetStringData("TransactionFile");
            }
            set
            {
                SetStringData("TransactionFile", value);
            }
        }

        public int? MaxCheckpointRemap
        {
            get
            {
                return GetIntData("MaxCheckpointRemap");
            }
            set
            {
                SetIntData("MaxCheckpointRemap", value);
            }
        }

        public int? Striping
        {
            get
            {
                return GetIntData("Striping");
            }
            set
            {
                SetIntData("Striping", value);
            }
        }
    }


}
