using UnityEngine;
using VRHoops.Gameplay;

namespace VRHoops.Core
{
    public enum GameState { Idle, RoundStarting, WaitingForThrow, BallInAir }

    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        [Header("Settings")]
        [Tooltip("מניעת ספירה כפולה של סל.")]
        [SerializeField] private float basketCooldownSeconds = 0.5f;

        [Header("Spawn Settings")]
        [SerializeField] private GameObject ballPrefab;
        [SerializeField] private Transform spawnPoint;

        private float _lastBasketTime = -999f;
        private GameState currentState = GameState.Idle;
        private BallController activeBall;
        [SerializeField] private GameObject rightHand;

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
            StartNewRound();
        }

        // מתחיל סיבוב חדש
        public void StartNewRound()
        {
            currentState = GameState.RoundStarting;
            Debug.Log("🏁 New round starting!");

            // מפיץ אירוע שריקה
            EventBus.PublishGameEvent(GameEventType.GameStart);

            // יוצר כדור חדש
            if (activeBall != null)
                Destroy(activeBall.gameObject);

            var newBall = Instantiate(ballPrefab, spawnPoint.position, spawnPoint.rotation);
            activeBall = newBall.GetComponent<BallController>();
            activeBall.gameObject.SetActive(true);
            activeBall.Initialize(this);
            activeBall.GetComponent<BasketballThrow>().rightHand = rightHand.transform;


            // לאחר השריקה, מאפשר זריקה
            Invoke(nameof(EnableThrow), 1.2f);
        }

        private void EnableThrow()
        {
            if (activeBall != null)
                activeBall.EnableThrow();

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
            if (currentState != GameState.BallInAir)
                return;

            ResolveShot(ShotResult.Missed);
        }

        public void ResolveShot(ShotResult result)
        {
            Debug.Log($"🎯 Shot resolved: {result}");
            EventBus.PublishShotResult(result);

            foreach (var evt in EventFactory.Build(result))
                EventBus.PublishGameEvent(evt);

            currentState = GameState.Idle;

            // מתחיל סיבוב חדש אחרי זמן קצר
            Invoke(nameof(StartNewRound), 2.0f);
        }
    }
}
