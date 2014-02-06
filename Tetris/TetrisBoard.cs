/*
 * Created by SharpDevelop.
 * User: Krzysiu
 * Date: 2014-02-11
 * Time: 20:21
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;

namespace Tetris
{
	/// <summary>
	/// Description of MyClass.
	/// </summary>
	public class TetrisBoard
	{
		private int[,] board;
		
		private int[,] block;
		
		private int blockX;
		
		private int blockY;
		
		private int[,] Tiles	{
			get	{
				int[,] result = (int[,])board.Clone();
				Merge(result, block, blockX, blockY);
				return result;
			}
		}
		
		public bool HasTileAt(int x, int y) {
			return Tiles[x,y] > 0;
		}
		
		public int Width {
			get	{ return board.GetLength(0);	}
		}
		
		public int Height {
			get { return board.GetLength(1); }
		}
		
		public TetrisBoard(int width, int height)
		{
			board = new int[width, height];
			block = new int[,]{{}};
		}
		
		public bool SetBlock(int atX, int atY, int[,] newBlock)
		{
			return new SimpleCommand(this, () => {
			                  	block = CreateSquareBlock(atX, atY, newBlock);
			                  }).Action();
		}
		
		private int[,] CreateSquareBlock(int atX, int atY, int[,] block)
		{
			int a = Math.Max(block.GetLength(0), block.GetLength(1));
			blockX = atX - (a-block.GetLength(0))/2;
			blockY = atY - (a-block.GetLength(1))/2;
			
			int[,] square = new int[a,a];
			for (int x = 0; x < block.GetLength(0); x++) {
				for(int y = 0; y < block.GetLength(1); y++) {
					square[x+atX-blockX, y+atY-blockY] = block[x,y];
				}
			}
			return square;
		}
		
		static void Merge(int[,] dst, int[,] src, int atX, int atY)
		{
			for (int x = 0; x < src.GetLength(0); x++) {
				for (int y = 0; y < src.GetLength(1); y++) {
					if(src[x, y]>0 && atX+x>=0 && atY+y>=0 && atX+x<dst.GetLength(0) && atY+y<dst.GetLength(1))
						dst[atX + x, atY + y] = src[x, y];
				}
			}
		}
		
		abstract class Command {
			
			TetrisBoard board;
			
			protected TetrisBoard Board {
				get { return board; }
			}
			
			public Command(TetrisBoard board) {
				this.board = board;
			}
			
			public bool Action() {
				DoAction();
				if (!board.IsValid()) {
					UndoAction();
					return false;
				}
				return true;
			}
			
			protected abstract void DoAction();
			
			protected abstract void UndoAction();
		}
		
		abstract class StatefulBackedCommand : Command {
			
			public StatefulBackedCommand(TetrisBoard board) : base(board) {}
			
			int blockX;
			int blockY;
			int[,] block;
			int[,] board;
			
			protected override void DoAction()
			{
				TakeBackup();
				DoSimpleAction();
			}
			
			void TakeBackup()
			{
				blockX = Board.blockX;
				blockY = Board.blockY;
				block = (int[,])Board.block.Clone();
				board = (int[,])Board.board.Clone();
			}
			
			protected abstract void DoSimpleAction();
			
			protected sealed override void UndoAction()
			{
				Board.blockX = blockX;
				Board.blockY = blockY;
				Board.block = (int[,])block.Clone();
				Board.board = (int[,])board.Clone();
			}
			
		}
		
		sealed class SimpleCommand : StatefulBackedCommand {
			
			public delegate void ActionDelegate();
			
			ActionDelegate action;
			
			public SimpleCommand(TetrisBoard board, ActionDelegate action) : base(board) {
				this.action = action;
			}
			
			protected override void DoSimpleAction()
			{
				action();
			}
		}
		
		public bool MoveRight()
		{
			return new SimpleCommand(this, () => {
			                  	blockY++;
			                  }).Action();
		}
		
		public bool MoveLeft()
		{
			return new SimpleCommand(this, () => {
			                  	blockY--;
			                  }).Action();
		}
		
		public bool MoveDown()
		{
			return new SimpleCommand(this, () => {
			                  	blockX++;
			                  }).Action();
		}
		
		public bool RotateCW()
		{
			return new SimpleCommand(this, () => {
			                  	RotateSquareCW(block);
			                  }).Action();
		}
		
		public bool RotateCCW()
		{
			return new SimpleCommand(this, () => {
			                  	RotateSquareCW(block);
			                  	RotateSquareCW(block);
			                  	RotateSquareCW(block);
			                  }).Action();
		}
		
		public static void RotateSquareCW(int[,] arr) {
			if (arr.GetLength(0) != arr.GetLength(1)) {
				throw new ArgumentException("Square array expected!");
			}
			int a = arr.GetLength(0);
			for (int x = 0; x < a/2; x++) {
				for (int y = x; y < a-1-x; y ++) {
					int temp = arr[y, x];
					arr[y, x] = arr[a-1-x, y];
					arr[a-1-x, y] = arr[a-1-y, a-1-x];
					arr[a-1-y, a-1-x] = arr[x, a-1-y];
					arr[x, a-1-y] = temp;
				}
			}
		}
		
		bool IsValid()
		{
			return Count(block) + Count(board) == Count(Tiles);
		}
		
		static int Count(int[,] block)
		{
			int count = 0;
			for (int x = 0; x < block.GetLength(0); x++) {
				for (int y = 0; y < block.GetLength(1); y++) {
					if (block[x,y] > 0) {
						count++;
					}
				}
			}
			return count;
		}
		
		public int RemoveLines()
		{
			int linesRemoved = 0;
			for(int x = board.GetLength(0)-1; x >= 0; x--) {
				while (IsLine(x))
				{
					RemoveLine(x);
					linesRemoved ++;
				}
			}
			return linesRemoved;
		}


		void RemoveLine(int x)
		{
			for (int y = 0; y < board.GetLength(1); y++) {
				RemoveAt(x, y);
			}
		}
		
		bool IsLine(int x) {
			for(int y = 0; y < board.GetLength(1); y++) {
				if (board[x,y]==0)
				{
					return false;
				}
			}
			return true;
		}

		void RemoveAt(int x, int y)
		{
			for (int z = x; z >= 0; z--) {
				board[z, y] = z == 0 ? 0 : board[z - 1, y];
			}
		}
		
		public void MergeBlock() {
			Merge(board, block, blockX, blockY);
			block = new int[,]{{}};
		}
		
	}
	
}