using UnityEngine;
using System.Collections;
using System;
using Assets.UnityFoundation.Code.Common;

namespace Assets.Scripts {
    public class GameController : Singleton<GameController> {

        private const string HIGH_SCORE = "High Score";
        private const string SELECTED_BIRD = "Selected Bird";
        private const string GREEN_BIRD = "Green Bird";
        private const string RED_BIRD = "Red Bird";

        protected override void OnAwake() {
            DontDestroyOnLoad(gameObject);
            IsTheGameStartedForTheFirstTime();
        }

        public void IsTheGameStartedForTheFirstTime() {
            if(!PlayerPrefs.HasKey("isTheGameStartedForTheFirstTime")) {
                PlayerPrefs.SetInt(HIGH_SCORE, 0);
                PlayerPrefs.SetInt(SELECTED_BIRD, 0);
                PlayerPrefs.SetInt(GREEN_BIRD, 0);
                PlayerPrefs.SetInt(RED_BIRD, 0);
                PlayerPrefs.SetInt("isTheGameStartedForTheFirstTime", 0);
            }
        }

        public int EvaluatePlayerScore(int score) {
            if(score > getHighScore()) {
                setHighScore(score);
            }

            if(score > 20 && score <= 40) {
                if(isGreenBirdUnlocked() == 0) {
                    unlockGreenBird();
                }
                return 1;
            }

            if(score > 40) {
                if(isGreenBirdUnlocked() == 0) {
                    unlockGreenBird();
                }
                if(isRedBirdUnlocked() == 0) {
                    unlockRedBird();
                }
                return 2;
            }
            return 0;
        }

        public void UnlockAllRewards() {
            PlayerPrefs.SetInt(HIGH_SCORE, 0);
            PlayerPrefs.SetInt(SELECTED_BIRD, 1);
            PlayerPrefs.SetInt(GREEN_BIRD, 1);
            PlayerPrefs.SetInt(RED_BIRD, 1);
            PlayerPrefs.SetInt("isTheGameStartedForTheFirstTime", 0);
        }

        public void setHighScore(int score) {
            PlayerPrefs.SetInt(HIGH_SCORE, score);
        }

        public int getHighScore() {
            return PlayerPrefs.GetInt(HIGH_SCORE);
        }

        public void setSelectedBird(int selectedBird) {
            PlayerPrefs.SetInt(SELECTED_BIRD, selectedBird);
        }

        public int getSelectedBird() {
            return PlayerPrefs.GetInt(SELECTED_BIRD);
        }

        public void unlockGreenBird() {
            PlayerPrefs.SetInt(GREEN_BIRD, 1);
        }

        public int isGreenBirdUnlocked() {
            return PlayerPrefs.GetInt(GREEN_BIRD);
        }

        public void unlockRedBird() {
            PlayerPrefs.SetInt(RED_BIRD, 1);
        }

        public int isRedBirdUnlocked() {
            return PlayerPrefs.GetInt(RED_BIRD);
        }
    }
}