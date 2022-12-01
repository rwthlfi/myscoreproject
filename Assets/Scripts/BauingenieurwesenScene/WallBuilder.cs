using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class WallBuilder : MonoBehaviour
{
	[Header("Container")]
	public Transform wallContainter;
	private float initContainerHeight;

	[Header("Brick Properties")]
	public GameObject brickObj;
	private GameObject cacheBrickObj;
	public float brickW = 0.1f; // change this value later in the editor
	public float brickH = 0.1f; // change this value later in the editor
	private float brickDepth = 0.24f;

	[Header("Wall Properties")]
	public float wallW = 1f; // change this value later in the editor 
	public float wallH = 1f; // change this value later in the editor
	private float wallDepth = 0.24f; // depth remains constant
	public Material brickMaterial;
	

	[Header("Foundation Properties")]
	public GameObject foundationObj;
	private GameObject cachefoundation;
	public float foundationW = 0.1f; // change this value later in the editor 
	public float foundationH = 0.1f; // change this value later in the editor 
	private float foundationDepth = 0.24f; // change this value later in the editor 

	[Header("Actuator Properties")]
	public GameObject ActuatorObj;
	private GameObject cacheActuator;
	private Transform cacheActuator_BrickBreaker;
	private Vector3 cacheActuator_BrickBreaker_pos;
	private float actuatorDepth = 0.26f; // change this value later in the editor 


	[Header("Setup Variable")]
	public float totalBricks;
	public float brickMass = 2f;
	public float displacement;

	[Header("Result Variable")]
	public float forceRequired;
	public float breakingForce = 250f;

	public buildEnum buildStatus = buildEnum.notStart;
	public enum buildEnum
    {
		notStart,
		failed_brickDoesntHaveSize,
		failed_WallToSmall,
		failed_brickDoesntHaveMass,
		failed_foundationDoesntHaveSize,
		failed_foundationTooBig,
		allReady
    }

	// Use this for initialization
	void Start()
	{
		/*
		//set Setup variables
		ResetSetupVariable();
		initContainerHeight = brickH / 2;
		SetInitialPosition(initContainerHeight*2);


		//build the Wall
		BuildFoundation(wallW, brickH, Vector3.zero);
		StartCoroutine(BuildWall(wallW, wallH));
		
		//Build Actuator
		BuildActuator((wallW + foundationW)*2, 
					   (wallH + brickH), 
					   new Vector3((-wallW - brickW-foundationW)/2 , 0, 0)
					  );
		*/

	}


    private void Update()
    {
		/*
		if(ForceCalculation()> breakingForce)
        {
			print("Activate");
			//activate the rigibody and break everything
			EnableBrickPhysics(true);
			Enable_BrickBreaker(true);
		}
		*/
		
	}

    private void ResetSetupVariable()
    {
		totalBricks = 0;
		displacement = 0;
		forceRequired = 0;
		//brickMass = 0;

	}

	//try to build the wall
    public buildEnum CheckTheBuildProperties()
    {
		//check for the size of the brick
		//if the brick width is smaller then
		if (brickW <= 0 || brickH <= 0)
			return buildEnum.failed_brickDoesntHaveSize;

		else if (foundationW <= 0 || foundationH <= 0)
			return buildEnum.failed_foundationDoesntHaveSize;

		else if (wallW <= brickW || wallH <= brickH)
			return buildEnum.failed_WallToSmall;

		else if (wallW <= foundationW || wallH<= foundationH)
			return buildEnum.failed_foundationTooBig;

		else if (brickMass <= 0)
			return buildEnum.failed_brickDoesntHaveMass;

		else
			return buildEnum.allReady;
    }

	//built the wall

	public IEnumerator BuiltTheWall(System.Action<string> _callback)
	{
		//set the initial position
		ResetSetupVariable();
		initContainerHeight = brickH / 2;
		SetInitialPosition(initContainerHeight * 2);


		//build the foundation
		BuildFoundation(wallW, brickH, Vector3.zero);

		//Build Actuator
		BuildActuator((wallW + foundationW) * 2,
					   (wallH + brickH),
					   new Vector3((-wallW - brickW - foundationW) / 2, 0, 0)
					  );

		//build the wall
		yield return StartCoroutine(BuildWall(wallW, wallH));
		print("Build");

		//return the value back for later usage.
		_callback("Success");
		
	}




	public void DestroyTheWall()
    {
		if (wallContainter.childCount <= 0)
			return;
		foreach(Transform t in wallContainter)
        {
			Destroy(t.gameObject);
        }
    }

	public float ForceCalculation()
    {
		//formula = displacement * totalbrick * brickMass
		forceRequired = displacement * totalBricks * brickMass;
		return forceRequired;
    }

	//set the initial height position áccording to the brick height
	private void SetInitialPosition(float _height)
    {
		wallContainter.position = new Vector3(wallContainter.position.x,
											  _height,
											  wallContainter.position.z);
    }

	/// <summary>
	/// build foundation of the wall
	/// </summary>
	/// <param name="_baseWidth">should be the same like the width of the wall</param>
	/// <param name="_position"></param>
	private void BuildFoundation(float _wallW, float _brickH, Vector3 _position)
	{
		//instantiate the foundation
		cachefoundation = Instantiate(foundationObj); // instantiate the foundation
		cachefoundation.transform.parent = wallContainter; // put it to a containter

		//to not waste resources in instantiating 3 objects
		//the foundation consist of 3 child. They are pillar0, pillar1 and base

		//the 0st is the Pillar 0
		cachefoundation.transform.GetChild(0).localScale = new Vector3(foundationW, foundationH, foundationDepth); // set the scaling
		cachefoundation.transform.GetChild(0).localPosition = new Vector3(-(foundationW/2), 
																		  -foundationH/2, 
																		  0); // set the position
		//cachefoundation.transform.GetChild(0).gameObject.SetActive(false);
		
		//the 1st is the Pillar 1
		cachefoundation.transform.GetChild(1).localScale = new Vector3(foundationW, foundationH, foundationDepth); // set the scaling
		cachefoundation.transform.GetChild(1).localPosition = new Vector3(_wallW + (foundationW / 2),
																		  -foundationH/2, 
																		  0); // set the position
		//cachefoundation.transform.GetChild(1).gameObject.SetActive(false);

		//the 2nd is the Base
		cachefoundation.transform.GetChild(2).localScale = new Vector3(_wallW, _brickH , foundationDepth); // set the scaling
		cachefoundation.transform.GetChild(2).localPosition = new Vector3(_wallW/2, -_brickH, 0); // set the position

		//set the position
		cachefoundation.transform.localPosition = _position; // set the position



	}
	/// <summary>
	/// To build the wall. the total brick will be adjusted according to the given individual brick width and height
	/// </summary>
	/// <param name="_width">how wide the wall should be built</param>
	/// <param name="_height">how high the wall should be built</param>
	/// <returns></returns>
	private IEnumerator BuildWall(float _width, float _height)
    {
		//instantiate the brick
		//the +brickW and +brickH is to give the brick a starting position 
		//cause if it appear at 0,0 it might collide with something or the floor.
		bool shiftBrick = false;
		float currentPosW = 0;
		float currentPosH = 0;

		//while the position is not completly filled yet, then fill it.(you dont say... :P )
		while (currentPosH < wallH  /* 1.0001f*/)  // the *1.0001 is there, due to inaccuracy of Unity calculation
        {
			while (currentPosW <=wallW || currentPosW-brickW/2 < wallW )  // this or is becaue of the shifted
			{
				cacheBrickObj = Instantiate(brickObj); // instantiate the obj
				cacheBrickObj.transform.parent = wallContainter; // put to the containter
				cacheBrickObj.transform.localScale = new Vector3(brickW, brickH, brickDepth); // set the brick's size
				cacheBrickObj.transform.localPosition = new Vector3(currentPosW, currentPosH, 0); // set the position

				currentPosW += brickW;
				yield return new WaitForSeconds(0.05f);

				//increment total bricks for later usage
				totalBricks++;
			}

			//shifting the value for the position(thus giving appearance like a brick wall)
			shiftBrick = !shiftBrick;
			if (shiftBrick) currentPosW = brickW / 2;
			else currentPosW = 0;

			//increase the height
			currentPosH += brickH;
		}

		//cut the bricks
		CutProtudingBricks();
	}

	/// <summary>
	///  to build the Actuator(The part that push the wall)
	/// </summary>
	/// <param name="_width">The height of the actuator, should be the same like the wallWidth + foundation</param>
	/// <param name="_height">The height of the actuator, should be the same like the wallHeight</param>
	/// <param name="_position">The ActuatorPosition</param>
	private void BuildActuator(float _width, float _height, Vector3 _position)
    {
		//instantiate the Actuator's pillar
		cacheActuator= Instantiate(ActuatorObj); // instantiate the foundation
		cacheActuator.transform.parent = wallContainter; // put it to a containter

		//to not waste resources in instantiatiing 2 object
		//the actuator consist of 3 child
		//the 0st is the Pillar
		cacheActuator.transform.GetChild(0).localScale = new Vector3(actuatorDepth, _height , actuatorDepth); // set the scaling
		cacheActuator.transform.GetChild(0).localPosition = new Vector3(_width, _height/2f - initContainerHeight*2, 0); // set the position
		//cacheActuator.transform.GetChild(0).gameObject.SetActive(false);

		//the 1st is the Roof
		cacheActuator.transform.GetChild(1).localScale = new Vector3(actuatorDepth, _width, actuatorDepth); // set the scaling
		cacheActuator.transform.GetChild(1).localPosition = new Vector3(_width/2, wallH + (actuatorDepth/2) - (initContainerHeight), 0); // set the position
																																		 
		//the 2nd is the BrickBreaker
		//cache the brickBreaker
		cacheActuator_BrickBreaker = cacheActuator.transform.GetChild(2);
		cacheActuator_BrickBreaker.localScale = new Vector3(actuatorDepth, _width/3.5f, actuatorDepth);
		cacheActuator_BrickBreaker_pos = new Vector3(_width, wallH + (actuatorDepth / 2) - (initContainerHeight), 0);
		cacheActuator_BrickBreaker.localPosition = cacheActuator_BrickBreaker_pos; // set the position
		

		//set the position
		cacheActuator.transform.localPosition = _position; // set the position

	}


	private void CutProtudingBricks()
    {
		foreach(WallSlicer ws in cachefoundation.GetComponentsInChildren<WallSlicer>())
        {
			ws.SliceAll();
        }

	}


	/// <summary>
	/// to disable the physics acting on the brick
	/// </summary>
	/// <param name="_isEnabled"></param>
	public void EnableBrickPhysics(bool _isEnabled)
    {
		foreach(Rigidbody rb in wallContainter.GetComponentsInChildren<Rigidbody>())
        {
			rb.isKinematic = !_isEnabled;
        }
    }

	public void Enable_BrickBreaker(bool _enabled)
    {
        if (enabled && cacheActuator_BrickBreaker)
        {
			StartCoroutine(LerpingExtensions.MoveToLocal(cacheActuator_BrickBreaker, new Vector3(0,
																								 cacheActuator_BrickBreaker_pos.y, 
																								 0)
																					,5f));
        }

        else if (!enabled && cacheActuator_BrickBreaker)
		{
			cacheActuator_BrickBreaker.localPosition = cacheActuator_BrickBreaker_pos;

		}
    }

}
