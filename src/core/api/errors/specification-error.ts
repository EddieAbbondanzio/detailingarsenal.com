/**
 * Thrown when an entity fails a domain constraint (ex: unique name).
 */
export class SpecificationError extends Error {
    constructor(public isSatisfied: boolean, message: string) {
        super(message);

        Object.setPrototypeOf(this, SpecificationError.prototype);
    }
}
