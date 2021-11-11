using UnityEngine;
using Assets.UnityFoundation.SceneFader;
using Assets.UnityFoundation.Code.Common;

namespace Assets.Scripts {
    public class MenuController : Singleton<MenuController> {

        [SerializeField]
        private GameObject[] birds;

        private bool isGreenBirdUnlock, isRedBirdUnlock;

        void Start() {
            SelectBird();
            CheckIfBirdsAreUnlocked();

            Time.timeScale = 1f;
        }

        private void SelectBird() {
            birds[GameController.Instance.getSelectedBird()].SetActive(true);
        }

        public void PlayGame() {
            SceneFader.Instance.FadeIn("core_gameplay_scene");
        }

        public void PlayThreeBirdsMode() {
            SceneFader.Instance.FadeIn("three_bird_mode_scene");
        }

        public void PlayShooterMode() {
            SceneFader.Instance.FadeIn("shooter_mode_scene");
        }

        void CheckIfBirdsAreUnlocked() {
            if(GameController.Instance.isGreenBirdUnlocked() == 1) {
                isGreenBirdUnlock = true;
            }
            if(GameController.Instance.isRedBirdUnlocked() == 1) {
                isRedBirdUnlock = true;
            }
        }

        public void ChangeBird() {
            foreach(var bird in birds) {
                bird.SetActive(false);
            }

            if(isGreenBirdUnlock && GameController.Instance.getSelectedBird() == 0) {
                GameController.Instance.setSelectedBird(1);
                SelectBird();
            } else if(isRedBirdUnlock && GameController.Instance.getSelectedBird() == 1) {
                GameController.Instance.setSelectedBird(2);
                SelectBird();
            } else {
                GameController.Instance.setSelectedBird(0);
                SelectBird();
            }
        }

    }
}