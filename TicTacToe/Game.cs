﻿using System;
using static TicTacToe.Board;

namespace TicTacToe
{
    public class Game
    {
        private Board m_Board;
        private readonly Player r_Player1;
        private readonly Player r_Player2;
        private Player m_CurrentPlayerTurn;
        private Player m_GameWinner;

        public Game(int boardSize, bool isTwoPlayerGame)
        {
            Player.ePlayerId player2Id = isTwoPlayerGame ? Player.ePlayerId.Player2 : Player.ePlayerId.Computer;

            m_Board = new Board(boardSize);
            r_Player1 = new Player(Player.ePlayerId.Player1);
            r_Player2 = new Player(player2Id);
            m_CurrentPlayerTurn = r_Player1;
            m_GameWinner = null;
        }

        public Player Winner
        {
            get
            {
                return m_GameWinner;
            }
            
        }

        public Player CurrentPlayerTurn
        {
            get
            {
                return m_CurrentPlayerTurn;
            }

        }



        public int BoardSize
        {
            get
            {
                return m_Board.Size;
            }
        }

        public Board.eSquareValue GetSignByCoordinates(int i_X, int i_Y)
        {
            return m_Board.GetSquareValue(i_X, i_Y);
        }

        public bool IsComputerTurn()
        {
            return m_CurrentPlayerTurn.Id == Player.ePlayerId.Computer;
        }

        public bool MarkSquare(int i_X, int i_Y)
        {
            Board.eSquareValue sign = convertEPlayerToESquareValue(m_CurrentPlayerTurn);
            bool successMark = m_Board.MarkSquare(i_X, i_Y, sign);

            if (successMark)
            {      
                switchBetweenCurrentPlayer();
                if (isSequance(i_X, i_Y))
                {
                    m_GameWinner = m_CurrentPlayerTurn;
                }
            }

            return successMark;
        }

        private void switchBetweenCurrentPlayer()
        {
            bool isCurrentPlayerIsOne = m_CurrentPlayerTurn == r_Player1;

            m_CurrentPlayerTurn = isCurrentPlayerIsOne ? r_Player2 : r_Player1;
        }

        private Board.eSquareValue convertEPlayerToESquareValue (Player i_Player)
        {
            Board.eSquareValue res;

            if (i_Player.Id == Player.ePlayerId.Player1)
            {
                res = Board.eSquareValue.Player1;
            }
            else
            {
                res = Board.eSquareValue.Player2;
            }

            return res;
        }

        public void PlayAsComputer()
        {
            int x, y;
            
            Random rand = new Random();
            x = rand.Next(m_Board.Size);
            y = rand.Next(m_Board.Size);
            while (!MarkSquare(x, y))// is readble? (!m_Board.IsEmpty(x, y))
            {
                x = rand.Next(m_Board.Size);
                y = rand.Next(m_Board.Size);
            }

            MarkSquare(x, y);
        }

        public bool IsGameOver()
        {
            return m_Board.AreAllSquaresMarked() || hasWinner();
        }

        public bool IsDraw()
        {
            return m_Board.AreAllSquaresMarked() && !hasWinner();
        }

        private bool hasWinner()
        {
            return m_GameWinner != null;
        }

        private bool isSequance(int i_X, int i_Y)
        {
            return checkSequanceInDiagonal() || checkAntiDiagonal() || checkSequanceInRow(i_X) || checkSequanceInColumn(i_Y);
        }

        private bool checkSequanceInRow(int i_Row) 
        {
            eSquareValue sign = m_Board.GetSquareValue(i_Row, 0);
            bool isAllRowTheSameSign = (sign != eSquareValue.Empty);

            for (int i = 0; i < m_Board.Size; i++)
            {
                if (m_Board.GetSquareValue(i_Row, i) != sign)
                {
                    isAllRowTheSameSign = false;
                }
            }

            return isAllRowTheSameSign;
        }

        private bool checkSequanceInColumn(int i_Col)
        {
            eSquareValue sign = m_Board.GetSquareValue(0, i_Col);
            bool isAllColumnTheSameSign = (sign != eSquareValue.Empty);

            for (int i = 0; i < m_Board.Size; i++)
            {
                if (m_Board.GetSquareValue(i, i_Col) != sign)
                {
                    isAllColumnTheSameSign = false;
                }
            }

            return isAllColumnTheSameSign;
        }

        private bool checkSequanceInDiagonal()
        {
            eSquareValue sign = m_Board.GetSquareValue(0, 0);
            bool isAllDiagTheSameSign = (sign != eSquareValue.Empty);

            for (int i = 0; i < m_Board.Size; i++)
            {
                if (m_Board.GetSquareValue(i, i) != sign)
                {
                    isAllDiagTheSameSign = false;
                }
            }

            return isAllDiagTheSameSign;
        }

        private bool checkAntiDiagonal()
        {
            eSquareValue sign = m_Board.GetSquareValue(0, m_Board.Size - 1);
            bool isAllDiagTheSameSign = (sign != eSquareValue.Empty);

            for (int i = 0; i < m_Board.Size; i++)
            {
                if (m_Board.GetSquareValue(i, m_Board.Size - i - 1) != sign)
                {
                    isAllDiagTheSameSign = false;
                }
            }

            return isAllDiagTheSameSign;
        }
    }
}
