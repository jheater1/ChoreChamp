﻿namespace ChoreChamp.API.Domain;

public class Chore
{
    int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public int Points { get; set; }
}
