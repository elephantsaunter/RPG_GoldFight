﻿using MySql.Data.MySqlClient;
using PEProtocol;
using System;

public class DBMgr {
    private static DBMgr instance = null;
    public static DBMgr Instance {
        get {
            if (instance == null) {
                instance = new DBMgr();
            }
            return instance;
        }
    }

    private MySqlConnection conn;
    public void Init () {
        conn = new MySqlConnection("server=localhost;User Id=root;password=123456;Database=darkGod;Charset=utf8");
        conn.Open();
        PECommon.Log("DBMgr is Done.");

        //QueryPlayerData("xxxx", "oooo");
    }

    public PlayerData QueryPlayerData (string acct, string pass) {
        bool isNew = true;
        PlayerData playerData = null;
        MySqlDataReader reader = null;


        try {
            MySqlCommand cmd = new MySqlCommand("select * from account where acct=@acct", conn);
            cmd.Parameters.AddWithValue("acct", acct);
            reader = cmd.ExecuteReader();
            if (reader.Read()) {
                isNew = false;
                string _pass = reader.GetString("pass");
                if (_pass.Equals(pass)) {
                    // password correct, return player data
                    playerData = new PlayerData {
                        id = reader.GetInt32("id"),
                        name = reader.GetString("name"),
                        lv = reader.GetInt32("lv"),
                        exp = reader.GetInt32("exp"),
                        power = reader.GetInt32("power"),
                        coin = reader.GetInt32("coin"),
                        diamond = reader.GetInt32("diamond"),
                        crystal = reader.GetInt32("crystal"),

                        hp = reader.GetInt32("hp"),
                        ad = reader.GetInt32("ad"),
                        ap = reader.GetInt32("ap"),
                        addef = reader.GetInt32("addef"),
                        apdef = reader.GetInt32("apdef"),
                        dodge = reader.GetInt32("dodge"),
                        pierce = reader.GetInt32("pierce"),
                        critical = reader.GetInt32("critical"),
                        guideid = reader.GetInt32("guideid"),
                        time = reader.GetInt64("time"),
                        mission = reader.GetInt32("mission")
                        // TOADD
                    };
                    #region Enhancement Arr
                    // Data Sample : 1#2#3#4#5#6#
                    // turn string_arr into int_arr
                    string[] strongStrArr = reader.GetString("strong").Split('#');

                    int[] _strongArr = new int[6];
                    for (int i = 0; i < strongStrArr.Length; i++) {
                        if (strongStrArr[i] == "") {
                            // end
                            continue;
                        }
                        if (int.TryParse(strongStrArr[i], out int starLv)) {
                            // whether it can parse into a integer
                            _strongArr[i] = starLv;
                        } else {
                            PECommon.Log("Parse Enhancement Data Error", LogType.Error);
                        }
                    }
                    playerData.strongArr = _strongArr;
                    #endregion
                    #region Task Arr
                    // taskid | progress | iftaked 1|1|0 # 2|1|0 # 3|1|0 #
                    string[] taskStrArr = reader.GetString("task").Split('#');
                    playerData.taskArr = new string[6];
                    for (int i = 0; i < taskStrArr.Length; i++) {
                        if (taskStrArr[i] == "") {
                            // last data, it already finished
                            continue;
                        } else if (taskStrArr[i].Length >= 5) {
                            // data format normal
                            playerData.taskArr[i] = taskStrArr[i];
                        } else {
                            throw new Exception("DataError");
                        }
                    }
                    #endregion
                    #region Option Arr
                    /*
                    // Data Sample : 1#2#3#4# 
                    // turn string_arr into int_arr
                    string[] optionStrArr = reader.GetString("option").Split('#');

                    int[] _optionArr = new int[4];
                    for (int i = 0; i < optionStrArr.Length; i++) {
                        if (optionStrArr[i] == "") {
                            // end
                            continue;
                        }
                        if (int.TryParse(optionStrArr[i], out int num)) {
                            // whether it can parse into a integer
                            _optionArr[i] = num;
                        } else {
                            PECommon.Log("Parse Option Data Error", LogType.Error);
                        }
                    }
                    playerData.optionArr = _optionArr;
    */
                    #endregion
                    //TODO
                }
            }
        } catch (Exception e) {
            PECommon.Log("Query PlayerData By Acct&Pass Error:" + e, LogType.Error);
        } finally {
            if (reader != null) {
                reader.Close();
            }
            if (isNew) {
                //account not exists, supposed to create a new account, and return
                playerData = new PlayerData {
                    id = -1,
                    name = "",
                    lv = 1,
                    exp = 0,
                    power = 150,
                    coin = 10000,
                    diamond = 0,
                    crystal = 100,

                    hp = 200,
                    ad = 20,
                    ap = 20,
                    addef = 1,
                    apdef = 1,
                    dodge = 1,
                    pierce = 1,
                    critical = 1,

                    guideid = 1001,
                    strongArr = new int[6], // new acount, all position's star is 0
                    time = TimerSvc.Instance.GetNowTime(),
                    taskArr = new string[6],
                    mission = 10001,
                    //optionArr = new int[4],
                };

                // taskid | progress | iftaked 1|1|0 # 2|1|0 # 3|1|0 #
                // initialize task reward data
                for (int i = 0; i < playerData.taskArr.Length; i++) {
                    playerData.taskArr[i] = (i + 1) + "|0|0";
                }
                playerData.id = InsertNewAcctData(acct, pass, playerData);
            }
        }
        return playerData;
    }

    private int InsertNewAcctData (string acct, string pass, PlayerData pd) {
        int id = -1;
        try {
            MySqlCommand cmd = new MySqlCommand(
                "insert into account set acct=@acct,pass=@pass,name=@name,lv=@lv,exp=@exp,power=@power," +
                "coin=@coin,diamond=@diamond,crystal=@crystal,hp=@hp,ad=@ad,ap=@ap,addef=@addef,apdef=@apdef," +
                "dodge=@dodge,pierce=@pierce,critical=@critical,guideid=@guideid,strong=@strong,time=@time," +
                "task=@task,mission=@mission", conn);
            cmd.Parameters.AddWithValue("acct", acct);
            cmd.Parameters.AddWithValue("pass", pass);
            cmd.Parameters.AddWithValue("name", pd.name);
            cmd.Parameters.AddWithValue("lv", pd.lv);
            cmd.Parameters.AddWithValue("exp", pd.exp);
            cmd.Parameters.AddWithValue("power", pd.power);
            cmd.Parameters.AddWithValue("coin", pd.coin);
            cmd.Parameters.AddWithValue("diamond", pd.diamond);
            cmd.Parameters.AddWithValue("crystal", pd.crystal);

            cmd.Parameters.AddWithValue("hp", pd.hp);
            cmd.Parameters.AddWithValue("ad", pd.ad);
            cmd.Parameters.AddWithValue("ap", pd.ap);
            cmd.Parameters.AddWithValue("addef", pd.addef);
            cmd.Parameters.AddWithValue("apdef", pd.apdef);
            cmd.Parameters.AddWithValue("dodge", pd.dodge);
            cmd.Parameters.AddWithValue("pierce", pd.pierce);
            cmd.Parameters.AddWithValue("critical", pd.critical);
            cmd.Parameters.AddWithValue("guideid", pd.guideid);

            string strongInfo = "";
            for (int i = 0; i < pd.strongArr.Length; i++) {
                strongInfo += pd.strongArr[i];
                strongInfo += '#';
            }
            cmd.Parameters.AddWithValue("strong", strongInfo);
            cmd.Parameters.AddWithValue("time", pd.time);

            // taskid | progress | iftaked 1|1|0 # 2|1|0 # 3|1|0 #
            string taskInfo = "";
            for (int i = 0; i < pd.taskArr.Length; i++) {
                taskInfo += pd.taskArr[i];
                taskInfo += "#";
            }
            cmd.Parameters.AddWithValue("task", taskInfo);
            cmd.Parameters.AddWithValue("mission", pd.mission);
            /*
            string optionInfo = "";
            for (int i = 0; i < pd.optionArr.Length; i++) {
                optionInfo += pd.optionArr[i];
                optionInfo += '#';
            }
            cmd.Parameters.AddWithValue("option", optionInfo);
            */
            cmd.ExecuteNonQuery();
            id = (int)cmd.LastInsertedId;

        } catch (Exception e) {
            PECommon.Log("Insert PlayerData Error:" + e, LogType.Error);
        }
        return id;
    }

    public bool QueryNameData (string name) {
        bool exist = false;
        MySqlDataReader reader = null;
        try {
            MySqlCommand cmd = new MySqlCommand("select * from account where name=@name", conn);
            cmd.Parameters.AddWithValue("name", name);
            reader = cmd.ExecuteReader();
            if (reader.Read()) {
                exist = true;
            }

        } catch (Exception e) {
            PECommon.Log("Query Name State Error:" + e, LogType.Error);
        } finally {
            if (reader != null) {
                reader.Close();
            }
        }
        return exist;
    }

    public bool UpdatePlayerData (int id, PlayerData pd) {
        try {
            MySqlCommand cmd = new MySqlCommand(
                "update account set name=@name,lv=@level,exp=@exp,power=@power,coin=@coin,diamond=@diamond,crystal=@crystal," +
                "hp=@hp,ad=@ad,ap=@ap,addef=@addef,apdef=@apdef,dodge=@dodge,pierce=@pierce,critical=@critical," +
                "guideid=@guideid,strong=@strong,time=@time,task=@task,mission=@mission" +
                " where id=@id", conn);
            cmd.Parameters.AddWithValue("id", id);
            cmd.Parameters.AddWithValue("name", pd.name);
            cmd.Parameters.AddWithValue("level", pd.lv);
            cmd.Parameters.AddWithValue("exp", pd.exp);
            cmd.Parameters.AddWithValue("power", pd.power);
            cmd.Parameters.AddWithValue("coin", pd.coin);
            cmd.Parameters.AddWithValue("diamond", pd.diamond);
            cmd.Parameters.AddWithValue("crystal", pd.crystal);


            cmd.Parameters.AddWithValue("hp", pd.hp);
            cmd.Parameters.AddWithValue("ad", pd.ad);
            cmd.Parameters.AddWithValue("ap", pd.ap);
            cmd.Parameters.AddWithValue("addef", pd.addef);
            cmd.Parameters.AddWithValue("apdef", pd.apdef);
            cmd.Parameters.AddWithValue("dodge", pd.dodge);
            cmd.Parameters.AddWithValue("pierce", pd.pierce);
            cmd.Parameters.AddWithValue("critical", pd.critical);
            cmd.Parameters.AddWithValue("guideid", pd.guideid);

            string strongInfo = "";
            for (int i = 0; i < pd.strongArr.Length; i++) {
                strongInfo += pd.strongArr[i];
                strongInfo += '#';
            }
            cmd.Parameters.AddWithValue("strong", strongInfo);
            cmd.Parameters.AddWithValue("time", pd.time);

            string taskInfo = "";
            for (int i = 0; i < pd.taskArr.Length; i++) {
                taskInfo += pd.taskArr[i];
                taskInfo += "#";
            }
            cmd.Parameters.AddWithValue("task", taskInfo);
            cmd.Parameters.AddWithValue("mission", pd.mission);
            /*
            string optionInfo = "";
            for (int i = 0; i < pd.optionArr.Length; i++) {
                optionInfo += pd.optionArr[i];
                optionInfo += '#';
            }
            cmd.Parameters.AddWithValue("option", optionInfo);
            */
            cmd.ExecuteNonQuery();
        } catch (Exception e) {
            PECommon.Log("Update PlayerData Error:" + e, LogType.Error);
            return false;
        }
        return true;
    }
}
