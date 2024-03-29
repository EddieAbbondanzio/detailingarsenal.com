import Vue, { DirectiveFunction, VNode } from 'vue';
import { DirectiveOptions } from 'vue/types/umd';
import { DirectiveBinding } from 'vue/types/options';
import { mouseObjectManager } from '@/core/directives/mouse/mouse-object-manager';
import { MouseObject } from '@/core/directives/mouse/mouse-object';
import { MouseActionFunction } from '@/core/directives/mouse/mouse-action-function';
import { MouseAction } from '@/core/directives/mouse/mouse-action';

/**
 * How many milliseconds before trigger a hold condition.
 */
export const HOLD_MIN = 250;

/**
 * Directive to abstract basic mouse events into a click, hold, or release event.
 * Expects a value of {click: () => any, hold: () => any, release: () => any}.
 */
export const mouse: DirectiveOptions = {
    bind: function(el: any, binding: DirectiveBinding, vnode: VNode) {
        const action = getAction(binding.arg);
        const callback = getCallback(binding.value);
        const button = getButton(binding.modifiers);

        let obj = mouseObjectManager.get(el);

        if (obj == null) {
            obj = new MouseObject(el);
            mouseObjectManager.add(obj);
        }

        obj.subscribe(action, callback, button);
        el.mouseObject = obj;
    },
    unbind: function(el: HTMLElement, binding: DirectiveBinding) {
        const obj = mouseObjectManager.get(el);

        if (obj == null) {
            return;
        }

        const action = getAction(binding.arg);
        const callback = getCallback(binding.value);
        const button = getButton(binding.modifiers);

        obj.desubscribe(action, callback, button);

        if (obj.subscriberCount == 0) {
            mouseObjectManager.remove(obj);
        }
    }
};

function getAction(arg?: string): MouseAction {
    if (arg != 'click' && arg != 'hold' && arg != 'release' && arg != 'drag') {
        throw new Error('Action must be click, hold, or release.');
    }

    return arg;
}

/**
 * Get the button to listen for. Default to either
 * @param modifiers Directive modifiers
 */
function getButton(modifiers: { [key: string]: boolean }) {
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
function getCallback(value: any): MouseActionFunction {
    if (typeof value !== 'function') {
        throw new Error('Callback must be a function: () => any');
    }

    return value;
}
