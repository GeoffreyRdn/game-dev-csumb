using UnityEngine;

public class CameraController : MonoBehaviour {
	
	[SerializeField] private float panSpeed = 30f;
	[SerializeField] private float panBorderThickness = 10f;

	[SerializeField] private float scrollSpeed = 5f;
	[SerializeField] private float minY = 10f;
	[SerializeField] private float maxY = 80f;
	
	private bool doMovement = true;
	
	private void Update () {
		// lock cam
		if (Input.GetKeyDown(KeyCode.Escape)) doMovement = !doMovement;

		if (!doMovement) return;

		// move cam with mouse or keyboard
		if (Input.GetKey("w") || Input.GetKey("z") || Input.mousePosition.y >= Screen.height - panBorderThickness)
			transform.Translate(Vector3.forward * panSpeed * Time.deltaTime, Space.World);
		
		if (Input.GetKey("s") || Input.mousePosition.y <= panBorderThickness)
			transform.Translate(Vector3.back * panSpeed * Time.deltaTime, Space.World);
		
		if (Input.GetKey("d") || Input.mousePosition.x >= Screen.width - panBorderThickness)
			transform.Translate(Vector3.right * panSpeed * Time.deltaTime, Space.World);
		
		if (Input.GetKey("a") || Input.GetKey("q") || Input.mousePosition.x <= panBorderThickness)
			transform.Translate(Vector3.left * panSpeed * Time.deltaTime, Space.World);
		

		// zoom in and zoom out
		var scroll = Input.GetAxis("Mouse ScrollWheel");

		Vector3 pos = transform.position;
		pos.y -= scroll * 1000 * scrollSpeed * Time.deltaTime;
		pos.y = Mathf.Clamp(pos.y, minY, maxY);
		transform.position = pos;
	}
}
