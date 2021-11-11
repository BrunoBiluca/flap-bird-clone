using Assets.UnityFoundation.Code.Common;
using Assets.UnityFoundation.SceneFader;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Assets.Scripts {
    public class StageUI : Singleton<StageUI> {

        private GameObject pausePanel;
        private GameObject gameOverText;

        private Text currentScoreText;
        private Text scoreText;
        private Text endScore;
        private Text bestScore;

        private Button pauseButton;
        private Button restartGameButton;
        private Button instructionsButton;
        private Button returnMainMenuButton;

        [SerializeField] private Sprite[] medals;
        private Image medalImage;

        protected override void OnAwake() {
            currentScoreText = transform.Find("current_score_text")
                .GetComponent<Text>();

            pausePanel = transform.Find("pause_panel").gameObject;

            instructionsButton = transform.Find("instructions_button")
                .GetComponent<Button>();
            instructionsButton.onClick.AddListener(PlayGame);

            pauseButton = transform.Find("pause_button").GetComponent<Button>();
            pauseButton.onClick.AddListener(PauseGame);

            gameOverText = pausePanel.transform.Find("game_over_text").gameObject;

            scoreText = transform.Find("current_score_text")
                .GetComponent<Text>();

            endScore = pausePanel.transform.Find("end_score_value_text")
                .GetComponent<Text>();

            bestScore = pausePanel.transform.Find("best_score_value_text")
                .GetComponent<Text>();

            restartGameButton = pausePanel.transform.Find("resume_game_button")
                .GetComponent<Button>();

            returnMainMenuButton = pausePanel.transform.Find("menu_button")
                .GetComponent<Button>();
            returnMainMenuButton.onClick.AddListener(GoToMenuButton);

            medalImage = pausePanel.transform.Find("medal_image")
                .GetComponent<Image>();

            var flapButton = transform.Find("flap_button")
                .GetComponent<Button>();
            flapButton.onClick.AddListener(
                () => GameplayController.Instance.FlapTheBird()
            );
        }

        public void SetScore(int score) {
            currentScoreText.text = score.ToString("0");
        }


        public void PlayerDied(int score, int medalIndex) {
            pausePanel.SetActive(true);
            gameOverText.gameObject.SetActive(true);
            scoreText.gameObject.SetActive(false);

            bestScore.text = "" + GameController.Instance.getHighScore();

            endScore.text = "" + score;

            medalImage.sprite = medals[medalIndex];

            restartGameButton.onClick.RemoveAllListeners();
            restartGameButton.onClick.AddListener(() => RestartGame());
        }

        public void PauseGame() {
            pausePanel.SetActive(true);
            gameOverText.gameObject.SetActive(false);
            endScore.text = "" + GameplayController.Instance.Score;
            bestScore.text = "" + GameController.Instance.getHighScore();
            Time.timeScale = 0f;
            restartGameButton.onClick.RemoveAllListeners();
            restartGameButton.onClick.AddListener(() => ResumeGame());
        }

        public void GoToMenuButton() {
            SceneFader.Instance.FadeIn("main_menu_scene");
        }

        public void ResumeGame() {
            pausePanel.SetActive(false);
            Time.timeScale = 1.0f;
        }

        public void RestartGame() {
            SceneFader.Instance.FadeIn(SceneManager.GetActiveScene().name);
        }

        public void PlayGame() {
            scoreText.gameObject.SetActive(true);
            instructionsButton.gameObject.SetActive(false);

            GameplayController.Instance.StartGame();
        }
    }
}