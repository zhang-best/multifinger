using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;

public class SavePara 
{
    string path = "C:\\Research\\Data\\JD";

    StreamWriter sw;
    StreamReader sr;
    string fileName;
    FileStream fs;

    private List<Value> paras;

    //public void Start(string user, bool saveModel)
    //{
    //    fileName = path + user + ".txt";

    //    if (saveModel)
    //    {
    //        fs = new FileStream(fileName, FileMode.Create);
    //        sw = new StreamWriter(fs);
    //    }
    //    else
    //    {
    //        fs = new FileStream(fileName, FileMode.Open);
    //        if (fs != null)
    //        {
    //            sr = new StreamReader(fs);
    //        }
    //    }        
    //}

    //public void ReadPara(ref List<Value> input)
    //{
    //    if (sr != null)
    //    {
    //        string _line = sr.ReadLine();
    //        while (_line != null)
    //        {
    //            char _spit = ' ';
    //            string[] _lineParts = _line.Split(_spit);
    //            Value _newelement = new Value();
    //            _newelement.targetforce = float.Parse(_lineParts[0]);
    //            _newelement.tolerance = float.Parse(_lineParts[1]);
    //            _newelement.upperbound = float.Parse(_lineParts[2]);
    //            _newelement.lowerbound = float.Parse(_lineParts[3]);
    //            _newelement.startdeadline = float.Parse(_lineParts[4]);
    //            input.Add(_newelement);
    //            _line = sr.ReadLine();
    //        }
    //    }
    //}

    //public void SaveFile(List<Value> _input)
    //{
    //    if (sw != null)
    //    {
    //        for (int i = 0; i < _input.Count; i++)
    //        {
    //            Value _element = _input[i];
    //            string _resultLine = string.Format("{0} {1} {2} {3} {4}", _element.targetforce, _element.tolerance, _element.upperbound, _element.lowerbound, _element.startdeadline);
    //            sw.WriteLine(_resultLine);
    //        }
    //    }
    //}

    public void Close()
    {
        if (sw != null)
            sw.Close();
        if (sr != null)
            sr.Close();
        if (fs != null)
            fs.Close();
    }
}
