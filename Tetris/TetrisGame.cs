/*
 * Created by SharpDevelop.
 * User: Krzysiu
 * Date: 2014-02-14
 * Time: 23:11
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;

namespace Tetris
{
	/// <summary>
	/// Description of TetrisGame.
	/// </summary>
	public class TetrisGame
	{
		public TetrisGame()
		{
			
		}
		
		public static void Main(string[] args) {
			while (true) {
				ConsoleKeyInfo key = Console.ReadKey(true);
				Console.WriteLine(key);
			}
		}
		
		public static void Main2(string[] args) {
			Console.SetWindowSize(12,22);
			TetrisBoard board = new TetrisBoard(10,20);
			board.SetBlock(0,0,new int[,]{
			          	{1,1,1,1},
			          	{0,0,0,1},
			          });
			do {
				for (int x = 0; x < board.Width; x++) {
					for (int y = 0; y < board.Height; y++) {
						if (board.HasTileAt(x, y)) {
							Console.SetCursorPosition(y, x);
							Console.Write('#');
						}
					}
				}
				//ConsoleKeyInfo key = Console.ReadKey(true);
				
			} while(true);
			
		}
	}
}
