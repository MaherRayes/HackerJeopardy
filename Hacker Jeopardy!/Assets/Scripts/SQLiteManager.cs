
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using Mono.Data.Sqlite;
using System.Data;
using System.Text;

public class SQLiteManager {

    private string connectionString;


    public SQLiteManager (string connectionString) { 
           this.connectionString = connectionString;
    }

    public void WriteClue(int Cat_ID, Clue clue)
    {
        int isDual;
        string sqlQuery;
        int ClueID = getHighestCluesID() + 1;

        // Open a connection with the database
        IDbConnection dbConnection = new SqliteConnection(connectionString);
        dbConnection.Open();

        // Make the connection a command and write a query
        IDbCommand dbCmd = dbConnection.CreateCommand();
        if (clue.IsDual())
        {
            isDual = 1;
            DualClue dual = (DualClue)clue;
            sqlQuery =
                string.Format(
                    "INSERT INTO Clues(ID, Cat_ID, Clue1, Clue2, Answer, Difficulty, IsDual) VALUES(@ID, @Cat_ID, @Clue1, @Clue2, @Answer, @Difficulty, @IsDual)");

            dbCmd.CommandText = sqlQuery;


            IDbDataParameter IDParameter = dbCmd.CreateParameter();
            IDbDataParameter Cat_IDParameter = dbCmd.CreateParameter();
            IDbDataParameter Clue1Parameter = dbCmd.CreateParameter();
            IDbDataParameter Clue2Parameter = dbCmd.CreateParameter();
            IDbDataParameter AnswerParameter = dbCmd.CreateParameter();
            IDbDataParameter DifficultyParameter = dbCmd.CreateParameter();
            IDbDataParameter IsDualParameter = dbCmd.CreateParameter();

            dbCmd.Parameters.Add(IDParameter);
            dbCmd.Parameters.Add(Cat_IDParameter);
            dbCmd.Parameters.Add(Clue1Parameter);
            dbCmd.Parameters.Add(Clue2Parameter);
            dbCmd.Parameters.Add(AnswerParameter);
            dbCmd.Parameters.Add(DifficultyParameter);
            dbCmd.Parameters.Add(IsDualParameter);

            IDParameter.ParameterName = "@ID";
            IDParameter.DbType = DbType.UInt32;
            IDParameter.Value = ClueID;

            Cat_IDParameter.ParameterName = "@Cat_ID";
            Cat_IDParameter.DbType = DbType.UInt32;
            Cat_IDParameter.Value = Cat_ID;

            Clue1Parameter.ParameterName = "@Clue1";
            Clue1Parameter.DbType = DbType.String;
            Clue1Parameter.Value = dual.GetClue1();

            Clue2Parameter.ParameterName = "@Clue2";
            Clue2Parameter.DbType = DbType.String;
            Clue2Parameter.Value = dual.GetClue2();

            AnswerParameter.ParameterName = "@Answer";
            AnswerParameter.DbType = DbType.String;
            AnswerParameter.Value = clue.GetAnswer();

            DifficultyParameter.ParameterName = "@Difficulty";
            DifficultyParameter.DbType = DbType.UInt32;
            DifficultyParameter.Value = clue.GetDifficulty();

            IsDualParameter.ParameterName = "@IsDual";
            IsDualParameter.DbType = DbType.UInt32;
            IsDualParameter.Value = isDual;
        }
        else {
            isDual = 0;
            SingleClue single = (SingleClue)clue;
            sqlQuery = string.Format(
                "INSERT INTO Clues(ID, Cat_ID, Clue1, Clue2, Answer, Difficulty, IsDual) VALUES(@ID, @Cat_ID, @Clue1, @Clue2, @Answer, @Difficulty, @IsDual)");

            dbCmd.CommandText = sqlQuery;

            IDbDataParameter IDParameter = dbCmd.CreateParameter();
            IDbDataParameter Cat_IDParameter = dbCmd.CreateParameter();
            IDbDataParameter Clue1Parameter = dbCmd.CreateParameter();
            IDbDataParameter Clue2Parameter = dbCmd.CreateParameter();
            IDbDataParameter AnswerParameter = dbCmd.CreateParameter();
            IDbDataParameter DifficultyParameter = dbCmd.CreateParameter();
            IDbDataParameter IsDualParameter = dbCmd.CreateParameter();

            dbCmd.Parameters.Add(IDParameter);
            dbCmd.Parameters.Add(Cat_IDParameter);
            dbCmd.Parameters.Add(Clue1Parameter);
            dbCmd.Parameters.Add(Clue2Parameter);
            dbCmd.Parameters.Add(AnswerParameter);
            dbCmd.Parameters.Add(DifficultyParameter);
            dbCmd.Parameters.Add(IsDualParameter);

            IDParameter.ParameterName = "@ID";
            IDParameter.DbType = DbType.UInt32;
            IDParameter.Value = ClueID;

            Cat_IDParameter.ParameterName = "@Cat_ID";
            Cat_IDParameter.DbType = DbType.UInt32;
            Cat_IDParameter.Value = Cat_ID;

            Clue1Parameter.ParameterName = "@Clue1";
            Clue1Parameter.DbType = DbType.String;
            Clue1Parameter.Value = single.GetClue();

            Clue2Parameter.ParameterName = "@Clue2";
            Clue2Parameter.DbType = DbType.String;
            Clue2Parameter.Value = "";

            AnswerParameter.ParameterName = "@Answer";
            AnswerParameter.DbType = DbType.String;
            AnswerParameter.Value = clue.GetAnswer();

            DifficultyParameter.ParameterName = "@Difficulty";
            DifficultyParameter.DbType = DbType.UInt32;
            DifficultyParameter.Value = clue.GetDifficulty();

            IsDualParameter.ParameterName = "@IsDual";
            IsDualParameter.DbType = DbType.UInt32;
            IsDualParameter.Value = isDual;
        }

        
        
        dbCmd.ExecuteScalar();

        dbConnection.Close();

        //Set the ID of the clue
        clue.SetID(ClueID);
    
    }

    public void WriteCategorie(Category category)    // please use Categorys Id instead auto increment
    {
        int catID;
        if (category.GetCatEditFlag())
        {
            catID = category.getId();
            this.removeCategoryClues(catID);
        }
        else
        {
            catID = getHighestCategoriesID() + 1;
            category.setId(catID);
        }
        // Open a connection with the database
        IDbConnection dbConnection = new SqliteConnection(connectionString);
        dbConnection.Open();

        // Make the connection a command and write a query
        IDbCommand dbCmd = dbConnection.CreateCommand();
        string sqlQuery = string.Format("INSERT OR REPLACE INTO Categories(ID, Name, Description) VALUES(\"{0}\",\"{1}\",\"{2}\")",
            category.getId(), category.getName(), category.getDescription());

        // Execute the command
        dbCmd.CommandText = sqlQuery;
        dbCmd.ExecuteScalar();
        dbConnection.Close();

        foreach (Clue c in category.getClues().Values)
        {
            WriteClue(catID, c);
        }
    }

    public void WriteGame(Game game)
    {
        // make string of the categories
        int GameID;
        if (game.GetGameEditFlag())
        {
            GameID = game.GetID();
        }
        else
        {
            GameID = getHighestGamesID() + 1; 
        }
      

        List<Category> categories = game.GetCategories();

        StringBuilder builder = new StringBuilder();

        foreach (Category cat in categories)
        {
            builder.Append(cat.getId().ToString()).Append(",");
        }

        string categoriesString = builder.ToString();
        if (!(categoriesString.Length == 0))
        {
            categoriesString = categoriesString.Remove(categoriesString.Length - 1);
        }


        // make string of the dailydoubles
        List<(int, int)> dailydoubles = game.GetDailyDoubles();

        StringBuilder builder2 = new StringBuilder();

        foreach ((int, int) dd in dailydoubles)
        {
            builder2.Append(dd.Item1).Append(",").Append(dd.Item2).Append("|");
        }

        string dailyDoublesString = builder2.ToString();
        if (!(dailyDoublesString.Length == 0))
        {
            dailyDoublesString = dailyDoublesString.Remove(dailyDoublesString.Length - 1);
        }

        // checking Double Jeopardy

        int DJ;

        if (game.IsDouble())
            DJ = 1;
        else
            DJ = 0;

        // Open a connection with the database
        IDbConnection dbConnection = new SqliteConnection(connectionString);
            dbConnection.Open();

        // get the final jeopardy clue
        Clue Finalclue = game.GetFinalJeopardyClue();
   
        // Make the connection a command and write a query
        IDbCommand dbCmd = dbConnection.CreateCommand();
        string sqlQuery = string.Format(
            "INSERT OR REPLACE INTO Games(ID, Name, Description, Categories, DailyDoubles, DoubleJeopardy, FinalJeopardy, FinalCategory) VALUES(\"{0}\",\"{1}\",\"{2}\",\"{3}\",\"{4}\",\"{5}\",\"{6}\",\"{7}\")",
            GameID, game.GetGameName(), game.GetDescription(), categoriesString, dailyDoublesString, DJ, Finalclue.GetID(), game.GetFinalJeopardyCat()
            );

        // Execute the command
        dbCmd.CommandText = sqlQuery;
        dbCmd.ExecuteScalar();
        dbConnection.Close();

        game.SetGameID(GameID);

    }

    /* Ninos Function */
    public Dictionary<int, Category> ReadCategories()
    {
        int i = 0;

        Dictionary<int,Category> Cat = new Dictionary<int,Category>();

        // Open a connection with the database
        IDbConnection dbConnection = new SqliteConnection(connectionString);
        dbConnection.Open();

        // Make the connection a command and write a query
        IDbCommand dbCmd = dbConnection.CreateCommand();
        string sqlQuery = "SELECT * FROM Categories";

        // Execute the command
        dbCmd.CommandText = sqlQuery;
        IDataReader reader = dbCmd.ExecuteReader();
        while (reader.Read())
        {

            Category c = new Category(reader.GetInt32(0), reader.GetString(1), reader.GetString(2), ReadCluesOfCategory(reader.GetInt32(0)));

            Cat.Add(i,c);
            i++;
        }

        dbConnection.Close();

        return Cat;
    }


    public Dictionary<int, Clue> ReadCluesOfCategory(int C_ID)
    {

        int i = 0;

        Dictionary<int, Clue> clues = new Dictionary<int, Clue>();

        IDbConnection dbConnection = new SqliteConnection(connectionString);
        dbConnection.Open();

        // Make the connection a command and write a query
        IDbCommand dbCmd = dbConnection.CreateCommand();
        string sqlQuery = string.Format("SELECT * FROM Clues WHERE Cat_ID = {0}", C_ID);

        // Execute the command
        dbCmd.CommandText = sqlQuery;
        IDataReader reader = dbCmd.ExecuteReader();
        while (reader.Read())
        {
                DualClue   DC;
                SingleClue SC;
            if (reader.GetInt32(6) == 1)
                {
                    DC = new DualClue(reader.GetString(2), reader.GetString(3), reader.GetString(4), reader.GetInt32(5));
                    DC.SetID(reader.GetInt32(0));
                    clues.Add(i, DC);
                }


           else
                {
                    SC = new SingleClue(reader.GetString(2), reader.GetString(4), reader.GetInt32(5));
                    SC.SetID(reader.GetInt32(0));
                    clues.Add(i, SC);
                }

        

            i++;

        }

        dbConnection.Close();

        return clues;
    }


    /* Maher Function Get Games */
    public List<string> ReadGames()
    {

        List<string> games = new List<string>();

        // Open a connection with the database
        IDbConnection dbConnection = new SqliteConnection(connectionString);
        dbConnection.Open();

        // Make the connection a command and write a query

        IDbCommand dbCmd = dbConnection.CreateCommand();
        string sqlQuery = "SELECT * FROM Games";

        // Execute the command
        dbCmd.CommandText = sqlQuery;
        IDataReader reader = dbCmd.ExecuteReader();
        while (reader.Read())
        {
            games.Add(reader.GetString(1));
        }

        dbConnection.Close();

        return games;
    }

    public List<Game> ReadGamesAsObjects()
    {
        List<Game> games = new List<Game>();
        List<Category> categories = new List<Category>();
        List<(int, int)> DailyDoubles = new List<(int, int)>();
        Clue finalClue;
        bool dj = false;

        Dictionary<int, Clue> Clues = new Dictionary<int, Clue>();


        // Open a connection with the database
        IDbConnection dbConnection = new SqliteConnection(connectionString);
        dbConnection.Open();

        // Make the connection a command and write a query
        IDbCommand dbCmd = dbConnection.CreateCommand();
        string sqlQuery = "SELECT * FROM Games";

        // Execute the command
        dbCmd.CommandText = sqlQuery;
        IDataReader reader = dbCmd.ExecuteReader();
        while (reader.Read())
        {
         
            categories = ReadCategoriesOfGame(reader.GetString(1));

            int clue_ID = reader.GetInt32(6);
            finalClue = getFinalClue(clue_ID);

            int DJ = reader.GetInt32(5);
            if (DJ == 1)
                dj = true;

            DailyDoubles = ReadDailyDoublesOfGame(reader.GetString(1));
            string finalCategory = reader.GetString(7);
            Game game = new Game(reader.GetInt32(0), reader.GetString(1), reader.GetString(2), categories, finalClue, dj, DailyDoubles, finalCategory);

            games.Add(game);
        }

        dbConnection.Close();

        return games;
    }

    public List<(int,int)> ReadDailyDoublesOfGame (string GameName)
    {
        List<(int,int)> dd = new List<(int,int)>();

        // Open a connection with the database
        IDbConnection dbConnection = new SqliteConnection(connectionString);
        dbConnection.Open();

        // Make the connection a command and write a query
        IDbCommand dbCmd = dbConnection.CreateCommand();
        string sqlQuery = string.Format("SELECT DailyDoubles FROM Games WHERE Name = '{0}'", GameName);

        // Execute the command
        dbCmd.CommandText = sqlQuery;
        IDataReader reader = dbCmd.ExecuteReader();
        reader.Read();
        string AllDailyDoublesAsString = reader.GetString(0);

        // Parsing the string
        if (!string.IsNullOrWhiteSpace(AllDailyDoublesAsString))
        {
            string[] ddTuples = AllDailyDoublesAsString.Split('|');

            foreach (string ddString in ddTuples)
            {
                string[] items = ddString.Split(',');
                (int, int) tuple;
                tuple.Item1 = int.Parse(items[0]);  //or Int32.TryParse(value, out num);
                tuple.Item2 = int.Parse(items[1]);
                dd.Add(tuple);
            }
        }
        

        dbConnection.Close();
        return dd;
    }

    /* Maher Function */
    public List<Category> ReadCategoriesOfGame(string GameName)
    {
    
        List <Category> Cat = new List<Category>();

        // Open a connection with the database
        IDbConnection dbConnection = new SqliteConnection(connectionString);
        dbConnection.Open();

        // Make the connection a command and write a query
        IDbCommand dbCmd = dbConnection.CreateCommand();
        string sqlQuery = string.Format("SELECT Categories FROM Games WHERE Name = '{0}'", GameName);
        
        // Execute the command
        dbCmd.CommandText = sqlQuery;
        IDataReader reader = dbCmd.ExecuteReader();
        reader.Read();
        string AllCategoriesAsString = reader.GetString(0);

        // Parsing the string
        string[] words = AllCategoriesAsString.Split(',');

        foreach (string word in words)
        {
            
            int n = int.Parse(word);
            Category c = GetSpecificCategory(n);

            Cat.Add(c);

        }

        dbConnection.Close();
        return Cat;
    }

    public Category GetSpecificCategory(int id)
    {
        Category c;

        // Open a connection with the database
        IDbConnection dbConnection = new SqliteConnection(connectionString);
        dbConnection.Open();

        // Make the connection a command and write a query

        IDbCommand dbCmd = dbConnection.CreateCommand();
        string sqlQuery = string.Format("SELECT * FROM Categories WHERE ID = {0}", id);

        // Execute the command
        dbCmd.CommandText = sqlQuery;
        IDataReader reader = dbCmd.ExecuteReader();
        reader.Read();
    
        c = new Category(reader.GetInt32(0), reader.GetString(1), reader.GetString(2), ReadCluesOfCategory(reader.GetInt32(0)));


        dbConnection.Close();
        return c;
    }

    public Clue getFinalClue (int id)
    {
        Clue FC;

        // Open a connection with the database
        IDbConnection dbConnection = new SqliteConnection(connectionString);
        dbConnection.Open();

        // Make the connection a command and write a query

        IDbCommand dbCmd = dbConnection.CreateCommand();
        string sqlQuery = string.Format("SELECT * FROM Clues WHERE ID = {0}", id);

        // Execute the command
        dbCmd.CommandText = sqlQuery;
        IDataReader reader = dbCmd.ExecuteReader();
        reader.Read();
        if (reader.GetInt32(6) == 1)
        {
            FC = new DualClue(reader.GetString(2), reader.GetString(3), reader.GetString(4), reader.GetInt32(5));
            
        }


        else
        {
            FC = new SingleClue(reader.GetString(2), reader.GetString(4), reader.GetInt32(5));
            
        }

        dbConnection.Close();
        return FC;
    }


    public int getHighestCluesID()
    {
        // Open a connection with the database
        IDbConnection dbConnection = new SqliteConnection(connectionString);
        dbConnection.Open();

        // Make the connection a command and write a query
        int num = 0;
        if (!this.IsCluesTableEmpty())
        {
            IDbCommand dbCmd = dbConnection.CreateCommand();
            string sqlQuery = "SELECT MAX(ID) FROM Clues";
            dbCmd.CommandText = sqlQuery;
            IDataReader reader = dbCmd.ExecuteReader();
            reader.Read();
            num = reader.GetInt32(0);
        }

        dbConnection.Close();
        return num;
    }

    public bool IsCategoryTableEmpty()
    {
        // Open a connection with the database
        IDbConnection dbConnection = new SqliteConnection(connectionString);
        dbConnection.Open();

        // Make the connection a command and write a query

        IDbCommand dbCmd = dbConnection.CreateCommand();
        string sqlQuery1 = "SELECT COUNT(*) FROM Categories";
        dbCmd.CommandText = sqlQuery1;
        IDataReader reader = dbCmd.ExecuteReader();
        reader.Read();
        int num = reader.GetInt32(0);
        dbConnection.Close();

        return num == 0 ? true : false;
    }

    public bool IsGamesTableEmpty()
    {
        // Open a connection with the database
        IDbConnection dbConnection = new SqliteConnection(connectionString);
        dbConnection.Open();

        // Make the connection a command and write a query

        IDbCommand dbCmd = dbConnection.CreateCommand();
        string sqlQuery1 = "SELECT COUNT(*) FROM Games";
        dbCmd.CommandText = sqlQuery1;
        IDataReader reader = dbCmd.ExecuteReader();
        reader.Read();
        int num = reader.GetInt32(0);
        dbConnection.Close();

        return num == 0 ? true : false;
    }

    public bool IsCluesTableEmpty()
    {
        // Open a connection with the database
        IDbConnection dbConnection = new SqliteConnection(connectionString);
        dbConnection.Open();

        // Make the connection a command and write a query

        IDbCommand dbCmd = dbConnection.CreateCommand();
        string sqlQuery1 = "SELECT COUNT(*) FROM Clues";
        dbCmd.CommandText = sqlQuery1;
        IDataReader reader = dbCmd.ExecuteReader();
        reader.Read();
        int num = reader.GetInt32(0);
        dbConnection.Close();

        return num == 0 ? true : false;
    }

    public int getHighestCategoriesID()
    {
        // Open a connection with the database
        IDbConnection dbConnection = new SqliteConnection(connectionString);
        dbConnection.Open();

        // Make the connection a command and write a query
        int num = 0;
        if (!this.IsCategoryTableEmpty())
        {
            IDbCommand dbCmd = dbConnection.CreateCommand();
            string sqlQuery = "SELECT MAX(ID) FROM Categories";
            dbCmd.CommandText = sqlQuery;
            IDataReader reader = dbCmd.ExecuteReader();
            reader.Read();
            num = reader.GetInt32(0);
        }
        
        dbConnection.Close();
        return num;
    }

    public int getHighestGamesID()
    {
        // Open a connection with the database
        IDbConnection dbConnection = new SqliteConnection(connectionString);
        dbConnection.Open();

        // Make the connection a command and write a query
        int num = 0;
        if (!this.IsGamesTableEmpty())
        {
            IDbCommand dbCmd = dbConnection.CreateCommand();
            string sqlQuery = "SELECT MAX(ID) FROM Games";
            dbCmd.CommandText = sqlQuery;
            IDataReader reader = dbCmd.ExecuteReader();
            reader.Read();
            num = reader.GetInt32(0);
        }

        dbConnection.Close();
        return num;
    }

    public void removeAllCategories()
    {
        IDbConnection dbConnection = new SqliteConnection(connectionString);
        dbConnection.Open();
        IDbCommand dbCmd = dbConnection.CreateCommand();
        string sqlQuery = string.Format("delete from Categories where 1=1");

        dbCmd.CommandText = sqlQuery;
        dbCmd.ExecuteReader();

        this.removeAllClues();
        dbConnection.Close();
    }

    public void removeAllClues()
    {
        IDbConnection dbConnection = new SqliteConnection(connectionString);
        dbConnection.Open();
        IDbCommand dbCmd = dbConnection.CreateCommand();
        string sqlQuery = string.Format("delete from Clues where 1=1");

        dbCmd.CommandText = sqlQuery;
        dbCmd.ExecuteReader();
        dbConnection.Close();
    }

    public void removeAllGames()
    {
        IDbConnection dbConnection = new SqliteConnection(connectionString);
        dbConnection.Open();
        IDbCommand dbCmd = dbConnection.CreateCommand();
        string sqlQuery = string.Format("delete from Games where 1=1");

        dbCmd.CommandText = sqlQuery;
        dbCmd.ExecuteReader();
        dbConnection.Close();
    }

    private List<int> getGameIDsContainingCat(int catID)
    {
        List<int> gamesIDs = new List<int>();

        IDbConnection dbConnection = new SqliteConnection(connectionString);
        dbConnection.Open();
        IDbCommand dbCmd = dbConnection.CreateCommand();
        string sqlQuery = "SELECT * FROM GAMES";
        dbCmd.CommandText = sqlQuery;
        IDataReader reader = dbCmd.ExecuteReader();

        while (reader.Read())
        {
            string categoriesString = reader.GetString(3);
            string[] catIDs = categoriesString.Split(',');
            int n;
            foreach (string id in catIDs)
            {
                n = int.Parse(id);
                if (n == catID)
                {
                    gamesIDs.Add(reader.GetInt32(0));
                }
            }
        }

        dbConnection.Close();
        return gamesIDs;
    }

    private void removeGamesContainingRemovedCategory(int catID)
    {
        List<int> gamesIDs = this.getGameIDsContainingCat(catID);

        foreach (int ID in gamesIDs)
        {
            this.removeSpecificGame(ID);
        }
    }

    public void removeSpecificCategory(int categoryId)
    {
        IDbConnection dbConnection = new SqliteConnection(connectionString);
        dbConnection.Open();
        IDbCommand dbCmd = dbConnection.CreateCommand();
        string sqlQuery = string.Format("delete from Categories where ID = {0}", categoryId);

        dbCmd.CommandText = sqlQuery;
        dbCmd.ExecuteReader();
        dbConnection.Close();

        //remove corresponding Clues and Games
        this.removeCategoryClues(categoryId);
        this.removeGamesContainingRemovedCategory(categoryId);
    }

    public void removeSpecificClue(int clueID)
    {
        IDbConnection dbConnection = new SqliteConnection(connectionString);
        dbConnection.Open();
        IDbCommand dbCmd = dbConnection.CreateCommand();
        string sqlQuery = string.Format("delete from Clues where ID = {0}", clueID);

        dbCmd.CommandText = sqlQuery;
        dbCmd.ExecuteReader();
        dbConnection.Close();
    }

    public void removeCategoryClues(int categoryId)
    {
        IDbConnection dbConnection = new SqliteConnection(connectionString);
        dbConnection.Open();
        IDbCommand dbCmd = dbConnection.CreateCommand();
        string sqlQuery = string.Format("delete from Clues where Cat_ID = {0}", categoryId);

        dbCmd.CommandText = sqlQuery;
        dbCmd.ExecuteReader();
        dbConnection.Close();
    }

    public void removeSpecificGame(int gameId)
    {
        removeGameRelatedFinalJeopardy(gameId);
        IDbConnection dbConnection = new SqliteConnection(connectionString);
        dbConnection.Open();
        IDbCommand dbCmd = dbConnection.CreateCommand();
        string sqlQuery = string.Format("delete from Games where ID = {0}", gameId);

        dbCmd.CommandText = sqlQuery;
        dbCmd.ExecuteReader();
        dbConnection.Close();
    }

    public void removeGameRelatedFinalJeopardy(int gameID)
    {
        IDbConnection dbConnection = new SqliteConnection(connectionString);
        dbConnection.Open();
        IDbCommand dbCmd = dbConnection.CreateCommand();
        string sqlQuery = string.Format("SELECT FinalJeopardy FROM Games WHERE ID = {0}", gameID);
        dbCmd.CommandText = sqlQuery;
        IDataReader reader = dbCmd.ExecuteReader();
        reader.Read();
        int clueID = (reader.GetInt32(0));
        dbConnection.Close();
        Debug.Log("clue id to remove: " + clueID);
        this.removeSpecificClue(clueID);
    }
    private void CreatClueTable()
    {
        IDbConnection dbConnection = new SqliteConnection(connectionString);
        dbConnection.Open();
        IDbCommand dbcmd;
        IDataReader reader;
        dbcmd = dbConnection.CreateCommand();
        string q_createTable =
          "CREATE TABLE IF NOT EXISTS Clues(" +
          "ID INTEGER PRIMARY KEY," +
          "Cat_ID INTEGER NULL," +
          "Clue1 TEXT NULL," +
          "Clue2 TEXT NULL," +
          "Answer TEXT NULL," +
          "Difficulty INTEGER NULL," +
          "IsDual INTEGER NULL" +
          ")";
        dbcmd.CommandText = q_createTable;
        reader = dbcmd.ExecuteReader();
        dbConnection.Close();
    }
    private void CreatCategoryTable()
    {
        IDbConnection dbConnection = new SqliteConnection(connectionString);
        dbConnection.Open();
        IDbCommand dbcmd;
        IDataReader reader;
        dbcmd = dbConnection.CreateCommand();
        string q_createTable =
          "CREATE TABLE IF NOT EXISTS Categories(" +
          "ID INTEGER PRIMARY KEY," +
          "Name TEXT NOT NULL," +
          "Description TEXT NULL" +
          ")";
        dbcmd.CommandText = q_createTable;
        reader = dbcmd.ExecuteReader();
        dbConnection.Close();
    }
    private void CreatGameTable()
    {
        IDbConnection dbConnection = new SqliteConnection(connectionString);
        dbConnection.Open();
        IDbCommand dbcmd;
        IDataReader reader;
        dbcmd = dbConnection.CreateCommand();
        string q_createTable =
          "CREATE TABLE IF NOT EXISTS Games("+
          "ID INTEGER PRIMARY KEY,"+
          "Name TEXT NOT NULL,"+
          "Description TEXT NULL,"+
          "Categories TEXT NOT NULL,"+
          "DailyDoubles TEXT NULL,"+
          "DoubleJeopardy INTEGER NULL,"+
          "FinalJeopardy INTEGER NULL,"+
          "FinalCategory TEXT NULL"+
          ")";
        dbcmd.CommandText = q_createTable;
        reader = dbcmd.ExecuteReader();
        dbConnection.Close();
    }
    public void CreateSQLiteTablesIfNotExists()
    {
        CreatClueTable();
        CreatCategoryTable();
        CreatGameTable();
    }
}