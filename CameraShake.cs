using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour {

	Vector3 cameraInitialPosition;
    public float shakeMagnetude;//0.05f, 0.5f
	public Camera mainCamera;
    public bool active;

    
    //Shakes the camera, based on a magnitude and for a shakeTime.
	public void ShakeIt(float shakeMag, float shakeTime)
	{
		cameraInitialPosition = mainCamera.transform.localPosition;
        shakeMagnetude = shakeMag;
		InvokeRepeating ("StartCameraShaking", 0f, 0.002f);
		Invoke ("StopCameraShaking", shakeTime);
	}

    //Used by ShakeIt() to start shaking the camera.
	void StartCameraShaking()
	{
		float cameraShakingOffsetX = Random.value * shakeMagnetude * 2 - shakeMagnetude;
		float cameraShakingOffsetY = Random.value * shakeMagnetude * 2 - shakeMagnetude;
		Vector3 cameraIntermadiatePosition = mainCamera.transform.localPosition;
		cameraIntermadiatePosition.x += cameraShakingOffsetX;
		cameraIntermadiatePosition.y += cameraShakingOffsetY;
		mainCamera.transform.localPosition = Vector3.Lerp(mainCamera.transform.localPosition,cameraIntermadiatePosition, 0.2f);
	}

    //Used by ShakeIt() to stop shaking the camera.
    void StopCameraShaking()
	{
		CancelInvoke ("StartCameraShaking");
		mainCamera.transform.localPosition = new Vector3(0,0,0);
	}

}
