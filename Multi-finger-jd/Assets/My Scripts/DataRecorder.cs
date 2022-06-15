using UnityEngine;
using System.Collections;
using System.IO;
using System.Text;
using OfficeOpenXml;
using UnityEditor;

public struct SaveDataElement
{
    public int trialNumber;
    public int fingerIndex;
    public float targetForce;
    public float tolerance;
    public float upperbound;
    public float lowerbound;
    public float deadline;
    public float reactingTime;
    public int result;

    
    
    //public float su1;
}



public class DataRecord
{

    public DataRecord()
    {
        isOpen = false;
    }

    // private parameters
    private StreamWriter sw;
    private static string fileName;
    private static string newname;
    private FileStream fs,ns;
    private bool isOpen;

    public static int lineNumber=0;
    public ExcelPackage package;
    public int sheetIndex;

    // Use this for initialization
    public void Start()
    {
        //fileName = ADAlgo.user + System.DateTime.Now.Day.ToString()
        //    + System.DateTime.Now.Hour.ToString()
        //        + System.DateTime.Now.Minute.ToString()
        //        + System.DateTime.Now.Second.ToString() + ".csv";

        newname = "C:\\Research\\Data\\JD" + ADAlgo.user + System.DateTime.Now.Day.ToString()
            + System.DateTime.Now.Hour.ToString()
                + System.DateTime.Now.Minute.ToString()
                + System.DateTime.Now.Second.ToString() + ".xlsx";

        //fs = new FileStream("E:\\data\\" + fileName, FileMode.Create);
        //sw = new StreamWriter(fs);
        //sw.WriteLine("trialNumber fingerIndex targetForce tolerance accuracy variance deadline reactingtime result");


        FileInfo newFile = new FileInfo(newname);
        if (newFile.Exists)
        {
            newFile.Delete();
            newFile = new FileInfo(newname);
        }
        package = new ExcelPackage(newFile);

        using (package)
        {
            ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("sheet1");
            sheetIndex = worksheet.Index;

            lineNumber++;
            worksheet.Cells[1, 1].Value = "trialNumber";
            worksheet.Cells[1, 2].Value = "fingerIndex";
            worksheet.Cells[1, 3].Value = "targetForce";
            worksheet.Cells[1, 4].Value = "tolerance";
            worksheet.Cells[1, 5].Value = "upperbound";
            worksheet.Cells[1, 6].Value = "lowerbound";
            worksheet.Cells[1, 7].Value = "deadline";
            worksheet.Cells[1, 8].Value = "reactingTime";
            worksheet.Cells[1, 9].Value = "result";




            package.Save();

        }


        //isOpen = true;
    }

    //public void WriteData(string data)
    //{
    //    sw.WriteLine(data);
    //}

    //public void WriteData(SaveDataElement element)
    //{
    //    element.upperbound = (float)ADAlgo.per;
    //    element.lowerbound = (float)ADAlgo.V;
    //    //element.su1 = (float)ADAlgo.avr0;
    //    string resultsLine = string.Format("{0} {1} {2} {3} {4} {5} {6} {7} {8}", element.trialNumber, element.fingerIndex, element.targetForce, element.tolerance, element.upperbound, element.lowerbound, element.deadline, element.reactingTime, element.result);
    //    sw.WriteLine(resultsLine);
    //}

    //public void Close()
    //{
    //    sw.Close();
    //    fs.Close();
    //    isOpen = false;
    //}

    //public bool IsOpen()//GameControl OnDestroy()
    //{
    //    return isOpen;
    //}

   

    public void WriteDataToExcel(SaveDataElement element)
    {
        ExcelPackage localPackage;
        element.upperbound = (float)ADAlgo.per;
        element.lowerbound = (float)ADAlgo.V;
        using (localPackage = new ExcelPackage(new FileInfo(newname))) 
        {
            lineNumber++;
            ExcelWorksheet worksheet = localPackage.Workbook.Worksheets["sheet1"];

            worksheet.Cells["A" + lineNumber.ToString()].Value = element.trialNumber;
            worksheet.Cells["B" + lineNumber.ToString()].Value = element.fingerIndex;
            worksheet.Cells["C" + lineNumber.ToString()].Value = element.targetForce;
            worksheet.Cells["D" + lineNumber.ToString()].Value = element.tolerance;
            worksheet.Cells["E" + lineNumber.ToString()].Value = element.upperbound;
            worksheet.Cells["F" + lineNumber.ToString()].Value = element.lowerbound;
            worksheet.Cells["G" + lineNumber.ToString()].Value = element.deadline;
            worksheet.Cells["H" + lineNumber.ToString()].Value = element.reactingTime;
            worksheet.Cells["I" + lineNumber.ToString()].Value = element.result;

            localPackage.Save();

        }
    }

    
}
