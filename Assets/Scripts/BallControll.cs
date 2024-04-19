using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class BallControll : MonoBehaviour
{
    [Header("References")]
    public Rigidbody rb;
    public Camera mainCamera;
    public GameObject arrow;
    public Slider powerSlider;
    public string floorTag;
    public string holeTag;
    public TMP_Text shotsText;

    [Header("Settings")]
    public float stopForce;
    public float shootForce;
    public float maxShootForce;
    public float maxLineLength;
    bool isAiming;
    bool isShooting;
    bool isIdle;

    private int numberOfShots = 0;
    Vector3? worldPosition;


    void Start()
    {
        rb.maxAngularVelocity = 1000;
        isAiming = false;
        powerSlider.gameObject.SetActive(false);
        arrow.SetActive(false);
    }

    void Update()
    {
        if (rb.velocity.magnitude < stopForce)
        {
            ProcessAim();

            if (Input.GetMouseButtonDown(0))
            {
                if (isIdle) isAiming = true;
            }

            if (Input.GetMouseButtonUp(0))
            {
                isShooting = true;
            }
        }
        shotsText.text = "SHOTS: " + GetNumberOfShots().ToString();
    }

    void FixedUpdate()
    {
        if (rb.velocity.magnitude < stopForce)
        {
            Stop();
        }
        if (isShooting && worldPosition.HasValue)
        {
            Shoot(worldPosition.Value);
            numberOfShots++;
            isShooting = false;
        }
    }

    private void ProcessAim()
    {
        if (isAiming && !isIdle) return;

        worldPosition = CastMouseClickRay();

        if (!worldPosition.HasValue) return;

        UpdateArrowDirection();
    }

    private void UpdateArrowDirection()
    {
        Vector3 lineDirection = worldPosition.Value - transform.position;

        arrow.SetActive(true);
        powerSlider.gameObject.SetActive(true);

        Quaternion rotation = Quaternion.LookRotation(lineDirection.normalized, Vector3.up);
        Vector3 eulerAngles = rotation.eulerAngles;
        eulerAngles.x = 90;
        arrow.transform.rotation = Quaternion.Euler(eulerAngles);

        float power = Mathf.Clamp01(lineDirection.magnitude / maxLineLength);
        powerSlider.value = power;
    }

    private Vector3? CastMouseClickRay()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            return hit.point;
        }
        return null;
    }

    private void Stop()
    {
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        isIdle = true;
        powerSlider.gameObject.SetActive(false);
    }

    private void Shoot(Vector3 point)
    {
        isAiming = false;
        arrow.SetActive(false);

        Vector3 horizontalWorldPosition = new Vector3(point.x, transform.position.y, point.z);
        Vector3 direction = (transform.position - horizontalWorldPosition).normalized;

        float distance = Vector3.Distance(horizontalWorldPosition, transform.position);
        rb.AddForce(-direction * distance * shootForce);
    }

    private int GetNumberOfShots()
    {
        return numberOfShots;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(floorTag))
        {
            StartCoroutine(RestartGameCoroutine());
        }
        if (other.CompareTag(holeTag))
        {
            StartCoroutine(LoadNextScene());
        }
    }

    IEnumerator RestartGameCoroutine()
    {
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    IEnumerator LoadNextScene()
    {
        yield return new WaitForSeconds(2);
        SceneManager.LoadSceneAsync(this.gameObject.scene.buildIndex + 1);
    }
}
