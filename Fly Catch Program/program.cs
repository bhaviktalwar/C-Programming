#include "splashkit.h"

int main()
{
    // ===========================
    // CONSTANT DEFINITIONS
    // ===========================
    const string GAME_TIMER = "GameTimer";     // Timer name for managing game events
    const int SCREEN_WIDTH = 800;              // Screen width in pixels
    const int SCREEN_HEIGHT = 600;             // Screen height in pixels
    const int SPIDER_RADIUS = 25;              // Radius of the spider (player)
    const int SPIDER_SPEED = 3;                // Movement speed of the spider
    const int FLY_RADIUS = 10;                 // Radius of the fly (target)

    // ===========================
    // INITIAL GAME SETUP
    // ===========================
    // Spider starts at the center of the screen
    int spiderX = SCREEN_WIDTH / 2;
    int spiderY = SCREEN_HEIGHT / 2;

    // Randomly position the fly somewhere on the screen
    int flyX = rnd(SCREEN_WIDTH);
    int flyY = rnd(SCREEN_HEIGHT);

    // Control variables for fly appearance and timing
    bool flyAppeared = false;                     // Whether the fly is currently visible
    long appearAtTime = 1000 + rnd(2000);         // Random time after which fly will appear
    long escapeAtTime = 0;                        // Time when fly will disappear again

    // ===========================
    // WINDOW AND TIMER SETUP
    // ===========================
    open_window("Fly Catch", SCREEN_WIDTH, SCREEN_HEIGHT);  // Open game window
    create_timer(GAME_TIMER);                               // Create game timer
    start_timer(GAME_TIMER);                                // Start timer

    // ===========================
    // MAIN GAME LOOP
    // ===========================
    while (!quit_requested())  // Run until user closes the window
    {
        // ===========================
        // HANDLE PLAYER MOVEMENT
        // ===========================
        // Move right if RIGHT arrow is pressed and within screen bounds
        if (key_down(RIGHT_KEY) && spiderX + SPIDER_RADIUS < SCREEN_WIDTH)
        {
            spiderX += SPIDER_SPEED;
        }
        // Move left if LEFT arrow is pressed and within screen bounds
        if (key_down(LEFT_KEY) && spiderX - SPIDER_RADIUS > 0)
        {
            spiderX -= SPIDER_SPEED;
        }
        // Move down if DOWN arrow is pressed and within screen bounds
        if (key_down(DOWN_KEY) && spiderY + SPIDER_RADIUS < SCREEN_HEIGHT)
        {
            spiderY += SPIDER_SPEED;
        }
        // Move up if UP arrow is pressed and within screen bounds
        if (key_down(UP_KEY) && spiderY - SPIDER_RADIUS > 0)
        {
            spiderY -= SPIDER_SPEED;
        }

        // ===========================
        // CONTROL FLY APPEARANCE
        // ===========================
        if (!flyAppeared && timer_ticks(GAME_TIMER) > appearAtTime)
        {
            // Make the fly appear at a random position
            flyAppeared = true;
            flyX = rnd(SCREEN_WIDTH);
            flyY = rnd(SCREEN_HEIGHT);

            // Schedule when the fly will escape/disappear
            escapeAtTime = timer_ticks(GAME_TIMER) + 2000 + rnd(5000);
        }
        else if (flyAppeared && timer_ticks(GAME_TIMER) > escapeAtTime)
        {
            // Hide the fly and set the next appearance time
            flyAppeared = false;
            appearAtTime = timer_ticks(GAME_TIMER) + 1000 + rnd(2000);
        }

        // ===========================
        // CHECK COLLISION (SPIDER CATCHES FLY)
        // ===========================
        if (circles_intersect(spiderX, spiderY, SPIDER_RADIUS, flyX, flyY, FLY_RADIUS))
        {
            // When spider touches the fly, remove fly and set next appearance
            flyAppeared = false;
            appearAtTime = timer_ticks(GAME_TIMER) + 1000 + rnd(2000);
        }

        // ===========================
        // DRAW GAME OBJECTS
        // ===========================
        clear_screen(color_white());  // Clear screen for each frame

        // Draw spider (player)
        fill_circle(color_black(), spiderX, spiderY, SPIDER_RADIUS);

        // Draw fly only when it has appeared
        if (flyAppeared)
        {
            fill_circle(color_orange(), flyX, flyY, FLY_RADIUS);
        }

        // Refresh the screen and process user inputs
        refresh_screen(100);  // Set refresh rate to 100 FPS
        process_events();     // Handle keyboard and system events
    }
}
