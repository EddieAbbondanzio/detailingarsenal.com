export class ValidationError extends Error {
    constructor(public valid: boolean, public errors: { field: string; message: string }[]) {
        super(errors[0].message);

        Object.setPrototypeOf(this, ValidationError.prototype);
    }

    getErrorByField(field: string) {
        return this.errors.find(e => e.field == field);
    }
}
