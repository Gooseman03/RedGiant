using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum ErrorTypes
{
    ErrorBadFuse,
    ErrorBadPowerConnector,
    ErrorBadMonitor,
    ErrorBadPowerSwitch,
    ErrorBadPump,
    ErrorLowAirCanister
}
public static class ErrorCodes
{
    public static string ErrorCheck(List<ErrorTypes> errorType, bool Printed)
    {
        string OutputString = "";
        //Pressure Errors
        if (errorType.Contains(ErrorTypes.ErrorLowAirCanister))
        {
            if (!Printed) { OutputString += "CF13\n"; }
            else OutputString += "LowAirCanister\n";
        }
        //Durability Errors
        if (errorType.Contains(ErrorTypes.ErrorBadPowerConnector))
        {
            if (!Printed) { OutputString += "EDA1"; }
            else OutputString += "BadPowerConnector\n";
        }
        if (errorType.Contains(ErrorTypes.ErrorBadFuse))
        {
            if(!Printed) { OutputString += "EDA2"; }
            else OutputString += "BadFuse\n";
        }
        if (errorType.Contains(ErrorTypes.ErrorBadPowerSwitch))
        {
            if (!Printed) { OutputString += "EDA3"; }
            else OutputString += "BadPowerSwitch\n";
        }
        if (errorType.Contains(ErrorTypes.ErrorBadPump))
        {
            if (!Printed) { OutputString += "EDA4"; }
            else OutputString += "BadPump\n";
        }
        if (errorType.Contains(ErrorTypes.ErrorBadMonitor))
        {
            if (!Printed) { OutputString += "EDA5"; }
            else OutputString += "BadMonitor\n";
        }
        return OutputString;
    }




}
