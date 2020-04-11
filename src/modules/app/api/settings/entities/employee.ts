import { Entity } from '@/core';

/**
 * Employee of a business that can perform services.
 */
export class Employee extends Entity {
    public static NAME_MAX_LENGTH = 64;
    public static POSITION_MAX_LENGTH = 32;

    /**
     * Create a new employee entity.
     * @param name The employee's full name.
     * @param position Their job title.
     */
    constructor(public name: string, public position: string = '') {
        super();

        if (name.length > Employee.NAME_MAX_LENGTH) {
            throw new RangeError('name');
        }

        if (position.length > Employee.POSITION_MAX_LENGTH) {
            throw new RangeError('position');
        }
    }
}
