using UnityEngine;
using UnityEngine.UI;

public class MissionWaypoint : MonoBehaviour
{
    // Indicator icon
    public Image img;
    public Image carImg;
    // The target (location, enemy, etc..)
    public Transform target;

    public Transform car_target;
    // UI Text to display the distance
    //public Text meter;
    // To adjust the position of the icon
    public Vector3 offset;
    [SerializeField] Camera cam;
    [SerializeField] GameObject city_checkpoint;

    public void SetTarget(Transform t)
    {
        target = t;
    }
    private void Update()
    {
        if (target == null)
        {
            img.gameObject.SetActive(false);
            city_checkpoint.SetActive(false);            
        }        
        else
        {
            img.gameObject.SetActive(true);
            city_checkpoint.SetActive(true);

            float minX = img.GetPixelAdjustedRect().width / 2;
            // Maximum X position: screen width - half of the icon width
            float maxX = Screen.width - minX;

            // Minimum Y position: half of the height
            float minY = img.GetPixelAdjustedRect().height / 2;
            // Maximum Y position: screen height - half of the icon height
            float maxY = Screen.height - minY;

            // Temporary variable to store the converted position from 3D world point to 2D screen point
            Vector2 pos = cam.WorldToScreenPoint(target.position + offset);
            
            // Check if the target is behind us, to only show the icon once the target is in front
            if (Vector3.Dot((target.position - transform.position), transform.forward) < 0)
            {
                // Check if the target is on the left side of the screen
                if (pos.x < Screen.width / 2)
                {
                    // Place it on the right (Since it's behind the player, it's the opposite)
                    pos.x = maxX;
                }
                else
                {
                    // Place it on the left side
                    pos.x = minX;
                }
            }

            pos.x = Mathf.Clamp(pos.x, minX, maxX);
            pos.y = Mathf.Clamp(pos.y, minY, maxY);



            // Update the marker's position
            img.transform.position = pos;
        }

        // Giving limits to the icon so it sticks on the screen
        // Below calculations witht the assumption that the icon anchor point is in the middle
        // Minimum X position: half of the icon width

        if (car_target == null)
        {
            carImg.gameObject.SetActive(false);
            
        }
        else
        {           
            

            float minX = carImg.GetPixelAdjustedRect().width / 2;
            // Maximum X position: screen width - half of the icon width
            float maxX = Screen.width - minX;

            // Minimum Y position: half of the height
            float minY = carImg.GetPixelAdjustedRect().height / 2;
            // Maximum Y position: screen height - half of the icon height
            float maxY = Screen.height - minY;

            // Temporary variable to store the converted position from 3D world point to 2D screen point
           
            Vector2 carpos = cam.WorldToScreenPoint(car_target.position + offset);       
          
            if (Vector3.Dot((car_target.position - transform.position), transform.forward) < 0)
            {
                // Check if the target is on the left side of the screen
                if (carpos.x < Screen.width / 2)
                {
                    // Place it on the right (Since it's behind the player, it's the opposite)
                    carpos.x = maxX;
                }
                else
                {
                    // Place it on the left side
                    carpos.x = minX;
                }
            }
            carpos.x = Mathf.Clamp(carpos.x, minX, maxX);
            carpos.y = Mathf.Clamp(carpos.y, minY, maxY);

            carImg.transform.position = carpos;
        }



        // Limit the X and Y positions
      

        // Change the meter text to the distance with the meter unit 'm'
        //meter.text = ((int)Vector3.Distance(target.position, transform.position)).ToString() + "m";
    }
    bool currentCarMarkerState = false;
    public void ToggleCarMarker(bool enabled)
    {
        carImg.gameObject.SetActive(enabled);
        currentCarMarkerState = enabled;
    }
    public void ChangeCarMarkerState()
    {
        currentCarMarkerState = !currentCarMarkerState;
        ToggleCarMarker(currentCarMarkerState);

    }
}