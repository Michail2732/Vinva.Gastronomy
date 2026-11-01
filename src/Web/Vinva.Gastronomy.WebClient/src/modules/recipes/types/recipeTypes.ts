import type { JSONContent } from '@tiptap/core'

export type RecipeCharacteristicType = 'String' | 'Number' | 'Range' | 'Array'
export type RecipeCharacteristicState = 'None' | 'New' | 'Change' | 'Remove'
export type RecipeFilterOperator =
	| 'Less'
	| 'LessOrEqual'
	| 'Larger'
	| 'LargerOrEqual'
	| 'Equals'
	| 'NotEquals'
	| 'StartWith'
	| 'EndWith'
	| 'Contains'
export type RecipeFilterLogic = 'Or' | 'And'

export interface RecipeRangeViewModel {
	min: number
	max: number
}

export interface RecipeIngredientQuantityViewModel {
	amount: number
	unit: string
}

export interface RecipeIngredientViewModel {
	id?: string | null
	name: string
	quantities: RecipeIngredientQuantityViewModel[]
}

export type RecipePortionsCalculateType = 'Multiplication' | 'MultiSets'

export interface RecipeIngredientsSetViewModel {
	id?: string | null
	name: string
	portionsQuantities: RecipeIngredientQuantityViewModel[]
	portionsCalculateType: RecipePortionsCalculateType
	ingredients: RecipeIngredientViewModel[]
}

export type RecipeCharacteristicValueViewModel =
	| string
	| number
	| RecipeRangeViewModel
	| string[]

interface RecipeCharacteristicBaseViewModel {
	name: string
	measure?: string | null
	state?: RecipeCharacteristicState
}

export type RecipeCharacteristicViewModel =
	| (RecipeCharacteristicBaseViewModel & { type: 'String'; value: string })
	| (RecipeCharacteristicBaseViewModel & { type: 'Number'; value: number })
	| (RecipeCharacteristicBaseViewModel & { type: 'Range'; value: RecipeRangeViewModel })
	| (RecipeCharacteristicBaseViewModel & { type: 'Array'; value: string[] })

export interface RecipeViewModel {
	id: string
	name: string
	description: string
	baseRecipeId: string | null
	document: JSONContent | null
	characteristics: RecipeCharacteristicViewModel[]
	ingredientsSets: RecipeIngredientsSetViewModel[]
}

export interface RecipeDraftViewModel {
	name: string
	description: string
	baseRecipeId?: string | null
	document: JSONContent
	characteristics: RecipeCharacteristicViewModel[]
	ingredientsSets: RecipeIngredientsSetViewModel[]
}

export interface RecipeCharacteristicFilterViewModel {
	name: string
	type: RecipeCharacteristicType
	operator: RecipeFilterOperator
	value: unknown
	logic?: RecipeFilterLogic
}

export interface RecipeSearchViewModel {
	filters?: RecipeCharacteristicFilterViewModel[] | null
	skip?: number
	take?: number
}

export interface RecipeCreatedViewModel {
	id: string
	name: string
}

export type RecipeCardViewModel = RecipeViewModel
export type RecipeDetailsViewModel = RecipeViewModel

