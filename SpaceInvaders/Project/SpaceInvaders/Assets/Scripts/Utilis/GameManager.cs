using SpaceInvaders.Character.Controller;
using SpaceInvaders.Character.Factory;
using SpaceInvaders.Character.Model;
using SpaceInvaders.Character.ViewPreseter;
using SpaceInvaders.Data;
using SpaceInvaders.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;
using Zenject;

namespace SpaceInvaders.Utilis
{
    public class GameManager : BaseDisposableMonoBehaviour
    {
        [SerializeField]
        private GameObject _enemys;

        [SerializeField]
        private GameObject _mainMenuPanel;

        public Dictionary<int, BaseCharacterController> _controllers;

        private CharacterControllerFactroy _characterControllerFactroy;
        private PlayerModel _playerModel;
        private EnemyModel _enemyModel;
        private SpacedeerModel _spacedeerModel;
        private IMessageBroker _messageBroker;
        private Vector2 _startPosition = new Vector2(0.0f, -6.0f);
        private Vector2 _spacedeerStartPosition = new Vector2(-15.0f, 6.0f);
        private bool _gameComplete = false;

        [Inject]
        private void Construct(CharacterControllerFactroy characterControllerFactroy,
            IMessageBroker messageBroker,
            PlayerModel playerModel,
            EnemyModel enemyModel,
            ObstacleModel obstacleModel,
            SpacedeerModel spacedeerModel)
        {
            _characterControllerFactroy = characterControllerFactroy;
            _messageBroker = messageBroker;
            _playerModel = playerModel;
            _enemyModel = enemyModel;
            _spacedeerModel = spacedeerModel;

            _controllers = new Dictionary<int, BaseCharacterController>();
            _playerModel.Score = 0;
            _playerModel.Life = 2;
        }

        private void Start()
        {
            _messageBroker.Receive<Events.PlayGame>()
                .Subscribe(_ =>
                {
                    //Add player
                    AddController(0, CharacterType.Player, _startPosition);
                    _playerModel.Score = 0;

                    //Add enemys
                    CreateEnemys(11, 5);

                    //Add obstacle
                    var obstacleSpawnPosition = new Vector2(-9.0f, -4.5f);
                    for (int i = 0; i < 5; i++)
                    {
                        AddController(GetLastId() + 1, CharacterType.Obstacle, obstacleSpawnPosition);
                        obstacleSpawnPosition += new Vector2(4.5f, 0.0f);
                    }

                    Disposable.Add(
                        _messageBroker.Receive<Events.RocketSpawnEvent>()
                            .Subscribe(val =>
                            {
                                var selectedObject = GameObject.Find(val.Id.ToString());
                                if (selectedObject != null && selectedObject.GetComponent<PlayerViewPresenter>())
                                {
                                    AddController(GetLastId() + 1, CharacterType.Rocket, val.spawnPosition, val.rocketDirection);
                                }
                                else
                                {
                                    AddController(GetLastId() + 1, CharacterType.Rocket, val.spawnPosition, val.rocketDirection);
                                }
                            })
                    );

                    Disposable.Add(
                        _messageBroker.Receive<Events.RocketCollision>()
                            .Subscribe(val =>
                            {
                                RemoveController(val.RocketId);
                                Destroy(GameObject.Find(val.RocketId.ToString()));

                                AddExplosion(val.Position);

                                switch (val.CollisionObjectTag)
                                {
                                    case "Enemy":
                                        _playerModel.Score += _enemyModel.EnemyList.Single(c => c.Id == val.CollisionObjectId).CharacterScore;
                                        var objectToRemove = _enemyModel.EnemyList.DefaultIfEmpty(null).Single(c => c.Id == val.CollisionObjectId);
                                        if (objectToRemove != null)
                                        {
                                            _enemyModel.EnemyList.Remove(objectToRemove);
                                            RemoveController(val.CollisionObjectId);
                                        }
                                        break;
                                    case "Player":
                                        _playerModel.Life--;
                                        if(_playerModel.Life < 0)
                                        {
                                            RemoveController(val.CollisionObjectId);
                                            Destroy(GameObject.FindGameObjectWithTag("Player"));
                                            GameOver();
                                        }
                                        break;
                                }
                            })
                    );

                    Disposable.Add(
                        _messageBroker.Receive<Events.EnemyCollision>()
                            .Subscribe(val =>
                            {
                                switch (val.CollisionObjectTag)
                                {
                                    case "MapEnd":
                                        GameOver();
                                        break;
                                    case "Obstacle":
                                        RemoveController(val.Id);
                                        Destroy(GameObject.Find(val.Id.ToString()));
                                        var objectToRemove = _enemyModel.EnemyList.DefaultIfEmpty(null).Single(c => c.Id == val.EnemyId);
                                        if (objectToRemove != null)
                                        {
                                            _enemyModel.EnemyList.Remove(objectToRemove);
                                            RemoveController(val.EnemyId);
                                            Destroy(GameObject.Find(val.EnemyId.ToString()));
                                            AddExplosion(objectToRemove.position);
                                        }
                                        break;
                                }
                            })
                    );

                    Disposable.Add(
                        Observable.Interval(TimeSpan.FromSeconds(1.5f))
                            .Subscribe(__ =>
                            {
                                MoveEnemys();
                                if (_enemyModel.EnemyList.Count == 0 && !_gameComplete)
                                {
                                    CreateEnemys(11, 5);
                                }
                            })
                    );

                    Disposable.Add(
                        Observable.Interval(TimeSpan.FromMilliseconds(UnityEngine.Random.Range(500.0f, 1000.0f)))
                            .Subscribe(__ =>
                            {
                                if(_enemyModel.EnemyList.Count != 0)
                                {
                                    var id = 0;
                                    var tmp = _enemyModel.EnemyList.RandomElement().position.x;
                                    var singleColumn = _enemyModel.EnemyList.Where(c => c.position.x == tmp && c.CharacterType != CharacterType.Spacedeer).ToList();
                                    if (singleColumn.Count != 0 && singleColumn != null)
                                    {
                                        var minY = singleColumn.Min(c => c.position.y);
                                        id = singleColumn.SingleOrDefault(c => c.position.y == minY).Id;
                                        _messageBroker.Publish(new Events.EnemySpawnRocket()
                                        {
                                            Id = id
                                        });
                                    }
                                }
                            })
                    );

                    Disposable.Add(
                        Observable.Interval(TimeSpan.FromSeconds(10.0f))
                            .Subscribe(__ =>
                            {
                                AddController(GetLastId() + 1, CharacterType.Spacedeer, _spacedeerStartPosition, SpacedeerDirection.ToRight);

                                _enemyModel.EnemyList.Add(new EnemyCoord()
                                {
                                    CharacterType = CharacterType.Spacedeer,
                                    Id = GetLastId(),
                                    CharacterScore = _spacedeerModel.Score
                                });
                            })
                        );
                });
        }

        private void GameOver()
        {
            foreach (var c in _enemyModel.EnemyList)
            {
                GameObject.Destroy(GameObject.Find(c.Id.ToString()));
            }
            _enemyModel.EnemyList.Clear();
            foreach (var c in _controllers)
            {
                Destroy(GameObject.Find(c.Key.ToString()));
            }
            _controllers.Clear();
            _mainMenuPanel.SetActive(true);
            _gameComplete = true;
            DisposeAll();
        }

        private void AddExplosion(Vector3 position)
        {
            GameObject explosion = new GameObject();
            explosion.AddComponent<SpriteRenderer>();
            explosion.GetComponent<SpriteRenderer>().sprite = DataManager.Data.LoadSprite("Explosion", "Data/Rocket/Explosion/");
            explosion.transform.position = position;
            Destroy(explosion, 0.3f);
        }

        private void CreateEnemys(int x, int y)
        {
            float _x = -7.5f;
            float _y = 6.0f;
            int lineCounter = 0;
            int characterScore = 0;
            _enemyModel.EnemyList = new List<EnemyCoord>();
            _enemys.transform.position = new Vector3(0.0f, 0.0f, -1.0f);

            for (int i = 0; i < y; i++)
            {
                _x = -7.5f;
                lineCounter++;

                for (int j = 0; j < x; j++)
                {
                    Vector2 spawnPosition = new Vector2(_x, _y);

                    if (lineCounter == 1)
                    {
                        AddController(GetLastId() + 1, CharacterType.EnemyStandard, spawnPosition, "Enemy3");
                        characterScore = 30;
                    }
                    else if (lineCounter >= 2 && lineCounter <= 3)
                    {
                        AddController(GetLastId() + 1, CharacterType.EnemyStandard, spawnPosition, "Enemy2");
                        characterScore = 20;
                    }
                    else
                    {
                        AddController(GetLastId() + 1, CharacterType.EnemyStandard, spawnPosition, "Enemy1");
                        characterScore = 10;
                    }

                    _enemyModel.EnemyList.Add(new EnemyCoord()
                    {
                        CharacterType = CharacterType.EnemyStandard,
                        position = spawnPosition,
                        Id = GetLastId(),
                        CharacterScore = characterScore
                    });
                    _x += 1.5f;
                }
                _y -= 1.0f;
            }
        }

        private bool nextDirectionFlag = false;
        private bool moveCompleteFlag = false;

        private void MoveEnemys()
        {
            if (_enemyModel != null && _enemyModel.EnemyList.Count != 0)
            {
                var maxRight = _enemyModel.EnemyList.Max(c => c.position.x) + _enemys.transform.position.x;
                var maxLeft = _enemyModel.EnemyList.Min(c => c.position.x) + _enemys.transform.position.x;
                var maxDown = _enemyModel.EnemyList.Min(c => c.position.y) + _enemys.transform.position.y;

                if (maxRight >= 9.0f)
                {
                    nextDirectionFlag = true;
                }
                else if (maxLeft <= -9.0f)
                {
                    nextDirectionFlag = false;
                }
                else
                {
                    moveCompleteFlag = false;
                }

                if ((maxLeft == -9.0f || maxRight == 9.0f) && !moveCompleteFlag)
                {
                    _enemys.transform.position += new Vector3(0.0f, -1.5f, 0.0f);
                    moveCompleteFlag = true;
                }
                else if (maxRight < 9.0f && !nextDirectionFlag)
                {
                    _enemys.transform.position += new Vector3(1.5f, 0.0f, 0.0f);
                }
                else if(maxLeft > -9.0f && nextDirectionFlag)
                {
                    _enemys.transform.position += new Vector3(-1.5f, 0.0f, 0.0f);
                }
            }
        }

        private void AddController(int id, CharacterType characterType, Vector2 position, string characterName = null)
        {
            _controllers.Add(id, _characterControllerFactroy.Create(id, characterType, position, characterName));
        }

        private void AddController(int id, CharacterType characterType, Vector2 position, RocketDirection direction)
        {
            _controllers.Add(id, _characterControllerFactroy.Create(id, characterType, position, direction));
        }

        private void AddController(int id, CharacterType characterType, Vector2 position, SpacedeerDirection direction)
        {
            _controllers.Add(id, _characterControllerFactroy.Create(id, characterType, position, direction));
        }

        private void RemoveController(int id)
        {
            _controllers.Remove(id);
        }

        private int GetLastId()
        {
            int selectedKey;
            if (_controllers != null)
            {
                selectedKey = _controllers.OrderBy(c => c.Key).DefaultIfEmpty().Last().Key;
                return selectedKey;
            }
            return -1;
        }
    }
}
