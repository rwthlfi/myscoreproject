using UnityEngine;

//an extension script to check if an object is visible from the camera renderer
//http://wiki.unity3d.com/index.php?title=IsVisibleFrom&_ga=2.195640239.2037607007.1587364531-1862652710.1586385784
public static class RendererExtensions
{
	public static bool IsVisibleFrom(this Renderer renderer, Camera camera)
	{
		Plane[] planes = GeometryUtility.CalculateFrustumPlanes(camera);
		return GeometryUtility.TestPlanesAABB(planes, renderer.bounds);
	}
}

//usage
/*
   if (renderer.IsVisibleFrom(Camera.main)) Debug.Log("Visible");
	 else Debug.Log("Not visible");
*/