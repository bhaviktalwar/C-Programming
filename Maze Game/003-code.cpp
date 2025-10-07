//Maze Game
#include "splashkit.h"
#include <string>
using std::to_string;

struct room_struct;
struct path_struct{
    string description;
    room_struct *destination;
};

struct dynamic_array{
    path_struct *data;
    int size;
    int capacity;
};

void initial(dynamic_array &arr, int initial_capacity = 2){
    arr.data = new path_struct[initial_capacity];
    arr.size = 0;
    arr.capacity = initial_capacity;
}

void add(dynamic_array &arr, path_struct path){
    if (arr.size == arr.capacity)
    {
        arr.capacity *= 2;
        path_struct *new_data = new path_struct[arr.capacity];
        for (int i = 0; i < arr.size; ++i)
        {
            new_data[i] = arr.data[i];
        }
        delete[] arr.data;
        arr.data = new_data;
    }
    arr.data[arr.size++] = path;
}

void del(dynamic_array &arr){
    delete[] arr.data;
    arr.data = nullptr;
}

struct room_struct{
    string title;
    string description;
    dynamic_array paths;
};

path_struct new_path(string description, room_struct *destination){
    path_struct result = {description, destination};
    return result;
}

room_struct new_room(string title, string description){
    room_struct result = {title, description};
    initial(result.paths);
    return result;
}

void add_path(room_struct &room, path_struct path){
    add(room.paths, path);
}

void print_room(const room_struct *room){
    write_line("Room: " + room->title);
    write_line(room->description);
    write_line("Paths:");
    for (int i = 0; i < room->paths.size; ++i)
    {
        write_line(" " + to_string(i) + ": " + room->paths.data[i].description);
    }
}

void move_player(room_struct *&room, const path_struct &path){
    room = path.destination;
}

bool explore(room_struct *&room)
{
    int option = 0;

    print_room(room);

    if (room->paths.size == 0)
    {
        write_line("No paths available.");
        return false;
    }
         write_line("Which path do you want to take?");
    option = std::stoi(read_line());

    while (option < 0 || option >= room->paths.size)
    {
        write_line("Choose a value between 0 and " + to_string(room->paths.size - 1));
        option = std::stoi(read_line());
    }
    move_player(room, room->paths.data[option]);
    return true;
}

int main()
{
    room_struct r1 = new_room("Room 1", "You are in a happy place");
    room_struct r2 = new_room("Room 2", "This is room 2");
    room_struct r3 = new_room("Room 3", "This is room 3");

    add_path(r1, new_path("A large sliding door", &r2));
    add_path(r1, new_path("An open corridor", &r3));
    add_path(r1, new_path("A small door", &r1));

    room_struct *current_room = &r1;

    while (explore(current_room)){}
    del(r1.paths);
    del(r2.paths);
    del(r3.paths);

    return 0;
}
//************************************************************
