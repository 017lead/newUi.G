using UnityEngine;
using UnityEngine.InputSystem;

public class ImageSizeToggle : MonoBehaviour
{
    public GameObject[] images; // Reference to your images on the canvas
    private bool[] isImageScaled; // Keep track of whether each image is scaled
    private bool xboxControllerConnected; // Flag to check if Xbox controller is connected

    private int selectedImageIndex = 0; // Keep track of the currently selected image
    private float lastStickInputTime = 0f; // Timestamp of the last left stick input

    public float stickInputCooldown = 0.5f; // Cooldown period for left stick input in seconds

    private void Start()
    {
        isImageScaled = new bool[images.Length];
        for (int i = 0; i < images.Length; i++)
        {
            isImageScaled[i] = false;
        }
    }

    private void Update()
    {
        // Check for Xbox controller input
        if (!xboxControllerConnected)
        {
            // Check if an Xbox controller is connected
            string[] controllerNames = Input.GetJoystickNames();
            foreach (string controller in controllerNames)
            {
                if (controller.Contains("Xbox"))
                {
                    xboxControllerConnected = true;
                    break;
                }
            }
        }

        if (xboxControllerConnected)
        {
            // Toggle size using the left stick
            ToggleImageSizeWithControllerStick();
        }
        else
        {
            // Fall back to keyboard input if Xbox controller is not connected
            HandleKeyboardInput();
        }
    }

    private void HandleKeyboardInput()
    {
        // Check for keyboard input
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SelectImage(0); // Select image 1
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SelectImage(1); // Select image 2
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            SelectImage(2); // Select image 3
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            SelectImage(3); // Select image 4
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            SelectImage(4); // Select image 5
        }
    }

    private void ToggleImageSizeWithControllerStick()
    {
        // Get input from the left stick of the Xbox controller using Input System
        Vector2 stickInput = Gamepad.current.leftStick.ReadValue();

        // Check for deadzone (small values)
        float deadzone = 0.2f;
        if (stickInput.x < -deadzone)
        {
            // Move the stick to the left, check cooldown before selecting the next image
            float currentTime = Time.time;
            if (currentTime - lastStickInputTime > stickInputCooldown)
            {
                lastStickInputTime = currentTime;
                SelectNextImage();
            }
        }
    }

    private void SelectNextImage()
    {
        // Deselect the currently selected image
        if (selectedImageIndex >= 0 && selectedImageIndex < images.Length)
        {
            if (isImageScaled[selectedImageIndex])
            {
                ToggleImageSize(selectedImageIndex);
            }
        }

        // Select the next image in the sequence
        selectedImageIndex = (selectedImageIndex + 1) % images.Length;
        ToggleImageSize(selectedImageIndex);
    }

    private void SelectImage(int index)
    {
        // Deselect the currently selected image
        if (selectedImageIndex >= 0 && selectedImageIndex < images.Length && selectedImageIndex != index)
        {
            if (isImageScaled[selectedImageIndex])
            {
                ToggleImageSize(selectedImageIndex);
            }
        }

        // Select the specified image
        selectedImageIndex = index;
        ToggleImageSize(selectedImageIndex);
    }

    private void ToggleImageSize(int index)
    {
        // Toggle the image's size
        if (isImageScaled[index])
        {
            images[index].transform.localScale -= new Vector3(0.3f, 0.3f, 0);
        }
        else
        {
            images[index].transform.localScale += new Vector3(0.3f, 0.3f, 0);
        }

        // Toggle the scaling state
        isImageScaled[index] = !isImageScaled[index];
    }
}
