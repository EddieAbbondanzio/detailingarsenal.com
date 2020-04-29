import { MouseButton } from '@/core/directives/mouse/mouse-button';
import { MouseObjectSubscriber } from '@/core/directives/mouse/mouse-object-subscriber';
import { MouseActionFunction } from '@/core/directives/mouse/mouse-action-function';
import { MouseAction } from '@/core/directives/mouse/mouse-action';

/**
 * How many milliseconds before trigger a hold condition.
 */
export const HOLD_MIN = 250;

export class MouseObject {
    get subscriberCount() {
        return this.subscribers.length;
    }

    element: HTMLElement;
    mouseDown: boolean = false;
    holding: boolean = false;

    private subscribers: MouseObjectSubscriber[];

    constructor(element: HTMLElement) {
        this.element = element;
        this.subscribers = [];

        element.addEventListener('mousedown', onMouseDown);
        element.addEventListener('click', onMouseUp);
        element.addEventListener('mouseout', onMouseUp);
        element.addEventListener('mousemove', onMouseMove);
    }

    dispose() {
        this.element.removeEventListener('mousedown', onMouseDown);
        this.element.removeEventListener('click', onMouseUp);
        this.element.removeEventListener('mouseout', onMouseUp);
        this.element.removeEventListener('mousemove', onMouseMove);
    }

    notify(action: MouseAction, button: MouseButton, event: MouseEvent) {
        for (let i = 0; i < this.subscribers.length; i++) {
            if (
                action == this.subscribers[i].action &&
                (button == this.subscribers[i].button || this.subscribers[i].button == 'either')
            ) {
                this.subscribers[i].callback(this.element, event);
            }
        }
    }

    subscribe(action: MouseAction, callback: MouseActionFunction, button: MouseButton = 'either') {
        if (this.subscribers.some(s => s.action == action && s.callback == callback && s.button == button)) {
            throw new Error('Duplicate subscriber');
        }

        this.subscribers.push(new MouseObjectSubscriber(action, callback, button));
    }

    desubscribe(action: MouseAction, callback: MouseActionFunction, button: MouseButton) {
        this.subscribers = this.subscribers.filter(
            s => s.action == action && s.callback == callback && s.button == button
        );
    }
}

/**
 * Mouse button was pressed event handler.
 * @param this HTMLElement event is on.
 * @param event MouseEvent details
 */
function onMouseDown(this: any, event: globalThis.MouseEvent) {
    event.stopImmediatePropagation();

    if (this.timer == null) {
        const mouseObject = this.mouseObject as MouseObject;
        mouseObject.mouseDown = true;

        this.timer = setTimeout(() => {
            mouseObject.holding = true;

            const button = getButton(event.button);
            mouseObject.notify('hold', button, event);
        }, HOLD_MIN);
    }
}

/**
 * Mouse is dragging an element.
 * @param this HTMLElement event is on.
 * @param event MouseEvent details
 */
function onMouseMove(this: any, event: globalThis.MouseEvent) {
    event.stopImmediatePropagation();

    const mouseObject = this.mouseObject as MouseObject;

    if (!mouseObject.mouseDown) {
        return;
    }

    const button = getButton(event.button);

    // Trigger the hold event, as soon as a drag starts
    if (!mouseObject.holding) {
        mouseObject.holding = true;
        mouseObject.notify('hold', button, event);
    }

    mouseObject.notify('drag', button, event);
}

/**
 * Mouse button was released event handler.
 * @param this HTMLElement event occured on.
 * @param event MouseEvent details
 */
function onMouseUp(this: any, event: globalThis.MouseEvent) {
    event.stopImmediatePropagation();

    if (this.timer != null) {
        const button = getButton(event.button);
        const mouseObject = this.mouseObject as MouseObject;

        if (!mouseObject.holding) {
            mouseObject.notify('click', button, event);
        } else {
            mouseObject.notify('release', button, event);
        }

        clearTimeout(this.timer);
        this.timer = null;
        mouseObject.holding = false;
        mouseObject.mouseDown = false;
    }
}

function getButton(index: number): MouseButton {
    if (index == 0) {
        return 'left';
    } else if (index == 2) {
        return 'right';
    } else {
        return 'either';
    }
}
