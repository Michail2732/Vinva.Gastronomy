using MediatR;

namespace Vinva.Gastronomy.Recipes.Application.Usecases.SearchByCharacteristics
{
    public sealed record SearchRecipesByCharacteristicsQuery : IRequest<SearchRecipesByCharacteristicsResponse>
    {
        public IReadOnlyList<CharacteristicFilterDto> Filters { get; init; } = [];
        public long Skip { get; init; }
        public int Take { get; init; }
    }
}
