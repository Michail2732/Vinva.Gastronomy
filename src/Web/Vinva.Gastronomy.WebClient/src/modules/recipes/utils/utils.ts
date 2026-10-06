export interface IngredientQuantityViewModel {
    quantity: number
    measure: string
}

export interface RecipeIngredientViewModel {
    quantities?: IngredientQuantityViewModel[] | null
}

export function getQuantitiesAsString(ingredient: RecipeIngredientViewModel): string {
    return ingredient.quantities?.map((quantity) => `${quantity.quantity}: ${quantity.measure}`).join('; ') || ''
}

export interface QuantitiesParseResult {
    success: boolean
    result: IngredientQuantityViewModel[] | null | undefined
}

export const isAlphaNumericWithSpaces = (str: string) => {
    const regex = /^[a-zA-Zа-яА-ЯёЁ0-9 ]+$/
    return regex.test(str);
}

export function tryParseQuantitiesString(quantitiesStr: string): QuantitiesParseResult {
    let success = true
    const quantities: IngredientQuantityViewModel[] = []
    const splitedQuantitiesStr = quantitiesStr.split(';')
    for (const quantityStr of splitedQuantitiesStr)
    {
        const splitedQuantityStr = quantityStr.trim().split(':')
        if (splitedQuantityStr.length != 2)
        {
            success = false
            break;
        }            
        const measure = splitedQuantityStr[1]!
        const quantity = Number(splitedQuantityStr[0]!)
        if (!measure || isNaN(quantity))
        {
            success = false
            break;
        }
        
        quantities.push({ measure, quantity })
    }
    return {
        success: success,
        result: quantities
    };
}