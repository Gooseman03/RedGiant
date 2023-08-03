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
    private const float ErrorThresholdDurability = 0.60f;
    private const float BrokenThresholdDurability = 0.20f;
    private const float ErrorThresholdPressure = 0.20f;
    private const float ErrorThresholdDirt = 0.80f;
    private static bool _Printed;
    public static bool Printed
    {
        get { return _Printed; }
        set { _Printed = value; }
    }
    public static void ErrorCheck(ItemRegister itemRegister, out string OutputString , out List<ErrorTypes> ErrorList)
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
                    if (Object.GetPersentDurability() < ErrorThresholdDurability)
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
                    if (Object.GetPersentPressure() < ErrorThresholdPressure)
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
                    if (Object.GetPersentDirt() > ErrorThresholdDirt)
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
            if (objectIn.Dirt >= objectIn.MaxDirt)
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
                if (Item.Dirt <= Item.MaxDirt)
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
                    if (Item.Dirt >= Item.MaxDirt)
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
