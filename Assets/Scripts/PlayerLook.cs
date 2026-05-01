using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerLook : MonoBehaviour
{
    [Header("Bakış Ayarları")]
    public float mouseSensitivity = 10f;
    public Transform playerBody; // FPS yürüme için eklendi
    
    private float xRotation = 0f;
    private float yRotation = 0f;
    
    // Gaze Tracker için hedef objeyi takip edeceğiz
    [Header("Odak Takibi (Gaze Tracking)")]
    public float interactionDistance = 5f;
    private string currentTargetTag = "";
    private float gazeStartTime;

    void Start()
    {
        // Fare imlecini oyun ekranının ortasına kilitler ve gizler
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        float mouseX = 0f;
        float mouseY = 0f;
        
        // Yeni Input System üzerinden fare hareketini alıyoruz
        if (Mouse.current != null)
        {
            Vector2 delta = Mouse.current.delta.ReadValue();
            mouseX = delta.x * mouseSensitivity * Time.deltaTime;
            mouseY = delta.y * mouseSensitivity * Time.deltaTime;
        }

        // Yukarı aşağı bakma (X ekseni dönüşü)
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f); // Boynun kırılmasını önler

        // Sağa sola bakma
        if (playerBody != null)
        {
            // FPS Yürüme modu: Tüm vücut sağa sola döner
            playerBody.Rotate(Vector3.up * mouseX);
            // Sadece kamerayı yukarı/aşağı çevir
            transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        }
        else
        {
            // Oturma modu: Sadece kamera döner
            yRotation += mouseX;
            yRotation = Mathf.Clamp(yRotation, -70f, 70f);
            transform.localRotation = Quaternion.Euler(xRotation, yRotation, 0f);
        }

        // Gaze Tracking (Bakış takibi) simülasyonu
        TrackGaze();
    }

    private void TrackGaze()
    {
        // Kameranın tam ortasından ileriye doğru bir ışın gönder
        Ray ray = new Ray(transform.position, transform.forward);
        
        if (Physics.Raycast(ray, out RaycastHit hit, interactionDistance))
        {
            string hitName = hit.collider.gameObject.name;
            
            // Eğer bakılan obje değiştiyse
            if (hitName != currentTargetTag)
            {
                currentTargetTag = hitName;
                gazeStartTime = Time.time;
            }
            else
            {
                // Aynı objeye 3 saniyeden fazla bakıyorsa logla
                if (Time.time - gazeStartTime > 3f && currentTargetTag != "Floor")
                {
                    Debug.Log($"[L.O.G.O.S.] Oyuncu {currentTargetTag} objesini uzun süredir inceliyor.");
                    // Bunu ileride GlobalStateManager.Instance.AddChaos() ile bağlayabiliriz
                    
                    // Log spami önlemek için süreyi artırıyoruz
                    gazeStartTime = Time.time + 1000f; 
                }
            }
        }
        else
        {
            currentTargetTag = "";
        }
    }
}
