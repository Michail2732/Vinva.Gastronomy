import {
	CharacteristicType,
	DtoState,
	Logic,
	Operator,
	type CharacteristicDto,
	type CreateRecipeCommand,
	type RecipeDto,
	type RecipeIngredientDto,
	type RecipeIngredientQuantityDto,
	type SearchRecipesByCharacteristicsQuery,
	type UpdateCommand,
} from '@/api/gastronomy_generated'
import type {
	RecipeCharacteristicState,
	RecipeCharacteristicType,
	RecipeCharacteristicViewModel,
	RecipeCreatedViewModel,
	RecipeDraftViewModel,
	RecipeIngredientViewModel,
	RecipeFilterLogic,
	RecipeFilterOperator,
	RecipeSearchViewModel,
	RecipeViewModel,
} from './recipeTypes'
import type { JSONContent } from '@tiptap/core'

const zeroGuid = '00000000-0000-0000-0000-000000000000'

function readValue(value: unknown, ...names: string[]): unknown {
	if (!value || typeof value !== 'object') return undefined
	const record = value as Record<string, unknown>
	for (const name of names) {
		const match = Object.keys(record).find((key) => key.toLowerCase() === name.toLowerCase())
		if (match) return record[match]
	}
	return undefined
}

function parseDocument(document?: string | null): JSONContent | null {
	if (!document) return null
	try {
		const parsed: unknown = JSON.parse(document)
		if (parsed && typeof parsed === 'object' && (parsed as { type?: unknown }).type === 'doc')
			return parsed as JSONContent
	} catch {
		return null
	}
	return null
}

function mapCharacteristic(dto: CharacteristicDto): RecipeCharacteristicViewModel {
	const value = dto.value as unknown
	const base = {
		name: dto.name ?? '',
		measure: dto.measure,
	}

	switch (dto.type) {
		case CharacteristicType.STRING:
			return { ...base, type: 'String', value: String(readValue(value, 'Data') ?? '') }
		case CharacteristicType.NUMBER:
			return { ...base, type: 'Number', value: Number(readValue(value, 'Data') ?? 0) }
		case CharacteristicType.RANGE:
			return {
				...base,
				type: 'Range',
				value: {
					min: Number(readValue(value, 'Min') ?? 0),
					max: Number(readValue(value, 'Max') ?? 0),
				},
			}
		case CharacteristicType.ARRAY: {
			const items = readValue(value, 'Items')
			return { ...base, type: 'Array', value: Array.isArray(items) ? items.map(String) : [] }
		}
	}
}

export function mapRecipeDto(dto: RecipeDto): RecipeViewModel {
	return {
		id: dto.id,
		name: dto.name ?? '',
		description: dto.description ?? '',
		baseRecipeId: dto.baseRecipe ?? null,
		document: parseDocument(dto.document),
		characteristics: (dto.characteristics ?? []).map(mapCharacteristic),
		ingredients: (dto.ingredients ?? []).map(mapIngredient),
	}
}

function mapIngredient(dto: RecipeIngredientDto): RecipeIngredientViewModel {
	return {
		name: dto.name,
		part: dto.part ?? null,
		quantities: (dto.quantities ?? []).map((quantity) => ({
			amount: quantity.amount,
			unit: quantity.unit,
		})),
	}
}

function mapCharacteristicToDto(
	characteristic: RecipeDraftViewModel['characteristics'][number],
	defaultState: RecipeCharacteristicState,
): CharacteristicDto {
	let type: CharacteristicType
	let value: Record<string, unknown>

	switch (characteristic.type) {
		case 'String':
			type = CharacteristicType.STRING
			value = { Data: characteristic.value }
			break
		case 'Number':
			type = CharacteristicType.NUMBER
			value = { Data: characteristic.value }
			break
		case 'Range':
			type = CharacteristicType.RANGE
			value = {
				Min: (characteristic.value as { min: number }).min,
				Max: (characteristic.value as { max: number }).max,
			}
			break
		case 'Array':
			type = CharacteristicType.ARRAY
			value = { Items: characteristic.value }
			break
	}

	return {
		recipeId: zeroGuid,
		name: characteristic.name,
		type,
		value: value as CharacteristicDto['value'],
		measure: characteristic.measure,
		state: mapState(characteristic.state ?? defaultState),
	}
}

function mapState(state: RecipeCharacteristicState): DtoState {
	switch (state) {
		case 'New': return DtoState.NEW
		case 'Change': return DtoState.CHANGE
		case 'Remove': return DtoState.REMOVE
		case 'None': return DtoState.NONE
	}
}

export function mapRecipeDraftToCreateCommand(draft: RecipeDraftViewModel): CreateRecipeCommand {
	return {
		name: draft.name,
		description: draft.description,
		baseRecipe: draft.baseRecipeId,
		document: JSON.stringify(draft.document),
		characteristics: draft.characteristics.map((characteristic) => mapCharacteristicToDto(characteristic, 'New')),
		ingredients: draft.ingredients.map((ingredient) => ({
			name: ingredient.name,
			part: ingredient.part,
			quantities: ingredient.quantities,
		})),
	}
}

export function mapRecipeDraftToUpdateCommand(id: string, draft: RecipeDraftViewModel): UpdateCommand {
	return {
		id,
		name: draft.name,
		description: draft.description,
		baseRecipe: draft.baseRecipeId,
		document: JSON.stringify(draft.document),
		characteristics: draft.characteristics.map((characteristic) => mapCharacteristicToDto(characteristic, 'Change')),
		ingredients: draft.ingredients.map((ingredient) => ({
			name: ingredient.name,
			part: ingredient.part,
			quantities: ingredient.quantities,
		})),
	}
}

const apiOperators: Record<RecipeFilterOperator, Operator> = {
	Less: Operator.LESS,
	LessOrEqual: Operator.LESS_OR_EQUAL,
	Larger: Operator.LARGER,
	LargerOrEqual: Operator.LARGER_OR_EQUAL,
	Equals: Operator.EQUALS,
	NotEquals: Operator.NOT_EQUALS,
	StartWith: Operator.START_WITH,
	EndWith: Operator.END_WITH,
	Contains: Operator.CONTAINS,
}

const apiLogic: Record<RecipeFilterLogic, Logic> = {
	Or: Logic.OR,
	And: Logic.AND,
}

const apiCharacteristicTypes: Record<RecipeCharacteristicType, CharacteristicType> = {
	String: CharacteristicType.STRING,
	Number: CharacteristicType.NUMBER,
	Range: CharacteristicType.RANGE,
	Array: CharacteristicType.ARRAY,
}

export function mapRecipeSearchToQuery(search: RecipeSearchViewModel): SearchRecipesByCharacteristicsQuery {
	const filters = search.filters == null
		? search.filters
		: search.filters.map((filter) => ({
			name: filter.name,
			type: apiCharacteristicTypes[filter.type],
			operator: apiOperators[filter.operator],
			value: filter.value,
			logic: filter.logic ? apiLogic[filter.logic] : undefined,
		}))

	return {
		skip: search.skip,
		take: search.take,
		filters,
	}
}

export function mapCreatedRecipeToViewModel(id: string, name: string | null): RecipeCreatedViewModel {
	return { id, name: name ?? '' }
}

