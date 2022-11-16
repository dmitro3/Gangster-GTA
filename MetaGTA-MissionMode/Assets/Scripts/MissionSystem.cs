using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;
public class MissionSystem : MonoBehaviour
{
    #region Singleton
    public static MissionSystem Instance;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
    #endregion

    [Header("Mission Details")]
    public List<Mission> mission_list = new List<Mission>();

    // Start is called before the first frame update
    void Start()
    {
        RefreshMissions();
    }   

    private void RefreshMissions()
    {
        if (mission_list.Count >= 4)
        {
            return;
        }

        while (mission_list.Count < 4)
        {
            Mission new_mission = GetNewMission();
            mission_list.Add(new_mission);
        }

        
    }

    // Update is called once per frame
    void Update()
    {
        if (MetaManager.Instance.myPlayerController != null)
        {

            openTasks_BTN.SetActive(!MetaManager.Instance.inRace);
        }
        

        running_mission_ui.SetActive(isDoingMission);

        if (isDoingMission)
        {
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                MissionFailed();
                return;
            }
            timer_text.text = string.Format("{0:00}:{1:00}", (int)timer / 60, (int)timer % 60);

            switch (_runningMission.missionType)
            {                
                case MissionType.FASTDRIVE:
                    {


                        if (currentTimer >= _runningMission.min_timer)
                        {
                            MissionPassed();
                        }
                        running_mission_amount.gameObject.SetActive(true);
                        running_mission_amount.text="Progress : " + ((int)currentTimer).ToString();
                        break;
                    }
                case MissionType.COLLECTCOINS:
                    {
                        if (coinsCollected >= _runningMission.coint_collect_amount)
                        {
                            MissionPassed();
                        }
                        running_mission_amount.gameObject.SetActive(true);
                        running_mission_amount.text = "Progress : " + ((int)coinsCollected).ToString();
                        break;
                    }
                default:
                    {
                        running_mission_amount.gameObject.SetActive(false);
                        break;
                    }
            }
        }

    }

#if UNITY_EDITOR
    [ContextMenu("Reset Missions")]
    public void ResetMissions()
    {
        mission_list.Clear();
        RefreshMissions();

    }
#endif

    #region Handle UI 
    [Space(20f)]
    [SerializeField] GameObject openTasks_BTN;
    [SerializeField] GameObject task_panel;
    [SerializeField] MissionUI[] task_objects;
    [SerializeField] GameObject running_mission_ui;
    [SerializeField] TMP_Text running_mission_task;
    [SerializeField] TMP_Text running_mission_amount;
    [SerializeField] TMP_Text timer_text;
    [System.Serializable]
    public class MissionUI
    {
        public GameObject main_ui;
        public TMP_Text mission_header;
        public TMP_Text mission_desc;
        public TMP_Text reward_count;
        public TMP_Text mission_timer;
        public Button start_mission_btn;
        public Mission mission;

        public void SetMissionInfo()
        {
            mission_header.text = mission.missionHeader;
            mission_desc.text = mission.missionDescription;
            reward_count.text = mission.reward_amount.ToString();
            mission_timer.text = mission.timer.ToString();

            if (mission.mission_state == MissionState.IDLE)
            {
                start_mission_btn.interactable = true;
                start_mission_btn.transform.GetChild(0).GetComponent<TMP_Text>().text ="Start";
            }
            else
            {
                start_mission_btn.interactable = false;
                start_mission_btn.transform.GetChild(0).GetComponent<TMP_Text>().text = "Running";
            }
        }
    }

    public void OpenMissions()
    {
        MetaManager.Instance.inputManager.ToggleInputs(false);
        SetMissionUI();

        task_panel.SetActive(true);
    }

    private void SetMissionUI()
    {
        for (int i = 0; i < mission_list.Count; i++)
        {
            Mission _m = mission_list[i];           
            task_objects[i].mission = _m;
            task_objects[i].SetMissionInfo();
        }
    }

    public void CloseMissions()
    {
        task_panel.SetActive(false);
        MetaManager.Instance.inputManager.ToggleInputs(true);
    }
   
    #endregion

    #region Handle Mission Generation

    [Header("Checkpoints")]
    [SerializeField] Transform[] pickup_points;
    [SerializeField] Transform[] end_points;
    public Mission GetNewMission()
    {
        Mission mission = new Mission();


        //Get Random Mission Type
        mission.missionType =(MissionType)Random.Range(0, 4);

        //Assign Random Value TO Mission
        switch (mission.missionType)
        {
            case MissionType.PICK_UP_AND_DELIVER:
                {
                    mission.missionHeader = "Pick Up Item And Deliver";
                    
                    mission.reward_amount = (Random.Range(100, 500)/100)*100;
                    mission.pickup_point = pickup_points[Random.Range(0, pickup_points.Length)];
                    mission.deliver_point = end_points[Random.Range(0, end_points.Length)];
                    mission.timer = (Random.Range(180, 900) / 60) * 60;
                    mission.min_speed_required = Random.Range(15, 30);

                    mission.missionDescription = "Pick up the parcel from dedicated location, then deliver to its address in"+ (int)mission.timer +" seconds. You will get incentive if you deliver before time.";


                    break;
                }            
            case MissionType.REACH_DEST:
                {
                    mission.missionHeader = "Reach destinatin in given time";
                                        
                    mission.reward_amount = (Random.Range(100, 500) / 100) * 100;
                    mission.pickup_point = pickup_points[Random.Range(0, pickup_points.Length)];
                    mission.deliver_point = end_points[Random.Range(0, end_points.Length)];
                    mission.timer = (Random.Range(120, 300) / 60) * 60;
                    mission.min_speed_required = Random.Range(15, 30);

                    mission.missionDescription = "Police is finding you in this area. reach a safer place in" +(int)mission.timer + " seconds.";
                    break;
                }

            case MissionType.FASTDRIVE:
                {
                    mission.missionHeader = "Drive in high speed for given time";
                    

                   
                    mission.reward_amount = (Random.Range(100, 500) / 100) * 100;
                    mission.pickup_point = pickup_points[Random.Range(0, pickup_points.Length)];
                    mission.deliver_point = end_points[Random.Range(0, end_points.Length)];
                    mission.timer = (Random.Range(120, 300) / 60) * 60;
                    mission.min_speed_required = Random.Range(20, 50);
                    mission.min_timer = Random.Range(7 ,20);
                    mission.missionDescription = "Lets see how fast you can drive. Upgrade your car if you want. And drive with speed" + mission.min_speed_required.ToString() + " for atleast " + (int)mission.min_timer + " seconds";
                    break;
                }
            case MissionType.COLLECTCOINS:
                {
                    mission.missionHeader = "Collect coins in given time";
                    mission.reward_amount = (Random.Range(100, 150) / 100) * 100;
                    mission.coint_collect_amount = Random.Range(5, 10);
                    mission.pickup_point = pickup_points[Random.Range(0, pickup_points.Length)];
                    mission.deliver_point = end_points[Random.Range(0, end_points.Length)];
                    mission.timer = (Random.Range(120, 300) / 60) * 60;
                    mission.min_speed_required = Random.Range(15, 30);
                    mission.missionDescription = "Collect "+mission.coint_collect_amount.ToString() + "coins in given time: "+ mission.timer.ToString()+ " seconds. Each coin collected will add to your account.";
                    break;
                }
        }

        return mission;
    }
    #endregion

    #region Handle Running Mission
    public bool isDoingMission=false;
    [SerializeField] MissionWaypoint missionWaypoint;
    [SerializeField] GameObject cityCheckpoint;
    [SerializeField] float timer;
    [SerializeField] int lastStartedMission;
    


    [Header("Running Mission Properties")]
    public static float currentTimer;
    public static int coinsCollected;
    public static int reward_got;
    public static int checkpointCount;
    [SerializeField] Mission _runningMission;
    public void ResetProperties()
    {
        checkpointCount = 0;
        currentTimer = 0;
        coinsCollected = 0;
        reward_got = 0;
    }

    public static Action<Mission> OnMissionStarted;
    public static Action<bool> OnMissionOver;

    public void StartMission(int index)
    {
        if (!isDoingMission)
        {
            ResetProperties();

            lastStartedMission = index;
            isDoingMission = true;
            
            Mission _m= _runningMission = mission_list[index];

            _m.mission_state = MissionState.RUNNING;

            SetMissionUI();
            timer = _m.timer;


            switch (_m.missionType)
            {
                case MissionType.PICK_UP_AND_DELIVER:
                    {
                        missionWaypoint.SetTarget(_m.pickup_point);
                        cityCheckpoint.transform.GetChild(0).gameObject.SetActive(true);
                        cityCheckpoint.transform.position = _m.pickup_point.position;

                        running_mission_task.text="Pick up parcel";
                        running_mission_amount.text="";
                        break;
                    }                    
               
                case MissionType.REACH_DEST:
                    {
                        missionWaypoint.SetTarget(_m.pickup_point);
                        cityCheckpoint.transform.GetChild(0).gameObject.SetActive(true);
                        cityCheckpoint.transform.position = _m.pickup_point.position;

                        running_mission_task.text = "Reach Destination in time";
                        running_mission_amount.text = "";
                        break;
                    }
                case MissionType.FASTDRIVE:
                    {
                        UIManager.Instance.ShowInformationMsg("Drive Above "+ _m.min_speed_required + " for "+ (int)_m.min_timer  +" seconds",3f);

                        running_mission_task.text = "Drive Above " + _m.min_speed_required + " for " + (int)_m.min_timer + " seconds";
                        
                        break;
                    }
                case MissionType.COLLECTCOINS:
                    {
                        missionWaypoint.SetTarget(_m.pickup_point);
                        cityCheckpoint.transform.GetChild(0).gameObject.SetActive(true);
                        cityCheckpoint.transform.position = _m.pickup_point.position;

                        running_mission_task.text = "Collect "+ _m.coint_collect_amount +" coins.";
                        break;
                    }
            }

            OnMissionStarted?.Invoke(_m);

            CloseMissions();
        }
        else
        {
            UIManager.Instance.ShowInformationMsg("Complete Running Mission First!",2f);
        }
    }


    public void MissionTaskComplete()
    {
        cityCheckpoint.transform.GetChild(0).gameObject.SetActive(false);
        missionWaypoint.SetTarget(null);

        checkpointCount++;
        switch (_runningMission.missionType)
        {
            case MissionType.PICK_UP_AND_DELIVER:
                {
                    if (checkpointCount == 1)
                    {
                        Debug.LogWarning("Parcel Picked UP");
                        UIManager.Instance.ShowInformationMsg("Parcel Picked Up!", 2f);

                        missionWaypoint.SetTarget(_runningMission.deliver_point);
                        cityCheckpoint.transform.GetChild(0).gameObject.SetActive(true);
                        cityCheckpoint.transform.position = _runningMission.deliver_point.position;

                        running_mission_task.text = "Deliver parcel to its location";
                        
                    }
                    else
                    {
                        MissionPassed();
                        Debug.LogWarning("Parcel Dropped to dest");
                    }

                    break;
                }
          
            case MissionType.REACH_DEST:
                {
                    MissionPassed();
                    break;
                }            
            case MissionType.COLLECTCOINS:
                {
                    GenerateCoinsAround();
                    Debug.LogWarning("Generate Coins Here"); 
                    break;
                }
        }
    }

    #region Coins Generation
    [SerializeField] GameObject coin_prefab;
    [SerializeField] Transform coins_parent;
    private void GenerateCoinsAround()
    {
        int maxCount = Random.Range(13, 20);
        for (int i = 0; i < maxCount; i++)
        {
            Vector3 distance = Random.insideUnitSphere * 25f;
            distance.y =1f;
            Instantiate(coin_prefab, cityCheckpoint.transform.position + distance, coin_prefab.transform.rotation, coins_parent);

        }
    }

    public void DestroyCoins()
    {
        for (int i = coins_parent.transform.childCount -1; i >=0; i--)
        {
            Destroy(coins_parent.transform.GetChild(i).gameObject);
        }
    }
    #endregion

    public void MissionFailed()
    {
        cityCheckpoint.transform.GetChild(0).gameObject.SetActive(false);
        isDoingMission = false;

        cityCheckpoint.transform.GetChild(0).gameObject.SetActive(false);
        missionWaypoint.SetTarget(null);

        UIManager.Instance.ShowInformationMsg("MISSION FAILED!", 3f);

        mission_list.RemoveAt(lastStartedMission);
        RefreshMissions();

        OnMissionOver?.Invoke(false);
        _runningMission = null;

        DestroyCoins();
    }
    public void MissionPassed()
    {
        isDoingMission = false;
        cityCheckpoint.transform.GetChild(0).gameObject.SetActive(false);
        missionWaypoint.SetTarget(null);

        UIManager.Instance.ShowInformationMsg("MISSION PASSED!", 3f);

        mission_list.RemoveAt(lastStartedMission);
        RefreshMissions();

        LocalData data=DatabaseManager.Instance.GetLocalData();
        data.coins += _runningMission.reward_amount;

        data.missionCompleted++;
        DatabaseManager.Instance.UpdateData(data);

        if (data.missionCompleted % 1 == 0)
        {
            UIManager.Instance.ShowTokenUI(_runningMission.reward_amount);
        }

        UIManager.Instance.SetCoinText();

        OnMissionOver?.Invoke(true);
        _runningMission = null;

        DestroyCoins();
    }
  
    #endregion



}

[System.Serializable]
public class Mission
{
    public MissionState mission_state = MissionState.IDLE;
    public MissionType missionType;
    public string missionHeader;
    public string missionDescription;
    public float timer;
    public float min_timer;
    public float min_speed_required;
    public int reward_amount;
    public int coint_collect_amount;
    public Transform pickup_point;
    public Transform deliver_point;
}

public enum MissionType
{
    PICK_UP_AND_DELIVER , REACH_DEST, FASTDRIVE , COLLECTCOINS
}

public enum MissionState
{
    IDLE,RUNNING
}