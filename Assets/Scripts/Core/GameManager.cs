using Oculus.Interaction.HandGrab;
using UnityEngine;
using VRHoops.Gameplay;

namespace VRHoops.Core
{
    public enum GameState { Idle, RoundStarting, WaitingForThrow, BallInAir }

    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        [Header("Experiment Logic")]
        private int throwsInCurrentStand = 0; // סופר כמה זריקות זרקנו בעמדה הנוכחית
        private int currentStandIndex = 0;    // איזה עמדה אנחנו כרגע
        private const int THROWS_PER_STAND = 5;

        [Header("Settings")]
        [Tooltip("מניעת ספירה כפולה של סל.")]
        [SerializeField] private float basketCooldownSeconds = 0.5f;

       

        private float _lastBasketTime = -999f;
        private GameState currentState = GameState.Idle;
        
        
        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        private void Start()
        {
            EventBus.StartGame();
            
            StartNewRound();
            
        }

        // מתחיל סיבוב חדש

        public void StartNewRound()
        {
            // --- לוגיקת הניסוי: בדיקה אם צריך לעבור עמדה ---
            if (throwsInCurrentStand >= THROWS_PER_STAND)
            {
                throwsInCurrentStand = 0;
                currentStandIndex++;
                EventBus.PublishStageChanged(currentStandIndex);


            }
            // -----------------------------------------------

            currentState = GameState.RoundStarting;
            Debug.Log("🏁 New round starting!");

            EventBus.PublishGameEvent(GameEventType.GameStart);

            
            Invoke(nameof(EnableThrow), 1.2f);
        }

        private void EnableThrow()
        {
            

            currentState = GameState.WaitingForThrow;
            Debug.Log("✅ Player can now throw the ball!");
        }
        
        public void OnBallThrown()
        {
            if (currentState != GameState.WaitingForThrow)
                return;

            currentState = GameState.BallInAir;
            Debug.Log("🏀 Ball thrown!");
        }

        public void OnBallScored()
        {
            if (Time.unscaledTime - _lastBasketTime < basketCooldownSeconds)
                return;

            _lastBasketTime = Time.unscaledTime;
            ResolveShot(ShotResult.Scored);
        }

        public void OnBallMissed()
        {
            //if (currentState != GameState.BallInAir)
            //    return;

            ResolveShot(ShotResult.Missed);
        }

        public void ResolveShot(ShotResult result)
        {
            // --- החלק החסר: קידום הספירה ---
            throwsInCurrentStand++;
            // -------------------------------

            Debug.Log($"🎯 Shot resolved: {result}");

            // ... (שאר הקוד הקיים שלך) ...
            EventBus.PublishShotResult(result);

            foreach (var evt in EventFactory.Build(result))
                EventBus.PublishGameEvent(evt);

            currentState = GameState.Idle;

            Invoke(nameof(StartNewRound), 2.0f);
        }
    }
}
