/***************************************
 * Adaptive Algorithm for Game AI
 * Author: yang xiaoxiao
 * Date: 2015.8.18
 * Version: 0.1
 * Mail: Doublexiao@hotmail.com
****************************************/

using UnityEngine;
using System.Collections;
using System.Linq;
using System.Runtime.InteropServices;

public class AdaptiveAlgorithm{

	[DllImport(@"Adaptive_Dll", EntryPoint = "AdaptiveAlgorithmGetProblem")]
	extern public static void GetProblem([In, Out] ref int param_1, [In, Out] ref int param_2, [In, Out] ref int param_3);
	
	[DllImport(@"Adaptive_Dll", EntryPoint = "AdaptiveAlgorithmSetResult")]
	extern public static void UpdateKnowledge(int isSuccess);
	
	[DllImport(@"Adaptive_Dll", EntryPoint = "AdaptiveAlgorithmGetTrialNumber")]
	extern public static int GetTrialNumber();

}
