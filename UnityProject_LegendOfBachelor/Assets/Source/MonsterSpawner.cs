using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System.Linq;

public class MonsterSpawner : MonoBehaviour
{
    public Terrain terrain;

    public List<MonsterType> monsterTypes= new List<MonsterType>();
    // Start is called before the first frame update
    void Start()
    {
        terrain = GameObject.Find("Terrain").GetComponent<Terrain>();
        foreach(MonsterType mt in monsterTypes)
        {
            mt.Setup();
            mt.containingClass = this;
        }
    }

    // Update is called once per frame
    void Update()
    {
        foreach(MonsterType mt in monsterTypes)
        {
            mt.Update();
        }
    }

    [System.Serializable]
    public class MonsterType
    {
        public string name;
        public int spawnAmount = 10;
        public List<SphereCollider> spawnAreas;
        public GameObject prefab;
        [Header("System")]
        public List<SpawnedObject> spawnedObjects = new List<SpawnedObject>();
        public MonsterSpawner containingClass;
        public Player player;



        public void Setup()
        {
            LOBEvents.current.onMonsterDeath += Monsterkilled;
            player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        }

        private void Monsterkilled(Monster monster)
        {
            IEnumerable<SpawnedObject> spObj = spawnedObjects.Where(spawnedObject => spawnedObject.gameObject == monster.gameObject);
            if (spObj.Count() >= 1)
            {
                spawnedObjects.Remove(spObj.First());
            }
        }

        public void Update()
        {
            if(spawnedObjects.Count < spawnAmount)
            {
                SphereCollider randArea = spawnAreas[(int)Random.Range(0, spawnAreas.Count)];
                float size = randArea.radius * randArea.transform.localScale.x ;
                var unitCircle = Random.insideUnitCircle;
                Vector3 pointInSpace = new Vector3(unitCircle.x, 0, unitCircle.y) * size;
                var pointOnMap = pointInSpace + randArea.transform.position;
                pointOnMap = new Vector3(pointOnMap.x, containingClass.terrain.SampleHeight(pointOnMap), pointOnMap.z);


                GameObject instance = Instantiate(prefab, containingClass.gameObject.transform);
                instance.GetComponent<NavMeshAgent>().Warp(pointOnMap); // ändern
                spawnedObjects.Add(new SpawnedObject( instance, pointOnMap, this));
                
            }

            Vector3 playerPos = player.transform.position;
            foreach (SpawnedObject spawnedObject in spawnedObjects)
            {
                if(Vector3.Distance(playerPos, spawnedObject.position) < SpawnedObject.disableDistance)
                {
                    spawnedObject.gameObject.SetActive(true);
                }
                else
                {
                    spawnedObject.gameObject.SetActive(false);
                }
            }
        }

        
        public class SpawnedObject
        {
            public GameObject gameObject;
            public Vector3 position;
            public MonsterType containingClass;

            public static float disableDistance = 200;

            public SpawnedObject(GameObject gameObject, Vector3 pos, MonsterType monsterType)
            {
                this.gameObject = gameObject;
                this.position = pos;
                this.containingClass = monsterType;

                if(Vector3.Distance( containingClass.player.transform.position, pos) > disableDistance)
                {
                    gameObject.SetActive(false);
                }
            }
        }
    }
}
