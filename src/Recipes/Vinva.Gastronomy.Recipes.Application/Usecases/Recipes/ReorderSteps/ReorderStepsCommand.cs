using MediatR;


namespace Vinva.Gastronomy.Recipes.Application.Usecases.Recipes.ReorderSteps
{    
    public readonly record struct ReorderStepsCommand : IRequest
    {
        public Guid RecipeId { get; init; }
        public ChangeSeqNumberDto[] Items { get; init; }
    }

    public readonly record struct ChangeSeqNumberDto
    {
        public int SeqNumber1 { get; init; }
        public int SeqNumber2 { get; init; }
    }
}