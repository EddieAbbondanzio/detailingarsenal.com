/**
 * Generate a full sized image url for the image id passed in.
 * @param imageId Id of the image.
 */
export function imageUrl(imageId: string | null): string | null {
    return imageId != null ? `${process.env.VUE_APP_API_DOMAIN}/images/${imageId}` : null;
}

/**
 * Generate a thumbnail url for the image id passed in.
 * @param imageId Id of the image.
 */
export function thumbnailUrl(imageId: string | null) {
    return imageId != null ? `${process.env.VUE_APP_API_DOMAIN}/images/${imageId}/thumbnail` : null;
}
