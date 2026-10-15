export function titleColorAttribute() {
    return {
        titleColor: {
            default: null as string | null,
            parseHTML: (element: HTMLElement) => element.getAttribute('data-title-color'),
            renderHTML: (attributes: { titleColor?: string | null }) => attributes.titleColor ? { 'data-title-color': attributes.titleColor } : {},
        },
    }
}

export function borderWidthAttribute(side: 'top' | 'right' | 'bottom' | 'left', defaultWidth: number) {
    const field = `border${side[0]!.toUpperCase()}${side.slice(1)}Width`
    const dataField = `data-border-${side}-width`
    return {
        default: defaultWidth,
        parseHTML: (element: HTMLElement) => Number(element.getAttribute(dataField) ?? defaultWidth),
        renderHTML: (attributes: Record<string, number>) => ({ [dataField]: String(attributes[field] ?? defaultWidth) }),
    }
}

export function cellBorderWidthAttribute(side: 'top' | 'right' | 'bottom' | 'left') {
    const field = `border${side[0]!.toUpperCase()}${side.slice(1)}Width`
    return {
        default: null as number | null,
        parseHTML: (element: HTMLElement) => {
            const raw = element.style.getPropertyValue(`--recipe-cell-border-${side}`) || element.style[`border${side[0]!.toUpperCase()}${side.slice(1)}Width` as 'borderTopWidth']
            const parsed = Number.parseFloat(raw)
            return Number.isFinite(parsed) ? parsed : null
        },
        renderHTML: (attributes: Record<string, number | null>) => {
            const width = Math.min(20, Math.max(0, Number(attributes[field]) || 0))
            return width > 0
                ? { class: 'has-cell-border', style: `--recipe-cell-border-${side}: ${width}px;` }
                : {}
        },
    }
}
