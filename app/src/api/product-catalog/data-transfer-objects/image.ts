export class Image {
    /**
     * Image of a product.
     * @param name The filename of the image.
     * @param data The dataURL (base64 encoded)
     */
    constructor(public name: string, public data: string) {}
}
