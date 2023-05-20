using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//
//  Creates an array of possible delays from 0 to 120 seconds. Array of 120 items.
//  Creates an array of possible delays from 0 to 1 seconds. Array of 50 items. per 20 ms step.
//
//  Returns delay or random delay as WaitForSeconds(). 
//

public static class Coroutine_Yielders
{
    static int delayArrayInSecondsAmount=121;
    static int delayArrayPerTwenty_msAmount = 51;
   // static int delayArrayPerFive_msAmount = 21;
    static WaitForSeconds[] waitForSecondsArray = new WaitForSeconds[delayArrayInSecondsAmount];
    static WaitForSeconds[] waitPer20msArray = new WaitForSeconds[delayArrayPerTwenty_msAmount];
    //static WaitForSeconds[] waitPer5msArray = new WaitForSeconds[delayArrayPerFive_msAmount];
    static int temp;
    static Coroutine_Yielders()
    {
        // seconds array. position = second. 0=0, 5=5, 120=120
        for (int i = 0; i < delayArrayInSecondsAmount; i++)
        {
            var tempw = new WaitForSeconds(i);
            waitForSecondsArray[i] = tempw;
        }
        // each position per 20 ms. 0=0 1=20ms, 5=100ms, 50=1000ms
        for (int i = 0; i < delayArrayPerTwenty_msAmount; i++)
        {
            var tempw = new WaitForSeconds(i*0.02f);
            waitPer20msArray[i] = tempw;
        }
        /*
        for (int i = 0; i < delayArrayPerFive_msAmount; i++)
        {
            var tempw = new WaitForSeconds(i * 0.05f);
            waitPer20msArray[i] = tempw;
        }
        */
    }
   

    public static WaitForSeconds DelayInSeconds(int i)
    {
        if (0 <= i && i < waitForSecondsArray.Length)
        {
            return waitForSecondsArray[i];
        }
        else
        {
            Debug.LogError("Request is out of array bounds!");
        }
        return null;
    }

    public static WaitForSeconds DelayInSecondsFromRangeRandom(int i, int j)
    {

        if (0<=i && i< waitForSecondsArray.Length && j < waitForSecondsArray.Length)
        {
            temp = UnityEngine.Random.Range(i, j);
            return waitForSecondsArray[temp];
        }
        else
        {
            Debug.LogError("Request is out of array bounds!");
        }


        return null;
    }

    public static WaitForSeconds DelayPerTwentyMS(int i)
    {
        if (0 <= i && i < waitForSecondsArray.Length)
        {
            return waitPer20msArray[i];
        }
        else
        {
            Debug.LogError("Request is out of array bounds!");
        }
        return null;
    }
    public static WaitForSeconds DelayPerTwentyMS_FromRangeRandom(int i, int j)
    {

        if (0 <= i && i < waitPer20msArray.Length && j < waitPer20msArray.Length)
        {
            temp = UnityEngine.Random.Range(i, j);
            
            return waitPer20msArray[temp];
        }
        else
        {
            Debug.LogError("Request is out of array bounds!");
        }


        return null;
    }


}
