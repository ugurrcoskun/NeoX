using UnityEngine;

public class GlobalStateManager : MonoBehaviour
{
    public static GlobalStateManager Instance { get; private set; }

    [Header("Sistem Değerleri")]
    [Range(0, 100)] public int Compliance = 50; // İtaat
    [Range(0, 100)] public int Chaos = 0;       // Kaos

    [Header("Oyun Evresi")]
    public GamePhase CurrentPhase = GamePhase.Optimization;

    public enum GamePhase { Optimization, Glitch, Exception }

    private void Awake()
    {
        // Singleton deseni: Oyunda sadece bir tane GlobalStateManager olmasını garantiliyoruz
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        ApplyEnvironmentState();
        CheckPhaseTransition();
    }

    private void ApplyEnvironmentState()
    {
        // İleride buraya Post-Processing (efektler) ve Ses kontrollerini ekleyeceğiz.
        // Chaos ve Compliance değerlerine göre ortam değişecek.
    }

    private void CheckPhaseTransition()
    {
        // GDD'ye göre evre geçiş kontrolleri (Örnek)
        if (CurrentPhase == GamePhase.Optimization && Compliance > 70)
        {
            // İleride buraya "TasksCompleted >= 3" gibi kontroller de ekleyeceğiz
            TransitionToPhase(GamePhase.Glitch);
        }
    }

    private void TransitionToPhase(GamePhase newPhase)
    {
        if (CurrentPhase == newPhase) return;
        
        CurrentPhase = newPhase;
        Debug.Log($"[L.O.G.O.S.] Sistem Evresi Değişti: {newPhase}");
    }

    // Kaos ve İtaat değerlerini değiştirmek için kullanacağımız yardımcı fonksiyonlar
    public void AddChaos(int amount)
    {
        Chaos = Mathf.Clamp(Chaos + amount, 0, 100);
        Debug.Log($"Yeni Kaos Değeri: {Chaos}");
    }

    public void AddCompliance(int amount)
    {
        Compliance = Mathf.Clamp(Compliance + amount, 0, 100);
        Debug.Log($"Yeni İtaat Değeri: {Compliance}");
    }
}
