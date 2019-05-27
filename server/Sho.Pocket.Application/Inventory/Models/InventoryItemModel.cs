﻿using Sho.Pocket.Domain.Entities;
using System;

namespace Sho.Pocket.Application.Inventory.Models
{
    public class InventoryItemModel
    {
        public InventoryItemModel()
        {
        }

        public InventoryItemModel(InventoryItem item)
        {
            Id = item.Id;
            Name = item.Name;
            Description = item.Description;
            Category = item.Category;
            UserOpenId = item.UserOpenId;
        }

        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string Category { get; set; }

        public Guid UserOpenId { get; set; }
    }
}
