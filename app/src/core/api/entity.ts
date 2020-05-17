export abstract class Entity {
    public id: string = '';

    /**
     * Clone the entity into a duplicate
     */
    public clone(): any {
        return Object.assign({}, this);
    }
}
