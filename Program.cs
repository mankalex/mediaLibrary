﻿using NLog;

// See https://aka.ms/new-console-template for more information
string path = Directory.GetCurrentDirectory() + "\\nlog.config";

// create instance of Logger
var logger = LogManager.LoadConfiguration(path).GetCurrentClassLogger();
logger.Info("Program started");

string scrubbedFile = FileScrubber.ScrubMovies("movies.csv");
logger.Info(scrubbedFile);
MovieFile movieFile = new MovieFile(scrubbedFile);

string choice = "";
do
{
    // display choices to user
  Console.WriteLine("1) Add Movie");
  Console.WriteLine("2) Display All Movies");
  Console.WriteLine("3) Find Movie");
  Console.WriteLine("Enter to quit");
  // input selection
  choice = Console.ReadLine();
  logger.Info("User choice: {Choice}", choice);
  if (choice == "1")
  {
    Movie movie = new Movie();
    // ask user to input movie title
    Console.WriteLine("Enter movie title");
    // input title
    movie.title = Console.ReadLine();
    // verify title is unique
    if (movieFile.isUniqueTitle(movie.title)){
      string input;
      do
      {
        // ask user to enter genre
        Console.WriteLine("Enter genre (or done to quit)");
        // input genre
        input = Console.ReadLine();
        // if user enters "done"
        // or does not enter a genre do not add it to list
        if (input != "done" && input.Length > 0)
        {
          movie.genres.Add(input);
        }
      } while (input != "done");
      // specify if no genres are entered
      if (movie.genres.Count == 0)
      {
        movie.genres.Add("(no genres listed)");
      }
      // ask user to enter director
      Console.WriteLine("Enter director");
      //input director
      movie.director = Console.ReadLine();
      // ask user for runningTime
      Console.WriteLine("Enter running time (h:m:s)");
      //input runningTime
      movie.runningTime = TimeSpan.Parse(Console.ReadLine());
      // add movie
      movieFile.AddMovie(movie);
    }
  } else if (choice == "2")
  {
    // Display All Movies
    foreach(Movie m in movieFile.Movies)
    {
      Console.WriteLine(m.Display());
    }
  } else if (choice == "3")
  {
    Console.WriteLine("Enter title to search");
    var mInput = Console.ReadLine();
    // LINQ - Where filter operator & Select projection operator & Contains quantifier operator
    var titles = movieFile.Movies.Where(m => m.title.Contains(mInput)).Select(m => m.title);
    // LINQ - Count aggregation method
    Console.WriteLine($"There are {titles.Count()} movies with \"Shark\" in the title:");
    foreach(string t in titles)
    {
        Console.WriteLine($"  {t}");
    }
  }
} while (choice == "1" || choice == "2" || choice == "3");

logger.Info("Program ended");
