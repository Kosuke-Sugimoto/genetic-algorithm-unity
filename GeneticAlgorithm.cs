using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GeneticAlgorithm : MonoBehaviour
{
    public GameObject prefab;
    public int n_envs;
    public int n_survive;
    public int n_mutate;
    public int n_mutate_gene;
    public float duration;



    private GameObject[] envs;
    private Vector3[,] gene;
    private Vector3[,] old_gene;
    private Vector3[] tmp_gene;
    private KeyValuePair<int, float>[] ranking;
    private float[] prob_map = {10f/55f, 19f/55f, 27f/55f, 34f/55f, 40f/55f, 45f/55f, 49f/55f, 53f/55f, 54f/55f, 1f};
    private Dictionary<int, float> idx_pnt;
    private Environment env;
    private Ball ball;
    private int amnt_gene;
    private int generation = 0;
    private float lft_time;
    private float mn = -2f;
    private float mx = 2f;



    void Start(){
        envs = new GameObject[n_envs];
        ranking = new KeyValuePair<int, float>[n_envs];
        lft_time = duration;
        amnt_gene = 50*(int)duration+1;
        gene = new Vector3[n_envs, amnt_gene];
        old_gene = new Vector3[n_envs, amnt_gene];
        tmp_gene = new Vector3[amnt_gene];

        Generate();
        Initiate();
    }



    void FixedUpdate(){
        lft_time -= Time.deltaTime;

        if(lft_time < 0f){
            lft_time = duration;
            generation++;
            Exploit();
            Destroy_all();
            Prepare();
            Mutation();
            Escape();
            Generate();
            Inherit();
            // for(int i=0;i<amnt_gene;i++){
            //     Debug.Log(gene[ranking[0].Key, i]);
            // }
        }
    }



    private void Generate(){
        int x_interval = 25, z_interval = 25;
        int hlf = n_envs/2;

        for(int i=0;i<hlf;i++){
            GameObject obj = Instantiate(prefab, new Vector3(0, 0, i*z_interval), Quaternion.Euler(0f, 0f, 0f)) as GameObject;
            envs[i] = obj;
        }

        for(int i=hlf;i<n_envs;i++){
            GameObject obj = Instantiate(prefab, new Vector3(x_interval, 0, (i-hlf)*z_interval), Quaternion.Euler(0f, 0f, 0f)) as GameObject;
            envs[i] = obj;
        }
    }



    private void Destroy_all(){
        for(int i=0;i<n_envs;i++){
            Destroy(envs[i]);
            envs[i] = null;
        }
    }    



    private void Initiate(){
        Vector3[] tmp_arr;
        for(int i=0;i<n_envs;i++){
            env = envs[i].GetComponent<Environment>();
            ball = envs[i].transform.GetChild(0).gameObject.GetComponent<Ball>();

            tmp_arr = Generate_random();

            env.Set(duration);
            ball.Set(duration, amnt_gene, tmp_arr);
        }
    }



    private void Exploit(){
        idx_pnt = new Dictionary<int, float>();

        Vector3[] tmp_arr;
        float tmp;

        for(int i=0;i<n_envs;i++){
            env = envs[i].GetComponent<Environment>();

            tmp = env.Get();

            idx_pnt.Add(i, tmp);
        }

        int cnt = 0;
        foreach(KeyValuePair<int, float> pr in idx_pnt.OrderByDescending((x) => x.Value)){
            ranking[cnt] = pr;

            ball = envs[pr.Key].transform.GetChild(0).gameObject.GetComponent<Ball>();

            tmp_arr = ball.Get();

            Store(pr.Key, tmp_arr);

            cnt++;
        }
    }



    private void Prepare(){
        for(int i=0;i<n_envs-n_survive;i++){
            int tmp1, tmp2;

            tmp1 = Ranking_select();
            tmp2 = Ranking_select();

            // int crss_plc1 = Random.Range(0, amnt_gene-1);
            // int crss_plc2 = Random.Range(crss_plc1, amnt_gene);

            // for(int j=0;j<crss_plc1;j++){
            //     gene[i, j] = old_gene[tmp1, j];
            // }
            // for(int j=crss_plc1;j<crss_plc2;j++){
            //     gene[i, j] = old_gene[tmp2, j];
            // }
            // for(int j=crss_plc2;j<amnt_gene;j++){
            //     gene[i, j] = old_gene[tmp1, j];
            // }

            //this is uniform cross
            for(int j=0;j<amnt_gene;j++){
                float tmp3 = Random.Range(0f, 1f);

                if(tmp3<0.5f){
                    gene[i, j] = old_gene[tmp1, j];
                }else{
                    gene[i, j] = old_gene[tmp2, j];
                }
            }
        }

        int cnt = 0;
        for(int i=n_envs-n_survive;i<n_envs;i++){
            for(int j=0;j<amnt_gene;j++){
                gene[i, j] = old_gene[ranking[cnt].Key, j];
            }
            cnt++;
        }
    }



    private void Inherit(){
        for(int i=0;i<n_envs;i++){
            Weed_out(i);

            env = envs[i].GetComponent<Environment>();
            ball = envs[i].transform.GetChild(0).gameObject.GetComponent<Ball>();

            env.Set(duration);
            ball.Set(duration, amnt_gene, tmp_gene);
        }
    }



    private void Mutation(){
        int tmp1 = Random.Range(0, 100);
        int tmp2;
        float tmp3, tmp4;

        if(tmp1 == 7 || tmp1 == 77){
            for(int i=0;i<n_mutate;i++){
                tmp1 = Random.Range(0, n_envs);

                for(int j=0;j<n_mutate_gene;j++){
                    tmp2 = Random.Range(0, amnt_gene);

                    tmp3 = Random.Range(mn, mx);
                    tmp4 = Random.Range(mn, mx);

                    gene[tmp1, tmp2] = new Vector3(tmp3, 0f, tmp4);
                }
            }
        }
    }



    private void Escape(){
        if(ranking[0].Value-ranking[n_envs-1].Value < 0.1f){
            int tmp1, tmp2;
            float tmp3, tmp4;

            for(int i=0;i<n_mutate;i++){
                tmp1 = Random.Range(0, n_envs);

                for(int j=0;j<n_mutate_gene;j++){
                    tmp2 = Random.Range(0, amnt_gene);

                    tmp3 = Random.Range(mn, mx);
                    tmp4 = Random.Range(mn, mx);

                    gene[tmp1, tmp2] = new Vector3(tmp3, 0f, tmp4);
                }
            }
        }
    }



    private Vector3[] Generate_random(){
        Vector3[] tmp_arr = new Vector3[amnt_gene];
        Vector3 tmp;
        float tmp1, tmp2;

        for(int i=0;i<amnt_gene;i++){
            tmp1 = Random.Range(mn, mx);
            tmp2 = Random.Range(mn, mx);

            tmp = new Vector3(tmp1, 0f, tmp2);

            tmp_arr[i] = tmp;
        }

        return tmp_arr;
    }



    private void Store(int idx, Vector3[] vec){
        for(int i=0;i<amnt_gene;i++){
            old_gene[idx, i] = vec[i];
        }
    }



    private int Ranking_select(){
        float tmp = Random.Range(0f, 1f);

        for(int i=0;i<10;i++){
            if(i==9){
                return ranking[9].Key;
            }else if(tmp < prob_map[0]){
                return ranking[0].Key;
            }else if(prob_map[i] < tmp && tmp < prob_map[i+1]){
                return ranking[i].Key;
            }
        }

        return 0;
    }

    private void Weed_out(int idx){
        for(int i=0;i<amnt_gene;i++){
            tmp_gene[i] = gene[idx, i];
        }
    }



    void OnGUI() {
		string str = "";
		str += string.Format("Generation: {0}\n", generation);
		str += string.Format("Time: {0}\n", lft_time);
		str += string.Format("\n", lft_time);
		str += string.Format("Best score\n");
		for(int i=0; i<10; ++i ) {
			str += string.Format("  {0:D2}: {1:F2}\n", ranking[i].Key, ranking[i].Value);
		}

		GUIStyle style = new GUIStyle();
		style.normal.textColor = Color.black;
		GUI.Label(new Rect(10, 10, 100, 40), str, style);
	}
}