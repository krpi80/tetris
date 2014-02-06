/*
 * Created by SharpDevelop.
 * User: Krzysiu
 * Date: 2014-02-11
 * Time: 20:24
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using NUnit.Framework;
using Tetris;
using nunit = NUnit.Framework;


namespace Tetris
{
	[TestFixture]
	public class TetrisBoardTest
	{
		TetrisBoard board;
			
		[SetUp]
		public void SetUp()
		{
			board = new TetrisBoard(10, 20);
		}
		
		[Test]
		public void TestTiles()
		{
			Assert.AreEqual(10, board.Width);
			Assert.AreEqual(20, board.Height);
		}
		
		[Test]
		public void TestSetBlock()
		{
			board.SetBlock(0, 0, new int[,]{{1,1,1}});
			Assert.True(board.HasTileAt(0,1));
			Assert.True(board.HasTileAt(0,0));
			Assert.True(board.HasTileAt(0,2));
			Assert.False(board.HasTileAt(0,3));
		}

		[Test]
		public void TestSetBlockAt()
		{
			board.SetBlock(2, 3, new int[,]{{1,1,1}});
			Assert.True(board.HasTileAt(2,3));
			Assert.True(board.HasTileAt(2,4));
			Assert.True(board.HasTileAt(2,5));
			Assert.False(board.HasTileAt(2,6));
		}
		
		[Test]
		public void TestMoveRigth()
		{
			board.SetBlock(0, 0, new int[,]{{1,1,1}});
			board.MoveRight();
			Assert.False(board.HasTileAt(0,0));
			Assert.True(board.HasTileAt(0,1));
			Assert.True(board.HasTileAt(0,2));
			Assert.True(board.HasTileAt(0,3));
			Assert.False(board.HasTileAt(0,4));
		}

		[Test]
		public void TestMoveLeft()
		{
			board.SetBlock(0, 1, new int[,]{{1,1,1}});
			board.MoveLeft();
			
			Assert.True(board.HasTileAt(0,0));
			Assert.True(board.HasTileAt(0,1));
			Assert.True(board.HasTileAt(0,2));
			Assert.False(board.HasTileAt(0,3));
		}

		[Test]
		public void TestMoveDown()
		{
			board.SetBlock(0, 0, new int[,]{{1,1,1}});
			board.MoveDown();
			Assert.True(board.HasTileAt(1,0));
			Assert.True(board.HasTileAt(1,1));
			Assert.True(board.HasTileAt(1,2));
			Assert.False(board.HasTileAt(0,0));
		}
		
		[Test]
		public void TestRotateCW()
		{
			board.SetBlock(1, 0, new int[,]{{1,1,1}});
			board.RotateCW();
			Assert.True(board.HasTileAt(0,1));
			Assert.True(board.HasTileAt(1,1));
			Assert.True(board.HasTileAt(2,1));
			Assert.False(board.HasTileAt(0,0));
		}
		
		[Test]
		public void TestRotateSquareCWOdd()
		{
			int[,] A1 = new int[,]{
				{1,2,2,2,2},
				{1,1,2,2,3},
				{1,1,5,3,3},
				{1,4,4,3,3},
				{4,4,4,4,3},
			};
			int[,] A2 = new int[,]{
				{4,1,1,1,1},
				{4,4,1,1,2},
				{4,4,5,2,2},
				{4,3,3,2,2},
				{3,3,3,3,2},
			};
			int[,] A3 = new int[,]{
				{3,4,4,4,4},
				{3,3,4,4,1},
				{3,3,5,1,1},
				{3,2,2,1,1},
				{2,2,2,2,1},
			};
			int[,] A4 = new int[,]{
				{2,3,3,3,3},
				{2,2,3,3,4},
				{2,2,5,4,4},
				{2,1,1,4,4},
				{1,1,1,1,4},
			};
			int[,] A = (int[,])A1.Clone();
			TetrisBoard.RotateSquareCW(A);
			Assert.AreEqual(A2, A);
			TetrisBoard.RotateSquareCW(A);
			Assert.AreEqual(A3, A);
			TetrisBoard.RotateSquareCW(A);
			Assert.AreEqual(A4, A);
			TetrisBoard.RotateSquareCW(A);
			Assert.AreEqual(A1, A);
		}
		
		[Test]
		public void TestRotateSquareCWEven()
		{
			int[,] A1 = new int[,]{
				{1,1,2,2},
				{1,1,2,2},
				{4,4,3,3},
				{4,4,3,3},
			};
			int[,] A2 = new int[,]{
				{4,4,1,1},
				{4,4,1,1},
				{3,3,2,2},
				{3,3,2,2},
			};
			int[,] A3 = new int[,]{
				{3,3,4,4},
				{3,3,4,4},
				{2,2,1,1},
				{2,2,1,1},
			};
			int[,] A4 = new int[,]{
				{2,2,3,3},
				{2,2,3,3},
				{1,1,4,4},
				{1,1,4,4},
			};
			int[,] A = (int[,])A1.Clone();
			TetrisBoard.RotateSquareCW(A);
			Assert.AreEqual(A2, A);
			TetrisBoard.RotateSquareCW(A);
			Assert.AreEqual(A3, A);
			TetrisBoard.RotateSquareCW(A);
			Assert.AreEqual(A4, A);
			TetrisBoard.RotateSquareCW(A);
			Assert.AreEqual(A1, A);
		}
		
		[Test]
		public void TestRotateSquareCwSame([ValueSource("ArraysSource")] int[,] arr)
		{
			int[,] A = (int[,])arr.Clone();
			TetrisBoard.RotateSquareCW(A);
			Assert.AreEqual(arr, A);
		}
		
		IEnumerable<int[,]> ArraysSource()
		{
			yield return new int[,]{{}};
			yield return new int[,]{{1}};
		}

		[Test]
		[ExpectedException(typeof(ArgumentException))]
		public void TestRotateNotSquareFails()
		{
			int[,] A = new int[,]{
				{1,2},
			};
			TetrisBoard.RotateSquareCW(A);
			Assert.Fail("Exception expected");
		}
		
		[Test]
		public void TestCannotMoveOutOfBoard()
		{
			Assert.IsTrue(board.SetBlock(0, 0, new int[,]{{1,1,1}}));
			Assert.IsFalse(board.MoveLeft());
			Assert.IsTrue(board.MoveRight());
		}
		
		[Test]
		public void TestCannotAddIfOverlapped()
		{
			Assert.IsTrue(board.SetBlock(0, 0, new int[,]{{1,1,1}}));
			board.MergeBlock();
			Assert.IsFalse(board.SetBlock(0, 0, new int[,]{{1,1,1}}));
		}
		
		[Test]
		public void TestCannotRotateIfOverlapped()
		{
			Assert.IsTrue(board.SetBlock(0, 0, new int[,]{{1,1,1}}));
			board.MergeBlock();
			Assert.IsTrue(board.SetBlock(1, 0, new int[,]{{1,1,1}}));
			Assert.IsFalse(board.RotateCW());
		}
		
		[Test]
		public void TestSimpleGame()
		{
			TetrisBoard board = new TetrisBoard(10, 5);
			bool success = board.SetBlock(0, 0, new int[,]{
			          	{1,1,1},
			          	{1,1,1}
			          });
			Assert.IsTrue(success);
			
			while (board.MoveDown());
			
			board.MergeBlock();
			
			success = board.SetBlock(0, 0, new int[,]{
			          	{1,1},
			          	{1,1},
			          });
			Assert.IsTrue(success);
			
			while(board.MoveRight());
			while(board.MoveDown());
			
			Assert.True(board.HasTileAt(9,0));
			
			board.MergeBlock();
			int lines = board.RemoveLines();
			Assert.AreEqual(2, lines);
			
			Assert.False(board.HasTileAt(9,0));
		}
	}
}
