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
    ErrorLowAirCanister,
    ErrorHighCarbonCanister,
    ErrorLowPump
}
public static class ErrorCodes
{
    private const int ErrorThresholdDurability = 60;
    private const int BrokenThresholdDurability = 20;
    private const int ErrorThresholdPressure = 20;
    private const int ErrorThresholdDirt = 80;
    public static void ErrorCheck(ItemRegister itemRegister, bool Printed, out string OutputString , out List<ErrorTypes> ErrorList)
    {
        ErrorList = new List<ErrorTypes>();
        if (CheckForBadCompontent(ObjectType.Monitor)) { ErrorList.Add(ErrorTypes.ErrorBadMonitor); }
        if (CheckForBadCompontent(ObjectType.Fuse)) { ErrorList.Add(ErrorTypes.ErrorBadFuse); }
        if (CheckForBadCompontent(ObjectType.PowerConnector)) { ErrorList.Add(ErrorTypes.ErrorBadPowerConnector); }
        if (CheckForBadCompontent(ObjectType.PowerSwitch)) { ErrorList.Add(ErrorTypes.ErrorBadPowerSwitch); }
        if (CheckForBadCompontent(ObjectType.Pump)) { ErrorList.Add(ErrorTypes.ErrorBadPump); }
        if (CheckForBadCompontent(ObjectType.Monitor)) { ErrorList.Add(ErrorTypes.ErrorBadMonitor); }
        if (CheckForLowCompontent(ObjectType.AirCanister)) { ErrorList.Add(ErrorTypes.ErrorLowAirCanister); }
        if (CheckForLowCompontent(ObjectType.Co2Canister)) { ErrorList.Add(ErrorTypes.ErrorHighCarbonCanister); }
        if (CheckForDirtyCompontent(ObjectType.AirFilter) || CheckForBadCompontent(ObjectType.Pump) || CheckForLowCompontent(ObjectType.Co2Canister)) { ErrorList.Add(ErrorTypes.ErrorLowPump); }
        bool CheckForBadCompontent(ObjectType objectType)
        {
            if (itemRegister.HasObject(objectType, out List<ObjectDirector> List))
            {
                foreach (ObjectDirector Object in List)
                {
                    if (Object.Durability < ErrorThresholdDurability)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        bool CheckForLowCompontent(ObjectType objectType)
        {
            if (itemRegister.HasObject(objectType, out List<ObjectDirector> List))
            {
                foreach (ObjectDirector Object in List)
                {
                    if (Object.Pressure < ErrorThresholdPressure)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        bool CheckForDirtyCompontent(ObjectType objectType)
        {
            if (itemRegister.HasObject(objectType, out List<ObjectDirector> List))
            {
                foreach (ObjectDirector Object in List)
                {
                    if (Object.Dirt > ErrorThresholdDirt)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        OutputString = "";
        //Pressure Errors
        if (ErrorList.Contains(ErrorTypes.ErrorLowAirCanister))
        {
            if (!Printed) { OutputString += "CF13\n"; }
            else OutputString += "LowAirCanister\n";
        }
        if (ErrorList.Contains(ErrorTypes.ErrorHighCarbonCanister))
        {
            if (!Printed) { OutputString += "CF14"; }
            else OutputString += "Full Carbon Canister\n";
        }
        //Durability Errors
        if (ErrorList.Contains(ErrorTypes.ErrorBadPowerConnector))
        {
            if (!Printed) { OutputString += "EDA1"; }
            else OutputString += "BadPowerConnector\n";
        }
        if (ErrorList.Contains(ErrorTypes.ErrorBadFuse))
        {
            if(!Printed) { OutputString += "EDA2"; }
            else OutputString += "BadFuse\n";
        }
        if (ErrorList.Contains(ErrorTypes.ErrorBadPowerSwitch))
        {
            if (!Printed) { OutputString += "EDA3"; }
            else OutputString += "BadPowerSwitch\n";
        }
        if (ErrorList.Contains(ErrorTypes.ErrorBadPump))
        {
            if (!Printed) { OutputString += "EDA4"; }
            else OutputString += "BadPump\n";
        }
        if (ErrorList.Contains(ErrorTypes.ErrorBadMonitor))
        {
            if (!Printed) { OutputString += "EDA5"; }
            else OutputString += "BadMonitor\n";
        }
        if (ErrorList.Contains(ErrorTypes.ErrorLowPump))
        {
            if (!Printed) { OutputString += "CFA0"; }
            else OutputString += "LowPump\n";
        }
    }
    public static bool CheckWorking(ObjectDirector objectIn)
    {
        bool HasWorking = true;
        if (objectIn.objectType == ObjectType.AirFilter)
        {
            if (objectIn.Dirt >= 100)
            {
                HasWorking = false;
            }
        }
        else if (objectIn.objectType == ObjectType.AirCanister || objectIn.objectType == ObjectType.Co2Canister)
        {
            if (objectIn.Pressure <= 100)
            {
                HasWorking = false;
            }
        }
        else if (objectIn.Durability < BrokenThresholdDurability)
        {
            HasWorking = false;
        }
        return HasWorking;
    }
    public static bool CheckWorking(List<ObjectDirector> objectsIn)
    {
        bool HasWorking = true; 
        foreach(ObjectDirector Item in objectsIn)
        {
            if (Item.objectType == ObjectType.AirFilter)
            {
                if (Item.Dirt <= 100)
                {
                    HasWorking = false;
                }
            }
            else if (Item.objectType == ObjectType.AirCanister || Item.objectType == ObjectType.Co2Canister)
            {
                if (Item.Pressure >= 100)
                {
                    HasWorking = false;
                }
            }
            else if (Item.Durability < BrokenThresholdDurability)
            {
                HasWorking = false;
            }
        }
        return HasWorking;
    }
    public static bool CheckWorking(ItemRegister register, ObjectType objectType)
    {
        if (register.HasObject(objectType, out List<ObjectDirector> ListToTest))
        {
            bool HasWorking = true;
            foreach (ObjectDirector Item in ListToTest)
            {
                if (Item.objectType == ObjectType.AirFilter)
                {
                    if (Item.Dirt >= 100)
                    {
                        HasWorking = false;
                    }
                }
                else if (Item.objectType == ObjectType.AirCanister || Item.objectType == ObjectType.Co2Canister)
                {
                    if (Item.Pressure <= 100)
                    {
                        HasWorking = false;
                    }
                }
                else if (Item.Durability < BrokenThresholdDurability)
                {
                    HasWorking = false;
                }
            }
            return HasWorking;
        }
        return false;
    }
}
