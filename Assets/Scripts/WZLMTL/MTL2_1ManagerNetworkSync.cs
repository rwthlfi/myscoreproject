using System.Collections;
using UnityEngine.UI;
using UnityEngine;
using Mirror;

public class MTL2_1ManagerNetworkSync : NetworkBehaviour
{
    public GameObject start, help, loading, measurementanalysisscreen, results, loading1, loading2general, loading2specifications, loading2settings;
    public Button startEstimationButton, manualButton, closeHelpButton, closeAnalysisButton, resultsButton, resultsHintButton, closeResultButton;
    public int caliperIndex, caliperNumberindex, caliperNumberCountdown;
    public GameObject[] gameObjectChildrenMeasurement, gameObjectChildrenEstimationPoints;
    public Text n_eff_text, t_text, ev_text, av_text, grr_text;


    // Start is called before the first frame update
    void Start()
    {
        caliperIndex = 0;
        caliperNumberCountdown = 0;
    }

    //////////////////////////////////////////////////////////////////////////

    public void StartEstimation()
    {
        Cmd_StartEstimation();
    }

    [Command(requiresAuthority = false)]
    private void Cmd_StartEstimation()
    {
        Rpc_StartEstimation();
    }

    [ClientRpc]
    private void Rpc_StartEstimation()
    {
        start.SetActive(false);
        loading.SetActive(true);

        Cmd_LoadingScreen();
    }

    //////////////////////////////////////////////////////////////////////////

    public void OpenManual()
    {
        Cmd_OpenManual();
    }

    [Command(requiresAuthority = false)]
    private void Cmd_OpenManual()
    {
        Rpc_OpenManual();
    }

    [ClientRpc]
    private void Rpc_OpenManual()
    {
        start.SetActive(false);
        help.SetActive(true);
    }

    //////////////////////////////////////////////////////////////////////////

    public void CloseManual()
    {
        Cmd_CloseManual();
    }

    [Command(requiresAuthority = false)]
    private void Cmd_CloseManual()
    {
        Rpc_CloseManual();
    }

    [ClientRpc]
    private void Rpc_CloseManual()
    {
        help.SetActive(false);
        start.SetActive(true);
    }

    //////////////////////////////////////////////////////////////////////////

    public void CloseMeasureSystem()
    {
        Cmd_CloseMeasureSystem();
    }

    [Command(requiresAuthority = false)]
    private void Cmd_CloseMeasureSystem()
    {
        Rpc_CloseMeasureSystem();
    }

    [ClientRpc]
    private void Rpc_CloseMeasureSystem()
    {
        measurementanalysisscreen.SetActive(false);
        start.SetActive(true);

        caliperIndex = 0;
        caliperNumberCountdown = 0;
        caliperNumberindex = 0;

        loading1.SetActive(false);
        loading2general.SetActive(false);
        loading2settings.SetActive(false);
        loading2specifications.SetActive(false);

        ResetLists();
    }

    //////////////////////////////////////////////////////////////////////////

    public void ShowResults()
    {
        Cmd_ShowResults();
    }

    [Command(requiresAuthority = false)]
    private void Cmd_ShowResults()
    {
        Rpc_ShowResults();
    }

    [ClientRpc]
    private void Rpc_ShowResults()
    {
        results.SetActive(true);
    }

    //////////////////////////////////////////////////////////////////////////

    public void CloseResults()
    {
        Cmd_CloseResults();
    }

    [Command(requiresAuthority = false)]
    private void Cmd_CloseResults()
    {
        Rpc_CloseResults();
    }

    [ClientRpc]
    private void Rpc_CloseResults()
    {
        results.SetActive(false);
    }

    //////////////////////////////////////////////////////////////////////////

    [Command(requiresAuthority = false)]
    private void Cmd_LoadingScreen()
    {
        Rpc_LoadingScreen();
    }

    [ClientRpc]
    private void Rpc_LoadingScreen()
    {
        StartCoroutine(LoadingScreen());
    }

    IEnumerator LoadingScreen()
    {
        ResetLists();

        yield return new WaitForSeconds(1f);
        loading1.SetActive(true);
        yield return new WaitForSeconds(2f);
        loading2general.SetActive(true);
        yield return new WaitForSeconds(5f);
        loading2general.SetActive(false);
        loading2specifications.SetActive(true);
        yield return new WaitForSeconds(5f);
        loading2specifications.SetActive(false);
        loading2settings.SetActive(true);
        yield return new WaitForSeconds(5f);
        loading.SetActive(false);
        measurementanalysisscreen.SetActive(true);
    }

    //////////////////////////////////////////////////////////////////////////

    public void GetEstimationValues(float caliperNumber)
    {
        Cmd_GetEstimationValues(caliperNumber);
    }

    [Command(requiresAuthority = false)]
    private void Cmd_GetEstimationValues(float caliperNumber)
    {
        Rpc_GetEstimationValues(caliperNumber);
    }

    [ClientRpc]
    private void Rpc_GetEstimationValues(float caliperNumber)
    {
        if (caliperNumberindex <= 99)
        {
            gameObjectChildrenMeasurement[caliperIndex].GetComponent<GameObjectChildrenList>().gameObjectChildren[caliperNumberCountdown].GetComponent<Text>().text = caliperNumber.ToString();
            gameObjectChildrenEstimationPoints[caliperNumberindex].SetActive(true);

            var x = gameObjectChildrenEstimationPoints[caliperNumberindex].GetComponent<RectTransform>().anchoredPosition.x;
            var y = (20 - caliperNumber) * 1000 * -1;
            gameObjectChildrenEstimationPoints[caliperNumberindex].GetComponent<RectTransform>().anchoredPosition = new Vector2(x, y);

            caliperNumberindex++;

            if (caliperNumberCountdown <= 3)
            {
                caliperNumberCountdown++;
            }
            else if (caliperNumberCountdown >= 4)
            {
                caliperNumberCountdown = 0;
                caliperIndex++;
            }

            // add here calculation with caliperNumber to use for result2
            n_eff_text.text = caliperNumberindex.ToString(); // ask if this is correct?
                                                             // t_text = ??
                                                             // ev_text = ??
                                                             // av_text = ??
                                                             // grr_text = ??
        }
    }

    //////////////////////////////////////////////////////////////////////////

    private void ResetLists()
    {
        Cmd_ResetLists();
    }

    [Command(requiresAuthority = false)]
    private void Cmd_ResetLists()
    {
        Rpc_ResetLists();
    }

    [ClientRpc]
    private void Rpc_ResetLists()
    {
        foreach (GameObject obj in gameObjectChildrenMeasurement)
        {
            obj.GetComponent<GameObjectChildrenList>().gameObjectChildren[0].GetComponent<Text>().text = "0";
            obj.GetComponent<GameObjectChildrenList>().gameObjectChildren[1].GetComponent<Text>().text = "0";
            obj.GetComponent<GameObjectChildrenList>().gameObjectChildren[2].GetComponent<Text>().text = "0";
            obj.GetComponent<GameObjectChildrenList>().gameObjectChildren[3].GetComponent<Text>().text = "0";
            obj.GetComponent<GameObjectChildrenList>().gameObjectChildren[4].GetComponent<Text>().text = "0";
        }

        foreach (GameObject obj2 in gameObjectChildrenEstimationPoints)
        {
            obj2.gameObject.SetActive(false);
        }
    }
    //////////////////////////////////////////////////////////////////////////
}