using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TodoApi.Models;

namespace TodoApi.Dtos;

public class UpdateItemDto
{

    // public long Id { get; set; }
    // public string? Name { get; set; }
    public bool IsComplete { get; set; }

}
public class ItemDto
{

    public long Id { get; set; }
    public string? Name { get; set; }
    public bool IsComplete { get; set; }

}
