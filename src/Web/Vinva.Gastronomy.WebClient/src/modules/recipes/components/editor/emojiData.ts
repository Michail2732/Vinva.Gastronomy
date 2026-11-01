export interface EmojiItem {
    emoji: string
    label: string
    keywords: string[]
}

export const emojiGroups: { label: string; items: EmojiItem[] }[] = [
    {
        label: 'Овощи и зелень',
        items: [
            { emoji: '🥕', label: 'Морковь', keywords: ['морковь', 'carrot'] },
            { emoji: '🥔', label: 'Картофель', keywords: ['картофель', 'potato'] },
            { emoji: '🍅', label: 'Томат', keywords: ['томат', 'помидор', 'tomato'] },
            { emoji: '🥒', label: 'Огурец', keywords: ['огурец', 'cucumber'] },
            { emoji: '🧅', label: 'Лук', keywords: ['лук', 'onion'] },
            { emoji: '🧄', label: 'Чеснок', keywords: ['чеснок', 'garlic'] },
            { emoji: '🌶️', label: 'Перец острый', keywords: ['перец', 'чили', 'pepper', 'chili'] },
            { emoji: '🫑', label: 'Перец болгарский', keywords: ['перец', 'болгарский', 'bell pepper'] },
            { emoji: '🥦', label: 'Брокколи', keywords: ['брокколи', 'broccoli'] },
            { emoji: '🥬', label: 'Листовой салат', keywords: ['салат', 'зелень', 'leafy'] },
            { emoji: '🌽', label: 'Кукуруза', keywords: ['кукуруза', 'corn'] },
            { emoji: '🍆', label: 'Баклажан', keywords: ['баклажан', 'eggplant'] },
            { emoji: '🎃', label: 'Тыква', keywords: ['тыква', 'pumpkin'] },
            { emoji: '🍄', label: 'Грибы', keywords: ['грибы', 'mushroom'] },
            { emoji: '🌿', label: 'Зелень', keywords: ['зелень', 'травы', 'herb'] },
            { emoji: '🌱', label: 'Росток', keywords: ['росток', 'зелень', 'sprout'] },
        ],
    },
    {
        label: 'Фрукты и ягоды',
        items: [
            { emoji: '🍎', label: 'Яблоко', keywords: ['яблоко', 'apple'] },
            { emoji: '🍐', label: 'Груша', keywords: ['груша', 'pear'] },
            { emoji: '🍊', label: 'Мандарин', keywords: ['мандарин', 'апельсин', 'orange'] },
            { emoji: '🍋', label: 'Лимон', keywords: ['лимон', 'lemon'] },
            { emoji: '🍌', label: 'Банан', keywords: ['банан', 'banana'] },
            { emoji: '🍉', label: 'Арбуз', keywords: ['арбуз', 'watermelon'] },
            { emoji: '🍇', label: 'Виноград', keywords: ['виноград', 'grapes'] },
            { emoji: '🍓', label: 'Клубника', keywords: ['клубника', 'strawberry'] },
            { emoji: '🫐', label: 'Черника', keywords: ['черника', 'ягоды', 'blueberry'] },
            { emoji: '🍒', label: 'Вишня', keywords: ['вишня', 'cherry'] },
            { emoji: '🍑', label: 'Персик', keywords: ['персик', 'peach'] },
            { emoji: '🥭', label: 'Манго', keywords: ['манго', 'mango'] },
            { emoji: '🍍', label: 'Ананас', keywords: ['ананас', 'pineapple'] },
            { emoji: '🥝', label: 'Киви', keywords: ['киви', 'kiwi'] },
            { emoji: '🥥', label: 'Кокос', keywords: ['кокос', 'coconut'] },
            { emoji: '🥑', label: 'Авокадо', keywords: ['авокадо', 'avocado'] },
        ],
    },
    {
        label: 'Мясо, рыба, молочное',
        items: [
            { emoji: '🥩', label: 'Мясо', keywords: ['мясо', 'стейк', 'meat', 'steak'] },
            { emoji: '🍗', label: 'Курица', keywords: ['курица', 'chicken'] },
            { emoji: '🥓', label: 'Бекон', keywords: ['бекон', 'bacon'] },
            { emoji: '🍖', label: 'Мясо на кости', keywords: ['мясо', 'ребра', 'meat'] },
            { emoji: '🐟', label: 'Рыба', keywords: ['рыба', 'fish'] },
            { emoji: '🦐', label: 'Креветки', keywords: ['креветки', 'shrimp'] },
            { emoji: '🦑', label: 'Кальмар', keywords: ['кальмар', 'squid'] },
            { emoji: '🥚', label: 'Яйцо', keywords: ['яйцо', 'egg'] },
            { emoji: '🧀', label: 'Сыр', keywords: ['сыр', 'cheese'] },
            { emoji: '🥛', label: 'Молоко', keywords: ['молоко', 'milk'] },
            { emoji: '🧈', label: 'Масло сливочное', keywords: ['масло', 'butter'] },
            { emoji: '🍦', label: 'Мороженое', keywords: ['мороженое', 'ice cream'] },
        ],
    },
    {
        label: 'Крупы, хлеб, паста',
        items: [
            { emoji: '🍞', label: 'Хлеб', keywords: ['хлеб', 'bread'] },
            { emoji: '🥖', label: 'Багет', keywords: ['багет', 'хлеб', 'baguette'] },
            { emoji: '🥐', label: 'Круассан', keywords: ['круассан', 'croissant'] },
            { emoji: '🥨', label: 'Крендель', keywords: ['крендель', 'pretzel'] },
            { emoji: '🍚', label: 'Рис', keywords: ['рис', 'rice'] },
            { emoji: '🍝', label: 'Паста', keywords: ['паста', 'спагетти', 'pasta'] },
            { emoji: '🍜', label: 'Лапша', keywords: ['лапша', 'рамен', 'noodles'] },
            { emoji: '🥣', label: 'Миска', keywords: ['миска', 'каша', 'bowl'] },
            { emoji: '🌾', label: 'Зерно', keywords: ['зерно', 'крупа', 'grain'] },
        ],
    },
    {
        label: 'Блюда',
        items: [
            { emoji: '🍲', label: 'Суп', keywords: ['суп', 'бульон', 'soup'] },
            { emoji: '🥘', label: 'Паэлья', keywords: ['паэлья', 'рагу', 'paella'] },
            { emoji: '🍳', label: 'Яичница', keywords: ['яичница', 'жарка', 'fried'] },
            { emoji: '🥗', label: 'Салат', keywords: ['салат', 'salad'] },
            { emoji: '🍕', label: 'Пицца', keywords: ['пицца', 'pizza'] },
            { emoji: '🌮', label: 'Тако', keywords: ['тако', 'taco'] },
            { emoji: '🥪', label: 'Сэндвич', keywords: ['сэндвич', 'sandwich'] },
            { emoji: '🍔', label: 'Бургер', keywords: ['бургер', 'burger'] },
            { emoji: '🍛', label: 'Карри', keywords: ['карри', 'curry'] },
            { emoji: '🍣', label: 'Суши', keywords: ['суши', 'sushi'] },
            { emoji: '🥟', label: 'Пельмени', keywords: ['пельмени', 'вареники', 'dumpling'] },
            { emoji: '🍤', label: 'Темпура', keywords: ['темпура', 'креветка', 'tempura'] },
        ],
    },
    {
        label: 'Десерты и сладости',
        items: [
            { emoji: '🍰', label: 'Торт', keywords: ['торт', 'cake'] },
            { emoji: '🧁', label: 'Капкейк', keywords: ['капкейк', 'cupcake'] },
            { emoji: '🍪', label: 'Печенье', keywords: ['печенье', 'cookie'] },
            { emoji: '🍩', label: 'Пончик', keywords: ['пончик', 'donut'] },
            { emoji: '🍫', label: 'Шоколад', keywords: ['шоколад', 'chocolate'] },
            { emoji: '🍬', label: 'Конфета', keywords: ['конфета', 'candy'] },
            { emoji: '🍯', label: 'Мёд', keywords: ['мёд', 'honey'] },
            { emoji: '🥞', label: 'Блины', keywords: ['блины', 'панкейки', 'pancakes'] },
            { emoji: '🍮', label: 'Пудинг', keywords: ['пудинг', 'крем', 'custard'] },
        ],
    },
    {
        label: 'Напитки',
        items: [
            { emoji: '☕', label: 'Кофе', keywords: ['кофе', 'coffee'] },
            { emoji: '🍵', label: 'Чай', keywords: ['чай', 'tea'] },
            { emoji: '🥃', label: 'Виски', keywords: ['виски', 'whiskey'] },
            { emoji: '🍷', label: 'Вино', keywords: ['вино', 'wine'] },
            { emoji: '🍺', label: 'Пиво', keywords: ['пиво', 'beer'] },
            { emoji: '🥤', label: 'Смузи', keywords: ['смузи', 'напиток', 'smoothie'] },
            { emoji: '🧃', label: 'Сок', keywords: ['сок', 'juice'] },
            { emoji: '🍸', label: 'Коктейль', keywords: ['коктейль', 'cocktail'] },
        ],
    },
    {
        label: 'Кухня и процесс',
        items: [
            { emoji: '🍴', label: 'Столовые приборы', keywords: ['вилка', 'нож', 'приборы', 'cutlery'] },
            { emoji: '🔪', label: 'Нож', keywords: ['нож', 'knife'] },
            { emoji: '🥄', label: 'Ложка', keywords: ['ложка', 'spoon'] },
            { emoji: '🍽️', label: 'Тарелка', keywords: ['тарелка', 'подача', 'plate'] },
            { emoji: '⏲️', label: 'Таймер', keywords: ['таймер', 'время', 'timer'] },
            { emoji: '🔥', label: 'Огонь', keywords: ['огонь', 'жарка', 'fire'] },
            { emoji: '❄️', label: 'Заморозка', keywords: ['холод', 'заморозка', 'freeze'] },
            { emoji: '⭐', label: 'Звезда', keywords: ['звезда', 'любимое', 'star'] },
            { emoji: '❤️', label: 'Сердце', keywords: ['сердце', 'любимое', 'heart'] },
            { emoji: '✅', label: 'Готово', keywords: ['готово', 'проверено', 'done'] },
            { emoji: '💡', label: 'Совет', keywords: ['совет', 'идея', 'tip'] },
            { emoji: '⚠️', label: 'Внимание', keywords: ['внимание', 'важно', 'warning'] },
        ],
    },
]

export const allEmojis: EmojiItem[] = emojiGroups.flatMap((group) => group.items)

export function searchEmojis(query: string): EmojiItem[] {
    const normalized = query.trim().toLocaleLowerCase('ru')
    if (!normalized) return allEmojis
    return allEmojis.filter((item) =>
        item.label.toLocaleLowerCase('ru').includes(normalized)
        || item.keywords.some((keyword) => keyword.includes(normalized)))
}
