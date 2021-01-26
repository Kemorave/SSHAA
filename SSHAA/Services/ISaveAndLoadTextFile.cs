using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace SSHAA.Services
{
    public interface ISaveAndLoadTextFile
    {
        void SaveText(string filename, string text);
        string LoadText(string filename);
        void DeleteText(string filename);
    }
}