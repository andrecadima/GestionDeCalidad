using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Xunit;
using EstablishmentModel = Establishment.Dom.Model.Establishment;

namespace Establishment.Tests.Model;

public class EstablishmentValidationTests
{
	private static List<ValidationResult> ValidateModel(EstablishmentModel model)
	{
		var context = new ValidationContext(model);
		var results = new List<ValidationResult>();
		Validator.TryValidateObject(model, context, results, true);
		return results;
	}

	private static EstablishmentModel CreateValidModel()
	{
		return new EstablishmentModel
		{
			Id = 1,
			Name = "Farmacia Central",
			TaxId = "1234567",
			SanitaryLicense = "LIC-12345",
			SanitaryLicenseExpiry = DateTime.Today.AddDays(30),
			Address = "Av. Principal #123",
			Phone = "71234567",
			Email = "test@email.com",
			EstablishmentType = "Farmacia",
			PersonInChargeId = 1,
			CreatedDate = DateTime.Now,
			LastUpdate = DateTime.Now,
			Status = true,
			CreatedBy = 1
		};
	}

	[Fact]
	public void Model_Should_Be_Valid_When_Data_Is_Correct()
	{
		var model = CreateValidModel();
		var results = ValidateModel(model);
		Assert.Empty(results);
	}

	[Fact]
	public void Should_Fail_When_Name_Is_Null()
	{
		var model = CreateValidModel();
		model.Name = null!;
		var results = ValidateModel(model);
		Assert.Contains(results, r => r.ErrorMessage == "El nombre es obligatorio.");
	}

	[Fact]
	public void Should_Fail_When_Name_Is_Too_Short()
	{
		var model = CreateValidModel();
		model.Name = "AB";
		var results = ValidateModel(model);
		Assert.Contains(results, r => r.ErrorMessage == "El nombre debe tener entre 3 y 100 caracteres.");
	}

	[Fact]
	public void Should_Fail_When_Name_Has_Invalid_Characters()
	{
		var model = CreateValidModel();
		model.Name = "Farmacia@123";
		var results = ValidateModel(model);
		Assert.Contains(results, r => r.ErrorMessage == "El nombre solo puede contener letras, números, espacios y puntos.");
	}

	[Fact]
	public void Should_Fail_When_TaxId_Is_Null()
	{
		var model = CreateValidModel();
		model.TaxId = null!;
		var results = ValidateModel(model);
		Assert.Contains(results, r => r.ErrorMessage == "El NIT es obligatorio.");
	}

	[Fact]
	public void Should_Fail_When_TaxId_Has_Invalid_Format()
	{
		var model = CreateValidModel();
		model.TaxId = "ABC123";
		var results = ValidateModel(model);
		Assert.Contains(results, r => r.ErrorMessage == "El NIT debe contener entre 7 y 10 dígitos numéricos.");
	}

	[Fact]
	public void Should_Fail_When_SanitaryLicense_Is_Null()
	{
		var model = CreateValidModel();
		model.SanitaryLicense = null!;
		var results = ValidateModel(model);
		Assert.Contains(results, r => r.ErrorMessage == "La licencia sanitaria es obligatoria.");
	}

	[Fact]
	public void Should_Fail_When_SanitaryLicense_Has_Invalid_Format()
	{
		var model = CreateValidModel();
		model.SanitaryLicense = "LIC 123";
		var results = ValidateModel(model);
		Assert.Contains(results, r => r.ErrorMessage == "La licencia sanitaria solo puede contener letras, números y guiones.");
	}

	[Fact]
	public void Should_Fail_When_SanitaryLicenseExpiry_Is_Null()
	{
		var model = CreateValidModel();
		model.SanitaryLicenseExpiry = null;
		var results = ValidateModel(model);
		Assert.Contains(results, r => r.ErrorMessage == "La fecha de vencimiento de la licencia sanitaria es obligatoria.");
	}

	[Fact]
	public void Should_Fail_When_Address_Is_Null()
	{
		var model = CreateValidModel();
		model.Address = null!;
		var results = ValidateModel(model);
		Assert.Contains(results, r => r.ErrorMessage == "La dirección es obligatoria.");
	}

	[Fact]
	public void Should_Fail_When_Address_Is_Too_Short()
	{
		var model = CreateValidModel();
		model.Address = "Calle 1";
		var results = ValidateModel(model);
		Assert.Contains(results, r => r.ErrorMessage == "La dirección debe tener entre 10 y 200 caracteres.");
	}

	[Fact]
	public void Should_Fail_When_Phone_Is_Null()
	{
		var model = CreateValidModel();
		model.Phone = null!;
		var results = ValidateModel(model);
		Assert.Contains(results, r => r.ErrorMessage == "El teléfono es obligatorio.");
	}

	[Fact]
	public void Should_Fail_When_Phone_Has_Invalid_Format()
	{
		var model = CreateValidModel();
		model.Phone = "123ABC";
		var results = ValidateModel(model);
		Assert.Contains(results, r => r.ErrorMessage == "El teléfono solo puede contener números.");
	}

	[Fact]
	public void Should_Fail_When_Email_Is_Null()
	{
		var model = CreateValidModel();
		model.Email = null!;
		var results = ValidateModel(model);
		Assert.Contains(results, r => r.ErrorMessage == "El correo electrónico es obligatorio.");
	}

	[Fact]
	public void Should_Fail_When_Email_Is_Invalid()
	{
		var model = CreateValidModel();
		model.Email = "correo-invalido";
		var results = ValidateModel(model);
		Assert.NotEmpty(results);
	}

	[Fact]
	public void Should_Fail_When_EstablishmentType_Is_Null()
	{
		var model = CreateValidModel();
		model.EstablishmentType = null!;
		var results = ValidateModel(model);
		Assert.Contains(results, r => r.ErrorMessage == "El tipo de establecimiento es obligatorio");
	}
}