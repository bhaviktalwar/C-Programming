// ======================================================
// Movie Database Management System
// Developed using SplashKit Framework
// Author: Bhavik
// Description:
//     This program allows users to manage a small movie
//     database by adding, viewing, modifying, filtering,
//     and reviewing movies. It demonstrates modular
//     programming, user interaction, and data structuring.
// ======================================================

#include "splashkit.h"
using std::to_string;
using std::stoi;
using std::stod;

// =========================
// CONSTANTS
// =========================
const int MAX_MOVIES = 100;     // Maximum number of movies allowed
const int MAX_REVIEWS = 100;    // Maximum number of reviews per movie

// =========================
// STRUCTURE DEFINITIONS
// =========================

// Structure to store individual review details
struct Review
{
    string reviewer_name;   // Name of the reviewer
    string review_text;     // Review description
    int review_score;       // Rating between 0 and 10
};

// Structure to store movie details and associated reviews
struct Movie
{
    string movie_name;                     // Title of the movie
    string movie_description;              // Short description or summary
    string movie_genre;                    // Genre (Action, Romance, etc.)
    Review movie_reviews[MAX_REVIEWS];     // Array to store multiple reviews
    int total_reviews = 0;                 // Number of reviews added
};

// Structure for the entire database of movies
struct Database
{
    Movie movie_list[MAX_MOVIES];          // Array of movies
    int total_movies = 0;                  // Count of total movies in database
};

// =========================
// FUNCTION DEFINITIONS
// =========================

// Function to add a new movie to the database
void add_movie(Database &database)
{
    Movie new_movie;
    write_line("Please input movie name: ");
    new_movie.movie_name = read_line();
    
    write_line("Please input movie description: ");
    new_movie.movie_description = read_line();

    // Ask for genre input with validation
    write_line("Enter movie genre: Action(0), Romance(1), Sci-Fi(2), Fantasy(3)");
    int genre_choice;
    do
    {
        genre_choice = stoi(read_line());
        if (genre_choice < 0 || genre_choice > 3)
        {
            write_line("Invalid choice. Please enter a number between 0 and 3.");
        }
    } while (genre_choice < 0 || genre_choice > 3);

    // Store genre as string representation
    new_movie.movie_genre = to_string(genre_choice);
    new_movie.total_reviews = 0;

    // Add new movie to database and increase count
    database.movie_list[database.total_movies++] = new_movie;

    // Prevent overflow of movie limit
    if (database.total_movies >= MAX_MOVIES)
    {
        write_line("Max limit reached. Can't add more movies.");
        return;
    }
}

// Function to remove an existing movie from the database
void remove_movie(Database &database)
{
    write("Enter the name of the movie to remove: ");
    string movie_name = read_line();

    for (int i = 0; i < database.total_movies; i++)
    {
        if (database.movie_list[i].movie_name == movie_name)
        {
            // Shift remaining movies one position up to fill gap
            for (int j = i; j < database.total_movies - 1; j++)
            {
                database.movie_list[j] = database.movie_list[j + 1];
            }

            database.total_movies--;
            write_line("Movie removed successfully.");
            return;
        }
    }

    write_line("Movie not found.");
}

// Function to add a review for an existing movie
void add_review(Database &database)
{
    write("Enter the name of the movie to review: ");
    string movie_name = read_line();

    for (int i = 0; i < database.total_movies; i++)
    {
        if (database.movie_list[i].movie_name == movie_name)
        {
            if (database.movie_list[i].total_reviews >= MAX_REVIEWS)
            {
                write_line("This movie has reached the maximum number of reviews.");
                return;
            }

            Review new_review;

            // Input review details
            write("Please input Review Description: ");
            new_review.review_text = read_line();

            // Input and validate score
            write("Enter your score (0-10): ");
            new_review.review_score = stoi(read_line());

            while (new_review.review_score < 0 || new_review.review_score > 10)
            {
                write("Please Enter a score between 0 and 10: ");
                new_review.review_score = stoi(read_line());
            }

            // Add review to the selected movie
            database.movie_list[i].movie_reviews[database.movie_list[i].total_reviews++] = new_review;
            write_line("Review added successfully.");
            return;
        }
    }

    write_line("Movie not found.");
}

// Function to calculate and return average rating of a movie
double calculate_average_rating(const Movie &movie)
{
    if (movie.total_reviews == 0)
        return 0;

    double total_score = 0;

    // Sum up all review scores
    for (int i = 0; i < movie.total_reviews; i++)
    {
        total_score += movie.movie_reviews[i].review_score;
    }

    // Return average
    return total_score / movie.total_reviews;
}

// Function to display details of a specific movie
void print_movie(const Movie &movie)
{
    string details[] = {
        "Name: " + movie.movie_name,
        "Description: " + movie.movie_description,
        "Genre: " + movie.movie_genre,
        "Average Rating: " + to_string(calculate_average_rating(movie))
    };

    // Print general movie details
    for (int i = 0; i < 4; i++)
    {
        write_line(details[i]);
    }

    // Print reviews if available
    if (movie.total_reviews == 0)
    {
        write_line("No reviews for this movie.");
    }
    else
    {
        write_line("Reviews:");
        for (int i = 0; i < movie.total_reviews; i++)
        {
            const Review &review = movie.movie_reviews[i];
            write_line(" - [" + to_string(review.review_score) + "] " + review.review_text);
        }
    }
}

// Function to display all movies and allow user actions on selection
void view_all_movies(Database &database)
{
    int index;
    int option;
    string input;

    // Display all movies by index
    for (int i = 0; i < database.total_movies; i++)
    {
        write_line("( " + to_string(i + 1) + " ) " + database.movie_list[i].movie_name);
    }

    // Let user choose a movie to interact with
    write_line("Please select a movie from the above list");
    index = stoi(read_line()) - 1;

    if (index < 0 || index >= database.total_movies)
    {
        write_line("Invalid choice.");
        return;
    }

    // Display possible operations
    write_line("What would you like to do with the chosen movie?");
    write_line("(1) - View Details");
    write_line("(2) - Alter Movie");
    write_line("(3) - Add Review");
    write_line("(4) - Delete Movie");
    option = stoi(read_line());

    Movie &selected_movie = database.movie_list[index];

    // Perform action based on user choice
    switch (option)
    {
    case 1:
        print_movie(selected_movie);
        break;

    case 2:
        write_line("What would you like to alter? (name / description / genre)");
        input = read_line();

        if (input == "name")
        {
            write_line("Enter new name:");
            selected_movie.movie_name = read_line();
        }
        else if (input == "description")
        {
            write_line("Enter new description:");
            selected_movie.movie_description = read_line();
        }
        else if (input == "genre")
        {
            write_line("Enter new genre (0: Action, 1: Romance, 2: Sci-Fi, 3: Fantasy):");
            int new_genre;
            do
            {
                new_genre = stoi(read_line());
                if (new_genre < 0 || new_genre > 3)
                {
                    write_line("Invalid genre. Please enter a number between 0 and 3.");
                }
            } while (new_genre < 0 || new_genre > 3);

            selected_movie.movie_genre = to_string(new_genre);
        }
        break;

    case 3:
        add_review(database);
        break;

    case 4:
        remove_movie(database);
        break;

    default:
        write_line("Invalid option.");
        break;
    }
}

// Function to filter and display movies by genre
void filter_by_genre(const Database &database)
{
    int genre_choice;
    write_line("Enter genre number: 0: Action, 1: Romance, 2: Sci-Fi, 3: Fantasy: ");
    genre_choice = stoi(read_line());

    string genre;
    switch (genre_choice)
    {
        case 0: genre = "Action"; break;
        case 1: genre = "Romance"; break;
        case 2: genre = "Sci-Fi"; break;
        case 3: genre = "Fantasy"; break;
        default:
            write_line("Invalid genre choice.");
            return;
    }

    // Print only movies that match the selected genre
    bool found = false;
    for (int i = 0; i < database.total_movies; i++)
    {
        if (database.movie_list[i].movie_genre == to_string(genre_choice))
        {
            print_movie(database.movie_list[i]); 
            found = true;           
        }
    }

    if (!found)
        write_line("No movies found in the selected genre.");
}

// Function to filter movies by a minimum average rating
void filter_by_rating(const Database &database)
{
    write("Enter minimum average rating to filter by: ");
    double min_rating = stod(read_line());

    for (int i = 0; i < database.total_movies; i++)
    {
        if (calculate_average_rating(database.movie_list[i]) >= min_rating)
        {
            print_movie(database.movie_list[i]); 
        }
    }
}

// Menu navigation and screen selection
void screen_selection(Database &database)
{
    int choice;
    do
    {
        // Display main options
        write_line("(1) Add Movie");
        write_line("(2) View All Movies");
        write_line("(3) List Movies by Genre");
        write_line("(4) List Movies by Rating");
        write_line("(5) Exit");
        write("Please select a screen: ");
        choice = stoi(read_line());

        // Handle user selection
        switch (choice)
        {
        case 1:
            add_movie(database);
            break;
        case 2:
            view_all_movies(database);
            break;
        case 3:
            filter_by_genre(database);
            break;
        case 4:
            filter_by_rating(database);
            break;
        case 5:
            write_line("Exiting the program. Goodbye!");
            break;
        default:
            write_line("Invalid choice, please try again.");
        }

    } while (choice != 5);
}

// =========================
// MAIN FUNCTION
// =========================
int main()
{
    string user_name;

    write_line("Welcome to the Movie Database!");
    write_line("Please enter your name:");
    user_name = read_line();

    // Initialize database
    Database database;

    // Display interactive menu
    screen_selection(database);
    
    write_line("Thank you for using the Movie Database, " + user_name + "! Have a great day!");
    return 0;
}
