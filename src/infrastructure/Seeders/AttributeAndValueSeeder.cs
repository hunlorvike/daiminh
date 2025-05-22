using AutoRegister;
using domain.Entities;
using infrastructure.Seeders.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using shared.Helpers;

namespace infrastructure.Seeders;

[Register(ServiceLifetime.Transient)]
public class AttributeAndValueSeeder : IDataSeeder
{
    private readonly ApplicationDbContext _dbContext;

    public AttributeAndValueSeeder(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public int Order => 7;

    public async Task SeedAsync()
    {
        Console.WriteLine("Seeding Attributes and Attribute Values...");

        if (await _dbContext.Attributes.AnyAsync())
        {
            return;
        }

        var attributes = new List<domain.Entities.Attribute>
        {
            new domain.Entities.Attribute { Name = "Dung Tích", Slug = SlugHelper.Generate("Dung Tích"), CreatedAt = DateTime.Now },
            new domain.Entities.Attribute { Name = "Bề Mặt", Slug = SlugHelper.Generate("Bề Mặt"), CreatedAt = DateTime.Now },
            new domain.Entities.Attribute { Name = "Độ Bóng", Slug = SlugHelper.Generate("Độ Bóng"), CreatedAt = DateTime.Now }
        };

        await _dbContext.Attributes.AddRangeAsync(attributes);
        await _dbContext.SaveChangesAsync(); // Save to get IDs

        var dungTichAttr = attributes.First(a => a.Slug == SlugHelper.Generate("Dung Tích"));
        var beMatAttr = attributes.First(a => a.Slug == SlugHelper.Generate("Bề Mặt"));
        var doBongAttr = attributes.First(a => a.Slug == SlugHelper.Generate("Độ Bóng"));

        var attributeValues = new List<AttributeValue>
        {
            // Dung Tích
            new AttributeValue { AttributeId = dungTichAttr.Id, Value = "1 Lít", Slug = SlugHelper.Generate("1 Lít"), CreatedAt = DateTime.Now },
            new AttributeValue { AttributeId = dungTichAttr.Id, Value = "5 Lít", Slug = SlugHelper.Generate("5 Lít"), CreatedAt = DateTime.Now },
            new AttributeValue { AttributeId = dungTichAttr.Id, Value = "18 Lít", Slug = SlugHelper.Generate("18 Lít"), CreatedAt = DateTime.Now },
            new AttributeValue { AttributeId = dungTichAttr.Id, Value = "20 Kg", Slug = SlugHelper.Generate("20 Kg"), CreatedAt = DateTime.Now },
            new AttributeValue { AttributeId = dungTichAttr.Id, Value = "Thùng 5 Kg", Slug = SlugHelper.Generate("Thùng 5 Kg"), CreatedAt = DateTime.Now },

            // Bề Mặt
            new AttributeValue { AttributeId = beMatAttr.Id, Value = "Tường Nội Thất", Slug = SlugHelper.Generate("Tường Nội Thất"), CreatedAt = DateTime.Now },
            new AttributeValue { AttributeId = beMatAttr.Id, Value = "Tường Ngoại Thất", Slug = SlugHelper.Generate("Tường Ngoại Thất"), CreatedAt = DateTime.Now },
            new AttributeValue { AttributeId = beMatAttr.Id, Value = "Sàn Bê Tông", Slug = SlugHelper.Generate("Sàn Bê Tông"), CreatedAt = DateTime.Now },
            new AttributeValue { AttributeId = beMatAttr.Id, Value = "Mái Tôn", Slug = SlugHelper.Generate("Mái Tôn"), CreatedAt = DateTime.Now },
            new AttributeValue { AttributeId = beMatAttr.Id, Value = "Kim Loại", Slug = SlugHelper.Generate("Kim Loại"), CreatedAt = DateTime.Now },

            // Độ Bóng
            new AttributeValue { AttributeId = doBongAttr.Id, Value = "Bóng", Slug = SlugHelper.Generate("Bóng"), CreatedAt = DateTime.Now },
            new AttributeValue { AttributeId = doBongAttr.Id, Value = "Bán Bóng", Slug = SlugHelper.Generate("Bán Bóng"), CreatedAt = DateTime.Now },
            new AttributeValue { AttributeId = doBongAttr.Id, Value = "Mờ", Slug = SlugHelper.Generate("Mờ"), CreatedAt = DateTime.Now },
            new AttributeValue { AttributeId = doBongAttr.Id, Value = "Siêu Mờ", Slug = SlugHelper.Generate("Siêu Mờ"), CreatedAt = DateTime.Now }
        };

        await _dbContext.AttributeValues.AddRangeAsync(attributeValues);
        await _dbContext.SaveChangesAsync();
    }
}

