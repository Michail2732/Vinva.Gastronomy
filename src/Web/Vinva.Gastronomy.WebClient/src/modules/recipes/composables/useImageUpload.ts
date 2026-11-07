import { ref } from 'vue'
import { imageCreateImage, imageGetImagesByIds } from '@/api/gastronomy_generated/sdk.gen'
import { formDataBodySerializer } from '@/api/gastronomy_generated/core/bodySerializer.gen'
import { ApiGastronomyError, type ApiDataResult } from '@/api/types'

// Чуть меньше серверного лимита RequestSizeLimit(10_500_000) с учётом multipart-обёртки
export const IMAGE_UPLOAD_MAX_SIZE = 10 * 1024 * 1024

// Параметры представления, по которым сервер строит URL изображения
const IMAGE_URL_WIDTH = 1200
const IMAGE_URL_HEIGHT = 1200
const IMAGE_URL_QUALITY = 80
const IMAGE_URL_FORMAT = 'webp'

export type UploadedImage = {
    imageId: string
    url: string
}

// Сервер может вернуть относительный URL (шаблон '/images?{0}'): в dev-сборке API
// на другом origin, поэтому резолвим его относительно базового адреса API
function resolveImageUrl(url: string): string {
    try {
        return new URL(url, new URL(import.meta.env.VITE_API_URL, window.location.origin)).href
    } catch {
        return url
    }
}

export function useImageUpload() {
    const uploading = ref(false)

    function validateFile(file: File): string | null {
        if (!file.type.startsWith('image/')) return `Файл «${file.name}» не является изображением`
        if (file.size > IMAGE_UPLOAD_MAX_SIZE) return `Файл «${file.name}» превышает допустимый размер 10 МБ`
        return null
    }

    async function uploadImage(file: File, group?: string): Promise<ApiDataResult<UploadedImage>> {
        const validationError = validateFile(file)
        if (validationError) return { isSuccess: false, error: validationError }

        uploading.value = true
        try {
            const created = await imageCreateImage({
                body: group ? { File: file, Group: group } : { File: file },
                bodySerializer: formDataBodySerializer.bodySerializer,
                // Убираем JSON-заголовок по умолчанию, чтобы браузер выставил multipart/form-data с boundary
                headers: { 'Content-Type': null },
            })
            const imageId = created.data?.imageId
            if (!imageId) return { isSuccess: false, error: 'Сервер не вернул идентификатор изображения' }

            const urls = await imageGetImagesByIds({
                body: {
                    imageDtos: [{
                        imageId,
                        width: IMAGE_URL_WIDTH,
                        height: IMAGE_URL_HEIGHT,
                        quality: IMAGE_URL_QUALITY,
                        format: IMAGE_URL_FORMAT,
                    }],
                },
            })
            const url = urls.data?.items?.[0]?.url
            if (!url) return { isSuccess: false, error: 'Сервер не вернул адрес изображения' }

            return { isSuccess: true, data: { imageId, url: resolveImageUrl(url) } }
        } catch (error) {
            if (error instanceof ApiGastronomyError) return { isSuccess: false, error: error.message }
            throw error
        } finally {
            uploading.value = false
        }
    }

    return { uploading, uploadImage }
}
