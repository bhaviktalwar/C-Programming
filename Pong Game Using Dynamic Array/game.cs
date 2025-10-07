// Code Using Dynamic Memory Allocation (Heap Memory)
#include "splashkit.h"
using std::to_string;
using std::stoi;

// =========================
// CONSTANT DEFINITIONS
// =========================
const int SCREEN_WIDTH = 800;
const int SCREEN_HEIGHT = 600;
const int PADDLE_WIDTH = 20;  
const int PADDLE_HEIGHT = 150;  
const int BALL_SIZE = 20;  
const int PADDLE_SPEED = 5;
const int BALL_SPEED = 4;  

// =========================
// STRUCTURES FOR GAME OBJECTS
// =========================

// Structure for paddle position
struct Paddle
{
    double x, y;  // Paddle’s position coordinates
};

// Structure for ball position and movement direction
struct Ball
{
    double x, y;   // Ball’s position
    double dx, dy; // Ball’s movement direction (velocity)
};

// =========================
// DRAWING FUNCTIONS
// =========================

// Draws a paddle on the screen
void draw_paddle(const Paddle &paddle)
{
    // Left paddle is red, right paddle is blue
    if (paddle.x < SCREEN_WIDTH / 2) 
        fill_rectangle(COLOR_RED, paddle.x, paddle.y, PADDLE_WIDTH, PADDLE_HEIGHT);
    else  
        fill_rectangle(COLOR_BLUE, paddle.x, paddle.y, PADDLE_WIDTH, PADDLE_HEIGHT);
}

// Draws the ball
void draw_ball(const Ball &ball)
{
    fill_ellipse(COLOR_BLACK, ball.x, ball.y, BALL_SIZE, BALL_SIZE);
}

// Displays the score for both players
void draw_score(const int* score, int num_players)
{
    for (int i = 0; i < num_players; i++)  
    {
        string player_score;
        if (i == 0)
        {
            player_score = "Left Player: " + to_string(score[i]);
            draw_text(player_score, COLOR_BLACK, 20, 20);  
        }
        else
        {
            player_score = "Right Player: " + to_string(score[i]);
            draw_text(player_score, COLOR_BLACK, SCREEN_WIDTH - 200, 20);  
        }
    }
}

// Displays both player names on the screen
void draw_player_names(const string* player_names)
{
    draw_text("Left Player: " + player_names[0], COLOR_BLACK, 20, 50);
    draw_text("Right Player: " + player_names[1], COLOR_BLACK, SCREEN_WIDTH - 200, 50);
}

// Displays the winner when the game ends
void draw_winner(const int* score, const string* player_names, int num_players, int winning_score)
{
    string winner_text;
    
    // Check which player reached the winning score
    for (int i = 0; i < num_players; i++)
    {
        if (score[i] >= winning_score)
        {
            winner_text = player_names[i] + " Wins!";
            write_line(player_names[i] + " Wins!");
            break;
        }
    }

    // Show winner text in red
    draw_text(winner_text, COLOR_RED, 400, 300);
    delay(2000);  // Pause to display the result
}

// =========================
// UPDATE FUNCTIONS
// =========================

// Updates paddle position based on player input
void update_paddle(Paddle &paddle, bool move_up, bool move_down)
{
    if (move_up && paddle.y > 0)
        paddle.y -= PADDLE_SPEED;  // Move up
    
    if (move_down && paddle.y < SCREEN_HEIGHT - PADDLE_HEIGHT)
        paddle.y += PADDLE_SPEED;  // Move down
}

// Handles ball movement, collision, and scoring
void update_ball(Ball &ball, Paddle &left_paddle, Paddle &right_paddle, int* score)
{
    // Update ball position
    ball.x += ball.dx;
    ball.y += ball.dy;

    // Bounce the ball off top and bottom screen edges
    if (ball.y <= 0 || ball.y >= SCREEN_HEIGHT - BALL_SIZE)
        ball.dy = -ball.dy;

    // Detect collision with left paddle
    if (ball.x <= left_paddle.x + PADDLE_WIDTH && ball.y >= left_paddle.y && ball.y <= left_paddle.y + PADDLE_HEIGHT)
        ball.dx = BALL_SPEED;

    // Detect collision with right paddle
    if (ball.x >= right_paddle.x - BALL_SIZE && ball.y >= right_paddle.y && ball.y <= right_paddle.y + PADDLE_HEIGHT)
        ball.dx = -BALL_SPEED;

    // If ball exits the left boundary → Right player scores
    if (ball.x <= 0 && ball.dx < 0) 
    {
        score[1]++;  // Increment right player score
        ball.x = SCREEN_WIDTH / 2;  // Reset ball position
        ball.y = SCREEN_HEIGHT / 2;
        ball.dx = BALL_SPEED;
        ball.dy = BALL_SPEED;
    }
    // If ball exits the right boundary → Left player scores
    else if (ball.x >= SCREEN_WIDTH - BALL_SIZE && ball.dx > 0)
    {
        score[0]++;  // Increment left player score
        ball.x = SCREEN_WIDTH / 2;  // Reset ball position
        ball.y = SCREEN_HEIGHT / 2;
        ball.dx = -BALL_SPEED;
        ball.dy = BALL_SPEED;
    }
}

// =========================
// MAIN FUNCTION
// =========================
int main()
{ 
    // ===== Player Setup =====
    string player_names[2];  
    int num_players = 2;

    // Get player names via console input
    write_line("Enter the name of the Left Player: ");
    player_names[0] = read_line();  
    write_line("Enter the name of the Right Player: ");
    player_names[1] = read_line();  

    // Get winning score from user
    int winning_score;
    write_line("Enter the number of points required to win the game: ");
    winning_score = stoi(read_line());  // Converts string input to integer

    // Create the game window
    open_window("Bhavik's Pong", SCREEN_WIDTH, SCREEN_HEIGHT);

    // Initialize paddles and ball
    Paddle left_paddle = {50, SCREEN_HEIGHT / 2 - PADDLE_HEIGHT / 2};
    Paddle right_paddle = {SCREEN_WIDTH - 50 - PADDLE_WIDTH, SCREEN_HEIGHT / 2 - PADDLE_HEIGHT / 2};
    Ball ball = {SCREEN_WIDTH / 2, SCREEN_HEIGHT / 2, BALL_SPEED, BALL_SPEED};
    
    // =========================
    // DYNAMIC MEMORY ALLOCATION
    // =========================
    // Create score array dynamically in heap memory
    int* score = new int[num_players] {0};  

    // =========================
    // MAIN GAME LOOP
    // =========================
    while (score[0] < winning_score && score[1] < winning_score)
    {
        process_events();  // Handle keyboard inputs and events

        // Detect paddle movement inputs
        bool left_paddle_up = key_down(W_KEY);
        bool left_paddle_down = key_down(S_KEY);
        bool right_paddle_up = key_down(UP_KEY);
        bool right_paddle_down = key_down(DOWN_KEY);

        // Update paddle positions based on inputs
        update_paddle(left_paddle, left_paddle_up, left_paddle_down);
        update_paddle(right_paddle, right_paddle_up, right_paddle_down);

        // Update ball movement and scoring logic
        update_ball(ball, left_paddle, right_paddle, score);

        // Clear screen for next frame
        clear_screen(COLOR_SKY_BLUE);

        // Draw all game elements
        draw_paddle(left_paddle);
        draw_paddle(right_paddle);
        draw_ball(ball);
        draw_score(score, num_players);
        draw_player_names(player_names);

        // Check for winner and display result
        if (score[0] >= winning_score || score[1] >= winning_score)
            draw_winner(score, player_names, num_players, winning_score);

        // Refresh the screen at 60 FPS
        refresh_screen(60);
    }

    // =========================
    // MEMORY DEALLOCATION
    // =========================
    // Free dynamically allocated heap memory
    delete[] score;

    return 0;
}
