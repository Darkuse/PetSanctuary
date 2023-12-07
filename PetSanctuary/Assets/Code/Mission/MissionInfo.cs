using System;
using System.Collections.Generic;

[Serializable]
public class MissionInfo
{
    public string missionName;
    public string missionType; 
    public string description;
}

[Serializable]
public class MissionsContainer
{
    public List<MissionInfo> missionDescirption;
}
