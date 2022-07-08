using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine;

[SelectionBase]
public class PlayerController : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private new Rigidbody rigidbody;
    [Header("Specs")]
    [SerializeField] private float speed = 10f;


    private Vector3 mouseRootPos;
    private Vector3 input = Vector3.zero;
    private bool isMoving = false;
    private const float dragDeltaToMove = 20f;
    private bool isStuck = false;


    void Update()
    {
        if (!isMoving)
            HandleWithInput();

        if (isMoving && rigidbody.velocity == Vector3.zero)
            ResetInputParams();
    }

    private void Move()
    {
        rigidbody.velocity = speed * input;
    }

    
    private void HandleWithInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            mouseRootPos = Input.mousePosition;
        }
        else if (Input.GetMouseButton(0))
        {
            var dragVec = Input.mousePosition - mouseRootPos;

            if (dragVec.magnitude >= dragDeltaToMove)
            {
                dragVec.Normalize();

                if (Mathf.Abs(dragVec.x) >= Mathf.Abs(dragVec.y))
                    dragVec.y = 0;
                else
                    dragVec.x = 0;

                Vector3 newInput = dragVec;
                newInput.z = newInput.y;
                newInput.y = 0;

                if (isStuck && newInput != Vector3.back)
                    return;
                else if (newInput == Vector3.back)
                    isStuck = false;

                input = newInput;

                mouseRootPos = Input.mousePosition;
                isMoving = true;

                Move();
            }
        }
    }

    
    private void ResetInputParams()
    {
        rigidbody.velocity = Vector3.zero;
        isMoving = false;
        mouseRootPos = Input.mousePosition;
    }
    

    private void HandleWithBridge(GameObject trigger)
    {

        bool bridgePassed = StackController.Instance.RemoveTile(trigger.transform.position);

        if (!bridgePassed)//cannot cross the bridge
        {
            isStuck = true;
            GameManager.Instance.RestartLevel();
            ResetInputParams();
        }
        else
        {
            Destroy(trigger);
            isStuck = false;// player can move on the bridge
        }
    }

    private void HandleWithFinish()
    {
        GameManager.ActionLevelPassed?.Invoke();
        Stop();
    }

    private void ChangeRoute(string routeName)
    {
        switch (routeName)
        {
            case "Left":
                ApplyNewRoute(Vector3.left);
                break;

            case "Right":
                ApplyNewRoute(Vector3.right);
                break;

            case "Forward":
                ApplyNewRoute(Vector3.forward);
                break;

            case "Back":
                ApplyNewRoute(Vector3.back);
                break;
        }
    }
    private void ApplyNewRoute(Vector3 newRoute)
    {
        rigidbody.velocity = Vector3.zero;
        input = newRoute;
        Move();
    }

    private void Stop()
    {
        rigidbody.velocity = Vector3.zero;
        enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Enter");

        if (other.CompareTag("StackTile"))
        {
            StackController.Instance.CollectTile(other.gameObject);
        }
        else if (other.CompareTag("BridgeTile"))
        {
            HandleWithBridge(other.gameObject);
        }
        else if (other.CompareTag("finish"))
        {
            HandleWithFinish();
        }
        if (other.CompareTag("BridgeRouter"))
        {
            ChangeRoute(other.name);
        }
    }
}
