using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
	public Transform look;
	float idle_speed=2f, triggered_speed = 4f;
	public aiState state;
	public float lookRadius = 2f;  // Detection range for player
	public Animator _anim;
	Transform target, playerTarget;   // Reference to the player
	NavMeshAgent agent; // Reference to the NavMeshAgent
	public GameObject targetsParent;
	//CharacterCombat combat;
	public List<Transform> targets;

	// Use this for initialization
	void Awake(){
	}
	public void LoadEnemy(AIdata data)
	{
		this.gameObject.transform.position = new Vector3(data.position[0],data.position[1],data.position[2]);
		state = (aiState)data.state;
		GameManager.instance.enemy_pos = new Vector3(data.position[0],data.position[1],data.position[2]);
	}
	void Start()
	{
		Debug.Log("alo");
		var tar = targetsParent.GetComponentsInChildren<Transform>();
		foreach(Transform t in tar){
			targets.Add(t);
		}
		_anim = GetComponentInChildren<Animator>();
		playerTarget = GameManager.instance.Player.transform;
		agent = GetComponent<NavMeshAgent>();
		//return new WaitUntil(()=>Player.instance.loaded);
		//Debug.Log("loaded");
		//LoadEnemy(GameManager.instance.GetEnemyData());
		//combat = GetComponent<CharacterCombat>();
	}
	public int route_id=-1;
	public bool route=false,triggered=false, c =false,wait=false;
	int prev= -1;
	// Update is called once per frame
	void Update()
	{
		// Distance to the player
		float distance = Vector3.Distance(playerTarget.position, transform.position);
		Debug.Log("hui");
		if (distance <= agent.stoppingDistance)
		{	
			if(!wait){
				StartCoroutine(Wait_());
				EventController.instance.StartHPchange(-1,1);
				EventController.instance.GameOverEvent();
			}
			//SoundManager.instance.PlaySe(Se.Screamer);
			_anim.SetBool("walk",false);
		}
		if (distance <= lookRadius)
		{
			if(state!=aiState.Near&&state!=aiState.Triggered&&state!=aiState.Search)
			{
				state = aiState.Near;
				SoundManager.instance.PlayAltBg(Bg.spooky);
				//return;
			}
			else if(state==aiState.Near)//near the player
			{
				FaceTarget(playerTarget);
				Debug.DrawRay(look.transform.position,-transform.position+playerTarget.position, Color.blue);
				RaycastHit hit;
				if(Physics.Raycast(look.transform.position, -transform.position+playerTarget.position, out hit, lookRadius)){
					Debug.Log("tag "+hit.collider.tag);
					if(hit.collider.tag=="Player") //triggers
					{
						state = aiState.Triggered;
						SoundManager.instance.PlayAltBg(Bg.fnaf);
						agent.speed = triggered_speed;
						target = playerTarget;
						agent.SetDestination(target.position);
						_anim.SetBool("walk",true);
						triggered=true;
						route=false; //?
						return;
					}
				}
			}
			else if(state==aiState.Triggered){
				FaceTarget(playerTarget);
				Debug.DrawRay(look.transform.position,-transform.position+playerTarget.position, Color.blue);
				RaycastHit hit;
				if(Physics.Raycast(look.transform.position, -transform.position+playerTarget.position, out hit, lookRadius)){
					Debug.Log("tag "+hit.collider.tag);
					if(hit.collider.tag=="Player") //moves towards
					{
						SoundManager.instance.PlayAltBg(Bg.fnaf);
						agent.speed = triggered_speed;
						target = playerTarget;
						_anim.SetBool("walk",true);
						agent.SetDestination(target.position);
					//	if(c)
					//	{

						//	StopCoroutine(Wait_());
						//	agent.isStopped=false;
							//agent.speed = triggered_speed;
							//target = playerTarget;
							//agent.SetDestination(target.position);
						//	_anim.SetBool("walk",true);
						//	c=false;
						//}
					}
					else{
						state = aiState.Search;
					//	if(!c)
					//	{
					//		StartCoroutine(Wait_());
					//		agent.isStopped=true;
					//	}
					}
			}
			return;
		}
			else if(state==aiState.Search)
		{
			SoundManager.instance.PlayAltBg(Bg.fnaf);
			FaceTarget(playerTarget);
			agent.isStopped=false;
			RaycastHit hit;
			if(Physics.Raycast(look.transform.position, -transform.position+playerTarget.position, out hit, lookRadius)){
				Debug.Log("tag "+hit.collider.tag);
				if(hit.collider.tag=="Player") //triggers again
				{
					if(c){
						c=false;
						StopCoroutine(Search());
					}
					state = aiState.Triggered;	
				}
				else //forgets
				{
					if(!c)
					{
						StartCoroutine(Search());
					}
				}
			}
				return;
			}
		}
		else if(state==aiState.Near)
		{
			state=aiState.Idle;
			SoundManager.instance.ResumeLastBg();
		}
		//else if(state!=aiState.Idle&&state!=aiState.Wait&&state!=aiState.Search)
		//{
		//	state = aiState.Idle;
		//	SoundManager.instance.PlayBg(Bg.scene1);
		//}
		if(!route)
		{
			if(state!=aiState.Near) //?
			{
				state = aiState.Idle;
				SoundManager.instance.ResumeLastBg();
			}
			while(prev==route_id){
				route_id = Random.Range(0,targets.Count);
			}
			prev = route_id;
			target = targets[route_id];_anim.SetBool("walk",true);
			agent.speed = idle_speed;
			agent.SetDestination(target.position);
			route=true;
			return;
		}
		else if(agent.velocity.magnitude<0.01f)
		{
			if(!c)StartCoroutine(Wait());
		}
		else if(c)
		{
			StopCoroutine(Wait());c=false;
			_anim.SetBool("walk",true);
		}
	}
	//bool wait=false;
	IEnumerator Wait_()
	{
		wait=true;
		yield return new WaitForSeconds(2f);
		wait=false;
	}
	IEnumerator Search(){
		c=true;
		//_anim.SetBool("walk",true);
		float secs = Random.Range(5f,25f);
		Debug.Log("search() "+secs);
		yield return new WaitForSeconds(secs);
		state=aiState.Idle;
		SoundManager.instance.ResumeLastBg();
		c=false;
	}
	IEnumerator Wait()
	{
		c=true;
		state = aiState.Wait;
		_anim.SetBool("walk",false);
		yield return new WaitForSeconds(Random.Range(1f,5f));
		route = false;
		state = aiState.Idle;//?
		c=false;
	}
	// Rotate to face the target
	void FaceTarget(Transform target)
	{
		Vector3 direction = (target.position - transform.position).normalized;
		Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
		transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
	}
	
	// Show the lookRadius in editor
	void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(transform.position, lookRadius);
		//Gizmos.DrawRay(transform.position,transform.position-playerTarget.position, Color.blue);
    }
}

public enum aiState
{
	Idle,
	Walk,
	Triggered,
	Wait,
	Near,
	Search
}
