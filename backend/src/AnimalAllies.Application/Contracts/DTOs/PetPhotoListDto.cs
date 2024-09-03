using System.Collections;
using AnimalAllies.Application.Contracts.DTOs.ValueObjects;

namespace AnimalAllies.Application.Contracts.DTOs;

public record PetPhotoListDto(IEnumerable<PetPhotoDto> PetPhotoDtos);