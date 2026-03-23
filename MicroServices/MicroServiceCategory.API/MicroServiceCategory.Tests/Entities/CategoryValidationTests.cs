using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Xunit;
using MicroServiceCategory.Domain.Entities;

namespace MicroServiceCategory.Tests.Entities;

public class CategoryValidationTests
{
    private static List<ValidationResult> ValidateModel(Category model)
    {
        var context = new ValidationContext(model);
        var results = new List<ValidationResult>();
        Validator.TryValidateObject(model, context, results, true);
        return results;
    }

    private static Category CreateValidCategory()
    {
        return new Category
        {
            Id = 1,
            Name = "Bebidas",
            Description = "Categoria general",
            BaseAmount = 100,
            CreatedBy = 1
        };
    }

    [Fact]
    public void Category_Should_Be_Valid_When_All_Fields_Are_Correct()
    {
        var model = CreateValidCategory();

        var results = ValidateModel(model);

        Assert.Empty(results);
    }

    [Fact]
    public void Category_Should_Fail_When_Name_Is_Null()
    {
        var model = CreateValidCategory();
        model.Name = null!;

        var results = ValidateModel(model);

        Assert.Contains(results, r => r.ErrorMessage == "El nombre es obligatorio.");
    }

    [Fact]
    public void Category_Should_Fail_When_Name_Is_Too_Short()
    {
        var model = CreateValidCategory();
        model.Name = "Ab";

        var results = ValidateModel(model);

        Assert.Contains(results, r => r.ErrorMessage == "El nombre puede tener entre 3 a 50 caracteres.");
    }

    [Fact]
    public void Category_Should_Fail_When_Name_Has_Invalid_Characters()
    {
        var model = CreateValidCategory();
        model.Name = "Bebidas123";

        var results = ValidateModel(model);

        Assert.Contains(results, r => r.ErrorMessage == "Solo se permiten letras, espacios, puntos y comas.");
    }

    [Fact]
    public void Category_Should_Fail_When_Description_Is_Null()
    {
        var model = CreateValidCategory();
        model.Description = null!;

        var results = ValidateModel(model);

        Assert.Contains(results, r => r.ErrorMessage == "La descripción es obligatorio.");
    }

    [Fact]
    public void Category_Should_Fail_When_Description_Is_Too_Short()
    {
        var model = CreateValidCategory();
        model.Description = "Ab";

        var results = ValidateModel(model);

        Assert.Contains(results, r => r.ErrorMessage == "El nombre puede tener entre 10 a 120 caracteres.");
    }

    [Fact]
    public void Category_Should_Fail_When_Description_Has_Invalid_Characters()
    {
        var model = CreateValidCategory();
        model.Description = "Desc123";

        var results = ValidateModel(model);

        Assert.Contains(results, r => r.ErrorMessage == "Solo se permiten letras, espacios, puntos y comas.");
    }

    [Fact]
    public void Category_Should_Fail_When_BaseAmount_Is_Negative()
    {
        var model = CreateValidCategory();
        model.BaseAmount = -1;

        var results = ValidateModel(model);

        Assert.Contains(results, r => r.ErrorMessage == "El monto base debe ser mayor a 0.");
    }

    [Fact]
    public void Category_Should_Fail_When_BaseAmount_Exceeds_Maximum()
    {
        var model = CreateValidCategory();
        model.BaseAmount = 100001;

        var results = ValidateModel(model);

        Assert.Contains(results, r => r.ErrorMessage == "El monto base debe ser mayor a 0.");
    }
}