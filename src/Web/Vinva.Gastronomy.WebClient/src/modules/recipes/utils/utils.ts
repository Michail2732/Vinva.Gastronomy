import type { RecipeIngredientDto, IngredientQuantityDto} from "@/api/gastronomy_generated";


export function getQuantitiesAsString(ingredient: RecipeIngredientDto) : string
{
    
    return ingredient.quantities?.map(a => `${a.quantity}: ${a.measure}`).join('; ') || '';    
}

export interface QuantitiesParseResult 
{
    success: boolean;
    result: Array<IngredientQuantityDto> | null | undefined;
}

export const isAlphaNumericWithSpaces = (str: string) =>
{
    const regex = /^[a-zA-Zа-яА-ЯёЁ0-9 ]+$/
    return regex.test(str);
}

export function tryParseQuantitiesString(quantitiesStr: string) : QuantitiesParseResult 
{
    let success = true;
    const quantities :IngredientQuantityDto[] = [];
    const splitedQuantitiesStr = quantitiesStr.split(';')
    for (const quantityStr of splitedQuantitiesStr)
    {
        const splitedQuantityStr = quantityStr.trim().split(':')
        if (splitedQuantityStr.length != 2)
        {
            success = false;
            break;
        }            
        const measure = splitedQuantityStr[1]!
        const quantity = Number(splitedQuantityStr[0]!)
        if (!measure || isNaN(quantity))
        {
            success = false;
            break;
        }
        
        quantities.push(
            {
                measure: measure,
                quantity: quantity
            }
        );
    }
    return {
        success: success,
        result: quantities
    };
}