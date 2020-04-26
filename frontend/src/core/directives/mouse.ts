import Vue, { DirectiveFunction, VNode } from 'vue';
import { DirectiveOptions } from 'vue/types/umd';
import { DirectiveBinding } from 'vue/types/options';

export const HOLD_MIN = 250;

export const mouse: DirectiveOptions = {
    bind: function(el: any, binding: DirectiveBinding, vnode: VNode) {
        el.button = getButtonCode(binding.modifiers);
        el.callbacks = getCallbacks(binding.value);

        el.addEventListener('mousedown', onMouseDown);
        el.addEventListener('click', onMouseUp);
        el.addEventListener('mouseout', onMouseUp);
    },
    unbind: function(el: HTMLElement, binding: DirectiveBinding) {
        el.removeEventListener('mousedown', onMouseDown);
        el.removeEventListener('click', onMouseUp);
        el.removeEventListener('mouseout', onMouseUp);
    }
};

function onMouseDown(this: any, event: globalThis.MouseEvent) {
    if (!isCorrectButton(event.button, this.button)) {
        return;
    }

    event.stopImmediatePropagation();

    if (this.timer == null) {
        this.timer = setTimeout(() => {
            this.holding = true;

            if (this.callbacks.hold != null) {
                this.callbacks.hold(this, event);
            }
        }, HOLD_MIN);
    }
}

function onMouseUp(this: any, event: globalThis.MouseEvent) {
    if (!isCorrectButton(event.button, this.button)) {
        return;
    }

    event.stopImmediatePropagation();

    if (this.timer != null) {
        if (!this.holding) {
            if (this.callbacks.click != null) {
                this.callbacks.click(this, event);
            }
        } else {
            if (this.callbacks.release != null) {
                this.callbacks.release(this, event);
            }
        }

        clearTimeout(this.timer);
        this.timer = null;
        this.holding = false;
    }
}

function isCorrectButton(buttonIndex: number, buttonCode: string) {
    switch (buttonCode) {
        case 'left':
            return buttonIndex == 0;
        case 'right':
            return buttonIndex == 2;
        default:
            return true;
    }
}

/**
 * Get the button to listen for.
 * @param modifiers Directive modifiers
 */
function getButtonCode(modifiers: { [key: string]: boolean }) {
    if (modifiers.left) {
        return 'left';
    } else if (modifiers.right) {
        return 'right';
    } else {
        return 'either';
    }
}

/**
 * Get the callback to notify of the event.
 * @param value Directive value
 */
function getCallbacks(value: any): () => any {
    if (typeof value !== 'object') {
        throw new Error('Callbacks must be an object: {click: () => any, hold: () => any, release: () => any}');
    }

    if (value.click != null && typeof value.click !== 'function') {
        throw new Error('Click callback must be a function.');
    }

    if (value.hold != null && typeof value.hold !== 'function') {
        throw new Error('Hold callback must be a function.');
    }

    if (value.release != null && typeof value.release !== 'function') {
        throw new Error('Release callback must be a function.');
    }

    return value;
}
