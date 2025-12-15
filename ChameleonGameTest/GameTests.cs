using ChameleonGame.Model;
using ChameleonGame.Persistance;
using Moq;
using System;
using Xunit;

namespace ChameleonGame.Test
{
    public class GameTests
    {

        private readonly Mock<IChameleonDataAccess> _mockDataAccess;
        private readonly GameModel _gameModel;

        public GameTests()
        {
            _mockDataAccess = new Mock<IChameleonDataAccess>();
            _gameModel = new GameModel(_mockDataAccess.Object);
        }


        [Fact]
        public void GameModel_NewGame_Correct()
        {
            bool hasBoardChangedRaised = false;
            _gameModel.BoardChanged += (sender, e) => hasBoardChangedRaised = true;

            bool hasCurrentPlayerChangedRaised = false;
            _gameModel.CurrentPlayerChanged += (sender, e) =>
            {
                hasCurrentPlayerChangedRaised = true;
                Assert.Equal(Player.Red, e);
            };

            _gameModel.NewGame(3);

            Assert.NotNull(_gameModel.Board);
            Assert.Equal(Player.Red, _gameModel.CurrentPlayer);
            Assert.True(_gameModel.Board.PlayerHasPieces(Player.Red));
            Assert.True(_gameModel.Board.PlayerHasPieces(Player.Green));
            Assert.Null(_gameModel.Winner);
            Assert.True(hasBoardChangedRaised);
            Assert.True(hasCurrentPlayerChangedRaised);
        }

        [Fact]
        public void GameModel_EndTurn_Correct()
        {
            bool hasCurrentPlayerChangedRaised = false;
            Player startingPlayer = _gameModel.CurrentPlayer;
            _gameModel.NewGame(3);
            _gameModel.CurrentPlayerChanged += (sender, e) =>
            {
                hasCurrentPlayerChangedRaised = true;
                Assert.Equal(Player.Green, e);
            };

            _gameModel.EndTurn();

            Assert.NotEqual(startingPlayer, _gameModel.CurrentPlayer);
            Assert.True(hasCurrentPlayerChangedRaised);
        }

        [Fact]
        public void GameModel_IsGameOver_Correct()
        {
            _gameModel.NewGame(3);
            bool hasGameOverRaised = false;
            _gameModel.GameOver += (sender, winner) =>
            {
                hasGameOverRaised = true;
                Assert.Equal(Player.Red, winner);
            };

            // Remove all Green pieces to simulate game over
            for (int i = 0; i < _gameModel.Board!.Size; i++)
            {
                for (int j = 0; j < _gameModel.Board.Size; j++)
                {
                    if (_gameModel.Board[i, j].Piece?.Owner == Player.Green)
                    {
                        _gameModel.Board[i, j].ChangePiece(null);
                    }
                }
            }
            bool isGameOver = _gameModel.IsGameOver();
            
            Assert.True(isGameOver);
            Assert.Equal(Player.Red, _gameModel.Winner);
            Assert.True(hasGameOverRaised);
        }

        [Fact]
        public void LoadGame_UsesDataAccessAndSetsBoardAndCurrentPlayer()
        {
            ChameleonBoardDTO mockBoardDTO = new ChameleonBoardDTO()
            {
                Size = 3,
                Pieces = new List<PieceDTO>()
                {
                    new PieceDTO() { Owner = PlayerDTO.Red, Row = 1, Col = 1, ColorChangeDelay = 1 }
                }
            };
            ChameleonBoard expectedBoard = new ChameleonBoard(3, new List<Piece>()
            {
                new Piece(Player.Red, 1, 1, 1)
            });
            PlayerDTO mockOutPlayer = PlayerDTO.Green;
            bool hasBoardChanged = false;
            _gameModel.BoardChanged += (s, e) => hasBoardChanged = true;

            _mockDataAccess
                .Setup(m => m.LoadGame(It.IsAny<string>(), out mockOutPlayer))
                .Returns(mockBoardDTO);
            _gameModel.LoadGame("somepath.txt");

            Assert.NotNull(_gameModel.Board);
            Assert.Equivalent(expectedBoard, _gameModel.Board);
            Assert.Equal(Player.Green, _gameModel.CurrentPlayer);
            Assert.Equal(Player.Red, _gameModel.Board![1, 1].Piece!.Owner);
            Assert.True(hasBoardChanged);
            _mockDataAccess.Verify(m => m.LoadGame("somepath.txt", out It.Ref<PlayerDTO>.IsAny), Times.Once);
        }

        [Fact]
        public void LoadGame_DataAccessThrowsException()
        {
            _mockDataAccess
                .Setup(m => m.LoadGame(It.IsAny<string>(), out It.Ref<PlayerDTO>.IsAny))
                .Throws(new ChameleonDataException("load failed"));

            // Act & Assert
            ChameleonDataException ex = Assert.Throws<ChameleonDataException>(() => _gameModel.LoadGame("badpath"));
            Assert.Equal("load failed", ex.Message);
            _mockDataAccess.Verify(m => m.LoadGame(It.IsAny<string>(), out It.Ref<PlayerDTO>.IsAny), Times.Once);
        }

        [Fact]
        public void SaveGame_CallsDataAccess_WithCorrectDTOs()
        {
            // Arrange
            _gameModel.NewGame(3);
            string testPath = "test_save.txt";
            _gameModel.EndTurn();
            Piece mockPiece = new Piece(Player.Red, 1, 1, 1);
            _gameModel.Board![1, 1].ChangePiece(mockPiece);

            ChameleonBoardDTO? capturedBoardDto = null;
            PlayerDTO? capturedPlayerDto = null;

            _mockDataAccess
                .Setup(m => m.SaveGame(testPath, It.IsAny<ChameleonBoardDTO>(), It.IsAny<PlayerDTO>()))
                .Callback<string, ChameleonBoardDTO, PlayerDTO>((path, board, player) =>
                {
                    capturedBoardDto = board;
                    capturedPlayerDto = player;
                });

            // Act
            _gameModel.SaveGame(testPath);

            // Assert
            _mockDataAccess.Verify(m => m.SaveGame(testPath, It.IsAny<ChameleonBoardDTO>(), It.IsAny<PlayerDTO>()), Times.Once);

            Assert.NotNull(capturedBoardDto);
            Assert.Equal(3, capturedBoardDto!.Size);
            Assert.Equal(PlayerDTO.Green, capturedPlayerDto); 

            Assert.Equal(9, capturedBoardDto.Pieces.Count);
            Assert.NotNull(_gameModel.Board![1, 1].Piece);
            Assert.Equal(mockPiece, _gameModel.Board![1, 1].Piece);
        }

        [Fact]
        public void SaveGame_RaisesException_WhenDataAccessFails()
        {
            _gameModel.NewGame(3);

            _mockDataAccess
                .Setup(m => m.SaveGame(It.IsAny<string>(), It.IsAny<ChameleonBoardDTO>(), It.IsAny<PlayerDTO>()))
                .Throws(new ChameleonDataException("Write error"));

            // Act & Assert
            var ex = Assert.Throws<ChameleonDataException>(() => _gameModel.SaveGame("path"));
            Assert.Equal("Write error", ex.Message);
        }
    }
}