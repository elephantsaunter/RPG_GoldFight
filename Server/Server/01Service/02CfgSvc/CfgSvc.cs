using System;
using System.Collections.Generic;
using System.Xml;

public class CfgSvc {
    private static CfgSvc instance = null;
    public static CfgSvc Instance {
        get {
            if (instance == null) {
                instance = new CfgSvc();
            }
            return instance;
        }
    }

    public void Init () {
        InitGuideCfg();
        InitStrongCfg();
        InitTaskRewardCfg();
        InitMapCfg();
        PECommon.Log("CgfSvc Init Done");
    }


    #region autoTask
    private Dictionary<int, AutoGuideCfg> guideTaskDic = new Dictionary<int, AutoGuideCfg>();
    private void InitGuideCfg () {
        XmlDocument doc = new XmlDocument();
        //doc.LoadXml(xml.text);
        doc.Load(@"G:\CODE\Unity\RPG_GoldFight\Client\Assets\Resources\ResCfgs\guide.xml");

        XmlNodeList nodLst = doc.SelectSingleNode("root").ChildNodes;

        for (int i = 0; i < nodLst.Count; i++) {
            XmlElement ele = nodLst[i] as XmlElement;

            if (ele.GetAttributeNode("ID") == null) {
                continue;
            }
            int ID = Convert.ToInt32(ele.GetAttributeNode("ID").InnerText);
            AutoGuideCfg mc = new AutoGuideCfg {
                ID = ID
            };
            foreach (XmlElement e in nodLst[i].ChildNodes) {
                switch (e.Name) {
                    case "coin":
                        mc.coin = int.Parse(e.InnerText);
                        break;
                    case "exp":
                        mc.exp = int.Parse(e.InnerText);
                        break;
                }
            }
            guideTaskDic.Add(ID, mc);
        }
        PECommon.Log("GuideCfg Init Done");
    }
    public AutoGuideCfg GetAutoGuideCfg (int id) {
        AutoGuideCfg agc = null;
        if (guideTaskDic.TryGetValue(id, out agc)) {
            return agc;
        }
        return null;
    }
    #endregion
    #region TaskReward
    private Dictionary<int, TaskRewardCfg> taskRewardDic = new Dictionary<int, TaskRewardCfg>();
    private void InitTaskRewardCfg () {
        XmlDocument doc = new XmlDocument();
        //doc.LoadXml(xml.text);
        doc.Load(@"G:\CODE\Unity\RPG_GoldFight\Client\Assets\Resources\ResCfgs\taskreward.xml");

        XmlNodeList nodLst = doc.SelectSingleNode("root").ChildNodes;

        for (int i = 0; i < nodLst.Count; i++) {
            XmlElement ele = nodLst[i] as XmlElement;

            if (ele.GetAttributeNode("ID") == null) {
                continue;
            }
            int ID = Convert.ToInt32(ele.GetAttributeNode("ID").InnerText);
            TaskRewardCfg trc = new TaskRewardCfg {
                ID = ID
            };
            foreach (XmlElement e in nodLst[i].ChildNodes) {
                switch (e.Name) {
                    case "count":
                        trc.count = int.Parse(e.InnerText);
                        break;
                    case "coin":
                        trc.coin = int.Parse(e.InnerText);
                        break;
                    case "exp":
                        trc.exp = int.Parse(e.InnerText);
                        break;
                }
            }
           taskRewardDic.Add(ID, trc);
        }
        PECommon.Log("TaskRewardCfg Init Done");
    }
    public TaskRewardCfg GetTaskRewardCfg (int id) {
        TaskRewardCfg trc = null;
        if (taskRewardDic.TryGetValue(id, out trc)) {
            return trc;
        }
        return null;
    }
    #endregion
    #region Map Config
    private Dictionary<int, MapCfg> mapDic = new Dictionary<int, MapCfg>();
    private void InitMapCfg () {
        XmlDocument doc = new XmlDocument();
        //doc.LoadXml(xml.text);
        doc.Load(@"G:\CODE\Unity\RPG_GoldFight\Client\Assets\Resources\ResCfgs\map.xml");

        XmlNodeList nodLst = doc.SelectSingleNode("root").ChildNodes;

        for (int i = 0; i < nodLst.Count; i++) {
            XmlElement ele = nodLst[i] as XmlElement;

            if (ele.GetAttributeNode("ID") == null) {
                continue;
            }
            int ID = Convert.ToInt32(ele.GetAttributeNode("ID").InnerText);
            MapCfg mc = new MapCfg {
                ID = ID
            };
            foreach (XmlElement e in nodLst[i].ChildNodes) {
                switch (e.Name) {
                    case "power":
                        mc.power = int.Parse(e.InnerText);
                        break;
                    case "coin":
                        mc.coin = int.Parse(e.InnerText);
                        break;
                    case "crystal":
                        mc.crystal = int.Parse(e.InnerText);
                        break;
                    case "exp":
                        mc.exp = int.Parse(e.InnerText);
                        break;

                }
            }
            mapDic.Add(ID, mc);
        }
        PECommon.Log("MapCfg Init Done");
    }
    public MapCfg GetMapCfg (int id) {
        MapCfg mc = null;
        if (mapDic.TryGetValue(id, out mc)) {
            return mc;
        }
        return null;
    }
    #endregion
    #region enhancement
    private Dictionary<int, Dictionary<int, StrongCfg>> strongDic = new Dictionary<int, Dictionary<int, StrongCfg>>();
    private void InitStrongCfg () {
        XmlDocument doc = new XmlDocument();
        //doc.LoadXml(xml.text);
        doc.Load(@"G:\CODE\Unity\RPG_GoldFight\Client\Assets\Resources\ResCfgs\strong.xml");

        XmlNodeList nodLst = doc.SelectSingleNode("root").ChildNodes;

        for (int i = 0; i < nodLst.Count; i++) {
            XmlElement ele = nodLst[i] as XmlElement;

            if (ele.GetAttributeNode("ID") == null) {
                continue;
            }
            int ID = Convert.ToInt32(ele.GetAttributeNode("ID").InnerText);
            StrongCfg sc = new StrongCfg {
                ID = ID
            };
            foreach (XmlElement e in nodLst[i].ChildNodes) {
                int val = int.Parse(e.InnerText);
                switch (e.Name) {
                    case "pos":
                        sc.pos = val;
                        break;
                    case "starlv":
                        sc.starlv = val;
                        break;
                    case "addhp":
                        sc.addhp = val;
                        break;
                    case "addhurt":
                        sc.addhurt = val;
                        break;
                    case "adddef":
                        sc.adddef = val;
                        break;
                    case "minlv":
                        sc.minlv = val;
                        break;
                    case "coin":
                        sc.coin = val;
                        break;
                    case "crystal":
                        sc.crystal = val;
                        break;
                }
            }

            Dictionary<int, StrongCfg> dic = null;
            if (strongDic.TryGetValue(sc.pos, out dic)) {
                dic.Add(sc.starlv, sc);
            } else {
                dic = new Dictionary<int, StrongCfg>();
                dic.Add(sc.starlv, sc);
                strongDic.Add(sc.pos, dic);
            }
        }
        PECommon.Log("StrongCfg Init Done");
    }
    public StrongCfg GetStrongCfg (int pos, int starlv) {
        StrongCfg sc = null;
        Dictionary<int, StrongCfg> dic = null;
        if (strongDic.TryGetValue(pos, out dic)) {
            if (dic.ContainsKey(starlv)) {
                sc = dic[starlv];
            }
        }
        return sc;
    }
    #endregion

}

public class MapCfg: BaseData<MapCfg> {
    public int power;
    public int coin;
    public int exp;
    public int crystal;
}
public class StrongCfg: BaseData<StrongCfg> {
    public int pos;
    public int starlv;
    public int addhp;
    public int addhurt;
    public int adddef;
    public int minlv;
    public int coin;
    public int crystal;
}
public class AutoGuideCfg: BaseData<AutoGuideCfg> {
    public int coin;
    public int exp;
}
public class TaskRewardCfg: BaseData<TaskRewardCfg> {
    public int count;
    public int exp;
    public int coin;
}

public class TaskRewardData: BaseData<TaskRewardData> {
    public int prgs;
    public bool taked;
}
public class BaseData<T> {
    public int ID;
}

