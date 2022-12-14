using Microsoft.Extensions.Logging;
using Models;
using RepoLayer;

namespace BusinessLayer
{

    public class GamePlay : IGamePlay, IGetStuff
    {
        /** 
            a class library is a class that has functionality that I hav eunilize in another class or program.
            The benefit of a class library is that I can swap out the file for another
            while keep the endpoints the same and completely change the functionality, security, databases used, 
            or methodology of the method used b the main program.
        **/
        private readonly adonetaccess _repo;
        private readonly MyCustomException _logger;
        private readonly Random _rand = new Random();// the Random class gets us a pseudorandom decimal between 0 and 1.
        // These List<>'s are analogous to saving the data permanently in a Db. (We aren't doing that... YET.)
        //create a List<Game> to hold all the games
        private readonly List<Game> _games = new List<Game>();
        // create a List<Player> to hold allthe players.
        readonly List<Player> _players = new List<Player>();
        // create a List<Round> to hold all the Rounds
        private readonly List<Round> _rounds = new List<Round>();
        private int player1wins = 0;//how many rounds p1 has won
        private int computerWins = 0;//how many rounds the compouter has won
        private Game _CurrentGame;

        public GamePlay(adonetaccess repo, MyCustomException logger)
        {
            _repo = repo;
            this._logger = logger;
        }

        /// <summary>
        /// This will create a game and add the computer to the P2 spot.
        /// </summary>
        public async Task NewGameAsync()
        {
            this._CurrentGame = new Game();
            await this.GetComputerIfExistsAsync();
        }

        /// <summary>
        /// query the Db for the Computer (if it exists) and place that PlayerId in the P2 position.
        /// The repo layer will return null if hte computer isn't in the Db.
        /// Otherwise, return the computer object/
        /// </summary>
        private async Task GetComputerIfExistsAsync()
        {
            Player? p = await this._repo.GetComputerIfExistsAsync();//if this returns null, I'll create a guid and assign it to P2.PlayerId
            if (p == null)
            {
                //this._CurrentGame.P2.PlayerId = Guid.NewGuid();
                this._logger.CheatingPlayer();
            }
            else
            {
                this._CurrentGame.P2 = p;
            }
        }


        /// <summary>
        /// This method will:
        /// 1) Take P1's first and lasts names, 
        /// 2) verify that the player doesn't exist already,
        /// 3) add the player to the game,
        /// 4) return true if the player was already in the players list
        /// 5) return false if not.
        /// </summary>
        /// <param name="playerNames"></param>
        /// <returns></returns>
        public async Task<Player> P1NameAsync(string fname = "default", string lname = "name")
        {
            #region no db code
            // if (playerNames.Length > 1)
            // {

            //     foreach (Player p1 in this._players) // [p1, p2, p3, p4, p5]
            //     {
            //         //see if the player is already in the List<Player>
            //         if (p1.Fname.Equals(playerNames[0]) && p1.Lname.Equals(playerNames[1]))
            //         {
            //             //add this existing player to the current game.
            //             this._CurrentGame.P1 = p1;
            //             return true;//tell the main method that the player was already in the system
            //         }
            //     }
            // }
            #endregion
            //instead of the above, we will search the Db for this player.
            //string fname, lname;
            // vette the array right now to the repo layer doesn't have to.
            //if (playerNames.Length > 1)
            //{
            //    fname = playerNames[0];
            //    lname = playerNames[1];
            //}
            //else if (playerNames.Length == 1)
            //{
            //    fname = playerNames[0];
            //    lname = "default";
            //}
            //else
            //{
            //    fname = "default";
            //    lname = "name";
            //}
            // send the repo the real names or the defaulted names.

            Player? p = await _repo.P1NameAsync(fname, lname);
            if (p == null)
            {
                this._CurrentGame.P1 = new Player(fname, lname);
                return this._CurrentGame.P1;// because the player did not exist... AND STILL DOESN'T IN THE DB.
            }
            else
            {
                this._CurrentGame.P1 = p;
                //Console.WriteLine($"{p.Fname} {p.Lname} {p.Wins} {p.Losses}");
                return this._CurrentGame.P1;// because the player already existed in the Db.
            }
        }

        public Player GetP2()
        {
            //this._logger.
            return this._CurrentGame.P2;
        }

        public Player GetP1()
        {
            return this._CurrentGame.P1;
        }

        /// <summary>
        /// returns true if one of the 2 players has won 2 rounds,
        /// return false if not.
        /// </summary>
        /// <returns></returns>
        public bool IsThereAWinner()
        {
            // return true of one of the players has 2 round wins.
            if (player1wins == 2 || computerWins == 2)
            {
                return true;
            }
            else return false;
        }

        /// <summary>
        /// This method creates a round, adds the players to it, and adds the round to the current game..
        /// </summary>
        public void PlayRound()
        {
            Round r = new Round(this._CurrentGame.P1, this._CurrentGame.P2);
            r.GameId = this._CurrentGame.GameId;
            this._CurrentGame.Rounds.Add(r);
        }

        /// <summary>
        /// This method will:
        /// 1) validate the string can be converted to an int
        /// 2) verify that the int is >0 and less than 4
        /// 3) if the conversion is successful and in range, get the computer choice and return true,
        /// 4) false if not.
        /// </summary>
        /// <param name="p12choiceStr"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public bool ValidateUserChoice(string p12choiceStr)
        {
            bool success = Int32.TryParse(p12choiceStr, out int result);// store these choices on the round object
            if (result > 0 && result < 4)
            {
                //ArrayName[0] will accesss the first elelment of an array.
                // arrayName[1] 2nd element
                // arrayName[3]
                // if there are 3 filled elements of an array or List<>, the Count = 3 but the last element is element #2

                int indexOfLastRoundAdded = this._CurrentGame.Rounds.Count - 1;// gets the element number of the final round added to the rounds list in the game
                Round lastRoundAddedToTheListOfRoundsInTheGame = this._CurrentGame.Rounds[indexOfLastRoundAdded];
                lastRoundAddedToTheListOfRoundsInTheGame.P2Choice = (GamePiece)((_rand.Next(1000) % 3) + 1);// this action would be better in another method that would be called from Main()

                this._CurrentGame.Rounds[this._CurrentGame.Rounds.Count - 1].P1Choice = (GamePiece)result;
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Thss method will evaluate the players choices and 
        /// return 0 if the round was a tie, 
        /// 1 of P1 won the round, 
        /// 2 of P2 won the round.
        /// </summary>
        /// <returns></returns>
        public int EvaluatePlayersChoices()
        {
            //I assign the value of the players choices in this variable PURELY for convenience
            GamePiece p1Choice = this._CurrentGame.Rounds[this._CurrentGame.Rounds.Count - 1].P1Choice;
            GamePiece p2Choice = this._CurrentGame.Rounds[this._CurrentGame.Rounds.Count - 1].P2Choice;
            Round round = this._CurrentGame.Rounds[this._CurrentGame.Rounds.Count - 1];

            // evaluate the choices to determine the winner of the round.            
            if (p1Choice == p2Choice)// if it was a tie
            {
                // update the tally for this gaming session of how many games the computer and the user have won.
                //numberOfTies++;// ++ increments the int by exactly 1.
                this._CurrentGame.NumberOfTies++;

                //update the roundwinner in the Round
                round.RoundWinner = new Guid();// when you use 'new Guid:', an empty guid is generated. "00000000-0000-0000-0000-00000000000";
                // add the round to the List of rounds
                //this._rounds.Add(round);// we don't need this because at the end of the game, the List<Round> will be parsed to insert the rounds into the Db.
                return 0;
            }
            // if the user won
            else if ((p1Choice == GamePiece.ROCK && p2Choice == GamePiece.SCISSORS) ||
                        (p1Choice == GamePiece.PAPER && p2Choice == GamePiece.ROCK) ||
                            (p1Choice == GamePiece.SCISSORS && p2Choice == GamePiece.PAPER))
            {
                player1wins = player1wins + 1;// this method gives you the option of incrementing by more than 1
                //update the roundwinner in the Round
                round.RoundWinner = this._CurrentGame.P1.PlayerId;
                // add the round to the List of rounds
                //this._rounds.Add(round);
                return 1;
            }
            else//if the computer won
            {
                // update the tally for this gaming session of how many games the computer and the user have won.
                computerWins += 1;// this method gives you the option of incrementing by more than 1.
                //update the roundwinner in the Round
                round.RoundWinner = this._CurrentGame.P2.PlayerId;
                // add the round to the List of rounds
                //this._rounds.Add(round);
                return 2;
            }
        }

        public Round GetLastRoundPlayed()
        {
            Round r = this._CurrentGame.Rounds[this._CurrentGame.Rounds.Count - 1];
            return r;
        }

        public int GetPlayer1RoundWins()
        {
            return this.player1wins;
        }

        public int GetComputerRoundWins()
        {
            return this.computerWins;
        }

        public int GetNumberOfTies()
        {
            return this._CurrentGame.NumberOfTies;
        }

        public async Task<Game> FinalizeGameAsync()// this method serves as a 'manager' of the closedown procedures
        {
            //assign the gamewinner
            if (this.FinalizeStats())
            {
                //STEP 1. Save the current p1 to the Db
                if (!await this.PersistPlayerAsync(this._CurrentGame.P1))
                {
                    throw new SystemException("There was a problem finalizing the game and all was lost.");
                };

                //STEP 2. Save the current p2 to the Db
                if (!await this.PersistPlayerAsync(this._CurrentGame.P2))
                {
                    throw new SystemException("There was a problem finalizing the game and all was lost.");
                };

                //STEP 3. add the game itself
                if (await this.PersistGameAsync() != 1)
                {
                    throw new SystemException("There was a problem finalizing the game and all was lost.");
                }

                //STEP 4.
                //add the rounds by calling the add Round method in a loop.
                if (await this.PersistRoundsAsync(this._CurrentGame.Rounds) != 1)
                {
                    throw new SystemException("There was a problem finalizing the game and all was lost.");
                }

                //STEP 5. add the game and Player ids to the games_players_Junction table

                //TODO

            }
            else throw new SystemException("There was a problem finalizing the game and all was lost.");
            return this._CurrentGame;
            #region no db code
            //add/update the p1 to the List<Player>
            // if (!_players.Exists(p => p.Fname == this._CurrentGame.P1.Fname && p.Lname == this._CurrentGame.P1.Lname))
            // {
            //     this._players.Add(this._CurrentGame.P1);
            //     /**
            //     in SQL you can add a new row with the INSERT keyword
            //     THe keyword 'ALTER' will find a row and change the specified data
            //     There is also the keywords 'IF EXISTS'
            //     Find out whe happens if the combination 'ALTER IF EXISTS' defaults to INSERT if that row does not exists in the table. 
            //     **/
            // }
            // else
            // {
            //     //shouldn't have to make any changes to the player in the list because the P1 in currentGame is a reference to it
            //     // this means that P1 points to the same player as the List player (on the Heap). 
            // }

            // // add the game to the game list
            // this._games.Add(this._CurrentGame);
            #endregion
        }

        private async Task<int> PersistGameAsync()
        {
            if (await this._repo.PersistGameAsync(this._CurrentGame) != 1)
            {
                return 0;
            }
            else return 1;
        }

        /// <summary>
        /// This method will save all the Game rounds to the Db.
        /// </summary>
        /// <param name="rounds"></param>
        /// <returns></returns>
        private async Task<int> PersistRoundsAsync(List<Round> rounds)
        {
            int ret = 0;
            foreach (Round r in rounds)
            {
                ret = await this._repo.PersistRoundsAsync(r);
                if (ret != 1)
                {
                    return 0;
                }
            }
            return ret;
        }

        private bool FinalizeStats()
        {
            try
            {
                if (player1wins == 2)
                {
                    this._CurrentGame.GameWinner = this._CurrentGame.P1;
                    this._CurrentGame.P1.Wins++;
                    this._CurrentGame.P2.Losses++;// at the end of the game, the repo layer will just increment the W/L of the computer in the Db.
                }
                else
                {
                    this._CurrentGame.GameWinner = this._CurrentGame.P2;
                    this._CurrentGame.P2.Wins++;
                    this._CurrentGame.P1.Losses++;// breakpoint here to check that the computer is being updated correctly.
                }
                return true;
            }
            catch (MyCustomException ex)
            {

                //probably ought to log this somewhere.
                Console.WriteLine(ex.CheatingPlayer());
                //you wojls REALLY be logging the results of hte exception 
                return false;
            }
        }

        /// <summary>
        /// This method saves both players to the Db or updates them if they already exist.
        /// Theis mehtod returns true if both saves were successful.
        /// /// </summary>
        /// <returns></returns>
        private async Task<bool> PersistPlayerAsync(Player p)
        {
            // if the player is there, we update its Data
            // if not, we insert the player.
            if (await this._repo.ExistsPlayerByIdAsync(p.PlayerId))
            {
                if (await this._repo.UpdatePlayerByIdAsync(p) != 1) return false;
            }
            // this was a whole 7 line else/if statement combined into one line! :)
            else if (await this._repo.InsertNewPlayerAsync(p) != 1)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// This method resets the GamePlaye class to start a new game.
        /// </summary>
        public void ResetForNewGame()
        {
            player1wins = 0;//how many rounds p1 has won
            computerWins = 0;//how many rounds the compouter has won
            _CurrentGame = null;
        }

        void IGamePlay.GetAnError() { }

        // public void ShowAccessAmbiguity()
        // {
        //     Player p = new Player();
        //     p.testint = 1;
        //     int testint = 0;
        //     testint = 5;
        //     p.testint = testint;
        //     Console.WriteLine(p.testint);
        // }

        // public void testQuery()
        // {
        //     // usually there will be something logical to do here.
        //     _repo.testQuery();
        //     //there may be somethign logical to do here too...
        // }


        //create a method to do whatever data manipulation you need done.
        // call the Repo layer method to check if that Username/Password combo exists already
        // if it already exists, return failure
        // if that uname and pword are not already there, call the method to insert the new user.
        // return the new user object

    }//EoC
}//EoN
