using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public struct Value
{
    public float targetforce;
    public float tolerance;
    public float upperbound;
    public float lowerbound;
    public float startdeadline;
    public float newDeadline;

    public float GetDeadLine()
    {
        //return (upperbound + lowerbound) / 2.0f;
        return newDeadline;
    }


}

public class ADAlgo
{


    public static int resultIndex;
    public static string user;
    // store the useful value
    private static List<Value> DiffArray;
    //private static List<int> yijie = new List<int>();
    private static HashSet<int> yijie = new HashSet<int>();
    // the current index of the array
    private static int iArrayIndex;
    private static int _randIndex;
    // target force and tolerance
    private static float[] TARGETFORCE = new float[4] { 1.0f, 2.0f, 3.0f, 4.0f };
    private static float[] TOLERANCE = new float[4] { 0.5f, 0.6f, 0.7f, 0.8f };
    private static float[] DIFFINDEX = new float[5] { 0.5f, 1f, 1.5f, 2f,2.5f};

    public static double[] Arr0 = new double[400];
    public static double[] Arr1 = new double[400];
    public static double[] Arr2 = new double[400];
    public static double[] Arr3 = new double[400];
    public static double[] Arr4 = new double[400];
    public static double[] Arr5 = new double[400];
    public static double[] Arr6 = new double[400];
    public static double[] Arr7 = new double[400];
    public static double[] Arr8 = new double[400];
    public static double[] Arr9 = new double[400];
    public static double[] Arr10 = new double[400];
    public static double[] Arr11 = new double[400];
    public static double[] Arr12 = new double[400];
    public static double[] Arr13 = new double[400];
    public static double[] Arr14 = new double[400];
    public static double[] Arr15 = new double[400];

    public static double per;
    public static int[] consecutive = new int[16];//连续正确地计数
   
    public static double V;
    


    public static Queue a0 = new Queue();
    public static Queue a1 = new Queue();
    public static Queue a2 = new Queue();
    public static Queue a3 = new Queue();
    public static Queue a4 = new Queue();
    public static Queue a5 = new Queue();
    public static Queue a6 = new Queue();
    public static Queue a7 = new Queue();
    public static Queue a8 = new Queue();
    public static Queue a9 = new Queue();
    public static Queue a10 = new Queue();
    public static Queue a11 = new Queue();
    public static Queue a12 = new Queue();
    public static Queue a13 = new Queue();
    public static Queue a14 = new Queue();
    public static Queue a15 = new Queue();

    public static Queue b0 = new Queue();
    public static Queue b1 = new Queue();
    public static Queue b2 = new Queue();
    public static Queue b3 = new Queue();
    public static Queue b4 = new Queue();
    public static Queue b5 = new Queue();
    public static Queue b6 = new Queue();
    public static Queue b7 = new Queue();
    public static Queue b8 = new Queue();
    public static Queue b9 = new Queue();
    public static Queue b10 = new Queue();
    public static Queue b11 = new Queue();
    public static Queue b12 = new Queue();
    public static Queue b13 = new Queue();
    public static Queue b14 = new Queue();
    public static Queue b15 = new Queue();

    public static int [] jlalel = new int [16];//允许时间数组编号
    

    public static double avr0 = 0;
    public static double avr1 = 0;
    public static double avr2 = 0;
    public static double avr3 = 0;
    public static double avr4 = 0;
    public static double avr5 = 0;
    public static double avr6 = 0;
    public static double avr7 = 0;
    public static double avr8 = 0;
    public static double avr9 = 0;
    public static double avr10 = 0;
    public static double avr11 = 0;
    public static double avr12 = 0;
    public static double avr13 = 0;
    public static double avr14 = 0;
    public static double avr15 = 0;

    public static double Dev0 = 0;
    public static double Dev1 = 0;
    public static double Dev2 = 0;
    public static double Dev3 = 0;
    public static double Dev4 = 0;
    public static double Dev5 = 0;
    public static double Dev6 = 0;
    public static double Dev7 = 0;
    public static double Dev8 = 0;
    public static double Dev9 = 0;
    public static double Dev10 = 0;
    public static double Dev11 = 0;
    public static double Dev12 = 0;
    public static double Dev13 = 0;
    public static double Dev14 = 0;
    public static double Dev15 = 0;

    private static int fai = 35;
    private static double Psi = 5.0;
    private static double Alpha = 0.45;
    private static double Beta = 0.6;
    private static double Gamma = 0.35;
    private static double Omega = 0.8;
    private static double Delte = 0.3;/////////// 还没用到
    private static double Epsilon = 0.8;////////还没用到
    private static double Eta = 0.75;

    private static List<Value> savePara;
    private static SavePara SP;
    //   private static GameControl GC;
    public static bool bSampleFlag = false;

    public bool isGameOver = false;

    public static int endThreshold = 30;


    /// <summary>
    /// initial the parameter
    /// </summary>
    public static void Initial(int x)
    {
        
        DiffArray = new List<Value>();
        savePara = new List<Value>();
        SP = new SavePara();
       // SP.Start(user, true);
        Reset(x);

    }

    /// <summary>
    /// reset the parameters

    public static void Reset(int x)
    {
        if (DiffArray.Count > 0)
            DiffArray.Clear();

        // initial the array
        for (int i = 0; i < TARGETFORCE.Length; i++)
        {
            for (int j = 0; j < TOLERANCE.Length; j++)
            {
                Value _newelement = new Value();
                _newelement.targetforce = TARGETFORCE[i];
                _newelement.tolerance = TOLERANCE[j]/DIFFINDEX[x-1];
                _newelement.newDeadline = CalculateStartDeadline(_newelement.targetforce, _newelement.tolerance);
                _newelement.upperbound = _newelement.newDeadline * 2;
                _newelement.lowerbound = 0f;
                DiffArray.Add(_newelement);
            }
        }
        resultIndex = 0;
        iArrayIndex = 0;
        bSampleFlag = false;

    }

    /// <summary>
    /// get the value from array list
    /// </summary>
    /// <returns></returns>
    public static Value GetValue(int number,bool plusindex)
    {
        Value _result = new Value();
        do
        {
            //_randIndex = UnityEngine.Random.Range(0, DiffArray.Count);
            _randIndex = UnityEngine.Random.Range(0, 12);// make it do not randomize
        } while (yijie.Contains(_randIndex));

        _result = DiffArray[_randIndex];
        if (plusindex)
        {
            resultIndex++;
        }
        return _result;
    }

    //public static void SetResult(bool input)
    //{
    //    // adjust the allowable time        
    //    //  bool bRet;
    //    if (!bSampleFlag)
    //    {
    //        if (input)  // if success
    //        {
    //            UpdateAllowableTime(true);
    //        }
    //        else  // if failed
    //        {
    //            UpdateAllowableTime(false);
    //        }
    //    }

    //    ////// Game over!!!移到Pause.CS了
    //    if (resultIndex >= endThreshold+1)
    //    //if (iArrayIndex >= 0 && resultIndex >= 2)
    //    {

    //        // check the terminate condition

    //        //Debug.Log("Reset the Camera Pos");
    //        //OVRManager.display.RecenterPose();
    //        SP.SaveFile(savePara);

    //        // set finish flag

    //        //fanxing(); 不停止

    //        //if (iArrayIndex >= 4)
    //        ////if (iArrayIndex >= DiffArray.Count)
    //        //{
    //        //    SP.SaveFile(savePara);
    //        //    bSampleFlag = true;
    //        //}
    //    }
    //}


    /// <summary>
    /// calculate the deadline according to the fitts' low

    private static float CalculateStartDeadline(float targetforce, float tolerance)
    {
        //// Value _newelement = new Value();
        //float _deadline = 0;

        //if (targetforce == 1.0f && tolerance == 0.5f)
        //{
        //    Arr0[0] = Psi * (0.051 + 0.234 * (Math.Sqrt(targetforce / tolerance)));
        //    //Arr0[0] = Psi * (0.051 + 0.234 * (Math.Log(targetforce / tolerance + 1) / Math.Log(2)));
        //    _deadline = (float)Arr0[0];
        //}

        //else if (targetforce == 1.0f && tolerance == 0.6f)
        //{
        //    Arr1[0] = Psi * (0.051 + 0.234 * (Math.Sqrt(targetforce / tolerance)));
        //    //Arr1[0] = Psi * (0.051 + 0.234 * (Math.Log(targetforce / tolerance + 1) / Math.Log(2)));
        //    _deadline = (float)Arr1[0];
        //}

        //else if (targetforce == 1.0f && tolerance == 0.7f)
        //{
        //    Arr2[0] = Psi * (0.051 + 0.234 * (Math.Sqrt(targetforce / tolerance)));
        //    //Arr2[0] = Psi * (0.051 + 0.234 * (Math.Log(targetforce / tolerance + 1) / Math.Log(2)));
        //    _deadline = (float)Arr2[0];
        //}

        //else if (targetforce == 1.0f && tolerance == 0.8f)
        //{
        //    Arr3[0] = Psi * (0.051 + 0.234 * (Math.Sqrt(targetforce / tolerance)));
        //    //Arr3[0] = Psi * (0.051 + 0.234 * (Math.Log(targetforce / tolerance + 1) / Math.Log(2)));
        //    _deadline = (float)Arr3[0];
        //}

        //else if (targetforce == 2.0f && tolerance == 0.5f)
        //{
        //    Arr4[0] = Psi * (0.051 + 0.234 * (Math.Sqrt(targetforce / tolerance)));
        //    //Arr4[0] = Psi * (0.051 + 0.234 * (Math.Log(targetforce / tolerance + 1) / Math.Log(2)));
        //    _deadline = (float)Arr4[0];
        //}

        //else if (targetforce == 2.0f && tolerance == 0.6f)
        //{
        //    Arr5[0] = Psi * (0.051 + 0.234 * (Math.Sqrt(targetforce / tolerance)));
        //    //Arr5[0] = Psi * (0.051 + 0.234 * (Math.Log(targetforce / tolerance + 1) / Math.Log(2)));
        //    _deadline = (float)Arr5[0];
        //}

        //else if (targetforce == 2.0f && tolerance == 0.7f)
        //{
        //    Arr6[0] = Psi * (0.051 + 0.234 * (Math.Sqrt(targetforce / tolerance)));
        //    //Arr6[0] = Psi * (0.051 + 0.234 * (Math.Log(targetforce / tolerance + 1) / Math.Log(2)));
        //    _deadline = (float)Arr6[0];
        //}

        //else if (targetforce == 2.0f && tolerance == 0.8f)
        //{
        //    Arr7[0] = Psi * (0.051 + 0.234 * (Math.Sqrt(targetforce / tolerance)));
        //    //Arr7[0] = Psi * (0.051 + 0.234 * (Math.Log(targetforce / tolerance + 1) / Math.Log(2)));
        //    _deadline = (float)Arr7[0];
        //}

        //else if (targetforce == 3.0f && tolerance == 0.5f)
        //{
        //    Arr8[0] = Psi * (0.051 + 0.234 * (Math.Sqrt(targetforce / tolerance)));
        //    //Arr8[0] = Psi * (0.051 + 0.234 * (Math.Log(targetforce / tolerance + 1) / Math.Log(2)));
        //    _deadline = (float)Arr8[0];
        //}

        //else if (targetforce == 3.0f && tolerance == 0.6f)
        //{
        //    Arr9[0] = Psi * (0.051 + 0.234 * (Math.Sqrt(targetforce / tolerance)));
        //    //Arr9[0] = Psi * (0.051 + 0.234 * (Math.Log(targetforce / tolerance + 1) / Math.Log(2)));
        //    _deadline = (float)Arr9[0];
        //}

        //else if (targetforce == 3.0f && tolerance == 0.7f)
        //{
        //    Arr10[0] = Psi * (0.051 + 0.234 * (Math.Sqrt(targetforce / tolerance)));
        //    //Arr10[0] = Psi * (0.051 + 0.234 * (Math.Log(targetforce / tolerance + 1) / Math.Log(2)));
        //    _deadline = (float)Arr10[0];
        //}

        //else if (targetforce == 3.0f && tolerance == 0.8f)
        //{
        //    Arr11[0] = Psi * (0.051 + 0.234 * (Math.Sqrt(targetforce / tolerance)));
        //    //Arr11[0] = Psi * (0.051 + 0.234 * (Math.Log(targetforce / tolerance + 1) / Math.Log(2)));
        //    _deadline = (float)Arr11[0];
        //}

        //else if (targetforce == 4.0f && tolerance == 0.5f)
        //{
        //    Arr12[0] = Psi * (0.051 + 0.234 * (Math.Sqrt(targetforce / tolerance)));
        //    //Arr12[0] = Psi * (0.051 + 0.234 * (Math.Log(targetforce / tolerance + 1) / Math.Log(2)));
        //    _deadline = (float)Arr12[0];
        //}

        //else if (targetforce == 4.0f && tolerance == 0.6f)
        //{
        //    Arr13[0] = Psi * (0.051 + 0.234 * (Math.Sqrt(targetforce / tolerance)));
        //    //Arr13[0] = Psi * (0.051 + 0.234 * (Math.Log(targetforce / tolerance + 1) / Math.Log(2)));
        //    _deadline = (float)Arr13[0];
        //}

        //else if (targetforce == 4.0f && tolerance == 0.7f)
        //{
        //    Arr14[0] = Psi * (0.051 + 0.234 * (Math.Sqrt(targetforce / tolerance)));
        //    //Arr14[0] = Psi * (0.051 + 0.234 * (Math.Log(targetforce / tolerance + 1) / Math.Log(2)));
        //    _deadline = (float)Arr14[0];
        //}

        //else if (targetforce == 4.0f && tolerance == 0.8f)
        //{
        //    Arr15[0] = Psi * (0.051 + 0.234 * (Math.Sqrt(targetforce / tolerance)));
        //    //Arr15[0] = Psi * (0.051 + 0.234 * (Math.Log(targetforce / tolerance + 1) / Math.Log(2)));
        //    _deadline = (float)Arr15[0];
        //}
        //return _deadline;
        return 3.0f;
    }


    public static double getAverage(int num)
    {
        double z = 0;
        double tem = 0;
        double sum0 = 0;
        double sum1 = 0;
        double sum2 = 0;
        double sum3 = 0;
        double sum4 = 0;
        double sum5 = 0;
        double sum6 = 0;
        double sum7 = 0;
        double sum8 = 0;
        double sum9 = 0;
        double sum10 = 0;
        double sum11 = 0;
        double sum12 = 0;
        double sum13 = 0;
        double sum14 = 0;
        double sum15 = 0;

        z = Convert.ToDouble(num);
        if (DiffArray[_randIndex].targetforce == 1.0f && DiffArray[_randIndex].tolerance == 0.5f)
        {
            a0.Enqueue(z);
            if (jlalel[0] >= fai)
            {
                a0.Dequeue();
            }
            foreach (double y in a0)
            {
                sum0 += y;
            }
            avr0 = sum0 / a0.Count;
            tem = avr0;
        }

        else if (DiffArray[_randIndex].targetforce == 1.0f && DiffArray[_randIndex].tolerance == 0.6f)
        {
            a1.Enqueue(z);
            if (jlalel[1] >= fai)
            {
                a1.Dequeue();
            }
            foreach (double y in a1)
            {
                sum1 += y;
            }
            avr1 = sum1 / a1.Count;
            tem = avr1;
        }

        else if (DiffArray[_randIndex].targetforce == 1.0f && DiffArray[_randIndex].tolerance == 0.7f)
        {
            a2.Enqueue(z);
            if (jlalel[2] >= fai)
            {
                a2.Dequeue();
            }
            foreach (double y in a2)
            {
                sum2 += y;
            }
            avr2 = sum2 / a2.Count;
            tem = avr2;
        }

        else if (DiffArray[_randIndex].targetforce == 1.0f && DiffArray[_randIndex].tolerance == 0.8f)
        {
            a3.Enqueue(z);
            if (jlalel[3] >= fai)
            {
                a3.Dequeue();
            }
            foreach (double y in a3)
            {
                sum3 += y;
            }
            avr3 = sum3 / a3.Count;
            tem = avr3;
        }

        else if (DiffArray[_randIndex].targetforce == 2.0f && DiffArray[_randIndex].tolerance == 0.5f)
        {
            a4.Enqueue(z);
            if (jlalel[4] >= fai)
            {
                a4.Dequeue();
            }
            foreach (double y in a4)
            {
                sum4 += y;
            }
            avr4 = sum4 / a4.Count;
            tem = avr4;
        }

        else if (DiffArray[_randIndex].targetforce == 2.0f && DiffArray[_randIndex].tolerance == 0.6f)
        {
            a5.Enqueue(z);
            if (jlalel[5] >= fai)
            {
                a5.Dequeue();
            }
            foreach (double y in a5)
            {
                sum5 += y;
            }
            avr5 = sum5 / a5.Count;
            tem = avr5;
        }

        else if (DiffArray[_randIndex].targetforce == 2.0f && DiffArray[_randIndex].tolerance == 0.7f)
        {
            a6.Enqueue(z);
            if (jlalel[6] >= fai)
            {
                a6.Dequeue();
            }
            foreach (double y in a6)
            {
                sum6 += y;
            }
            avr6 = sum6 / a6.Count;
            tem = avr6;
        }

        else if (DiffArray[_randIndex].targetforce == 2.0f && DiffArray[_randIndex].tolerance == 0.8f)
        {
            a7.Enqueue(z);
            if (jlalel[7] >= fai)
            {
                a7.Dequeue();
            }
            foreach (double y in a7)
            {
                sum7 += y;
            }
            avr7 = sum7 / a7.Count;
            tem = avr7;
        }

        else if (DiffArray[_randIndex].targetforce == 3.0f && DiffArray[_randIndex].tolerance == 0.5f)
        {
            a8.Enqueue(z);
            if (jlalel[8] >= fai)
            {
                a8.Dequeue();
            }
            foreach (double y in a8)
            {
                sum8 += y;
            }
            avr8 = sum8 / a8.Count;
            tem = avr8;
        }

        else if (DiffArray[_randIndex].targetforce == 3.0f && DiffArray[_randIndex].tolerance == 0.6f)
        {
            a9.Enqueue(z);
            if (jlalel[9] >= fai)
            {
                a9.Dequeue();
            }
            foreach (double y in a9)
            {
                sum9 += y;
            }
            avr9 = sum9 / a9.Count;
            tem = avr9;
        }

        else if (DiffArray[_randIndex].targetforce == 3.0f && DiffArray[_randIndex].tolerance == 0.7f)
        {
            a10.Enqueue(z);
            if (jlalel[10] >= fai)
            {
                a10.Dequeue();
            }
            foreach (double y in a10)
            {
                sum10 += y;
            }
            avr10 = sum10 / a10.Count;
            tem = avr10;
        }

        else if (DiffArray[_randIndex].targetforce == 3.0f && DiffArray[_randIndex].tolerance == 0.8f)
        {
            a11.Enqueue(z);
            if (jlalel[11] >= fai)
            {
                a11.Dequeue();
            }
            foreach (double y in a11)
            {
                sum11 += y;
            }
            avr11 = sum11 / a11.Count;
            tem = avr11;
        }

        else if (DiffArray[_randIndex].targetforce == 4.0f && DiffArray[_randIndex].tolerance == 0.5f)
        {
            a12.Enqueue(z);
            if (jlalel[12] >= fai)
            {
                a12.Dequeue();
            }
            foreach (double y in a12)
            {
                sum12 += y;
            }
            avr12 = sum12 / a12.Count;
            tem = avr12;
        }

        else if (DiffArray[_randIndex].targetforce == 4.0f && DiffArray[_randIndex].tolerance == 0.6f)
        {
            a13.Enqueue(z);
            if (jlalel[13] >= fai)
            {
                a13.Dequeue();
            }
            foreach (double y in a13)
            {
                sum13 += y;
            }
            avr13 = sum13 / a13.Count;
            tem = avr13;
        }

        else if (DiffArray[_randIndex].targetforce == 4.0f && DiffArray[_randIndex].tolerance == 0.7f)
        {
            a14.Enqueue(z);
            if (jlalel[14] >= fai)
            {
                a14.Dequeue();
            }
            foreach (double y in a14)
            {
                sum14 += y;
            }
            avr14 = sum14 / a14.Count;
            tem = avr14;
        }

        else if (DiffArray[_randIndex].targetforce == 4.0f && DiffArray[_randIndex].tolerance == 0.8f)
        {
            a15.Enqueue(z);
            if (jlalel[15] >= fai)
            {
                a15.Dequeue();
            }
            foreach (double y in a15)
            {
                sum15 += y;
            }
            avr15 = sum15 / a15.Count;
            tem = avr15;
        }
        return tem;
    }

    public static double getVariance(double Num)
    {

        double cas = 0;
        double Sum0 = 0;
        double Sum1 = 0;
        double Sum2 = 0;
        double Sum3 = 0;
        double Sum4 = 0;
        double Sum5 = 0;
        double Sum6 = 0;
        double Sum7 = 0;
        double Sum8 = 0;
        double Sum9 = 0;
        double Sum10 = 0;
        double Sum11 = 0;
        double Sum12 = 0;
        double Sum13 = 0;
        double Sum14 = 0;
        double Sum15 = 0;

        if (DiffArray[_randIndex].targetforce == 1.0f && DiffArray[_randIndex].tolerance == 0.5f)
        {
            b0.Enqueue(Num);
            if (jlalel[0] >= 5)
            {
                b0.Dequeue();
            }
            foreach (double y in b0)
            {
                Sum0 += (y - Eta) * (y - Eta);
            }
            Dev0 = Sum0 / b0.Count;
            cas = Dev0;
        }

        else if (DiffArray[_randIndex].targetforce == 1.0f && DiffArray[_randIndex].tolerance == 0.6f)
        {
            b1.Enqueue(Num);
            if (jlalel[1] >= 5)
            {
                b1.Dequeue();
            }
            foreach (double y in b1)
            {
                Sum1 += (y - Eta) * (y - Eta);
            }
            Dev1 = Sum1 / b1.Count;
            cas = Dev1;
        }

        else if (DiffArray[_randIndex].targetforce == 1.0f && DiffArray[_randIndex].tolerance == 0.7f)
        {
            b2.Enqueue(Num);
            if (jlalel[2] >= 5)
            {
                b2.Dequeue();
            }
            foreach (double y in b2)
            {
                Sum2 += (y - Eta) * (y - Eta);
            }
            Dev2 = Sum2 / b2.Count;
            cas = Dev2;
        }

        else if (DiffArray[_randIndex].targetforce == 1.0f && DiffArray[_randIndex].tolerance == 0.8f)
        {
            b3.Enqueue(Num);
            if (jlalel[3] >= 5)
            {
                b3.Dequeue();
            }
            foreach (double y in b3)
            {
                Sum3 += (y - Eta) * (y - Eta);
            }
            Dev3 = Sum3 / b3.Count;
            cas = Dev3;
        }

        else if (DiffArray[_randIndex].targetforce == 2.0f && DiffArray[_randIndex].tolerance == 0.5f)
        {
            b4.Enqueue(Num);
            if (jlalel[4] >= 5)
            {
                b4.Dequeue();
            }
            foreach (double y in b4)
            {
                Sum4 += (y - Eta) * (y - Eta);
            }
            Dev4 = Sum4 / b4.Count;
            cas = Dev4;
        }

        else if (DiffArray[_randIndex].targetforce == 2.0f && DiffArray[_randIndex].tolerance == 0.6f)
        {
            b5.Enqueue(Num);
            if (jlalel[5] >= 5)
            {
                b5.Dequeue();
            }
            foreach (double y in b5)
            {
                Sum5 += (y - Eta) * (y - Eta);
            }
            Dev5 = Sum5 / b5.Count;
            cas = Dev5;
        }

        else if (DiffArray[_randIndex].targetforce == 2.0f && DiffArray[_randIndex].tolerance == 0.7f)
        {
            b6.Enqueue(Num);
            if (jlalel[6] >= 5)
            {
                b6.Dequeue();
            }
            foreach (double y in b6)
            {
                Sum6 += (y - Eta) * (y - Eta);
            }
            Dev6 = Sum6 / b6.Count;
            cas = Dev6;
        }

        else if (DiffArray[_randIndex].targetforce == 2.0f && DiffArray[_randIndex].tolerance == 0.8f)
        {
            b7.Enqueue(Num);
            if (jlalel[7] >= 5)
            {
                b7.Dequeue();
            }
            foreach (double y in b7)
            {
                Sum7 += (y - Eta) * (y - Eta);
            }
            Dev7 = Sum7 / b7.Count;
            cas = Dev7;
        }

        else if (DiffArray[_randIndex].targetforce == 3.0f && DiffArray[_randIndex].tolerance == 0.5f)
        {
            b8.Enqueue(Num);
            if (jlalel[8] >= 5)
            {
                b8.Dequeue();
            }
            foreach (double y in b8)
            {
                Sum8 += (y - Eta) * (y - Eta);
            }
            Dev8 = Sum8 / b8.Count;
            cas = Dev8;
        }

        else if (DiffArray[_randIndex].targetforce == 3.0f && DiffArray[_randIndex].tolerance == 0.6f)
        {
            b9.Enqueue(Num);
            if (jlalel[9] >= 5)
            {
                b9.Dequeue();
            }
            foreach (double y in b9)
            {
                Sum9 += (y - Eta) * (y - Eta);
            }
            Dev9 = Sum9 / b9.Count;
            cas = Dev9;
        }

        else if (DiffArray[_randIndex].targetforce == 3.0f && DiffArray[_randIndex].tolerance == 0.7f)
        {
            b10.Enqueue(Num);
            if (jlalel[10] >= 5)
            {
                b10.Dequeue();
            }
            foreach (double y in b10)
            {
                Sum10 += (y - Eta) * (y - Eta);
            }
            Dev10 = Sum10 / b10.Count;
            cas = Dev10;
        }

        else if (DiffArray[_randIndex].targetforce == 3.0f && DiffArray[_randIndex].tolerance == 0.8f)
        {
            b11.Enqueue(Num);
            if (jlalel[11] >= 5)
            {
                b11.Dequeue();
            }
            foreach (double y in b11)
            {
                Sum11 += (y - Eta) * (y - Eta);
            }
            Dev11 = Sum11 / b11.Count;
            cas = Dev11;
        }

        else if (DiffArray[_randIndex].targetforce == 4.0f && DiffArray[_randIndex].tolerance == 0.5f)
        {
            b12.Enqueue(Num);
            if (jlalel[12] >= 5)
            {
                b12.Dequeue();
            }
            foreach (double y in b12)
            {
                Sum12 += (y - Eta) * (y - Eta);
            }
            Dev12 = Sum12 / b12.Count;
            cas = Dev12;
        }

        else if (DiffArray[_randIndex].targetforce == 4.0f && DiffArray[_randIndex].tolerance == 0.6f)
        {
            b13.Enqueue(Num);
            if (jlalel[13] >= 5)
            {
                b13.Dequeue();
            }
            foreach (double y in b13)
            {
                Sum13 += (y - Eta) * (y - Eta);
            }
            Dev13 = Sum13 / b13.Count;
            cas = Dev13;
        }

        else if (DiffArray[_randIndex].targetforce == 4.0f && DiffArray[_randIndex].tolerance == 0.7f)
        {
            b14.Enqueue(Num);
            if (jlalel[14] >= 5)
            {
                b14.Dequeue();
            }
            foreach (double y in b14)
            {
                Sum14 += (y - Eta) * (y - Eta);
            }
            Dev14 = Sum14 / b14.Count;
            cas = Dev14;
        }

        else if (DiffArray[_randIndex].targetforce == 4.0f && DiffArray[_randIndex].tolerance == 0.8f)
        {
            b15.Enqueue(Num);
            if (jlalel[15] >= 5)
            {
                b15.Dequeue();
            }
            foreach (double y in b15)
            {
                Sum15 += (y - Eta) * (y - Eta);
            }
            Dev15 = Sum15 / b15.Count;
            cas = Dev15;
        }
        return cas;
    }




    //private static void UpdateAllowableTime(bool bRet)
    //{
    //    Value _newelement = new Value();
    //    _newelement.targetforce = DiffArray[_randIndex].targetforce;
    //    _newelement.tolerance = DiffArray[_randIndex].tolerance;
    //    per = getAverage(GameControl.my);
    //    V = getVariance(per);
    //    _newelement.upperbound = (float)per;
    //    _newelement.lowerbound = (float)V;


    //    if (DiffArray[_randIndex].targetforce == 1.0f && DiffArray[_randIndex].tolerance == 0.5f)
    //    {
    //        jlalel[0]++;            
    //        if (per > 0.775)
    //        {
    //            if (bRet)
    //            {
    //                Arr0[jlalel[0]] = Arr0[jlalel[0] - 1] - Alpha * (per - Eta) - Gamma * (Arr0[jlalel[0] - 1] - GameControl.RT);
    //                if (Arr0[jlalel[0]] < 0.26)
    //                {
    //                    Arr0[jlalel[0]] = 0.26;
    //                }
    //            }
    //            else
    //            {
    //                Arr0[jlalel[0]] = Arr0[jlalel[0] - 1] - Alpha * (per - Eta) - (Epsilon * Arr0[jlalel[0] - 1] -  GameControl.RT);
    //                if (Arr0[jlalel[0]] < 0.26)
    //                {
    //                    Arr0[jlalel[0]] = 0.26;
    //                }
    //            }
    //        }
    //        else if (per < 0.725)
    //        {
    //            if (bRet)
    //            {
    //                Arr0[jlalel[0]] = Arr0[jlalel[0] - 1] - Beta * (per - Eta) - Gamma * (Arr0[jlalel[0] - 1] - GameControl.RT);
    //            }
    //            else
    //            {
    //                Arr0[jlalel[0]] = Arr0[jlalel[0] - 1] - Beta * (per - Eta) + (Arr0[jlalel[0] - 1] - Delte * GameControl.RT);
    //            }
    //        }
    //        else if (Math.Abs(per - Eta) <= 0.025)
    //        {
    //            //Arr0[jlalel[0]] = Arr0[jlalel[0] - 1];
    //            if (bRet)  // if success
    //            {
    //                consecutive[0]++;
    //                if (consecutive[0] >= 3)
    //                {
    //                    consecutive[0] = 0;
    //                    Arr0[jlalel[0]] = Arr0[jlalel[0] - 1] - Omega * (Arr0[jlalel[0] - 1] - GameControl.RT);
    //                    if (Arr0[jlalel[0]] < 0.26)
    //                    {
    //                        Arr0[jlalel[0]] = 0.26;
    //                    }
    //                }
    //                else
    //                {
    //                    Arr0[jlalel[0]] = Arr0[jlalel[0] - 1];
    //                }
    //            }
    //            else // if failed
    //            {
    //                consecutive[0] = 0;
    //                Arr0[jlalel[0]] = Arr0[jlalel[0] - 1] + (Arr0[jlalel[0] - 1] - Omega * GameControl.RT);
    //            }
    //        }
    //        _newelement.newDeadline = (float)Arr0[jlalel[0]];///******            
    //    }

    //    else if (DiffArray[_randIndex].targetforce == 1.0f && DiffArray[_randIndex].tolerance == 0.6f)
    //    {
    //        jlalel[1]++;
    //        if (per > 0.775)
    //        {
    //            if (bRet)
    //            {
    //                Arr1[jlalel[1]] = Arr1[jlalel[1] - 1] - Alpha * (per - Eta) - Gamma * (Arr1[jlalel[1] - 1] - GameControl.RT);
    //                if (Arr1[jlalel[1]] < 0.23)
    //                {
    //                    Arr1[jlalel[1]] = 0.23;
    //                }
    //            }
    //            else
    //            {
    //                Arr1[jlalel[1]] = Arr1[jlalel[1] - 1] - Alpha * (per - Eta) - (Epsilon * Arr1[jlalel[1] - 1] - GameControl.RT);
    //                if (Arr1[jlalel[1]] < 0.23)
    //                {
    //                    Arr1[jlalel[1]] = 0.23;
    //                }
    //            }
    //        }
    //        else if (per < 0.725)
    //        {
    //            if (bRet)
    //            {
    //                Arr1[jlalel[1]] = Arr1[jlalel[1] - 1] - Beta * (per - Eta) - Gamma * (Arr1[jlalel[1] - 1] - GameControl.RT);
    //            }
    //            else
    //            {
    //                Arr1[jlalel[1]] = Arr1[jlalel[1] - 1] - Beta * (per - Eta) + (Arr1[jlalel[1] - 1] - Delte * GameControl.RT);
    //            }
    //        }
    //        else if (Math.Abs(per - Eta) <= 0.025)
    //        {
    //            //Arr1[jlalel[1]] = Arr1[jlalel[1] - 1];
    //            if (bRet)  // if success
    //            {
    //                consecutive[1]++;
    //                if (consecutive[1] >= 3)
    //                {
    //                    consecutive[1] = 0;
    //                    Arr1[jlalel[1]] = Arr1[jlalel[1] - 1] - Omega * (Arr1[jlalel[1] - 1] - GameControl.RT);
    //                    if (Arr1[jlalel[1]] < 0.23)
    //                    {
    //                        Arr1[jlalel[1]] = 0.23;
    //                    }
    //                }
    //                else
    //                {
    //                    Arr1[jlalel[1]] = Arr1[jlalel[1] - 1];
    //                }
    //            }
    //            else // if failed
    //            {
    //                consecutive[1] = 0;
    //                Arr1[jlalel[1]] = Arr1[jlalel[1] - 1] + (Arr1[jlalel[1] - 1] - Omega * GameControl.RT);
    //            }
    //        }
    //        _newelement.newDeadline = (float)Arr1[jlalel[1]];///****** 
    //    }

    //    else if (DiffArray[_randIndex].targetforce == 1.0f && DiffArray[_randIndex].tolerance == 0.7f)
    //    {
    //        jlalel[2]++;
    //        if (per > 0.775)
    //        {
    //            if (bRet)
    //            {
    //                Arr2[jlalel[2]] = Arr2[jlalel[2] - 1] - Alpha * (per - Eta) - Gamma * (Arr2[jlalel[2] - 1] - GameControl.RT);
    //                if (Arr2[jlalel[2]] < 0.21)
    //                {
    //                    Arr2[jlalel[2]] = 0.21;
    //                }
    //            }
    //            else
    //            {
    //                Arr2[jlalel[2]] = Arr2[jlalel[2] - 1] - Alpha * (per - Eta) - (Epsilon * Arr2[jlalel[2] - 1] - GameControl.RT);
    //                if (Arr2[jlalel[2]] < 0.21)
    //                {
    //                    Arr2[jlalel[2]] = 0.21;
    //                }
    //            }
    //        }
    //        else if (per < 0.725)
    //        {
    //            if (bRet)
    //            {
    //                Arr2[jlalel[2]] = Arr2[jlalel[2] - 1] - Beta * (per - Eta) - Gamma * (Arr2[jlalel[2] - 1] - GameControl.RT);
    //            }
    //            else
    //            {
    //                Arr2[jlalel[2]] = Arr2[jlalel[2] - 1] - Beta * (per - Eta) + (Arr2[jlalel[2] - 1] - Delte * GameControl.RT);
    //            }
    //        }
    //        else if (Math.Abs(per - Eta) <= 0.025)
    //        {
    //            //Arr2[jlalel[2]] = Arr2[jlalel[2] - 1];
    //            if (bRet)  // if success
    //            {
    //                consecutive[2]++;
    //                if (consecutive[2] >= 3)
    //                {
    //                    consecutive[2] = 0;
    //                    Arr2[jlalel[2]] = Arr2[jlalel[2] - 1] - Omega * (Arr2[jlalel[2] - 1] - GameControl.RT);
    //                    if (Arr2[jlalel[2]] < 0.21)
    //                    {
    //                        Arr2[jlalel[2]] = 0.21;
    //                    }
    //                }
    //                else
    //                {
    //                    Arr2[jlalel[2]] = Arr2[jlalel[2] - 1];
    //                }
    //            }
    //            else // if failed
    //            {
    //                consecutive[2] = 0;
    //                Arr2[jlalel[2]] = Arr2[jlalel[2] - 1] + (Arr2[jlalel[2] - 1] - Omega * GameControl.RT);
    //            }
    //        }
    //        _newelement.newDeadline = (float)Arr2[jlalel[2]];///****** 
    //    }

    //    else if (DiffArray[_randIndex].targetforce == 1.0f && DiffArray[_randIndex].tolerance == 0.8f)
    //    {
    //        jlalel[3]++;
    //        if (per > 0.775)
    //        {
    //            if (bRet)
    //            {
    //                Arr3[jlalel[3]] = Arr3[jlalel[3] - 1] - Alpha * (per - Eta) - Gamma * (Arr3[jlalel[3] - 1] - GameControl.RT);
    //                if (Arr3[jlalel[3]] < 0.19)
    //                {
    //                    Arr3[jlalel[3]] = 0.19;
    //                }
    //            }
    //            else
    //            {
    //                Arr3[jlalel[3]] = Arr3[jlalel[3] - 1] - Alpha * (per - Eta) - (Epsilon * Arr3[jlalel[3] - 1] - GameControl.RT);
    //                if (Arr3[jlalel[3]] < 0.19)
    //                {
    //                    Arr3[jlalel[3]] = 0.19;
    //                }
    //            }
    //        }
    //        else if (per < 0.725)
    //        {
    //            if (bRet)
    //            {
    //                Arr3[jlalel[3]] = Arr3[jlalel[3] - 1] - Beta * (per - Eta) - Gamma * (Arr3[jlalel[3] - 1] - GameControl.RT);
    //            }
    //            else
    //            {
    //                Arr3[jlalel[3]] = Arr3[jlalel[3] - 1] - Beta * (per - Eta) + (Arr3[jlalel[3] - 1] - Delte * GameControl.RT);
    //            }
    //        }
    //        else if (Math.Abs(per - Eta) <= 0.025)
    //        {
    //            //Arr3[jlalel[3]] = Arr3[jlalel[3] - 1];
    //            if (bRet)  // if success
    //            {
    //                consecutive[3]++;
    //                if (consecutive[3] >= 3)
    //                {
    //                    consecutive[3] = 0;
    //                    Arr3[jlalel[3]] = Arr3[jlalel[3] - 1] - Omega * (Arr3[jlalel[3] - 1] - GameControl.RT);
    //                    if (Arr3[jlalel[3]] < 0.19)
    //                    {
    //                        Arr3[jlalel[3]] = 0.19;
    //                    }
    //                }
    //                else
    //                {
    //                    Arr3[jlalel[3]] = Arr3[jlalel[3] - 1];
    //                }
    //            }
    //            else // if failed
    //            {
    //                consecutive[3] = 0;
    //                Arr3[jlalel[3]] = Arr3[jlalel[3] - 1] + (Arr3[jlalel[3] - 1] - Omega * GameControl.RT);
    //            }
    //        }
    //        _newelement.newDeadline = (float)Arr3[jlalel[3]];///****** 
    //    }

    //    else if (DiffArray[_randIndex].targetforce == 2.0f && DiffArray[_randIndex].tolerance == 0.5f)
    //    {
    //        jlalel[4]++;
    //        if (per > 0.775)
    //        {
    //            if (bRet)
    //            {
    //                Arr4[jlalel[4]] = Arr4[jlalel[4] - 1] - Alpha * (per - Eta) - Gamma * (Arr4[jlalel[4] - 1] - GameControl.RT);
    //                if (Arr4[jlalel[4]] < 0.32)
    //                {
    //                    Arr4[jlalel[4]] = 0.32;
    //                }
    //            }
    //            else
    //            {
    //                Arr4[jlalel[4]] = Arr4[jlalel[4] - 1] - Alpha * (per - Eta) - (Epsilon * Arr4[jlalel[4] - 1] - GameControl.RT);
    //                if (Arr4[jlalel[4]] < 0.32)
    //                {
    //                    Arr4[jlalel[4]] = 0.32;
    //                }
    //            }
    //        }
    //        else if (per < 0.725)
    //        {
    //            if (bRet)
    //            {
    //                Arr4[jlalel[4]] = Arr4[jlalel[4] - 1] - Beta * (per - Eta) - Gamma * (Arr4[jlalel[4] - 1] - GameControl.RT);
    //            }
    //            else
    //            {
    //                Arr4[jlalel[4]] = Arr4[jlalel[4] - 1] - Beta * (per - Eta) + (Arr4[jlalel[4] - 1] - Delte * GameControl.RT);
    //            }
    //        }
    //        else if (Math.Abs(per - Eta) <= 0.025)
    //        {
    //            //Arr4[jlalel[4]] = Arr4[jlalel[4] - 1];
    //            if (bRet)  // if success
    //            {
    //                consecutive[4]++;
    //                if (consecutive[4] >= 3)
    //                {
    //                    consecutive[4] = 0;
    //                    Arr4[jlalel[4]] = Arr4[jlalel[4] - 1] - Omega * (Arr4[jlalel[4] - 1] - GameControl.RT);
    //                    if (Arr4[jlalel[4]] < 0.32)
    //                    {
    //                        Arr4[jlalel[4]] = 0.32;
    //                    }
    //                }
    //                else
    //                {
    //                    Arr4[jlalel[4]] = Arr4[jlalel[4] - 1];
    //                }
    //            }
    //            else // if failed
    //            {
    //                consecutive[4] = 0;
    //                Arr4[jlalel[4]] = Arr4[jlalel[4] - 1] + (Arr4[jlalel[4] - 1] - Omega * GameControl.RT);
    //            }
    //        }
    //        _newelement.newDeadline = (float)Arr4[jlalel[4]];///****** 
    //    }

    //    else if (DiffArray[_randIndex].targetforce == 2.0f && DiffArray[_randIndex].tolerance == 0.6f)
    //    {
    //        jlalel[5]++;
    //        if (per > 0.775)
    //        {
    //            if (bRet)
    //            {
    //                Arr5[jlalel[5]] = Arr5[jlalel[5] - 1] - Alpha * (per - Eta) - Gamma * (Arr5[jlalel[5] - 1] - GameControl.RT);
    //                if (Arr5[jlalel[5]] < 0.29)
    //                {
    //                    Arr5[jlalel[5]] = 0.29;
    //                }
    //            }
    //            else
    //            {
    //                Arr5[jlalel[5]] = Arr5[jlalel[5] - 1] - Alpha * (per - Eta) - (Epsilon * Arr5[jlalel[5] - 1] - GameControl.RT);
    //                if (Arr5[jlalel[5]] < 0.29)
    //                {
    //                    Arr5[jlalel[5]] = 0.29;
    //                }
    //            }
    //        }
    //        else if (per < 0.725)
    //        {
    //            if (bRet)
    //            {
    //                Arr5[jlalel[5]] = Arr5[jlalel[5] - 1] - Beta * (per - Eta) - Gamma * (Arr5[jlalel[5] - 1] - GameControl.RT);
    //            }
    //            else
    //            {
    //                Arr5[jlalel[5]] = Arr5[jlalel[5] - 1] - Beta * (per - Eta) + (Arr5[jlalel[5] - 1] - Delte * GameControl.RT);
    //            }
    //        }
    //        else if (Math.Abs(per - Eta) <= 0.025)
    //        {
    //            //Arr5[jlalel[5]] = Arr5[jlalel[5] - 1];
    //            if (bRet)  // if success
    //            {
    //                consecutive[5]++;
    //                if (consecutive[5] >= 3)
    //                {
    //                    consecutive[5] = 0;
    //                    Arr5[jlalel[5]] = Arr5[jlalel[5] - 1] - Omega * (Arr5[jlalel[5] - 1] - GameControl.RT);
    //                    if (Arr5[jlalel[5]] < 0.29)
    //                    {
    //                        Arr5[jlalel[5]] = 0.29;
    //                    }
    //                }
    //                else
    //                {
    //                    Arr5[jlalel[5]] = Arr5[jlalel[5] - 1];
    //                }
    //            }
    //            else // if failed
    //            {
    //                consecutive[5] = 0;
    //                Arr5[jlalel[5]] = Arr5[jlalel[5] - 1] + (Arr5[jlalel[5] - 1] - Omega * GameControl.RT);
    //            }
    //        }
    //        _newelement.newDeadline = (float)Arr5[jlalel[5]];///****** 
    //    }

    //    else if (DiffArray[_randIndex].targetforce == 2.0f && DiffArray[_randIndex].tolerance == 0.7f)
    //    {
    //        jlalel[6]++;
    //        if (per > 0.775)
    //        {
    //            if (bRet)
    //            {
    //                Arr6[jlalel[6]] = Arr6[jlalel[6] - 1] - Alpha * (per - Eta) - Gamma * (Arr6[jlalel[6] - 1] - GameControl.RT);
    //                if (Arr6[jlalel[6]] < 0.26)
    //                {
    //                    Arr6[jlalel[6]] = 0.26;
    //                }
    //            }
    //            else
    //            {
    //                Arr6[jlalel[6]] = Arr6[jlalel[6] - 1] - Alpha * (per - Eta) - (Epsilon * Arr6[jlalel[6] - 1] - GameControl.RT);
    //                if (Arr6[jlalel[6]] < 0.26)
    //                {
    //                    Arr6[jlalel[6]] = 0.26;
    //                }
    //            }
    //        }
    //        else if (per < 0.725)
    //        {
    //            if (bRet)
    //            {
    //                Arr6[jlalel[6]] = Arr6[jlalel[6] - 1] - Beta * (per - Eta) - Gamma * (Arr6[jlalel[6] - 1] - GameControl.RT);
    //            }
    //            else
    //            {
    //                Arr6[jlalel[6]] = Arr6[jlalel[6] - 1] - Beta * (per - Eta) + (Arr6[jlalel[6] - 1] - Delte * GameControl.RT);
    //            }
    //        }
    //        else if (Math.Abs(per - Eta) <= 0.025)
    //        {
    //            //Arr6[jlalel[6]] = Arr6[jlalel[6] - 1];
    //            if (bRet)  // if success
    //            {
    //                consecutive[6]++;
    //                if (consecutive[6] >= 3)
    //                {
    //                    consecutive[6] = 0;
    //                    Arr6[jlalel[6]] = Arr6[jlalel[6] - 1] - Omega * (Arr6[jlalel[6] - 1] - GameControl.RT);
    //                    if (Arr6[jlalel[6]] < 0.26)
    //                    {
    //                        Arr6[jlalel[6]] = 0.26;
    //                    }
    //                }
    //                else
    //                {
    //                    Arr6[jlalel[6]] = Arr6[jlalel[6] - 1];
    //                }
    //            }
    //            else // if failed
    //            {
    //                consecutive[6] = 0;
    //                Arr6[jlalel[6]] = Arr6[jlalel[6] - 1] + (Arr6[jlalel[6] - 1] - Omega * GameControl.RT);
    //            }
    //        }
    //        _newelement.newDeadline = (float)Arr6[jlalel[6]];///****** 
    //    }

    //    else if (DiffArray[_randIndex].targetforce == 2.0f && DiffArray[_randIndex].tolerance == 0.8f)
    //    {
    //        jlalel[7]++;
    //        if (per > 0.775)
    //        {
    //            if (bRet)
    //            {
    //                Arr7[jlalel[7]] = Arr7[jlalel[7] - 1] - Alpha * (per - Eta) - Gamma * (Arr7[jlalel[7] - 1] - GameControl.RT);
    //                if (Arr7[jlalel[7]] < 0.24)
    //                {
    //                    Arr7[jlalel[7]] = 0.24;
    //                }
    //            }
    //            else
    //            {
    //                Arr7[jlalel[7]] = Arr7[jlalel[7] - 1] - Alpha * (per - Eta) - (Epsilon * Arr7[jlalel[7] - 1] - GameControl.RT);
    //                if (Arr7[jlalel[7]] < 0.24)
    //                {
    //                    Arr7[jlalel[7]] = 0.24;
    //                }
    //            }
    //        }
    //        else if (per < 0.725)
    //        {
    //            if (bRet)
    //            {
    //                Arr7[jlalel[7]] = Arr7[jlalel[7] - 1] - Beta * (per - Eta) - Gamma * (Arr7[jlalel[7] - 1] - GameControl.RT);
    //            }
    //            else
    //            {
    //                Arr7[jlalel[7]] = Arr7[jlalel[7] - 1] - Beta * (per - Eta) + (Arr7[jlalel[7] - 1] - Delte * GameControl.RT);
    //            }
    //        }
    //        else if (Math.Abs(per - Eta) <= 0.025)
    //        {
    //            //Arr7[jlalel[7]] = Arr7[jlalel[7] - 1];
    //            if (bRet)  // if success
    //            {
    //                consecutive[7]++;
    //                if (consecutive[7] >= 3)
    //                {
    //                    consecutive[7] = 0;
    //                    Arr7[jlalel[7]] = Arr7[jlalel[7] - 1] - Omega * (Arr7[jlalel[7] - 1] - GameControl.RT);
    //                    if (Arr7[jlalel[7]] < 0.24)
    //                    {
    //                        Arr7[jlalel[7]] = 0.24;
    //                    }
    //                }
    //                else
    //                {
    //                    Arr7[jlalel[7]] = Arr7[jlalel[7] - 1];
    //                }
    //            }
    //            else // if failed
    //            {
    //                consecutive[7] = 0;
    //                Arr7[jlalel[7]] = Arr7[jlalel[7] - 1] + (Arr7[jlalel[7] - 1] - Omega * GameControl.RT);
    //            }
    //        }
    //        _newelement.newDeadline = (float)Arr7[jlalel[7]];///****** 
    //    }

    //    else if (DiffArray[_randIndex].targetforce == 3.0f && DiffArray[_randIndex].tolerance == 0.5f)
    //    {
    //        jlalel[8]++;
    //        if (per > 0.775)
    //        {
    //            if (bRet)
    //            {
    //                Arr8[jlalel[8]] = Arr8[jlalel[8] - 1] - Alpha * (per - Eta) - Gamma * (Arr8[jlalel[8] - 1] - GameControl.RT);
    //                if (Arr8[jlalel[8]] < 0.41)
    //                {
    //                    Arr8[jlalel[8]] = 0.41;
    //                }
    //            }
    //            else
    //            {
    //                Arr8[jlalel[8]] = Arr8[jlalel[8] - 1] - Alpha * (per - Eta) - (Epsilon * Arr8[jlalel[8] - 1] - GameControl.RT);
    //                if (Arr8[jlalel[8]] < 0.41)
    //                {
    //                    Arr8[jlalel[8]] = 0.41;
    //                }
    //            }
    //        }
    //        else if (per < 0.725)
    //        {
    //            if (bRet)
    //            {
    //                Arr8[jlalel[8]] = Arr8[jlalel[8] - 1] - Beta * (per - Eta) - Gamma * (Arr8[jlalel[8] - 1] - GameControl.RT);
    //            }
    //            else
    //            {
    //                Arr8[jlalel[8]] = Arr8[jlalel[8] - 1] - Beta * (per - Eta) + (Arr8[jlalel[8] - 1] - Delte * GameControl.RT);
    //            }
    //        }
    //        else if (Math.Abs(per - Eta) <= 0.025)
    //        {
    //            //Arr8[jlalel[8]] = Arr8[jlalel[8] - 1];
    //            if (bRet)  // if success
    //            {
    //                consecutive[8]++;
    //                if (consecutive[8] >= 3)
    //                {
    //                    consecutive[8] = 0;
    //                    Arr8[jlalel[8]] = Arr8[jlalel[8] - 1] - Omega * (Arr8[jlalel[8] - 1] - GameControl.RT);
    //                    if (Arr8[jlalel[8]] < 0.41)
    //                    {
    //                        Arr8[jlalel[8]] = 0.41;
    //                    }
    //                }
    //                else
    //                {
    //                    Arr8[jlalel[8]] = Arr8[jlalel[8] - 1];
    //                }
    //            }
    //            else // if failed
    //            {
    //                consecutive[8] = 0;
    //                Arr8[jlalel[8]] = Arr8[jlalel[8] - 1] + (Arr8[jlalel[8] - 1] - Omega * GameControl.RT);
    //            }
    //        }
    //        _newelement.newDeadline = (float)Arr8[jlalel[8]];///****** 
    //    }

    //    else if (DiffArray[_randIndex].targetforce == 3.0f && DiffArray[_randIndex].tolerance == 0.6f)
    //    {
    //        jlalel[9]++;
    //        if (per > 0.775)
    //        {
    //            if (bRet)
    //            {
    //                Arr9[jlalel[9]] = Arr9[jlalel[9] - 1] - Alpha * (per - Eta) - Gamma * (Arr9[jlalel[9] - 1] - GameControl.RT);
    //                if (Arr9[jlalel[9]] < 0.38)
    //                {
    //                    Arr9[jlalel[9]] = 0.38;
    //                }
    //            }
    //            else
    //            {
    //                Arr9[jlalel[9]] = Arr9[jlalel[9] - 1] - Alpha * (per - Eta) - (Epsilon * Arr9[jlalel[9] - 1] - GameControl.RT);
    //                if (Arr9[jlalel[9]] < 0.38)
    //                {
    //                    Arr9[jlalel[9]] = 0.38;
    //                }
    //            }
    //        }
    //        else if (per < 0.725)
    //        {
    //            if (bRet)
    //            {
    //                Arr9[jlalel[9]] = Arr9[jlalel[9] - 1] - Beta * (per - Eta) - Gamma * (Arr9[jlalel[9] - 1] - GameControl.RT);
    //            }
    //            else
    //            {
    //                Arr9[jlalel[9]] = Arr9[jlalel[9] - 1] - Beta * (per - Eta) + (Arr9[jlalel[9] - 1] - Delte * GameControl.RT);
    //            }
    //        }
    //        else if (Math.Abs(per - Eta) <= 0.025)
    //        {
    //            //Arr9[jlalel[9]] = Arr9[jlalel[9] - 1];
    //            if (bRet)  // if success
    //            {
    //                consecutive[9]++;
    //                if (consecutive[9] >= 3)
    //                {
    //                    consecutive[9] = 0;
    //                    Arr9[jlalel[9]] = Arr9[jlalel[9] - 1] - Omega * (Arr9[jlalel[9] - 1] - GameControl.RT);
    //                    if (Arr9[jlalel[9]] < 0.38)
    //                    {
    //                        Arr9[jlalel[9]] = 0.38;
    //                    }
    //                }
    //                else
    //                {
    //                    Arr9[jlalel[9]] = Arr9[jlalel[9] - 1];
    //                }
    //            }
    //            else // if failed
    //            {
    //                consecutive[9] = 0;
    //                Arr9[jlalel[9]] = Arr9[jlalel[9] - 1] + (Arr9[jlalel[9] - 1] - Omega * GameControl.RT);
    //            }
    //        }
    //        _newelement.newDeadline = (float)Arr9[jlalel[9]];///******           
    //    }

    //    else if (DiffArray[_randIndex].targetforce == 3.0f && DiffArray[_randIndex].tolerance == 0.7f)
    //    {
    //        jlalel[10]++;
    //        if (per > 0.775)
    //        {
    //            if (bRet)
    //            {
    //                Arr10[jlalel[10]] = Arr10[jlalel[10] - 1] - Alpha * (per - Eta) - Gamma * (Arr10[jlalel[10] - 1] - GameControl.RT);
    //                if (Arr10[jlalel[10]] < 0.35)
    //                {
    //                    Arr10[jlalel[10]] = 0.35;
    //                }
    //            }
    //            else
    //            {
    //                Arr10[jlalel[10]] = Arr10[jlalel[10] - 1] - Alpha * (per - Eta) - (Epsilon * Arr10[jlalel[10] - 1] - GameControl.RT);
    //                if (Arr10[jlalel[10]] < 0.35)
    //                {
    //                    Arr10[jlalel[10]] = 0.35;
    //                }
    //            }
    //        }
    //        else if (per < 0.725)
    //        {
    //            if (bRet)
    //            {
    //                Arr10[jlalel[10]] = Arr10[jlalel[10] - 1] - Beta * (per - Eta) - Gamma * (Arr10[jlalel[10] - 1] - GameControl.RT);
    //            }
    //            else
    //            {
    //                Arr10[jlalel[10]] = Arr10[jlalel[10] - 1] - Beta * (per - Eta) + (Arr10[jlalel[10] - 1] - Delte * GameControl.RT);
    //            }
    //        }
    //        else if (Math.Abs(per - Eta) <= 0.025)
    //        {
    //            //Arr10[jlalel[10]] = Arr10[jlalel[10] - 1];
    //            if (bRet)  // if success
    //            {
    //                consecutive[10]++;
    //                if (consecutive[10] >= 3)
    //                {
    //                    consecutive[10] = 0;
    //                    Arr10[jlalel[10]] = Arr10[jlalel[10] - 1] - Omega * (Arr10[jlalel[10] - 1] - GameControl.RT);
    //                    if (Arr10[jlalel[10]] < 0.35)
    //                    {
    //                        Arr10[jlalel[10]] = 0.35;
    //                    }
    //                }
    //                else
    //                {
    //                    Arr10[jlalel[10]] = Arr10[jlalel[10] - 1];
    //                }
    //            }
    //            else // if failed
    //            {
    //                consecutive[10] = 0;
    //                Arr10[jlalel[10]] = Arr10[jlalel[10] - 1] + (Arr10[jlalel[10] - 1] - Omega * GameControl.RT);
    //            }
    //        }
    //        _newelement.newDeadline = (float)Arr10[jlalel[10]];///******            
    //    }

    //    else if (DiffArray[_randIndex].targetforce == 3.0f && DiffArray[_randIndex].tolerance == 0.8f)
    //    {
    //        jlalel[11]++;
    //        if (per > 0.775)
    //        {
    //            if (bRet)
    //            {
    //                Arr11[jlalel[11]] = Arr11[jlalel[11] - 1] - Alpha * (per - Eta) - Gamma * (Arr11[jlalel[11] - 1] - GameControl.RT);
    //                if (Arr11[jlalel[11]] < 0.33)
    //                {
    //                    Arr11[jlalel[11]] = 0.33;
    //                }
    //            }
    //            else
    //            {
    //                Arr11[jlalel[11]] = Arr11[jlalel[11] - 1] - Alpha * (per - Eta) - (Epsilon * Arr11[jlalel[11] - 1] - GameControl.RT);
    //                if (Arr11[jlalel[11]] < 0.33)
    //                {
    //                    Arr11[jlalel[11]] = 0.33;
    //                }
    //            }
    //        }
    //        else if (per < 0.725)
    //        {
    //            if (bRet)
    //            {
    //                Arr11[jlalel[11]] = Arr11[jlalel[11] - 1] - Beta * (per - Eta) - Gamma * (Arr11[jlalel[11] - 1] - GameControl.RT);
    //            }
    //            else
    //            {
    //                Arr11[jlalel[11]] = Arr11[jlalel[11] - 1] - Beta * (per - Eta) + (Arr11[jlalel[11] - 1] - Delte * GameControl.RT);
    //            }
    //        }
    //        else if (Math.Abs(per - Eta) <= 0.025)
    //        {
    //            //Arr11[jlalel[11]] = Arr11[jlalel[11] - 1];
    //            if (bRet)  // if success
    //            {
    //                consecutive[11]++;
    //                if (consecutive[11] >= 3)
    //                {
    //                    consecutive[11] = 0;
    //                    Arr11[jlalel[11]] = Arr11[jlalel[11] - 1] - Omega * (Arr11[jlalel[11] - 1] - GameControl.RT);
    //                    if (Arr11[jlalel[11]] < 0.33)
    //                    {
    //                        Arr11[jlalel[11]] = 0.33;
    //                    }
    //                }
    //                else
    //                {
    //                    Arr11[jlalel[11]] = Arr11[jlalel[11] - 1];
    //                }
    //            }
    //            else // if failed
    //            {
    //                consecutive[11] = 0;
    //                Arr11[jlalel[11]] = Arr11[jlalel[11] - 1] + (Arr11[jlalel[11] - 1] - Omega * GameControl.RT);
    //            }
    //        }
    //        _newelement.newDeadline = (float)Arr11[jlalel[11]];///******            
    //    }

    //    else if (DiffArray[_randIndex].targetforce == 4.0f && DiffArray[_randIndex].tolerance == 0.5f)
    //    {
    //        jlalel[12]++;
    //        if (per > 0.775)
    //        {
    //            if (bRet)
    //            {
    //                Arr12[jlalel[12]] = Arr12[jlalel[12] - 1] - Alpha * (per - Eta) - Gamma * (Arr12[jlalel[12] - 1] - GameControl.RT);
    //                if (Arr12[jlalel[12]] < 0.45)
    //                {
    //                    Arr12[jlalel[12]] = 0.45;
    //                }
    //            }
    //            else
    //            {
    //                Arr12[jlalel[12]] = Arr12[jlalel[12] - 1] - Alpha * (per - Eta) - (Epsilon * Arr12[jlalel[12] - 1] - GameControl.RT);
    //                if (Arr12[jlalel[12]] < 0.45)
    //                {
    //                    Arr12[jlalel[12]] = 0.45;
    //                }
    //            }
    //        }
    //        else if (per < 0.725)
    //        {
    //            if (bRet)
    //            {
    //                Arr12[jlalel[12]] = Arr12[jlalel[12] - 1] - Beta * (per - Eta) - Gamma * (Arr12[jlalel[12] - 1] - GameControl.RT);
    //            }
    //            else
    //            {
    //                Arr12[jlalel[12]] = Arr12[jlalel[12] - 1] - Beta * (per - Eta) + (Arr12[jlalel[12] - 1] - Delte * GameControl.RT);
    //            }
    //        }
    //        else if (Math.Abs(per - Eta) <= 0.025)
    //        {
    //            //Arr12[jlalel[12]] = Arr12[jlalel[12] - 1];
    //            if (bRet)  // if success
    //            {
    //                consecutive[12]++;
    //                if (consecutive[12] >= 3)
    //                {
    //                    consecutive[12] = 0;
    //                    Arr12[jlalel[12]] = Arr12[jlalel[12] - 1] - Omega * (Arr12[jlalel[12] - 1] - GameControl.RT);
    //                    if (Arr12[jlalel[12]] < 0.45)
    //                    {
    //                        Arr12[jlalel[12]] = 0.45;
    //                    }
    //                }
    //                else
    //                {
    //                    Arr12[jlalel[12]] = Arr12[jlalel[12] - 1];
    //                }
    //            }
    //            else // if failed
    //            {
    //                consecutive[12] = 0;
    //                Arr12[jlalel[12]] = Arr12[jlalel[12] - 1] + (Arr12[jlalel[12] - 1] - Omega * GameControl.RT);
    //            }
    //        }
    //        _newelement.newDeadline = (float)Arr12[jlalel[12]];///******             
    //    }

    //    else if (DiffArray[_randIndex].targetforce == 4.0f && DiffArray[_randIndex].tolerance == 0.6f)
    //    {
    //        jlalel[13]++;
    //        if (per > 0.775)
    //        {
    //            if (bRet)
    //            {
    //                Arr13[jlalel[13]] = Arr13[jlalel[13] - 1] - Alpha * (per - Eta) - Gamma * (Arr13[jlalel[13] - 1] - GameControl.RT);
    //                if (Arr13[jlalel[13]] < 0.40)
    //                {
    //                    Arr13[jlalel[13]] = 0.40;
    //                }
    //            }
    //            else
    //            {
    //                Arr13[jlalel[13]] = Arr13[jlalel[13] - 1] - Alpha * (per - Eta) - (Epsilon * Arr13[jlalel[13] - 1] - GameControl.RT);
    //                if (Arr13[jlalel[13]] < 0.40)
    //                {
    //                    Arr13[jlalel[13]] = 0.40;
    //                }
    //            }
    //        }
    //        else if (per < 0.725)
    //        {
    //            if (bRet)
    //            {
    //                Arr13[jlalel[13]] = Arr13[jlalel[13] - 1] - Beta * (per - Eta) - Gamma * (Arr13[jlalel[13] - 1] - GameControl.RT);
    //            }
    //            else
    //            {
    //                Arr13[jlalel[13]] = Arr13[jlalel[13] - 1] - Beta * (per - Eta) + (Arr13[jlalel[13] - 1] - Delte * GameControl.RT);
    //            }
    //        }
    //        else if (Math.Abs(per - Eta) <= 0.025)
    //        {
    //            //Arr13[jlalel[13]] = Arr13[jlalel[13] - 1];
    //            if (bRet)  // if success
    //            {
    //                consecutive[13]++;
    //                if (consecutive[13] >= 3)
    //                {
    //                    consecutive[13] = 0;
    //                    Arr13[jlalel[13]] = Arr13[jlalel[13] - 1] - Omega * (Arr13[jlalel[13] - 1] - GameControl.RT);
    //                    if (Arr13[jlalel[13]] < 0.40)
    //                    {
    //                        Arr13[jlalel[13]] = 0.40;
    //                    }
    //                }
    //                else
    //                {
    //                    Arr13[jlalel[13]] = Arr13[jlalel[13] - 1];
    //                }
    //            }
    //            else // if failed
    //            {
    //                consecutive[13] = 0;
    //                Arr13[jlalel[13]] = Arr13[jlalel[13] - 1] + (Arr13[jlalel[13] - 1] - Omega * GameControl.RT);
    //            }
    //        }
    //        _newelement.newDeadline = (float)Arr13[jlalel[13]];///******            
    //    }

    //    else if (DiffArray[_randIndex].targetforce == 4.0f && DiffArray[_randIndex].tolerance == 0.7f)
    //    {
    //        jlalel[14]++;
    //        if (per > 0.775)
    //        {
    //            if (bRet)
    //            {
    //                Arr14[jlalel[14]] = Arr14[jlalel[14] - 1] - Alpha * (per - Eta) - Gamma * (Arr14[jlalel[14] - 1] - GameControl.RT);
    //                if (Arr14[jlalel[14]] < 0.38)
    //                {
    //                    Arr14[jlalel[14]] = 0.38;
    //                }
    //            }
    //            else
    //            {
    //                Arr14[jlalel[14]] = Arr14[jlalel[14] - 1] - Alpha * (per - Eta) - (Epsilon * Arr14[jlalel[14] - 1] - GameControl.RT);
    //                if (Arr14[jlalel[14]] < 0.38)
    //                {
    //                    Arr14[jlalel[14]] = 0.38;
    //                }
    //            }
    //        }
    //        else if (per < 0.725)
    //        {
    //            if (bRet)
    //            {
    //                Arr14[jlalel[14]] = Arr14[jlalel[14] - 1] - Beta * (per - Eta) - Gamma * (Arr14[jlalel[14] - 1] - GameControl.RT);
    //            }
    //            else
    //            {
    //                Arr14[jlalel[14]] = Arr14[jlalel[14] - 1] - Beta * (per - Eta) + (Arr14[jlalel[14] - 1] - Delte * GameControl.RT);
    //            }
    //        }
    //        else if (Math.Abs(per - Eta) <= 0.025)
    //        {
    //            //Arr14[jlalel[14]] = Arr14[jlalel[14] - 1];
    //            if (bRet)  // if success
    //            {
    //                consecutive[14]++;
    //                if (consecutive[14] >= 3)
    //                {
    //                    consecutive[14] = 0;
    //                    Arr14[jlalel[14]] = Arr14[jlalel[14] - 1] - Omega * (Arr14[jlalel[14] - 1] - GameControl.RT);
    //                    if (Arr14[jlalel[14]] < 0.38)
    //                    {
    //                        Arr14[jlalel[14]] = 0.38;
    //                    }
    //                }
    //                else
    //                {
    //                    Arr14[jlalel[14]] = Arr14[jlalel[14] - 1];
    //                }
    //            }
    //            else // if failed
    //            {
    //                consecutive[14] = 0;
    //                Arr14[jlalel[14]] = Arr14[jlalel[14] - 1] + (Arr14[jlalel[14] - 1] - Omega * GameControl.RT);
    //            }
    //        }
    //        _newelement.newDeadline = (float)Arr14[jlalel[14]];///******            
    //    }

    //    else if(DiffArray[_randIndex].targetforce == 4.0f && DiffArray[_randIndex].tolerance == 0.8f)
    //    {
    //        jlalel[15]++;
    //        if (per > 0.775)
    //        {
    //            if (bRet)
    //            {
    //                Arr15[jlalel[15]] = Arr15[jlalel[15] - 1] - Alpha * (per - Eta) - Gamma * (Arr15[jlalel[15] - 1] - GameControl.RT);
    //                if (Arr15[jlalel[15]] < 0.36)
    //                {
    //                    Arr15[jlalel[15]] = 0.36;
    //                }
    //            }
    //            else
    //            {
    //                Arr15[jlalel[15]] = Arr15[jlalel[15] - 1] - Alpha * (per - Eta) - (Epsilon * Arr15[jlalel[15] - 1] - GameControl.RT);
    //                if (Arr15[jlalel[15]] < 0.36)
    //                {
    //                    Arr15[jlalel[15]] = 0.36;
    //                }
    //            }
    //        }
    //        else if (per < 0.725)
    //        {
    //            if (bRet)
    //            {
    //                Arr15[jlalel[15]] = Arr15[jlalel[15] - 1] - Beta * (per - Eta) - Gamma * (Arr15[jlalel[15] - 1] - GameControl.RT);
    //            }
    //            else
    //            {
    //                Arr15[jlalel[15]] = Arr15[jlalel[15] - 1] - Beta * (per - Eta) + (Arr15[jlalel[15] - 1] - Delte * GameControl.RT);
    //            }
    //        }
    //        else if (Math.Abs(per - Eta) <= 0.025)
    //        {
    //            //Arr15[jlalel[15]] = Arr15[jlalel[15] - 1];
    //            if (bRet)  // if success
    //            {
    //                consecutive[15]++;
    //                if (consecutive[15] >= 3)
    //                {
    //                    consecutive[15] = 0;
    //                    Arr15[jlalel[15]] = Arr15[jlalel[15] - 1] - Omega * (Arr15[jlalel[15] - 1] - GameControl.RT);
    //                    if (Arr15[jlalel[15]] < 0.36)
    //                    {
    //                        Arr15[jlalel[15]] = 0.36;
    //                    }
    //                }
    //                else
    //                {
    //                    Arr15[jlalel[15]] = Arr15[jlalel[15] - 1];
    //                }
    //            }
    //            else // if failed
    //            {
    //                consecutive[15] = 0;
    //                Arr15[jlalel[15]] = Arr15[jlalel[15] - 1] + (Arr15[jlalel[15] - 1] - Omega * GameControl.RT);
    //            }
    //        }
    //        _newelement.newDeadline = (float)Arr15[jlalel[15]];///****** 
    //    }

    //    DiffArray[_randIndex] = _newelement;
    //}

    public static void fanxing()
    {
        iArrayIndex++;
        if (iArrayIndex <= 3)
        {
            yijie.Add(_randIndex);
        }            
    }

    /// <summary>
    /// update the upper bound 

    public static void Close()
    {
        SP.Close();
    }
}
