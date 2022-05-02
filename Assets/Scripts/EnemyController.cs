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
	void Start()
	{
		var tar = targetsParent.GetComponentsInChildren<Transform>();
		foreach(Transform t in tar){
			targets.Add(t);
		}
		_anim = GetComponentInChildren<Animator>();
		playerTarget = GameManager.instance.Player.transform;
		agent = GetComponent<NavMeshAgent>();
		//combat = GetComponent<CharacterCombat>();
	}
	public int route_id=-1;
	public bool route=false,triggered=false;
	int prev= -1;
	// Update is called once per frame
	void Update()
	{
		// Distance to the player
		float distance = Vector3.Distance(playerTarget.position, transform.position);
		//if(wait)
		//{
			//if(agent.pathStatus== NavMeshPathStatus.PathInvalid)
			//{

			//}
		//}
		//else{
		// If inside the lookRadius
		if (distance <= lookRadius)
		{
			FaceTarget(playerTarget);
			Debug.DrawRay(look.transform.position,-transform.position+playerTarget.position, Color.blue);
			RaycastHit hit;
			if(Physics.Raycast(look.transform.position, -transform.position+playerTarget.position, out hit, lookRadius)){
				Debug.Log("tag "+hit.collider.tag);
				if(hit.collider.tag=="Player"){
					if(!triggered){
					state = aiState.Triggered;
					agent.speed = triggered_speed;
					target = playerTarget;
					agent.SetDestination(target.position);
					_anim.SetBool("walk",true);
					triggered=true;
					}
					
				return;
				}
				else{
					if(triggered&&!c){
						StartCoroutine(Wait_());
						//return;
					}
					else if(!triggered){

					}
				}
			}
			
			// Move towards the target
			
			//if(agent.velocity.magnitude<0.1f&&!c){
			//	StartCoroutine(Wait_());
			//}
			// If within attacking distance
			if (distance <= agent.stoppingDistance)
			{
				_anim.SetBool("walk",false);
				//CharacterStats targetStats = target.GetComponent<CharacterStats>();
				//if (targetStats != null)
				//{
				//	combat.Attack(targetStats);
				//}
				//FaceTarget();   // Make sure to face towards the target
			}
		}//}
		if(!route)
		{
			state = aiState.Idle;
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
		//else if(Vector3.Distance(target.position, transform.position)<=agent.stoppingDistance&&!c)
		//{
		//	StartCoroutine(Wait());
		//}
		else if(agent.velocity.magnitude<0.01f)
		{
			if(!c)StartCoroutine(Wait());
			//route = false;
		}
		else{
			StopCoroutine(Wait());
		}
		//if(_anim.GetBool("walk")) _anim.SetBool("walk",false);
			
	}
	bool c=false, wait=false;
	IEnumerator Wait_()
	{
		if(agent.pathStatus== NavMeshPathStatus.PathInvalid)
			{
				Debug.Log("invalid");
			}
		c=true;
		_anim.SetBool("walk",false);
		yield return new WaitForSeconds(Random.Range(5f,25f));
		triggered = false;
		c=false;
	}
	IEnumerator Wait()
	{
		c=true;
		state = aiState.Wait;
		_anim.SetBool("walk",false);
		yield return new WaitForSeconds(Random.Range(1f,5f));
		route = false;
		c=false;
	}
	// Rotate to face the target
	void FaceTarget(Transform target)
	{
		Vector3 direction = (target.position - transform.position).normalized;
		Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
		transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
	}
	public void LoadEnemy(AIdata data)
	{
		this.gameObject.transform.position = new Vector3(data.position[0],data.position[1],data.position[2]);
		state = (aiState)data.state;
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
	Wait
}
